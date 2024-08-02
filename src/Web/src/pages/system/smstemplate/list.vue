<template>
  <div>
    <a-card class="eip-admin-card-small" :bordered="false">
      <vxe-toolbar ref="toolbar" custom print export :refresh="{ query: tableInit }">
        <template v-slot:buttons>
          <eip-toolbar @del="del" @update="update" @add="add" @copy="copy" @onload="toolbarOnload"></eip-toolbar>
        </template>
      </vxe-toolbar>
      <vxe-table :loading="table.loading" ref="table" id="systemSmsTemplateList" size="small" :seq-config="{
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
        <vxe-column field="name" title="模板名称" width="150">
        </vxe-column>
        <vxe-column field="code" title="系统代码" width="150">
        </vxe-column>
        <vxe-column field="sign" title="签名" width="150">
        </vxe-column>
        <vxe-column field="templateId" title="模板代码" width="150">
        </vxe-column>
        <vxe-column field="template" title="模板内容" width="150" showOverflow="ellipsis">
        </vxe-column>
        <vxe-column field="remark" title="备注" min-width="150"></vxe-column>
        <vxe-column field="orderNo" title="排序" align="center" width="80"></vxe-column>
        <vxe-column field="createUserName" title="创建人" width="100"></vxe-column>
        <vxe-column field="createTime" title="创建时间" width="160"></vxe-column>
        <vxe-column field="updateUserName" title="修改人" width="100"></vxe-column>
        <vxe-column field="updateTime" title="修改时间" width="160"></vxe-column>
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

    <edit ref="edit" v-if="edit.visible" :visible.sync="edit.visible" :title="edit.title" :copy="edit.copy"
      :smsTemplateId="edit.smsTemplateId" @ok="tableInit"></edit>
  </div>
</template>

<script>
import { query, del } from "@/services/system/smstemplate/list";
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
            sord: "asc",
            sidx: "Id",
            filters: "",
          },
          total: 0,
          sizeOptions: this.eipPage.sizeOptions,
        },
        loading: true,
        data: [],
        height: this.eipHeaderHeight() - 68 + "px",
      },
      visible: {
        update: true,
        del: true,
      },
      edit: {
        visible: false,
        smsTemplateId: "",
        title: "",
        copy: false,
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
     * 
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
     * 修改
     */
    tableUpdateRow(row) {
      this.edit.smsTemplateId = row.smsTemplateId;
      this.edit.title = "短信模版配置-" + row.name;
      this.edit.visible = true;
      this.edit.copy = false;
    },
    /**
     * 删除
     */
    tableDelRow(row) {
      let that = this;
      deleteConfirm(
        "短信模版配置【" + row.name + "】" + that.eipMsg.delete,
        function () {
          that.$loading.show({ text: that.eipMsg.delloading });
          del({ id: row.smsTemplateId }).then((result) => {
            that.$loading.hide();
            if (result.code == that.eipResultCode.success) {
              that.tableInit();
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
      this.edit.smsTemplateId = null;
      this.edit.title = "新增短信模版配置";
      this.edit.visible = true;
      this.edit.copy = false;
    },

    /**
     * 修改
     */
    update() {
      let that = this;
      selectTableRow(
        that.$refs.table,
        function (row) {
          that.edit.smsTemplateId = row.smsTemplateId;
          that.edit.title = "短信模版配置-" + row.name;
          that.edit.visible = true;
          that.edit.copy = false;
        },
        that
      );
    },
    /**
     * 复制
     */
    copy() {
      let that = this;
      selectTableRow(
        that.$refs.table,
        function (row) {
          that.edit.smsTemplateId = row.smsTemplateId;
          that.edit.title = "模块复制-" + row.name;
          that.edit.visible = true;
          that.edit.copy = true;
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
              let ids = that.$utils.map(rows, (item) => item.smsTemplateId);
              del({ id: ids.join(",") }).then((result) => {
                that.$loading.hide();
                if (result.code == that.eipResultCode.success) {
                  that.tableInit();
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
        this.tableInit();
      }
    },

    /**
     * 权限按钮加载完毕
     */
    toolbarOnload(buttons) {
      this.visible.update =
        buttons.filter((f) => f.method == "update").length > 0;
      this.visible.del = buttons.filter((f) => f.method == "del").length > 0;
    },
  },
};
</script>