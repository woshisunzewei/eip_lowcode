<template>
  <div style="width: 100%">
    <a-card class="eip-admin-card-small" :bordered="false">
      <vxe-toolbar ref="toolbar" custom print export :refresh="{ query: tableInit }">
        <template v-slot:buttons>
          <eip-toolbar @del="del" @update="update" @add="add" @onload="toolbarOnload"></eip-toolbar>
        </template>
      </vxe-toolbar>
      <vxe-table :loading="table.loading" ref="table" id="systemdataBaselist" :height="table.height" :export-config="{}"
        :print-config="{}" :data="table.data">
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
        <vxe-column field="name" title="系统名称" width="150"></vxe-column>
        <vxe-column field="connectionType" title="数据库类型" width="150"></vxe-column>
        <vxe-column field="connectionString" title="数据库连接名" width="250"></vxe-column>
        <vxe-column field="remark" title="备注"></vxe-column>
        <vxe-column field="createUserName" title="创建人" width="100"></vxe-column>
        <vxe-column field="createTime" title="创建时间" width="160"></vxe-column>
        <vxe-column field="updateUserName" title="修改人" width="100"></vxe-column>
        <vxe-column field="updateTime" title="修改时间" width="160"></vxe-column>
        <vxe-column field="orderNo" title="排序" align="center" width="80"></vxe-column>
        <vxe-column title="操作" align="center" fixed="right" width="160px">
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

    <edit ref="edit" v-if="edit.visible" :visible.sync="edit.visible" :title="edit.title" :dataBaseId="edit.dataBaseId"
      @ok="operateStatus"></edit>
  </div>
</template>

<script>
import { query, del } from "@/services/system/database/list";
import edit from "./edit";


import { selectTableRow, deleteConfirm } from "@/utils/util";
export default {
  data() {
    return {
      table: {
        loading: true,
        data: [],
        height: this.eipHeaderHeight() - 68 + "px",
        search: {
          option: {
            num: 6,
            config: [
              {
                field: "Name",
                op: "cn",
                placeholder: "请输入标题",
                title: "标题",
                value: "",
                type: "text",
              },
              {
                field: "Remark",
                op: "cn",
                placeholder: "请输入简介",
                title: "简介",
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
      edit: {
        visible: false,
        dataBaseId: "",
        title: "",
        data: [],
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

    //初始化按钮数据
    tableInit() {
      let that = this;
      that.table.loading = true;
      query().then(function (result) {
        if (result.code == that.eipResultCode.success) {
          that.table.data = result.data;
        }
        that.table.loading = false;
      });
    },

    /**
     * 树更新
     */
    tableUpdateRow(row) {
      this.edit.title = "系统配置-" + row.name;
      this.edit.visible = true;
      this.edit.dataBaseId = row.dataBaseId;
    },
    /**
     * 删除
     */
    tableDelRow(row) {
      let that = this;
      deleteConfirm(
        "系统配置【" + row.name + "】" + that.eipMsg.delete,
        function () {
          that.$loading.show({ text: that.eipMsg.delloading });
          del({ id: row.dataBaseId }).then((result) => {
            if (result.code == that.eipResultCode.success) {
              that.tableInit();
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
      this.edit.dataBaseId = null;
      this.edit.title = "新增系统配置";
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
          that.edit.dataBaseId = row.dataBaseId;
          that.edit.title = "系统配置-" + row.name;
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
              let ids = that.$utils.map(rows, (item) => item.dataBaseId);
              del({ id: ids.join(",") }).then((result) => {
                if (result.code == that.eipResultCode.success) {
                  that.tableInit();
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

    //提示状态信息
    operateStatus(result) {
      if (result.code === this.eipResultCode.success) {
        this.tableInit();
      }
    },

    /**
    * 权限按钮加载完毕
    */
    toolbarOnload(buttons) {
      this.visible.add = buttons.filter((f) => f.method == "add").length > 0;
      this.visible.update =
        buttons.filter((f) => f.method == "update").length > 0;
      this.visible.del = buttons.filter((f) => f.method == "del").length > 0;
    },
  },
};
</script>