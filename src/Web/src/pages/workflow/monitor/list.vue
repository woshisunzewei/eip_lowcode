<template>
  <div style="width: 100%">
    <a-card class="eip-admin-card-small eip-admin-card-small-bottom-border" :bordered="false">
      <eip-search :option="table.search.option" @search="tableSearch"></eip-search>
    </a-card>

    <a-card class="eip-admin-card-small" :bordered="false">
      <vxe-toolbar ref="toolbar" custom print export :refresh="{ query: tableInit }">
        <template v-slot:buttons>
          <eip-toolbar @flowDoList="flowDoList" @flowDesigner="flowDesigner" @flowMonitor="flowMonitor"></eip-toolbar>
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
        <vxe-column field="sn" title="流水号" width="150"></vxe-column>
        <vxe-column field="title" title="流程名称" min-width="120"></vxe-column>

        <vxe-column field="createTime" title="创建时间" width="160"></vxe-column>
        <vxe-column field="createUserName" title="创建人" width="100"></vxe-column>

        <vxe-column field="statusRemark" title="状态描述" showOverflow="ellipsis" width="150"></vxe-column>
        <vxe-column field="receiveTime" title="流程状态" align="center" width="100"><template v-slot="{ row }">
            <eip-workflow-processInstance-status :status="row.status"></eip-workflow-processInstance-status>
          </template></vxe-column>
        <vxe-column field="urgency" title="紧急程度" width="80px" align="center">
          <template v-slot="{ row }">
            <a-tag v-if="row.urgency == 0" color="#108ee9"> 正常 </a-tag>
            <a-tag v-if="row.urgency == 2" color="#87d068"> 重要 </a-tag>
            <a-tag v-if="row.urgency == 4" color="red"> 紧急 </a-tag>
          </template>
        </vxe-column>
        <!-- <vxe-column title="操作" align="center" fixed="right" width="160px">
          <template #default="{ row }">
            <a-tooltip @click="tableUpdateRow(row)" title="编辑" v-if="visible.update">
              <label class="text-eip eip-cursor-pointer">编辑</label>
            </a-tooltip>
            <a-divider type="vertical" v-if="visible.update" />
            <a-tooltip title="删除" v-if="visible.del" @click="tableDelRow(row)">
              <label class="text-red eip-cursor-pointer">删除</label>
            </a-tooltip>
          </template>
        </vxe-column> -->
      </vxe-table>

      <a-pagination class="padding-top-sm float-right" v-model="table.page.param.current" show-size-changer
        show-quick-jumper showSizeChanger showQuickJumper :page-size-options="table.page.sizeOptions"
        :show-total="(total) => `共 ${total} 条`" :page-size="table.page.param.size" :total="table.page.total"
        @change="tableInit" @showSizeChange="tableSizeChange"></a-pagination>
    </a-card>

    <workflowHandleDoList v-if="workflow.dolist.visible" :visible.sync="workflow.dolist.visible"
      :processInstanceId="workflow.form.processInstanceId">
    </workflowHandleDoList>

    <workflowDesigner v-if="workflow.designer.visible" :visible.sync="workflow.designer.visible"
      :title="workflow.designer.title" :processInstanceId="workflow.form.processInstanceId"></workflowDesigner>

    <workflowView ref="edit" v-if="workflow.view.visible" :visible.sync="workflow.view.visible"
      :title="workflow.view.title" :isWorkflow="true" :workflowData="workflow.view.data"></workflowView>
  </div>
</template>

<script>
import { query } from "@/services/workflow/monitor/list";
import { selectTableRow } from "@/utils/util";
import workflowHandleDoList from "@/pages/workflow/handle/doList";
import workflowDesigner from "@/pages/workflow/process/designer";
import workflowView from "@/pages/system/agile/run/edit";
export default {
  components: {
    workflowHandleDoList,
    workflowDesigner,
    workflowView,
  },
  data() {
    return {
      table: {
        page: {
          param: {
            current: 1,
            size: this.eipPage.size,
            sord: "desc",
            sidx: "processInstance.CreateTime",
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
                field: "processInstance.Sn",
                op: "cn",
                placeholder: "请输入流水号",
                title: "流水号",
                value: "",
                type: "text",
              },
              {
                field: "processInstance.Title",
                op: "cn",
                placeholder: "请输入流程名称",
                title: "流程名称",
                value: "",
                type: "text",
              },
              {
                field: "processInstance.StatusRemark",
                op: "cn",
                placeholder: "请输入状态描述",
                title: "状态描述",
                value: "",
                type: "text",
              },
            ],
          },
        },
      },
      workflow: {
        form: {
          processInstanceId: null
        },
        dolist: {
          visible: false,
        },

        designer: {
          visible: false,
          title: '',
        },

        view: {
          visible: false,
          title: '',
          data: null
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
        * 查看流程
        * @param {*} row
        */
    flowMonitor() {
      let that = this;
      selectTableRow(
        that.$refs.table,
        function (row) {
          that.workflow.view.title = "流程查看-" + row.title;
          that.workflow.view.data = {
            processId: null,
            processInstanceId: row.processInstanceId,
            activityId: null,
            taskId: null,
            type: that.eipWorkflowDoType["流程监控"],
          };
          that.workflow.view.visible = true;
        },
        that
      );

    },
    /**
     * 
     */
    flowDoList() {
      let that = this;
      selectTableRow(
        that.$refs.table,
        function (row) {
          that.workflow.form.processInstanceId = row.processInstanceId;
          that.workflow.dolist.visible = true;
        },
        that
      );
    },

    /**
     * 
     */
    flowDesigner() {
      let that = this;
      selectTableRow(
        that.$refs.table,
        function (row) {
          that.workflow.form.processInstanceId = row.processInstanceId;
          that.workflow.designer.visible = true;
          that.workflow.designer.title = "流程图-" + row.title;
        },
        that
      );
    },
    /**
     * 流程意见查询
     */
    tableSearch(filters) {
      this.table.page.param.current = 1;
      this.table.page.param.filters = filters;
      this.tableInit();
    },

    /**
     * 初始化流程意见数据
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
     * 提示状态信息
     * @param {*} result 
     */
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