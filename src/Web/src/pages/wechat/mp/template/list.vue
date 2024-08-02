<template>
  <div style="width: 100%">
    <a-card class="eip-admin-card-small eip-admin-card-small-bottom-border" :bordered="false">
      <eip-search :option="table.search.option" @search="tableSearch"></eip-search>
    </a-card>

    <a-card class="eip-admin-card-small" :bordered="false">
      <vxe-toolbar ref="toolbar" custom print export :refresh="{ query: tableInit }">
        <template v-slot:buttons>
          <eip-toolbar @onload="toolbarOnload" @del="del" @update="update" @design="designClick" @send="sendClick"
            @add="add"></eip-toolbar>
        </template>
      </vxe-toolbar>
      <vxe-table :loading="table.loading" ref="table" id="wechatmptemplatelist" size="small" :seq-config="{
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
          </template>
        </vxe-column>
        <vxe-column type="seq" title="序号" width="60"></vxe-column>
        <vxe-column field="code" title="模版代码" width="300"></vxe-column>
        <vxe-column field="title" title="名称"></vxe-column>
        <vxe-column field="industryOne" title="一级行业" width="150"></vxe-column>
        <vxe-column field="industryTwo" title="二级行业" width="250"></vxe-column>

        <vxe-column title="操作" align="center" fixed="right" width="260px">
          <template #default="{ row }">
            <a-tooltip @click="tableUpdateRow(row)" title="编辑" v-if="visible.update">
              <label class="text-eip eip-cursor-pointer">编辑</label>
            </a-tooltip>
            <a-divider type="vertical" v-if="visible.update" />
            <a-tooltip title="删除" v-if="visible.del" @click="tableDelRow(row)">
              <label class="text-red eip-cursor-pointer">删除</label>
            </a-tooltip>
            <a-divider type="vertical" v-if="visible.update" />
            <a-tooltip title="模版设计" v-if="visible.design" @click="tableDesignRow(row)">
              <label class="eip-cursor-pointer">模版设计</label>
            </a-tooltip>

            <a-divider type="vertical" v-if="visible.send" />
            <a-tooltip title="消息发送" v-if="visible.send" @click="tableTemplateSendRow(row)">
              <label class="eip-cursor-pointer">消息发送</label>
            </a-tooltip>
          </template>
        </vxe-column>
      </vxe-table>

      <a-pagination class="padding-top-sm float-right" v-model="table.page.param.current" show-size-changer
        show-quick-jumper showSizeChanger showQuickJumper :page-size-options="table.page.sizeOptions"
        :show-total="(total) => `共 ${total} 条`" :page-size="table.page.param.size" :total="table.page.total"
        @change="tableInit" @showSizeChange="tableSizeChange"></a-pagination>
    </a-card>
    <edit ref="edit" v-if="edit.visible" :visible.sync="edit.visible" :title="edit.title" :templateId="edit.templateId"
      @ok="operateStatus"></edit>

    <design ref="design" v-if="design.visible" :visible.sync="design.visible" :title="design.title"
      :templateId="design.templateId" @ok="operateStatus"></design>

    <send ref="templatesend" v-if="templatesend.visible" :visible.sync="templatesend.visible"
      :title="templatesend.title" :templateId="templatesend.templateId" @ok="operateStatus"></send>
  </div>
</template>

<script>
import { query, del } from "@/services/wechat/mp/template/list";
import edit from "./edit";
import design from "./design";
import send from "./send/list";



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
            sidx: "Id",
            filters: "",
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
                field: "code",
                op: "cn",
                placeholder: "请输入模版代码",
                title: "模版代码",
                value: "",
                type: "text",
              },
              {
                field: "title",
                op: "cn",
                placeholder: "请输入名称",
                title: "名称",
                value: "",
                type: "text",
              },
              {
                field: "industryOne",
                op: "cn",
                placeholder: "请输入一级行业",
                title: "一级行业",
                value: "",
                type: "text",
              },
              {
                field: "industryTwo",
                op: "cn",
                placeholder: "请输入二级行业",
                title: "二级行业",
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
        design: true,
        send: true,
      },
      edit: {
        visible: false,
        templateId: "",
        title: "",
      },
      design: {
        visible: false,
        templateId: "",
        title: "",
      },
      templatesend: {
        visible: false,
        templateId: "",
        title: "",
      },
    };
  },
  components: { send, design, edit },
  created() {
    this.tableInit();
  },
  mounted() {
    this.$refs.table.connect(this.$refs.toolbar);
  },
  methods: {
    /**
     * 查询
     */
    tableSearch(filters) {
      this.table.page.param.current = 1;
      this.table.page.param.filters = filters;
      this.tableInit();
    },

    /**
     * 初始化微信授权用户数据
     */
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
     * 修改
     */
    tableUpdateRow(row) {
      this.edit.templateId = row.templateId;
      this.edit.title = "微信公众号模版配置-" + row.title;
      this.edit.visible = true;
    },

    /**
     * 设计
     */
    tableDesignRow(row) {
      this.design.templateId = row.templateId;
      this.design.title = "微信公众号模版设计-" + row.title;
      this.design.visible = true;
    },

    /**
     * 设计
     */
    tableTemplateSendRow(row) {
      this.templatesend.templateId = row.templateId;
      this.templatesend.title = "微信公众号模版信息发送-" + row.title;
      this.templatesend.visible = true;
    },

    /**
     * 删除
     */
    tableDelRow(row) {
      let that = this;
      deleteConfirm(
        "微信公众号模版配置【" + row.title + "】" + that.eipMsg.delete,
        function () {
          that.$loading.show({ text: that.eipMsg.delloading });
          del({ id: row.templateId }).then((result) => {
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

    /**
     * 新增
     */
    add() {
      this.edit.templateId = null;
      this.edit.title = "新增微信公众号模版配置";
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
          that.edit.templateId = row.templateId;
          that.edit.title = "微信公众号模版配置-" + row.title;
          that.edit.visible = true;
        },
        that
      );
    },
    /**
     * 修改
     */
    designClick() {
      let that = this;
      selectTableRow(
        that.$refs.table,
        function (row) {
          that.design.templateId = row.templateId;
          that.design.title = "微信公众号模版设计-" + row.title;
          that.design.visible = true;
        },
        that
      );
    },

    /**
     *
     */
    sendClick() {
      let that = this;
      selectTableRow(
        that.$refs.table,
        function (row) {
          that.templatesend.templateId = row.templateId;
          that.templatesend.title = "微信公众号模版发送-" + row.title;
          that.templatesend.visible = true;
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
              let ids = that.$utils.map(rows, (item) => item.templateId);
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
      this.visible.update =
        buttons.filter((f) => f.method == "update").length > 0;
      this.visible.del = buttons.filter((f) => f.method == "del").length > 0;

      this.visible.design =
        buttons.filter((f) => f.method == "design").length > 0;

      this.visible.send = buttons.filter((f) => f.method == "send").length > 0;
    },
  },
};
</script>