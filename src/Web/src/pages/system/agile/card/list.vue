<template>
  <div style="width: 100%">
    <a-card class="eip-admin-card-small eip-admin-card-small-bottom-border" :bordered="false">
      <eip-search :option="table.search.option" @search="tableSearch"></eip-search>
    </a-card>

    <a-card class="eip-admin-card-small" :bordered="false">
      <vxe-toolbar ref="toolbar" custom print export :refresh="{ query: tableInit }">
        <template v-slot:buttons>
          <a-space>
            <a-button type="primary" @click="add">
              <a-icon type="plus" />新增
            </a-button>
            <a-button type="primary" @click="update">
              <a-icon type="edit" />编辑
            </a-button>
            <a-button type="danger"> <a-icon type="delete" />删除 </a-button>
            <a-button type="primary" @click="designerConfig">
              <a-icon type="delete" />设计
            </a-button>
          </a-space>
        </template>
      </vxe-toolbar>
      <vxe-table ref="table" :seq-config="{
        startIndex: (table.page.param.current - 1) * table.page.param.size,
      }" :height="table.height" :export-config="{}" :print-config="{}" :data="table.data">
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
          </template></vxe-column>
        <vxe-column type="seq" title="序号" width="60"></vxe-column>
        <vxe-column field="name" title="名称" min-width="150"></vxe-column>
        <vxe-column field="isFreeze" title="禁用" align="center" width="80">
          <template v-slot="{ row }">
            <a-switch :checked="row.isFreeze" @change="isFreezeChange(row)" />
          </template>
        </vxe-column>
        <vxe-column field="orderNo" title="排序" align="center" width="80"></vxe-column>
        <vxe-column field="remark" title="备注" showOverflow="ellipsis"></vxe-column>
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
              <label class="text-red eip-cursor-pointer">设计</label>
            </a-tooltip>
          </template>
        </vxe-column>
      </vxe-table>
      <a-pagination class="padding-top-sm float-right" v-model="table.page.param.current" show-size-changer
        show-quick-jumper showSizeChanger showQuickJumper :page-size-options="table.page.sizeOptions"
        :show-total="(total) => `共 ${total} 条`" :page-size="table.page.param.size" :total="table.page.total"
        @change="tableInit" @showSizeChange="tableSizeChange"></a-pagination>
    </a-card>

    <edit ref="edit" :visible.sync="edit.visible" :data="edit.data" :title="edit.title" :configId="edit.configId"
      @cancel="editCancel" @ok="operateStatus"></edit>

    <designer ref="designer" :visible.sync="designer.visible" :data="designer.data" :title="designer.title"
      :configId="designer.configId" @cancel="designerCancel"></designer>
  </div>
</template>

<script>
import { query, del } from "@/services/system/agile/designer/data/list";
import edit from "./edit";
import designer from "./designer";

import { selectTableRow, deleteConfirm } from "@/utils/util";
export default {
  data() {
    return {
      table: {
        page: {
          param: {
            current: 1,
            size: this.eipPage.size,
            sord: "desc",
            sidx: "",
            filters: "",
            type: 0,
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
        data: [],
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
     * 树更新
     */
    tableUpdateRow(row) {
      this.edit.title = "列表编辑-" + row.name;
      this.edit.visible = true;
      this.$refs.edit.find(row.configId);
    },
    /**
     * 删除
     */
    tableDelRow(row) {
      let that = this;
      deleteConfirm(
        "列表编辑【" + row.name + "】" + that.eipMsg.delete,
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
      that.$refs.designer.find(row.configId);
    },

    /**
     * 新增
     */
    add() {
      this.edit.title = "新增列表编辑";
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
          that.edit.title = "编辑列表-" + row.name;
          that.edit.visible = true;
          that.$refs.edit.find(row.configId);
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
          that.$refs.designer.find(row.configId);
        },
        that
      );
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
  },
};
</script>