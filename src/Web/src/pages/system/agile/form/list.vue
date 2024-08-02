<template>
  <div style="width: 100%">
    <a-card class="eip-admin-card-small eip-admin-card-small-bottom-border" :bordered="false">
      <eip-search :option="table.search.option" @search="tableSearch"></eip-search>
    </a-card>

    <a-card class="eip-admin-card-small" :bordered="false">
      <vxe-toolbar ref="toolbar" custom print export :refresh="{ query: tableInit }">
        <template v-slot:buttons>
          <eip-toolbar @del="del" @update="update" @add="add" @designer="designerConfig"
            @onload="toolbarOnload"></eip-toolbar>
        </template>
      </vxe-toolbar>
      <vxe-table :loading="table.loading" id="formlist" ref="table" :seq-config="{
        startIndex: (table.page.param.current - 1) * table.page.param.size,
      }" :height="table.height" :export-config="{}" :print-config="{}" :data="table.data"
        :tooltip-config="tooltipConfig">
        <template #loading>
          <a-spin></a-spin>
        </template>
        <template #empty>
          <eip-empty />
        </template>
        <vxe-column type="checkbox" width="40" align="center" fixed="left">
          <template #header="{ checked, indeterminate }">
            <span @click.stop="$refs.table.toggleAllCheckboxRow()">
              <a-checkbox :indeterminate="indeterminate" :checked="checked">
              </a-checkbox>
            </span>
          </template>
          <template #checkbox="{ row, checked, indeterminate }">
            <span @click.stop="$refs.table.toggleCheckboxRow(row)">
              <a-checkbox :indeterminate="indeterminate" :checked="checked">
              </a-checkbox>
            </span>
          </template>
        </vxe-column>
        <vxe-column type="seq" title="序号" width="60"></vxe-column>

        <vxe-column field="name" title="名称" width="250" showOverflow="ellipsis"></vxe-column>
        <vxe-column field="isFreeze" title="禁用" align="center" width="80">
          <template v-slot="{ row }">
            <a-switch :checked="row.isFreeze" @change="isFreezeChange(row)" />
          </template>
        </vxe-column>
        <!-- <vxe-column field="useType" title="使用类型" align="center" width="80">
          <template v-slot="{ row }">
            <a-tag color="#2db7f5" v-if="row.useType == 1">普通表单</a-tag>
            <a-tag color="#f50" v-if="row.useType == 2">流程表单</a-tag>
          </template>
        </vxe-column> -->
        <vxe-column field="formCategory" title="表单类型" align="center" width="80">
          <template v-slot="{ row }">
            <a-tag color="#2db7f5" v-if="row.formCategory == 1">设计器</a-tag>
            <a-tag color="#f50" v-if="row.formCategory == 2">自定义</a-tag>
          </template>
        </vxe-column>
        <vxe-column field="orderNo" title="排序" align="center" width="80"></vxe-column>
        <vxe-column field="remark" title="备注" min-width="150" showOverflow="ellipsis"></vxe-column>
        <vxe-column field="createUserName" title="创建人" width="100"></vxe-column>
        <vxe-column field="createTime" title="创建时间" width="160"></vxe-column>
        <vxe-column field="updateUserName" title="修改人" width="100"></vxe-column>
        <vxe-column field="updateTime" title="修改时间" width="160"></vxe-column>
        <vxe-column field="configId" title="记录ID" width="350">
          <template v-slot="{ row }">
            <a-tooltip>
              <template slot="title">
                点击复制
              </template>
              <a-icon class="configIdCopy" :data-clipboard-text="row.configId" @click="configIdCopy()"
                type="copy"></a-icon>
            </a-tooltip>
            <label>{{ row.configId }}</label>
          </template>
        </vxe-column>
        <vxe-column title="操作" align="center" fixed="right" width="160px">
          <template #default="{ row }">
            <a-tooltip @click="tableUpdateRow(row)" title="编辑" v-if="visible.update">
              <label class="text-eip eip-cursor-pointer">编辑</label>
            </a-tooltip>
            <a-divider type="vertical" v-if="visible.update" />
            <a-tooltip title="删除" v-if="visible.del" @click="tableDelRow(row)">
              <label class="text-red eip-cursor-pointer">删除</label>
            </a-tooltip>
            <a-divider type="vertical" v-if="visible.update" />
            <a-tooltip title="设计" v-if="visible.designer" @click="tableDesignerRow(row)">
              <label class="text-eip eip-cursor-pointer">设计</label>
            </a-tooltip>
          </template>
        </vxe-column>
      </vxe-table>
      <a-pagination class="padding-top-sm float-right" v-model="table.page.param.current" show-size-changer
        show-quick-jumper :page-size-options="table.page.sizeOptions" :show-total="(total) => `共 ${total} 条`"
        :page-size="table.page.param.size" :total="table.page.total" @change="tableInit"
        @showSizeChange="tableSizeChange"></a-pagination>
    </a-card>
    <edit ref="edit" v-if="edit.visible" :visible.sync="edit.visible" :data="edit.data" :title="edit.title"
      :configId="edit.configId" @ok="operateStatus"></edit>

    <designer ref="designer" v-if="designer.visible" :visible.sync="designer.visible" :title="designer.title"
      :publicField="false" :configId="designer.configId" @close="designerClose"></designer>
  </div>
</template>

<script>
import Clipboard from "clipboard";
import {
  query,
  del,
  isFreeze,
} from "@/services/system/agile/form/list";
import edit from "./edit";
import designer from "./designer";

import { selectTableRow, deleteConfirm } from "@/utils/util";
export default {
  data() {
    return {
      tooltipConfig: {
        showAll: true,
        enterable: true,
        contentMethod: ({ type, column, row, items, _columnIndex }) => {
          const { field, title } = column
          if (title == '操作' || field === 'configId') {
            return ''
          }
          return null
        }
      },
      table: {
        page: {
          param: {
            current: 1,
            size: this.eipPage.size,
            sord: "asc",
            sidx: "",
            filters: "",
            configType: 2,
            type: undefined,
            menuIdNull: true
          },
          total: 0,
          sizeOptions: this.eipPage.sizeOptions,
        },

        loading: true,
        data: [],
        height: this.eipHeaderHeight() - 162 + "px",
        search: {
          option: {
            num: 6,
            config: [
              {
                field: "Name",
                op: "cn",
                placeholder: "请输入名称",
                title: "名称",
                value: "",
                type: "text",
              },
              {
                field: "useType",
                op: "eq",
                title: "使用类型",
                placeholder: "请选择使用类型",
                type: "select",
                options: {
                  multiple: true, //多选
                  source: {
                    type: "custom",
                    format: [
                      { key: "1", value: "敏捷开发" },
                      { key: "2", value: "流程表单" },
                    ],
                  },
                }
              },
              {
                field: "dataFromName",
                op: "eq",
                title: "表单类型",
                placeholder: "请选择表单类型",
                type: "select",
                options: {
                  multiple: true, //多选
                  source: {
                    type: "custom",
                    format: [
                      { key: "1", value: "设计器" },
                      { key: "2", value: "自定义" },
                    ],
                  },
                }
              },
              {
                field: "remark",
                op: "cn",
                placeholder: "请输入备注",
                title: "备注",
                value: "",
                type: "text",
              },
            ],
          },
        },
      },
      visible: {
        update: true,
        del: true,
        designer: true,
      },
      detail: {
        data: {},
        visible: false,
      },

      edit: {
        visible: false,
        configId: "",
        title: "",
        data: [],
      },

      designer: {
        visible: false,
        configId: "",
        title: "",
        bodyStyle: {
          padding: "0",
        },
      },
    };
  },
  components: { edit, designer },
  created() {
    this.tableInit();
  },
  mounted() {
    this.$refs.table.connect(this.$refs.toolbar);
  },
  methods: {
    /**
        * 
        */
    configIdCopy() {
      // 复制数据
      let clipboard = new Clipboard('.configIdCopy');
      clipboard.on("success", () => {
        this.$message.success("复制成功");
      });
      clipboard.on("error", () => {
        this.$message.error("复制失败");
      });
      setTimeout(() => {
        // 销毁实例
        clipboard.destroy();
      }, 122);
    },
    /**
     *
     */
    isFreezeChange(row) {
      if (this.visible.update) {
        let that = this;
        this.$message.loading("保存中...", 0);
        isFreeze({ id: row.configId }).then((result) => {
          that.$message.destroy();
          if (result.code == that.eipResultCode.success) {
            that.reload(false);
            that.$message.success(result.msg);
          } else {
            that.$message.error(result.msg);
          }
        });
      }
    },


    /**
     * 列表查询
     */
    tableSearch(filters) {
      this.table.page.param.current = 1;
      this.table.page.param.filters = filters;
      this.tableInit();
    },

    //初始化列表数据
    tableInit() {
      let that = this;
      that.table.loading = true;
      query(that.table.page.param).then(function (result) {
        if (result.code == that.eipResultCode.success) {
          that.table.data = result.data;
          that.table.page.total = result.total;
        }
        that.table.loading = false;
      });
    },
    /**
     *数量改变
     */
    tableSizeChange(current, pageSize) {
      this.table.page.param.size = pageSize;
      this.tableInit();
    },

    /**
     * 更新
     */
    tableUpdateRow(row) {
      this.edit.title = "编辑表单-" + row.name;
      this.edit.visible = true;
      this.edit.configId = row.configId;
    },
    /**
     * 删除
     */
    tableDelRow(row) {
      let that = this;
      deleteConfirm(
        "编辑表单" + row.name + "】" + that.eipMsg.delete,
        function () {
          that.$loading.show({ text: that.eipMsg.delloading });
          del({ id: row.configId }).then((result) => {
            if (result.code == that.eipResultCode.success) {
              that.reload();
            }
            that.$loading.hide();
            that.$message.success(result.msg);
          });
        },
        that
      );
    },

    /**
     * 设计
     */
    tableDesignerRow(row) {
      let that = this;
      that.designer.title = row.name;
      that.designer.visible = true;
      that.designer.configId = row.configId;
    },

    /**
     * 新增
     */
    add() {
      this.edit.configId = null;
      this.edit.title = "新增表单";
      this.edit.visible = true;
    },

    /**
     * 修改
     */
    update() {
      let that = this;
      selectTableRow(
        that.$refs.table,
        function (row) {
          that.edit.title = "编辑表单-" + row.name;
          that.edit.configId = row.configId;
          that.edit.visible = true;
        },
        that
      );
    },

    /**
     * 删除
     */
    del() {
      let that = this;
      selectTableRow(
        that.$refs.table,
        function (rows) {
          //提示是否删除
          deleteConfirm(
            that.eipMsg.delete,
            function () {
              //加载提示
              that.$loading.show({ text: that.eipMsg.delloading });
              let ids = that.$utils.map(rows, (item) => item.configId);
              del({ id: ids.join(",") }).then((result) => {
                that.$loading.hide();
                if (result.code == that.eipResultCode.success) {
                  that.reload();
                  that.$message.success(result.msg);
                } else {
                  that.$message.error(result.msg);
                }
              });
            },
            that
          );
        },
        that,
        false
      );
    },
    /**
     * 设计
     */
    designerConfig() {
      let that = this;
      selectTableRow(
        that.$refs.table,
        function (row) {
          that.designer.title = row.name;
          that.designer.visible = true;
          that.designer.configId = row.configId;
        },
        that
      );
    },

    /**
     * 编辑关闭
     */
    designerCancel() {
      this.designer.visible = false;
    },

    //提示状态信息
    operateStatus(result) {
      if (result.code === this.eipResultCode.success) {
        this.reload();
      }
    },

    /**
     * 重新加载
     */
    reload() {
      this.table.page.param.current = 1;
      this.tableInit();
    },

    /**
     * 权限按钮加载完毕
     */
    toolbarOnload(buttons) {
      this.visible.designer =
        buttons.filter((f) => f.method == "designer").length > 0;
      this.visible.update =
        buttons.filter((f) => f.method == "update").length > 0;
      this.visible.del = buttons.filter((f) => f.method == "del").length > 0;
    },

    /**
     *
     */
    designerClose() {
      this.designer.visible = false;
    },
  },
};
</script>