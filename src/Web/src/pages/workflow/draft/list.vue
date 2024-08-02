<template>
  <div style="width: 100%">
    <a-card class="eip-admin-card-small eip-admin-card-small-bottom-border" :bordered="false">
      <eip-search :option="table.search.option" @search="tableSearch"></eip-search>
    </a-card>

    <a-card class="eip-admin-card-small" :bordered="false">
      <vxe-toolbar ref="toolbar" custom print export :refresh="{ query: tableInit }">
        <template v-slot:buttons>
          <eip-toolbar @del="del" @update="update" @add="add" @onload="toolbarOnload"></eip-toolbar>
        </template>
      </vxe-toolbar>
      <vxe-table :loading="table.loading" ref="table" id="workflowcommentlist" size="small" :seq-config="{
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
        <vxe-column field="title" title="流程名称" min-width="120"></vxe-column>

        <vxe-column field="createTime" title="创建时间" width="160"></vxe-column>
        <vxe-column field="createUserName" title="创建人" width="100"></vxe-column>

        <vxe-column field="updateTime" title="修改日期" width="150"></vxe-column>
        <vxe-column field="updateUserName" title="修改人" width="100"></vxe-column>
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

      <a-pagination class="padding-top-sm float-right" v-model="table.page.param.current" show-size-changer
        show-quick-jumper showSizeChanger showQuickJumper :page-size-options="table.page.sizeOptions"
        :show-total="(total) => `共 ${total} 条`" :page-size="table.page.param.size" :total="table.page.total"
        @change="tableInit" @showSizeChange="tableSizeChange"></a-pagination>
    </a-card>
  </div>
</template>

<script>
import { query, del } from "@/services/workflow/draft/list";



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
                field: "title",
                op: "cn",
                placeholder: "请输入流程名称",
                title: "流程名称",
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
      },
    };
  },
  created() {
    this.tableInit();
  },
  mounted() {
    this.$refs.table.connect(this.$refs.toolbar);
  },
  methods: {

    /**
     * 流程意见查询
     */
    tableSearch(filters) {
      this.table.page.param.current = 1;
      this.table.page.param.filters = filters;
      this.tableInit();
    },

    //初始化流程意见数据
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
      this.edit.commentId = row.commentId;
      this.edit.title = "流程意见配置-" + row.content;
      this.edit.visible = true;
    },
    /**
     * 删除
     */
    tableDelRow(row) {
      let that = this;
      deleteConfirm(
        "流程意见配置【" + row.content + "】" + that.eipMsg.delete,
        function () {
          that.$loading.show({ text: that.eipMsg.delloading });
          del({ id: row.commentId }).then((result) => {
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
      this.edit.commentId = null;
      this.edit.title = "新增流程意见配置";
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
          that.edit.commentId = row.commentId;
          that.edit.title = "流程意见配置-" + row.content;
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
              let ids = that.$utils.map(rows, (item) => item.commentId);
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
     * 权限意见加载完毕
     */
    toolbarOnload(comments) {
      this.visible.update =
        comments.filter((f) => f.method == "update").length > 0;
      this.visible.del = comments.filter((f) => f.method == "del").length > 0;
    },
  },
};
</script>