<template>
  <div style="width: 100%">
    <a-card class="eip-admin-card-small" :bordered="false">
      <vxe-toolbar ref="toolbar" custom print :refresh="{ query: tableInit }">
        <template v-slot:buttons>
          <eip-toolbar @del="del" @update="update" @add="add" @resumeJob="resumeJob" @resumeAllJob="resumeAllJob"
            @pauseJob="pauseJob" @pauseAllJob="pauseAllJob" @onload="toolbarOnload"></eip-toolbar>
        </template>
      </vxe-toolbar>
      <vxe-table :loading="table.loading" ref="table" :column-config="{ isCurrent: true, isHover: true }"
        :row-config="{ isCurrent: true, isHover: true }" :height="table.height" :export-config="{}" :print-config="{}"
        :data="table.data" :toolbar-config="table.toolbar">
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
        <vxe-column field="triggerState" title="状态" align="center" width="96" showOverflow="ellipsis">
          <template v-slot="{ row }">
            <a-tag color="orange" v-if="row.triggerState == 'Paused'">暂停中</a-tag>
            <a-tag color="blue" v-if="row.triggerState == 'Normal'">运行中</a-tag>
            <a-tag color="purple" v-if="row.triggerState == 'Blocked'">堵塞中</a-tag>
            <a-tag color="green" v-if="row.triggerState == 'Complete'">已完成</a-tag>
            <a-tag color="red" v-if="row.triggerState == 'Error'">错误</a-tag>
          </template>
        </vxe-column>
        <vxe-column field="jobName" title="作业名称" width="150" showOverflow="ellipsis"></vxe-column>
        <!-- <vxe-column field="jobGroup" title="作业组名称" width="150" showOverflow="ellipsis"></vxe-column> -->
        <vxe-column field="className" title="方法" width="150" showOverflow="ellipsis"></vxe-column>
        <!-- <vxe-column field="triggerName" title="触发器名称" width="150" showOverflow="ellipsis"></vxe-column>
        <vxe-column field="triggerGroup" title="触发器组名" width="150" showOverflow="ellipsis"></vxe-column>
        <vxe-column field="triggerType" title="触发器类别" width="100" showOverflow="ellipsis"></vxe-column> -->
        <vxe-column field="nextFireTime" title="下一次运行时间" width="160" showOverflow="ellipsis"></vxe-column>
        <vxe-column field="previousFireTime" title="上一次运行时间" width="160" showOverflow="ellipsis"></vxe-column>
        <vxe-column field="jobDescription" title="作业描述" min-width="150" showOverflow="ellipsis"></vxe-column>
        <vxe-column title="操作" align="center" fixed="right" width="100px">
          <template #default="{ row }">
            <label @click="jobLog(row)" class="text-eip eip-cursor-pointer">日志</label>
          </template>
        </vxe-column>
      </vxe-table>
    </a-card>

    <edit ref="edit" v-if="edit.visible" :visible.sync="edit.visible" :title="edit.title" :jobName="edit.jobName"
      :jobGroup="edit.jobGroup" :triggerName="edit.triggerName" :triggerGroup="edit.triggerGroup" @ok="operateStatus">
    </edit>

    <log ref="log" v-if="log.visible" :visible.sync="log.visible" :title="log.title" :correlation="log.correlation">
    </log>
  </div>
</template>

<script>
import edit from "./edit";
import log from "./log";
import {
  query,
  del,
  pause,
  pauseall,
  resume,
  resumeall
} from "@/services/system/job/list";

import { selectTableRow, deleteConfirm } from "@/utils/util";
export default {
  components: {
    edit,
    log
  },
  data() {
    return {
      table: {
        columns: [],
        toolbar: {
          export: true,
          print: true,
          zoom: true,
          custom: true,
          slots: {
            buttons: "buttons"
          }
        },
        loading: true,
        data: [],
        height: window.innerHeight - 160
      },
      edit: {
        visible: false,
        title: "",
        jobName: "",
        jobGroup: "",
        triggerName: "",
        triggerGroup: ""
      },
      visible: {
        update: false,
        del: false
      },
      log: {
        visible: false,
        title: "",
        correlation: ''
      }
    };
  },
  created() {
    this.tableInit();
  },
  mounted() {
    //表格和工具栏进行关联
    this.$refs.table.connect(this.$refs.toolbar);
  },
  methods: {
    /**
     * 新增
     */
    add() {
      this.edit.jobName = "";
      this.edit.title = "新增定时任务";
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
          that.edit.jobName = row.jobName;
          that.edit.jobGroup = row.jobGroup;
          that.edit.triggerName = row.triggerName;
          that.edit.triggerGroup = row.triggerGroup;
          that.edit.title = "编辑定时任务-" + row.jobName;
          that.edit.visible = true;
        },
        that
      );
    },

    /**
     * log
     */
    jobLog(row) {
      this.log.correlation = JSON.parse(row.parametersJson)[0].Value;
      this.log.title = "任务日志-" + row.jobName;
      this.log.visible = true;
    },
    /**
     * 启动
     */
    resumeJob() {
      let that = this;
      selectTableRow(
        that.$refs.table,
        function (row) {
          resume({
            jobName: row.jobName,
            jobGroup: row.jobGroup,
            triggerName: row.triggerName,
            triggerGroups: row.triggerGroup
          }).then(result => {
            if (result.code == that.eipResultCode.success) {
              that.tableInit();
            }
            that.$message.destroy();
            that.$message.success(result.msg);
          });
        },
        that
      );
    },
    /**
     * 启动所有作业
     */
    resumeAllJob() {
      let that = this;
      that.$confirm({
        title: "操作提示?",
        content: "是否启动所有作业",
        okText: "确定",
        okType: "danger",
        cancelText: "取消",
        onOk() {
          that.$message.loading("启动中...", 0);
          resumeall({}).then(result => {
            if (result.code == that.eipResultCode.success) {
              that.tableInit();
            }
            that.$message.destroy();
            that.$message.success(result.msg);
          });
        },
        onCancel() { }
      });
    },
    /**
     * 暂停
     */
    pauseJob() {
      let that = this;
      selectTableRow(
        that.$refs.table,
        function (row) {
          pause({
            jobName: row.jobName,
            jobGroup: row.jobGroup,
            triggerName: row.triggerName,
            triggerGroups: row.triggerGroup
          }).then(result => {
            if (result.code == that.eipResultCode.success) {
              that.tableInit();
            }
            that.$message.destroy();
            that.$message.success(result.msg);
          });
        },
        that
      );
    },
    /**
     * 暂停所有
     */
    pauseAllJob() {
      let that = this;
      that.$confirm({
        title: "操作提示?",
        content: "是否暂停所有作业",
        okText: "确定",
        okType: "danger",
        cancelText: "取消",
        onOk() {
          that.$message.loading("暂停中...", 0);
          pauseall({}).then(result => {
            if (result.code == that.eipResultCode.success) {
              that.tableInit();
            }
            that.$message.destroy();
            that.$message.success(result.msg);
          });
        },
        onCancel() { }
      });
    },
    /**
     * 删除
     */
    del() {
      let that = this;
      selectTableRow(
        that.$refs.table,
        function (row) {
          //提示是否删除
          deleteConfirm(
            that.eipMsg.delete,
            function () {
              that.$loading.show({ text: that.eipMsg.delloading });
              del({
                jobName: row.jobName,
                jobGroup: row.jobGroup,
                triggerName: row.triggerName,
                triggerGroup: row.triggerGroup
              }).then(result => {
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
        true
      );
    },
    /**
     * 提示状态信息
     */
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
        buttons.filter(f => f.method == "update").length > 0;
      this.visible.del = buttons.filter(f => f.method == "del").length > 0;
    },

    //初始化列表数据
    tableInit() {
      let that = this;
      that.table.loading = true;
      query().then(function (result) {
        that.table.data = result.data;
        that.table.loading = false;
      });
    }
  }
};
</script>