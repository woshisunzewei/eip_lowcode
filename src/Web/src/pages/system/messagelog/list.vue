<template>
  <div style="width: 100%">
    <a-card class="eip-admin-card-small eip-admin-card-small-bottom-border" :bordered="false">
      <eip-search :option="table.search.option" @search="tableSearch"></eip-search>
    </a-card>

    <a-card class="eip-admin-card-small" :bordered="false">
      <vxe-toolbar ref="toolbar" custom print export :refresh="{ query: tableInit }">
        <template v-slot:buttons>
          <eip-toolbar @onload="toolbarOnload"></eip-toolbar>
        </template>
      </vxe-toolbar>
      <vxe-table :loading="table.loading" ref="table" id="systemnoticelist" :seq-config="{
        startIndex: (table.page.param.current - 1) * table.page.param.size,
      }" :height="table.height" :export-config="{}" :print-config="{}" :data="table.data">
        <template #loading>
          <a-spin></a-spin>
        </template>
        <template #empty>
          <eip-empty />
        </template>
        <vxe-column type="seq" title="序号" width="60"></vxe-column>
        <vxe-column field="createTime" title="接收时间" width="160"></vxe-column>
        <vxe-column field="title" title="标题" width="250"></vxe-column>
        <vxe-column field="isRead" title="已阅" width="150">
          <template v-slot="{ row }">
            <a-switch :checked="row.isRead" /> </template></vxe-column>
        <vxe-column field="message" title="内容" min-width="150"></vxe-column>

        <vxe-column title="操作" align="center" fixed="right" width="100px">
          <template #default="{ row }">
            <a-tooltip @click="tableUpdateRow(row)" title="编辑">
              <label class="text-eip eip-cursor-pointer">编辑</label>
            </a-tooltip>
          </template>
        </vxe-column>
      </vxe-table>

      <a-pagination class="padding-top-sm float-right" v-model="table.page.param.current" show-size-changer
        show-quick-jumper :page-size-options="table.page.sizeOptions" :show-total="(total) => `共 ${total} 条`"
        :page-size="table.page.param.size" :total="table.page.total" @change="tableInit"
        @showSizeChange="tableSizeChange"></a-pagination>
    </a-card>

    <workflow-view ref="workflowView" v-if="workflowDo.visible" :visible.sync="workflowDo.visible"
      :title="workflowDo.title" :isWorkflow="true" :workflowData="workflowDo.data"></workflow-view>
  </div>
</template>

<script>
import workflowView from "@/pages/system/agile/run/edit";
import { messageLogMy } from "@/services/system/messagelog/list";
import { findById, read } from "@/services/system/messagelog/header";


import { selectTableRow, deleteConfirm } from "@/utils/util";
export default {
  data() {
    return {
      //编辑组件
      workflowDo: {
        visible: false,
        title: null,
        isWorkflow: false,
        data: {
          processId: null, //流程实例Id
          processInstanceId: null,
          activityId: null,
          taskId: null,
          doType: 1, //处理类型1审核，2知会，3加签，4阅示，5流程监控，6阅示审批处理,有通过和拒绝按钮
        },
        update: {
          relationId: null, //业务表记录IDId
        },
      },
      table: {
        page: {
          param: {
            current: 1,
            size: this.eipPage.size,
            sord: "desc",
            sidx: "createTime",
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
                placeholder: "请输入标题",
                title: "标题",
                value: "",
                type: "text",
              },
              {
                field: "message",
                op: "cn",
                placeholder: "请输入内容",
                title: "内容",
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
    };
  },
  components: { workflowView },
  created() {
    this.tableInit();
  },
  mounted() {
    this.$refs.table.connect(this.$refs.toolbar);
  },
  methods: {
    /**
     * 按钮查询
     */
    tableSearch(filters) {
      this.table.page.param.current = 1;
      this.table.page.param.filters = filters;
      this.tableInit();
    },

    //初始化按钮数据
    tableInit() {
      let that = this;
      that.table.loading = true;
      messageLogMy(that.table.page.param).then(function (result) {
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
     *
     */
    tableUpdateRow(messageLog) {
      let that = this;
      this.show = false;
      read({
        messageLogIds: messageLog.messageLogId,
      }).then((result) => {
        that.reload();
      });
      //读取消息明细
      findById(messageLog.messageLogId).then((result) => {
        var row = result.data;
        if (row.openUrl == "workflowView") {
          that.workflowDo.title = "处理流程-" + row.title;
          var workflowData = JSON.parse(row.customerData);
          that.workflowDo.data = {
            processInstanceId: workflowData.ProcessInstanceId,
            activityId: workflowData.ActivityId,
            taskId: workflowData.TaskId,
            type: that.eipWorkflowDoType["阅示审批"],
          };
          that.workflowDo.visible = true;
        }

        if (row.openUrl == "workflwCuiBan") {
          that.workflowDo.title = "处理流程-" + row.title;
          var workflowData = JSON.parse(row.customerData);
          that.workflowDo.data = {
            processInstanceId: workflowData.ProcessInstanceId,
            activityId: workflowData.ActivityId,
            taskId: workflowData.TaskId,
            type: that.eipWorkflowDoType["审核"],
          };
          that.workflowDo.visible = true;
        }
      });
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
    toolbarOnload(buttons) { },
  },
};
</script>