<template>
  <div class="analysis">
    <div style="text-align: center">
      <h2 style="
            margin-top: 170px;
            margin-bottom: 20px;
            font-size: 28px;
            color: #91addc;
          "></h2>
      <img src="/images/welcome.png" style="max-width: 100%" />
    </div>
  </div>
</template>

<script>
import workflowView from "@/pages/system/agile/run/edit";
import {
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
      height: this.eipHeaderHeight() - 232 + "px",
      antv: {
        height: 385,
      },
      updateLog: {
        loading: true,
        data: [],
      },
      needDo: {
        data: [],
        pagerInfo: {
          recordCount: 0,
        },
      },
      haveDo: {
        data: [],
        pagerInfo: {
          recordCount: 0,
        },
      },
      haveSend: {
        data: [],
        pagerInfo: {
          recordCount: 0,
        },
      },
      overTime: {
        data: [],
        pagerInfo: {
          recordCount: 0,
        },
      },

      numbers: [
        {
          icon: "tool",
          name: "待处理的公文",
          number: 0,
        },
        {
          icon: "solution",
          name: "已办的公文",
          number: 0,
        },
        {
          icon: "user",
          name: "发起的公文",
          number: 0,
        },
        {
          icon: "history",
          name: "已超时公文",
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
    };
  },
  mounted() {
    this.initWorkflow();
  },
  components: {
    countTo,
    workflowView,
  },
  methods: {

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
      workflowNeedDo({
        sidx: "ReceiveTime",
      }).then(function (result) {
        if (result.code == that.eipResultCode.success) {
          that.needDo = result.data;
          that.numbers[0].number = that.needDo.pagerInfo.recordCount;
        }
      });

      workflowHaveDo({
        sidx: "task.CompletedTime",
      }).then(function (result) {
        if (result.code == that.eipResultCode.success) {
          that.haveDo = result.data;
          that.numbers[1].number = that.haveDo.pagerInfo.recordCount;
        }
      });

      workflowHaveSend({
        sidx: "processInstance.CreateTime",
      }).then(function (result) {
        if (result.code == that.eipResultCode.success) {
          that.haveSend = result.data;
          that.numbers[2].number = that.haveSend.pagerInfo.recordCount;
        }
      });

      workflowOverTime({
        sidx: "task.ReceiveTime",
      }).then(function (result) {
        if (result.code == that.eipResultCode.success) {
          that.overTime = result.data;
          that.numbers[3].number = that.overTime.pagerInfo.recordCount;
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
        processId: null,
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

.ant-row>div:nth-child(1) .border {
  border-color: rgba(251, 98, 96, 1);
}

.ant-row>div:nth-child(2) .border {
  border-color: rgba(0, 153, 255, 1);
}

.ant-row>div:nth-child(3) .border {
  border-color: rgba(129, 103, 245, 1);
}

.ant-row>div:nth-child(4) .border {
  border-color: rgba(81, 211, 81, 1);
}

.ant-row>div:nth-child(5) .border {
  border-color: rgba(255, 169, 76, 1);
}

.ant-row>div:nth-child(6) .border {
  border-color: rgba(255, 122, 140, 1);
}

.card-item {
  font-family: "微软雅黑", sans-serif;
  font-weight: 400;
  font-style: normal;
  font-size: 14px;
  letter-spacing: normal;
  color: #666666;
  border-width: 0px;
  background-color: rgba(255, 255, 255, 1);
  height: 120px;
  width: 100%;

  .border {
    border-width: 0px;
    height: 120px;
    background: inherit;
    background-color: rgba(255, 255, 255, 1);
    box-sizing: border-box;
    border-width: 5px;
    border-style: solid;
    border-top: 0px;
    border-right: 0px;
    border-bottom: 0px;
    border-radius: 0px;
    border-top-left-radius: 0px;
    border-bottom-left-radius: 0px;
    -moz-box-shadow: 0px 0px 5px rgba(0, 0, 0, 0.0470588235294118);
    -webkit-box-shadow: 0px 0px 5px rgba(0, 0, 0, 0.0470588235294118);
    box-shadow: 0px 0px 5px rgba(0, 0, 0, 0.0470588235294118);
  }

  .text {
    padding: 15px 30px 0px 30px;
    box-sizing: border-box;
    width: 100%;
  }

  .title {
    font-family: "微软雅黑", sans-serif;
    font-weight: 400;
    color: #999999;
  }

  .number {
    font-family: "Montserrat Bold", "Montserrat", sans-serif;
    font-weight: 700;
    color: #333333;
  }
}

.relative {
  position: relative;
}

/deep/ .ant-col-lg-5 {
  width: 20%;
}
</style>
