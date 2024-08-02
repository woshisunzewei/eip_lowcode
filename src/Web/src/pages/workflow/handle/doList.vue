<template>
  <a-modal width="1100px" v-drag :destroyOnClose="true" :maskClosable="false" :visible="visible"
    :bodyStyle="{ padding: '1px' }" title="过程列表" @cancel="cancel">
    <a-tabs default-active-key="1">
      <a-tab-pane key="1">
        <span slot="tab">
          列表
        </span>
        <vxe-table show-header-overflow size="small" :height="height" :data="data">
          <template #loading>
            <a-spin></a-spin>
          </template>
          <template #empty>
            <eip-empty />
          </template>
          <vxe-column type="seq" title="序号" width="60"></vxe-column>
          <vxe-column field="type" align="center" title="当前节点" width="100px">
            <template v-slot="{ row }">
              <a-tag v-if="row.activityType == 2" color="#108ee9"> 开始 </a-tag>
              <a-tag v-if="row.activityType == 4" color="#87d068"> 审批 </a-tag>
              <a-tag v-if="row.activityType == 6" color="red"> 子流程 </a-tag>
              <a-tag v-if="row.activityType == 12" color="red"> 知会 </a-tag>
              <a-tag v-if="row.activityType == 14" color="red"> 阅示 </a-tag>
              <a-tag v-if="row.activityType == 16" color="red"> 加签 </a-tag>
              <a-tag v-if="row.activityType == 100" color="red"> 结束 </a-tag>
            </template>
          </vxe-column>
          <vxe-column field="title" title="步骤名称" width="200px"></vxe-column>
          <vxe-column field="sendUserName" title="发送人" width="100"></vxe-column>
          <vxe-column field="receiveUserName" title="接收人" width="100"></vxe-column>
          <vxe-column field="receiveTime" title="接收时间" width="160"></vxe-column>
          <vxe-column field="doUserName" title="处理人" width="100"></vxe-column>
          <vxe-column field="completedTime" title="处理时间" width="160"></vxe-column>
          <vxe-column field="comment" title="审批意见" width="160"></vxe-column>
          <vxe-column field="approveUserName" title="审核人" width="100"></vxe-column>
          <vxe-column field="approveTime" title="审核时间" width="160"></vxe-column>
          <vxe-column field="approveComment" title="审核意见" width="160"></vxe-column>
          <vxe-column field="status" title="状态" width="100px" align="center">
            <template v-slot="{ row }">
              <eip-workflow-task-status :status="row.status"></eip-workflow-task-status>
            </template>
          </vxe-column>
        </vxe-table>
      </a-tab-pane>
      <a-tab-pane key="2">
        <span slot="tab">
          图形
        </span>
        <div class="padding beauty-scroll" :style="{ height: height, overflow: 'auto' }">
          <a-timeline>
            <a-timeline-item v-for="(item, index) in data" :key="index">
              <div slot="dot"
                :class="(item.completedTime ? 'pointerItem' : 'pointerItem active') + (index == 0 ? ' begin ' : '') + '' + (item.activityType == 100 ? ' end ' : '')">
              </div>
              <div style="font-weight: 700;font-size:15px" :style="{ color: item.completedTime ? '#333' : '#1e88e5' }">
                {{ item.completedTime }}</div>
              <div class="padding workflowCard"
                style="margin-top: 10px;background-color: #fff;border-radius: 5px; box-shadow: 0 1px 3px rgb(0 0 0 / 10%);">
                <div style="font-size: 15px;font-weight: 700;" :style="{ color: item.completedTime ? '#333' : '#333' }">
                  {{ item.title
                  }}
                </div>
                <div class="flex justify-start align-center margin-top-xs">
                  <div>
                    <img style="width: 32px; height: 32px; border-radius: 50%" v-real-img="item.headImage" />
                  </div>
                  <div class="margin-left-xs">
                    <div style="font-size: 15px;color:#333;font-weight: 700">{{ item.receiveUserName }}
                      <label class="margin-left-xs" v-if="item.status == 16 && index != 0" style="color:#ff1e02">
                        拒绝</label>
                      <label class="margin-left-xs" v-if="item.status == 6 && index != 0" style="color:#4caf50">
                        通过</label>
                    </div>
                    <div style="font-size: 14px;color:#9e9e9e">{{ item.comment }}</div>
                  </div>
                </div>
                <div style="margin-left:30px">
                  <a-upload :showUploadList="{ showRemoveIcon: false }" :file-list="item.files"> </a-upload>
                </div>
              </div>
            </a-timeline-item>
          </a-timeline>
        </div>
      </a-tab-pane>
    </a-tabs>
  </a-modal>
</template>

<script>
import {
  workflowInstanceProcess,
  fileCorrelationId,
} from "@/services/workflow/send/index";
export default {
  components: {},
  name: "dolist",
  data() {
    return {
      height: "500px",
      loading: false,
      data: [],
    };
  },

  props: {
    visible: {
      type: Boolean,
      default: false,
    },
    processInstanceId: {
      type: String,
    },
  },
  mounted() {
    this.initInstanceProcess();
  },
  methods: {
    /**
     *
     */
    async initInstanceProcess() {
      let that = this;
      this.loading = true;
      workflowInstanceProcess({ id: this.processInstanceId }).then(async (result) => {
        result.data.forEach((item) => {
          item.files = [];
        });
        result.data.forEach(async (item) => {
          var fileResult = await fileCorrelationId(item.taskId);
          var uploads = [];
          fileResult.forEach((upload, index) => {
            uploads.push({
              fileId: upload.fileId || "",
              type: "file",
              name: upload.name,
              status: "done",
              uid: index,
              url: upload.path || "",
            });
          });
          item.files = uploads;
        });
        that.data = result.data;
        that.loading = false;
      });
    },
    /**
     * 取消
     */
    cancel() {
      this.$emit("update:visible", false);
    },

    /**
     * 保存
     */
    save() {
      let that = this;
      this.$refs.form.validate((valid) => {
        if (valid) {
        } else {
          return false;
        }
      });
    },
  },
};
</script>

<style lang="less" scoped>
/deep/.ant-timeline-item-content {
  margin: 0 0 0 30px !important
}

.pointerItem {
  background-color: #ddd;
  border-radius: 50%;
  flex-shrink: 0;
  height: 11px;
  width: 11px;
}

.pointerItem.active {
  background-color: #fff;
  box-shadow: 0 0 10px 1px #2196f3;
  flex: 0 0 20px;
  height: 20px;
  margin-top: -6px;
  position: relative;
  width: 20px;
}

.pointerItem.active:after {
  background: #2196f3;
  border-radius: 50%;
  content: "";
  height: 16px;
  left: 2px;
  position: absolute;
  top: 2px;
  width: 16px;
}

.pointerItem.begin:after {
  background: #108ee9;
  border-radius: 50%;
  content: "";
  height: 16px;
  left: 2px;
  position: absolute;
  top: 2px;
  width: 16px;
}

.pointerItem.begin {
  background-color: #fff;
  box-shadow: 0 0 10px 1px #108ee9;
  flex: 0 0 20px;
  height: 20px;
  margin-top: -6px;
  position: relative;
  width: 20px;
}

.pointerItem.end {
  background-color: #fff;
  box-shadow: 0 0 10px 1px #ff4d52;
  flex: 0 0 20px;
  height: 20px;
  margin-top: -6px;
  position: relative;
  width: 20px;
}

.pointerItem.end:after {
  background: #ff4d52;
  border-radius: 50%;
  content: "";
  height: 16px;
  left: 2px;
  position: absolute;
  top: 2px;
  width: 16px;
}
</style>
