<template>
  <div style="width: 100%">
    <a-card class="eip-admin-card-small" :bordered="false">
      <vxe-toolbar ref="toolbar" custom print export :refresh="{ query: tableInit }">
        <template v-slot:buttons>
          <eip-toolbar @onload="toolbarOnload" @del="del" @update="update" @add="add"></eip-toolbar>
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

        <vxe-column field="name" title="名称"></vxe-column>
        <vxe-column field="appId" title="AppId"></vxe-column>
        <vxe-column title="类型" align="center" width="100">
          <template v-slot="{ row }">
            <a-tag color="#f50" v-if="row.type == 1">公众号</a-tag>
            <a-tag color="#2db7f5" v-if="row.type == 2">小程序</a-tag>
            <a-tag color="#87d068" v-if="row.type == 3">企业微信</a-tag>
          </template>
        </vxe-column>
        <vxe-column title="操作" align="center" fixed="right" width="260px">
          <template #default="{ row }">
            <a-tooltip @click="tableUpdateRow(row)" title="编辑" v-if="visible.update">
              <label class="text-eip eip-cursor-pointer">编辑</label>
            </a-tooltip>
            <a-divider type="vertical" v-if="visible.update" />
            <a-tooltip title="删除" v-if="visible.del" @click="tableDelRow(row)">
              <label class="text-red eip-cursor-pointer">删除</label>
            </a-tooltip>
          </template>
        </vxe-column>
      </vxe-table>
    </a-card>
    <edit ref="edit" v-if="edit.visible" :visible.sync="edit.visible" :title="edit.title"
      :weChatAccountId="edit.weChatAccountId" @ok="operateStatus"></edit>
  </div>
</template>

<script>
import { query, del } from "@/services/wechat/account/list";
import edit from "./edit";


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
        height: window.innerHeight - 164,
      },

      visible: {
        update: true,
        del: true,
        design: true,
      },
      edit: {
        visible: false,
        weChatAccountId: "",
        title: "",
      },
    };
  },
  components: { edit },
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
      this.edit.weChatAccountId = row.weChatAccountId;
      this.edit.title = "微信帐号配置-" + row.name;
      this.edit.visible = true;
    },

    /**
     * 设计
     */
    tableDesignRow(row) {
      this.design.weChatAccountId = row.weChatAccountId;
      this.design.title = "微信帐号-" + row.name;
      this.design.visible = true;
    },

    /**
     * 删除
     */
    tableDelRow(row) {
      let that = this;
      deleteConfirm(
        "微信帐号配置【" + row.name + "】" + that.eipMsg.delete,
        function () {
          that.$loading.show({ text: that.eipMsg.delloading });
          del({ id: row.weChatAccountId }).then((result) => {
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
     * 新增
     */
    add() {
      this.edit.weChatAccountId = null;
      this.edit.title = "新增微信帐号配置";
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
          that.edit.weChatAccountId = row.weChatAccountId;
          that.edit.title = "微信帐号配置-" + row.name;
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
              let ids = that.$utils.map(rows, (item) => item.weChatAccountId);
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
    },
  },
};
</script>