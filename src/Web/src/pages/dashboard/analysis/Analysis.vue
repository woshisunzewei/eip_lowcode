<template>
  <div class="analysis">
    <div class="cardList-wrapper">
      <a-row :gutter="24">
        <a-col :xs="12" :md="12" :sm="12" :lg="6" :key="index" v-for="(item, index) in numbers">
          <div class="card-item">
            <div class="card-title">
              <a-space :size="8">
                <a-icon style="font-size: 16px" :type="item.icon"></a-icon>
                {{ item.name }}
              </a-space>
            </div>
            <div class="all-count">
              <count-to :startVal="0" :endVal="item.number" :duration="2200" />
              <span class="suffix"></span>
            </div>
            <div class="card-info relative">
              <div class="abs-bottom">
                <div style="margin-right: 20px" class="inline-block">
                  <a-space :size="8"> </a-space>
                </div>
                <div class="inline-block"></div>
              </div>
            </div>
            <!-- <div class="link-block-band">待审批</div> -->
          </div>
        </a-col>
      </a-row>

      <a-row :gutter="12">
        <a-col :span="24">
          <a-card size="small">
            <a-tabs>
              <a-tab-pane key="1">
                <span slot="tab">
                  <a-icon type="file-search" />待办事项（{{
                    numbers[0].number
                  }}）
                </span>
                <vxe-table ref="table" id="needdo" :height="height" :data="needDo.data" :loading="needDo.loading">
                  <template #loading>
                    <a-spin></a-spin>
                  </template>
                  <template #empty>
                    <eip-empty />
                  </template>
                  <vxe-column type="seq" width="60"></vxe-column>
                  <vxe-column field="sn" title="流水号" width="150px"></vxe-column>
                  <vxe-column field="title" title="流程名称" min-width="150px"></vxe-column>
                  <vxe-column field="createUserName" title="发起人" width="150px"></vxe-column>
                  <vxe-column field="sendUserName" title="上步骤处理人" width="150px"></vxe-column>
                  <vxe-column field="receiveTime" title="步骤时间" width="150px"></vxe-column>
                  <vxe-column field="activityType" align="center" title="当前类别" width="150px">
                    <template v-slot="{ row }">
                      <a-tag v-if="row.activityType == 2" color="#108ee9">
                        开始
                      </a-tag>

                      <a-tag v-if="row.activityType == 4" color="#87d068">
                        审批
                      </a-tag>

                      <a-tag v-if="row.activityType == 6" color="red">
                        子流程
                      </a-tag>

                      <a-tag v-if="row.activityType == 12" color="red">
                        知会
                      </a-tag>

                      <a-tag v-if="row.activityType == 14" color="red">
                        阅示
                      </a-tag>

                      <a-tag v-if="row.activityType == 16" color="red">
                        加签
                      </a-tag>

                      <a-tag v-if="row.activityType == 100" color="red">
                        结束
                      </a-tag>
                    </template>
                  </vxe-column>
                  <vxe-column field="activityName" title="当前步骤" min-width="150px"></vxe-column>
                  <vxe-column field="urgency" title="紧急程度" width="80px" align="center">
                    <template v-slot="{ row }">
                      <a-tag v-if="row.urgency == 0" color="#108ee9">
                        正常
                      </a-tag>
                      <a-tag v-if="row.urgency == 2" color="#87d068">
                        重要
                      </a-tag>
                      <a-tag v-if="row.urgency == 4" color="red"> 紧急 </a-tag>
                    </template>
                  </vxe-column>
                  <vxe-column title="操作" width="100px" align="center">
                    <template #default="{ row }">
                      <label @click="workflowNeedDoClick(row)" class="text-eip eip-cursor-pointer">处理</label>
                    </template></vxe-column>
                </vxe-table>
              </a-tab-pane>
              <a-tab-pane key="2">
                <span slot="tab">
                  <a-icon type="file-done" />已办事项（{{ numbers[1].number }}）
                </span>
                <vxe-table ref="table" id="needdo" :height="height" :data="haveDo.data" :loading="haveDo.loading">
                  <template #loading>
                    <a-spin></a-spin>
                  </template>
                  <template #empty>
                    <eip-empty />
                  </template>
                  <vxe-column type="seq" width="60"></vxe-column>
                  <vxe-column field="sn" title="流水号" width="150px"></vxe-column>
                  <vxe-column field="title" title="流程名称" min-width="150px"></vxe-column>
                  <vxe-column field="createUserName" title="创建人" width="100"></vxe-column>
                  <vxe-column field="createTime" title="创建时间" width="160"></vxe-column>
                  <!-- <vxe-column
                    field="sendUserName"
                    title="紧急程度"
                    align="center"
                    width="100"
                  >
                    <template v-slot="{ row }">
                      <a-tag v-if="row.urgency == 0" color="#108ee9">
                        正常
                      </a-tag>
                      <a-tag v-if="row.urgency == 2" color="#87d068">
                        重要
                      </a-tag>
                      <a-tag v-if="row.urgency == 4" color="red"> 紧急 </a-tag>
                    </template></vxe-table-column
                  > -->
                  <vxe-column field="receiveTime" title="任务状态" align="center" width="100"><template v-slot="{ row }">
                      <eip-workflow-task-status :status="row.status"></eip-workflow-task-status>
                    </template></vxe-column>
                  <vxe-column field="age" title="备注" min-width="150px"></vxe-column>
                  <vxe-column title="操作" width="100px">
                    <template #default="{ row }">
                      <a-tooltip @click="workflowHaveDoClick(row)" title="查看">
                        <label class="text-eip eip-cursor-pointer">查看</label>
                      </a-tooltip>
                    </template></vxe-column>
                </vxe-table>
              </a-tab-pane>
              <a-tab-pane key="3">
                <span slot="tab">
                  <a-icon type="user" />我的请求（{{ numbers[2].number }}）
                </span>
                <vxe-table ref="table" id="needdo" :height="height" :data="haveSend.data" :loading="haveSend.loading">
                  <template #loading>
                    <a-spin></a-spin>
                  </template>
                  <template #empty>
                    <eip-empty />
                  </template>
                  <vxe-column type="seq" width="60"></vxe-column>
                  <vxe-column field="sn" title="流水号" width="150px"></vxe-column>
                  <vxe-column field="title" title="流程名称"></vxe-column>
                  <vxe-column field="createTime" title="创建时间" width="150px"></vxe-column>
                  <vxe-column field="status" title="流程状态" width="150px"><template v-slot="{ row }">
                      <eip-workflow-processInstance-status :status="row.status"></eip-workflow-processInstance-status>
                    </template></vxe-column>
                  <vxe-column field="statusRemark" title="状态描述" showOverflow="ellipsis"></vxe-column>
                  <vxe-column title="操作" width="100px">
                    <template #default="{ row }">
                      <a-tooltip @click="workflowMonitorClick(row)" title="查看">
                        <label class="text-eip eip-cursor-pointer">查看</label>
                      </a-tooltip>
                    </template></vxe-column>
                </vxe-table>
              </a-tab-pane>
              <a-tab-pane key="4">
                <span slot="tab">
                  <a-icon type="hourglass" />已超时（{{ numbers[3].number }}）
                </span>
                <vxe-table ref="table" id="needdo" :height="height" :data="overTime.data" :loading="overTime.loading">
                  <template #loading>
                    <a-spin></a-spin>
                  </template>
                  <template #empty>
                    <eip-empty />
                  </template>
                  <vxe-column type="seq" width="60"></vxe-column>
                  <vxe-column field="sn" title="流水号"></vxe-column>
                  <vxe-column field="title" title="流程名称"></vxe-column>
                  <vxe-column field="processTypeName" title="类别"></vxe-column>
                  <vxe-column field="createUserName" title="流程拥有人"></vxe-column>
                  <vxe-column field="sendUserName" title="上步骤处理人"></vxe-column>
                  <vxe-column field="receiveTime" title="步骤时间"></vxe-column>
                  <vxe-column field="receiveTime" title="超时时间"></vxe-column>
                  <vxe-column field="age" title="当前类别"></vxe-column>
                  <vxe-column field="age" title="当前步骤"></vxe-column>
                  <vxe-column field="age" title="紧急程度"></vxe-column>
                  <vxe-column title="操作"></vxe-column>
                </vxe-table>
              </a-tab-pane>
              <!-- <a-tab-pane key="5">
                <span slot="tab"> <a-icon type="rest" />草稿 </span>
                <vxe-table
                  ref="table"
                  id="needdo"
                  :height="height"
                  :data="[]"
                  
                >
                  <vxe-column type="seq" width="60"></vxe-column>
                  <vxe-column
                    field="title"
                    title="流程名称"
                  ></vxe-column>
                  <vxe-column
                    field="createTime"
                    title="创建日期"
                  ></vxe-column>
                  <vxe-column title="操作"></vxe-column>
                </vxe-table>
              </a-tab-pane>
              <a-tab-pane key="6">
                <span slot="tab"> <a-icon type="read" />范本 </span>
                <vxe-table
                  ref="table"
                  id="model"
                  :height="height"
                  :data="[]"
                  
                >
                  <vxe-column type="seq" width="60"></vxe-column>
                  <vxe-column
                    field="title"
                    title="流程名称"
                  ></vxe-column>
                  <vxe-column
                    field="createTime"
                    title="创建日期"
                  ></vxe-column>
                  <vxe-column title="操作"></vxe-column>
                </vxe-table>
              </a-tab-pane> -->
            </a-tabs>
          </a-card>
        </a-col>
        <!-- <a-col :span="6">
          <a-card size="small">
            <a-tabs>
              <a-tab-pane key="1">
                <span slot="tab">
                  <a-icon type="file-search" />系统公告
                </span>
                <a-spin :spinning="notice.loading">
                  <vxe-table :seq-config="{
                    startIndex: (notice.page.param.current - 1) * notice.page.param.size,
                  }"   :height="height" :data="notice.data"
                    >
                    <vxe-column field="title" title="标题"></vxe-column>
                    <vxe-column field="createTime" title="创建时间" width="160"></vxe-column>
                    <vxe-column title="操作" align="center" fixed="right" width="60px">
                      <template #default="{ row }">
                        <label @click="noticeDetail(row)" class="text-eip eip-cursor-pointer">查看</label>
                      </template>
                    </vxe-column>
                  </vxe-table>
                </a-spin>
              </a-tab-pane>
            </a-tabs>

          </a-card>
        </a-col> -->
      </a-row>
    </div>
    <workflow-view ref="edit" v-if="workflowDo.visible" :visible.sync="workflowDo.visible" :title="workflowDo.title"
      :isWorkflow="true" :workflowData="workflowDo.data" @ok="initWorkflow"></workflow-view>

    <a-modal width="98%" :dialog-style="{ top: '20px' }" :footer="null" v-drag :visible="notice.detail.show"
      :destroyOnClose="true" :maskClosable="false" :title="notice.detail.title" @cancel="notice.detail.show = false">
      <a-spin :spinning="notice.detail.spinning">
        <div class="beauty-scroll" style="min-height: 500px;" v-html="notice.detail.msg"></div>
      </a-spin>
    </a-modal>
  </div>
</template>

<script>
import workflowView from "@/pages/system/agile/run/edit";
import {
  noticeQuery,
  noticeFindById,
  workflowNeedDo,
  workflowHaveDo,
  workflowHaveSend,
  workflowOverTime,
} from "@/services/dashboard/analysis/analysis";

import countTo from "vue-count-to";
export default {
  name: "Analysis",
  i18n: require("./i18n"),
  data() {
    return {
      organizations: [],
      height: this.eipHeaderHeight() - 332 + "px",
      antv: {
        height: 385,
      },
      updateLog: {
        loading: true,
        data: [],
      },
      needDo: {
        data: [],
        loading: true,
        pagerInfo: {
          recordCount: 0,
        },
      },
      haveDo: {
        data: [],
        loading: true,
        pagerInfo: {
          recordCount: 0,
        },
      },
      haveSend: {
        data: [],
        loading: true,
        pagerInfo: {
          recordCount: 0,
        },
      },
      overTime: {
        data: [],
        loading: true,
        pagerInfo: {
          recordCount: 0,
        },
      },

      numbers: [
        {
          icon: "tool",
          name: "待办事项",
          number: 0,
        },
        {
          icon: "solution",
          name: "已办事项",
          number: 0,
        },
        {
          icon: "user",
          name: "我的请求",
          number: 0,
        },
        {
          icon: "history",
          name: "已超时",
          number: 0,
        },
      ],
      loading: true,

      //编辑组件
      workflowDo: {
        visible: false,
        title: null,
        data: {
          processInstanceId: null,
          activityId: null,
          taskId: null,
        },
      },

      notice: {
        page: {
          param: {
            current: 1,
            size: 99999,
            sord: "asc",
            sidx: "OrderNo",
            filters: "",
          },
          total: 0,
          sizeOptions: this.eipPage.sizeOptions,
        },
        loading: true,
        data: [],
        detail: {
          show: false,
          title: "",
          msg: '',
          spinning: false
        }
      },
    };
  },
  mounted() {
    this.initWorkflow();
    this.initNotice()
  },
  components: {
    countTo,
    workflowView,
  },
  methods: {
    /**
     * 初始化通知
     */
    initNotice() {
      let that = this;
      that.notice.loading = true;
      noticeQuery(that.notice.page.param).then(function (result) {
        if (result.code == that.eipResultCode.success) {
          that.notice.data = result.data;
          that.notice.page.total = result.total;
        }
        that.notice.loading = false;
      });
    },

    /**
     * 通知详情
     */
    noticeDetail(row) {
      let that = this;
      that.notice.detail.spinning = true;
      that.notice.detail.show = true;
      that.notice.detail.title = row.title;
      noticeFindById(row.noticeId).then(function (result) {
        if (result.code == that.eipResultCode.success) {
          that.notice.detail.msg = result.data.msg;
        }
        that.notice.detail.spinning = false;
      });
    },


    /**
     * 发起流程
     */
    workflowCreate() {
      this.workflowDo.data = {
        processId: "b7f27eab-0bcd-2d13-264b-cbbc6df63566",
        processInstanceId: null,
        activityId: null,
        taskId: null,
        type: this.eipWorkflowDoType["审核"],
      };
      this.workflowDo.title = "公文发起";
      this.workflowDo.visible = true;
    },

    /**
     *
     * @param {*} row
     */
    workflowHaveDoClick(row) {
      this.workflowDo.title = "处理流程-" + row.title;
      this.workflowDo.data = {
        processId: null,
        processInstanceId: row.processInstanceId,
        activityId: null,
        taskId: null,
        type: this.eipWorkflowDoType["流程监控"],
      };
      this.workflowDo.visible = true;
    },

    /**
     * 查看流程
     * @param {*} row
     */
    workflowMonitorClick(row) {
      this.workflowDo.title = "流程查看-" + row.title;
      this.workflowDo.data = {
        processId: null,
        processInstanceId: row.processInstanceId,
        activityId: null,
        taskId: null,
        type: this.eipWorkflowDoType["流程监控"],
      };
      this.workflowDo.visible = true;
    },

    /**
     * 初始化流程数据
     */
    initWorkflow() {
      let that = this;
      this.needDo.loading = true;
      this.haveDo.loading = true;
      this.haveSend.loading = true;
      this.overTime.loading = true;
      workflowNeedDo({
        sidx: "ReceiveTime",
      }).then(function (result) {
        if (result.code == that.eipResultCode.success) {
          that.needDo = result.data;
          that.numbers[0].number = that.needDo.pagerInfo.recordCount;
          that.needDo.loading = false;
        }
      });

      workflowHaveDo({
        sidx: "CreateTime",
      }).then(function (result) {
        if (result.code == that.eipResultCode.success) {
          that.haveDo = result.data;
          that.numbers[1].number = that.haveDo.pagerInfo.recordCount;
          that.haveDo.loading = false;
        }
      });

      workflowHaveSend({
        sidx: "processInstance.CreateTime",
      }).then(function (result) {
        if (result.code == that.eipResultCode.success) {
          that.haveSend = result.data;
          that.numbers[2].number = that.haveSend.pagerInfo.recordCount;
          that.haveSend.loading = false;
        }
      });

      workflowOverTime({
        sidx: "task.ReceiveTime",
      }).then(function (result) {
        if (result.code == that.eipResultCode.success) {
          that.overTime = result.data;
          that.numbers[3].number = that.overTime.pagerInfo.recordCount;
          that.overTime.loading = false;
        }
      });
    },

    /**
     * 初始化数量
     */
    initNumber() {
      let that = this;
      number({}).then(function (result) {
        result.data.forEach((item, i) => {
          switch (i) {
            case 0:
              item.icon = "appstore";
              break;
            case 1:
              item.icon = "user";
              break;
            case 2:
              item.icon = "audit";
              break;
            case 3:
              item.icon = "car";
              break;
          }
        });
        that.numbers = result.data;
      });
    },

    /**
     * 待办点击
     */
    workflowNeedDoClick(row) {
      this.workflowDo.title = "处理流程-" + row.title;
      this.workflowDo.data = {
        processId: row.processId,
        processInstanceId: row.processInstanceId,
        activityId: row.activityId,
        taskId: row.taskId,
        type: this.eipWorkflowDoType["审核"],
      };
      this.workflowDo.visible = true;
      switch (row.activityType) {
        case 12: //知会处理
          this.workflowDo.data.type = this.eipWorkflowDoType["知会"];
          break;
        case 16: //加签处理
          this.workflowDo.data.type = this.eipWorkflowDoType["加签"];
          break;
        case 14: //阅示处理
          this.workflowDo.data.type = this.eipWorkflowDoType["阅示"];
          break;
        default:
      }
    },
  },
};
</script>

<style lang="less" scoped>
.extra-wrap {
  .extra-item {
    display: inline-block;
    margin-right: 24px;

    a:not(:first-child) {
      margin-left: 24px;
    }
  }
}

@media screen and (max-width: 992px) {
  .extra-wrap .extra-item {
    display: none;
  }
}

@media screen and (max-width: 576px) {
  .extra-wrap {
    display: none;
  }
}

.cardList-wrapper {
  .ant-row>div:nth-child(1) .card-item {
    background: linear-gradient(90deg, #5171fd, #c97afd);

    &:hover {
      box-shadow: 0 5px 10px #c97afd;
    }
  }

  .ant-row>div:nth-child(2) .card-item {
    background: linear-gradient(90deg, #3dadf6, #737bfc);

    &:hover {
      box-shadow: 0 5px 10px #737bfc;
    }
  }

  .ant-row>div:nth-child(3) .card-item {
    background: linear-gradient(90deg, #ea677c, #ef9b5f);

    &:hover {
      box-shadow: 0 5px 10px #ef9b5f;
    }
  }

  .ant-row>div:nth-child(4) .card-item {
    background: linear-gradient(90deg, #42d79b, #a6e25f);

    &:hover {
      box-shadow: 0 5px 10px #a6e25f;
    }
  }

  .ant-row>div:nth-child(5) .card-item {
    background-color: #feaa4f;
  }

  .ant-row>div:last-child .card-item {
    background-color: #9bc539;
  }

  .card-item {
    position: relative;
    overflow: hidden;
    box-sizing: border-box;
    height: 150px;
    padding: 15px 15px 0;
    border-radius: 6px;
    cursor: pointer;
    margin-bottom: 10px;
    box-shadow: 3px 5px 15px rgba(40, 40, 40, 0.36);

    .link-block-band {
      color: #fff;
      width: 100px;
      font-size: 14px;
      padding: 2px 0 3px 0;
      background-color: #e32a16;
      line-height: inherit;
      text-align: center;
      position: absolute;
      top: 8px;
      right: -30px;
      transform-origin: center;
      transform: rotate(45deg) scale(0.8);
      opacity: 0.95;
      z-index: 2;
    }

    &:hover {
      transform: scale(1.03);
      transition: all 0.1s ease-out;
    }

    .card-title {
      color: #fff;
      font-size: 1rem;
    }

    .all-count {
      color: #fff;
      font-size: 1.9rem;
      font-weight: bold;
      margin-top: 12px;
      padding-left: 25px;

      .suffix {
        font-size: 0.75rem;
      }
    }

    .card-info {
      color: #fff;
      font-size: 0.85rem;
      height: 46px;
      margin-top: 8px;

      .abs-bottom {
        position: absolute;
        bottom: 7px;
        left: 0;
      }

      .up {
        color: #1afa29;
      }

      .down {
        color: rgb(250, 30, 16);
      }
    }
  }

  .relative {
    position: relative;
  }
}

.ant-modal {
  width: 100vw !important;
  max-width: 100vw !important;
  top: 0;
  padding-bottom: 0;
}

.ant-modal-body {
  height: calc(100vh - 55px - 53px);
  overflow-y: auto;
}
</style>
