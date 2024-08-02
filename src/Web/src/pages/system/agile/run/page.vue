<template>
  <div style="background-color: white" v-if="options">
    <div
      :class="options.showFormType == 'drawer' ? 'eip-drawer-body' : ''"
      style="overflow: hidden"
      :style="
        options.showFormType == 'drawer'
          ? { height: 'calc(100vh - 110px)' }
          : ''
      "
    >
      <a-spin :spinning="spinning">
        <a-config-provider>
          <div
            class="eip-form-designer-container"
            :style="options.showFormType != 'drawer' ? { height: '100%' } : ''"
          >
            <slot name="header"> </slot>
            <div class="content toolbars-top">
              <section
                :style="{
                  'max-width': drawerRightShow
                    ? 'calc(100% - 270px)'
                    : 'calc(100%)',
                  'max-height':
                    options.showFormType == 'drawer'
                      ? ''
                      : 'calc(100vh - 310px)',
                  'box-shadow': dataLog.length > 0 ? '' : 'none',
                }"
              >
                <a-skeleton :loading="skeleton">
                  <k-form-build
                    ref="kfb"
                    v-if="!skeleton"
                    :dynamicData="dynamicData"
                    :defaultValue="defaultValue"
                    :formValue="formValue"
                    :value="publicJson"
                    :disabled="disabled"
                    @change="handleChange"
                    @complete="kfbComplate"
                  />
                </a-skeleton>
              </section>
              <aside
                class="right"
                v-if="drawerRightShow"
                style="width: 400px; background: #fafafa"
              >
                <a-tabs default-active-key="2" style="margin: 0">
                  <!-- <a-tab-pane key="1" tab="流程日志">
                                        <eip-empty />
                                    </a-tab-pane> -->
                  <a-tab-pane key="2" tab="数据日志" force-render>
                    <eip-empty v-if="dataLog.length == 0" />
                    <div
                      class="worksheetRecordLog"
                      :style="{
                        'max-height':
                          options.showFormType == 'drawer'
                            ? ''
                            : 'calc(100vh - 360px)',
                      }"
                    >
                      <div class="logBox">
                        <div
                          class="worksheetRocordLogCard"
                          v-for="(item, index) in dataLog"
                          :key="index"
                        >
                          <div class="worksheetRocordLogCardTopBox">
                            <div class="worksheetRocordLogCardTitle flex">
                              <span class="selectTriggerChildAvatar"
                                ><span
                                  class="avatarBox worksheetRocordLogCardTitleAvatar"
                                  style="width: 20px; height: 20px"
                                ></span
                                ><span class="titleAvatarText accountName"
                                  ><img
                                    :src="item.createUserHeadImage"
                                    style="
                                      width: 20px;
                                      height: 20px;
                                      border-radius: 50%;
                                      margin-right: 2px;
                                    "
                                  />{{ item.createUserName }}</span
                                ></span
                              ><span
                                ><span class="Gray_9e">
                                  <span class="mLeft2" v-if="item.type == 2"
                                    >更新{{
                                      item.contentJson.length
                                    }}个字段</span
                                  >
                                  <span class="mLeft2" v-if="item.type == 1"
                                    >创建了记录</span
                                  ></span
                                ></span
                              >
                            </div>
                            <div
                              class="worksheetRocordLogCardName nowrap Gray_9e mLeft12"
                            >
                              {{ item.createTime }}
                            </div>
                          </div>
                          <div class="worksheetRocordLogCardHrCon">
                            <div
                              class="worksheet-rocord-log-item"
                              v-for="(
                                contentItem, contentIndex
                              ) in item.contentJson"
                              :key="contentIndex"
                            >
                              <div class="widgetTitle">
                                <span class="selectTriggerChild hasHover"
                                  ><span>{{ contentItem.Name }}</span></span
                                ><span class="extendText"></span>
                              </div>
                              <div
                                class="WorksheetRocordLogSelectTags paddingLeft27"
                              >
                                <span
                                  class="WorksheetRocordLogSelectTag oldValue"
                                  v-if="contentItem.Before"
                                >
                                  {{ contentItem.Before }}</span
                                ><span
                                  class="WorksheetRocordLogSelectTag newValue"
                                  v-if="contentItem.After"
                                >
                                  {{ contentItem.After }}</span
                                >
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>
                  </a-tab-pane>
                </a-tabs>
              </aside>
            </div>

            <div
              v-if="dataLog.length > 0"
              @click="logContract"
              :title="drawerRightShow ? '隐藏' : '显示'"
              class="menu-collapse"
              :style="{ right: drawerRightShow ? '408px' : '10px' }"
            >
              <div class="ico-button">
                <a-icon
                  style="font-size: 10px"
                  :type="drawerRightShow ? 'caret-right' : 'caret-left'"
                ></a-icon>
              </div>
            </div>
          </div>
        </a-config-provider>
      </a-spin>
    </div>
    <div class="eip-drawer-toolbar" v-if="complete && !disabled">
      <div class="flex justify-between align-center" v-if="!isWorkflow">
        <div>
          <a-checkbox
            v-if="
              !update &&
              buttons.filter(
                (f) => f.method == 'saveClose' && f.triggerType == 1
              ).length > 0
            "
            v-model="save.retain"
          >
            继续创建时，保留本次提交内容
          </a-checkbox>
        </div>
        <a-space>
          <a-button
            v-for="(bu, index) in buttons"
            :key="index"
            @click="buttonClick(bu)"
            :type="bu.style"
            ><a-icon :type="bu.icon" />{{ bu.name }}</a-button
          >

          <a-button
            type="primary"
            v-if="
              !update &&
              buttons.filter(
                (f) => f.method == 'saveClose' && f.triggerType == 1
              ).length > 0
            "
            key="submit"
            @click="saveContinue"
            :loading="loading"
            ><a-icon type="save" />提交并继续创建</a-button
          >
        </a-space>
      </div>

      <div class="flex justify-between align-center" v-else>
        <div></div>
        <a-space>
          <a-button key="back" @click="cancel" :disabled="loading">
            <a-icon type="close" />取消</a-button
          >

          <!-- 若为审批 -->
          <template v-if="workflowData.type == 1">
            <a-space>
              <template v-if="workflow.props.button.length <= 8">
                <a-tooltip
                  v-for="(item, index) in workflow.props.button"
                  :key="index"
                >
                  <template slot="title"
                    >{{ item.name }}：{{ item.remark }}
                  </template>

                  <a-button
                    @click="workflowButtonClick(item)"
                    :type="item.style"
                  >
                    <a-icon :type="item.icon" /> {{ item.name }}
                  </a-button>
                </a-tooltip></template
              >

              <template v-if="workflow.props.button.length > 8">
                <a-tooltip
                  v-for="(item, index) in workflow.props.button.slice(0, 8)"
                  :key="index"
                >
                  <template slot="title"
                    >{{ item.name }}：{{ item.remark }}
                  </template>

                  <a-button
                    @click="workflowButtonClick(item)"
                    :type="item.style"
                  >
                    <a-icon :type="item.icon" /> {{ item.name }}
                  </a-button>
                </a-tooltip>
              </template>

              <a-popover
                placement="topRight"
                title="更多操作按钮"
                v-if="workflow.props.button.length > 8"
              >
                <template slot="content">
                  <a-space>
                    <a-tooltip
                      v-for="(item, index) in workflow.props.button.slice(
                        8,
                        workflow.props.button.length
                      )"
                      :key="index"
                    >
                      <template slot="title"
                        >{{ item.name }}：{{ item.remark }}
                      </template>

                      <a-button
                        @click="workflowButtonClick(item)"
                        :type="item.style"
                      >
                        <a-icon :type="item.icon" /> {{ item.name }}
                      </a-button>
                    </a-tooltip>
                  </a-space>
                </template>
                <a-button type="primary"
                  ><a-icon type="caret-up" /> 更多
                </a-button>
              </a-popover>
            </a-space>
          </template>

          <!-- 知会处理 -->
          <template v-if="workflowData.type == 2">
            <a-space>
              <a-button type="primary" @click="workflowUnderStandingRead"
                ><a-icon type="container" />已阅</a-button
              >
              <a-button
                type="primary"
                @click="workflowButtonClick({ script: 'flowUnderstanding();' })"
                ><a-icon type="reconciliation" />知会</a-button
              >
            </a-space>
          </template>

          <!-- 加签 -->
          <template v-if="workflowData.type == 3">
            <a-space>
              <a-button type="primary" @click="flowAddApprove"
                ><a-icon type="caret-right" />提交</a-button
              >
              <a-button
                type="primary"
                @click="workflowButtonClick({ script: 'flowAdd();' })"
                ><a-icon type="file-add" />加签</a-button
              >
              <a-button
                type="primary"
                @click="workflowButtonClick({ script: 'flowUnderstanding();' })"
                ><a-icon type="notification" />知会</a-button
              >
              <a-button
                type="primary"
                @click="workflowButtonClick({ script: 'flowDoList();' })"
                ><a-icon type="calendar" />过程列表</a-button
              >
              <a-button
                type="primary"
                @click="workflowButtonClick({ script: 'flowDesigner();' })"
                ><a-icon type="control" />流程图</a-button
              >
            </a-space>
          </template>

          <!-- 阅示处理 -->
          <template v-if="workflowData.type == 4">
            <a-space>
              <a-button
                type="primary"
                @click="
                  workflowButtonClick({
                    script: 'flowInvitationReadSure();',
                  })
                "
                ><a-icon type="container" />已阅</a-button
              >
              <a-button
                type="primary"
                @click="workflowButtonClick({ script: 'flowUnderstanding();' })"
                ><a-icon type="reconciliation" />知会</a-button
              >
            </a-space>
          </template>

          <!-- 流程监控 -->
          <template v-if="workflowData.type == 5">
            <a-space>
              <a-button
                type="primary"
                @click="workflowButtonClick({ script: 'flowDoList();' })"
                ><a-icon type="calendar" />过程列表</a-button
              >
              <a-button
                type="primary"
                @click="workflowButtonClick({ script: 'flowDesigner();' })"
                ><a-icon type="control" />流程图</a-button
              >
            </a-space>
          </template>

          <!--  阅示审批处理,有通过和拒绝按钮 -->
          <template v-if="workflowData.type == 6">
            <a-space>
              <a-button
                type="primary"
                @click="workflowButtonClick({ script: 'flowDoList();' })"
                ><a-icon type="calendar" />过程列表</a-button
              >
              <a-button
                type="primary"
                @click="workflowButtonClick({ script: 'flowDesigner();' })"
                ><a-icon type="control" />流程图</a-button
              >

              <a-button
                v-if="workflow.task.status == 30"
                type="primary"
                @click="workflowInvitationReadApprovePass"
                ><a-icon type="check-circle" />通过</a-button
              >

              <a-button
                v-if="workflow.task.status == 30"
                type="primary"
                @click="workflowInvitationReadApproveRefuse"
                ><a-icon type="close-circle" />拒绝</a-button
              >
            </a-space>
          </template>
        </a-space>

        <workflowHandleStart
          v-if="workflow.start.visible"
          :visible.sync="workflow.start.visible"
          :notice="workflow.sure.notice"
          :person="workflow.chosenPerson.data"
          @confirm="workflowNextStartNotice"
        >
        </workflowHandleStart>

        <workflowHandleRefuse
          v-if="workflow.refuse.visible"
          :visible.sync="workflow.refuse.visible"
          @confirm="workflowRefuse"
        >
        </workflowHandleRefuse>

        <workflowHandleSure
          v-if="workflow.sure.visible"
          :taskId="workflowData.taskId"
          :visible.sync="workflow.sure.visible"
          :notice="workflow.sure.notice"
          :person="workflow.chosenPerson.data"
          @confirm="workflowTaskProcessSureComment"
        >
        </workflowHandleSure>

        <workflowHandleEnd
          v-if="workflow.end.visible"
          :visible.sync="workflow.end.visible"
          @confirm="workflowTaskProcessEndComment"
        >
        </workflowHandleEnd>

        <workflowHandleReturn
          v-if="workflow.return.visible"
          :visible.sync="workflow.return.visible"
          :processInstanceId="workflow.form.processInstanceId"
          :taskId="workflowData.taskId"
          @confirm="workflowReturn"
        >
        </workflowHandleReturn>

        <workflowHandleReturnAndWrite
          v-if="workflow.returnAndWrite.visible"
          :visible.sync="workflow.returnAndWrite.visible"
          @confirm="workflowReturnAndWrite"
        >
        </workflowHandleReturnAndWrite>

        <workflowHandleUnderStandingStart
          v-if="workflow.understandingStart.visible"
          :visible.sync="workflow.understandingStart.visible"
          @confirm="workflowUnderStandingStart"
        >
        </workflowHandleUnderStandingStart>

        <workflowHandleUnderStanding
          v-if="workflow.understanding.visible"
          :visible.sync="workflow.understanding.visible"
          @confirm="workflowUnderStanding"
        >
        </workflowHandleUnderStanding>

        <workflowHandleInvitationRead
          v-if="workflow.invitationread.visible"
          :visible.sync="workflow.invitationread.visible"
          @confirm="workflowInvitationRead"
        >
        </workflowHandleInvitationRead>

        <workflowHandleInvitationReadSure
          v-if="workflow.invitationreadSure.visible"
          :visible.sync="workflow.invitationreadSure.visible"
          :taskId="workflowData.taskId"
          @confirm="workflowInvitationReadSure"
        >
        </workflowHandleInvitationReadSure>

        <workflowHandleInvitationReadApprove
          v-if="workflow.invitationreadApprove.visible"
          :visible.sync="workflow.invitationreadApprove.visible"
          :taskId="workflowData.taskId"
          @confirm="workflowInvitationReadApprove"
        >
        </workflowHandleInvitationReadApprove>

        <workflowHandleRevoke
          v-if="workflow.revoke.visible"
          :visible.sync="workflow.revoke.visible"
          @confirm="workflowRevoke"
        >
        </workflowHandleRevoke>

        <workflowHandleDeleteTask
          v-if="workflow.deleteTask.visible"
          :visible.sync="workflow.deleteTask.visible"
          @confirm="workflowDeleteTask"
        >
        </workflowHandleDeleteTask>

        <workflowHandleAdd
          v-if="workflow.add.visible"
          :visible.sync="workflow.add.visible"
          @confirm="flowAdd"
        >
        </workflowHandleAdd>

        <workflowHandleDoList
          v-if="workflow.doList.visible"
          :visible.sync="workflow.doList.visible"
          :processInstanceId="workflow.form.processInstanceId"
        >
        </workflowHandleDoList>

        <workflowDesigner
          v-if="workflow.designer.visible"
          :visible.sync="workflow.designer.visible"
          title="流程监控"
          :processInstanceId="workflow.form.processInstanceId"
        ></workflowDesigner>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from "vuex";
import moment from "moment";
import {
  businessData,
  businessDataById,
  businessDataBatch,
  businessDataFormSource,
  eventApi,
  fileCorrelationId,
  agileDataLogRelationId,
  menuAgile,
  serialNoCreate,
  dictionaryQueryByIds,
  dictionaryParentId,
} from "@/services/system/agile/run/edit";
import { queryTable } from "@/services/system/agile/run/list";
import { newGuid } from "@/utils/util";
import { getAuthorization } from "@/utils/request";
import {
  workflowEngineActivityStart,
  workflowStart,
  workflowStartRun,
  workflowTaskProcess,
  workflowTaskProcessRun,
  workflowInstanceProcess,
  workflowEngineActivityByTaskId,
  workflowEngineTaskById,
  workflowEngineRefuse,
  workflowEngineReturnAndWrite,
  workflowEngineUnderStanding,
  workflowEngineUnderStandingRead,
  workflowEngineDraftSave,
  workflowEngineModelSave,
  workflowEngineInvitationRead,
  workflowEngineInvitationReadSure,
  workflowEngineInvitationReadApprove,
  workflowEngineInvitationReadApprovePass,
  workflowEngineInvitationReadApproveRefuse,
  workflowEngineRevoke,
  workflowEngineAdd,
  workflowEngineAddApprove,
  workflowEngineDeleteTask,
  workflowEngineDetail,
  workflowEngineEnd,
  workflowEngineEvent,
} from "@/services/workflow/send/index";
import workflowHandleRefuse from "@/pages/workflow/handle/refuse";
import workflowHandleDoList from "@/pages/workflow/handle/doList";
import workflowHandleReturnAndWrite from "@/pages/workflow/handle/returnAndWrite";
import workflowHandleReturn from "@/pages/workflow/handle/return";
import workflowHandleUnderStandingStart from "@/pages/workflow/handle/underStandingStart";
import workflowHandleUnderStanding from "@/pages/workflow/handle/understanding";
import workflowHandleInvitationRead from "@/pages/workflow/handle/invitationread";
import workflowHandleInvitationReadSure from "@/pages/workflow/handle/invitationreadSure";
import workflowHandleInvitationReadApprove from "@/pages/workflow/handle/invitationreadApprove";
import workflowHandleRevoke from "@/pages/workflow/handle/revoke";
import workflowHandleSure from "@/pages/workflow/handle/sure";
import workflowHandleEnd from "@/pages/workflow/handle/end";
import workflowHandleStart from "@/pages/workflow/handle/start";
import workflowHandleAdd from "@/pages/workflow/handle/add";
import workflowHandleDeleteTask from "@/pages/workflow/handle/delete";
import workflowDesigner from "@/pages/workflow/process/designer";

export default {
  components: {
    workflowHandleInvitationRead,
    workflowHandleInvitationReadSure,
    workflowHandleInvitationReadApprove,
    workflowHandleRefuse,
    workflowHandleDoList,
    workflowHandleReturnAndWrite,
    workflowHandleReturn,
    workflowHandleUnderStandingStart,
    workflowHandleUnderStanding,
    workflowDesigner,
    workflowHandleRevoke,
    workflowHandleAdd,
    workflowHandleSure,
    workflowHandleStart,
    workflowHandleDeleteTask,
    workflowHandleEnd,
  },
  name: "edit",
  computed: {
    ...mapGetters("account", ["user", "systemAgile"]),
  },
  data() {
    return {
      drawerRightShow: false,
      relationTxt: "_Txt",

      modal: {
        dialogStyle: {},
        centered: false,
        width: "400px",
        formWidth: "850px",
        bodyStyle: {
          padding: "1px",
        },
        button: {
          submit: {
            name: "保存",
            success: 0,
          },
        },
      },
      dynamicData: {}, //数据

      submitForm: {
        configId: null,
        relationId: newGuid(),
      },
      publicJson: {}, //当前表单配置
      columnJson: [], //表单项
      defaultValue: {},
      formValue: {}, //表单值

      spinning: false, //等待框loading
      loading: false, //按钮loading
      skeleton: false,

      editConfig: {
        formJson: "",
        columnJson: {},
      },
      save: {
        continue: false,
        retain: false, //提交是否保留本次内容
      },

      buttons: [], //底部按钮
      complete: false, //是否加载完成

      dataLog: [], //数据日志

      workflow: {
        form: {
          extend: null,
          title: "",
          processId: this.workflowData.processId,
          processInstanceId: "",
          activityId: "", //开始流程节点
          formId: "", //表单Id
          json: "", //Json
          publicJson: "",
          activityType: "", //当前活动类型
          urgency: 0,
          comment: "", //处理意见
          notice: "", //通知数组
          taskId: null, //当前任务Id
        },

        rules: {
          title: [
            {
              required: true,
              message: "请输入流程标题",
            },
          ],
          comment: [
            {
              required: true,
              message: "请输入处理意见",
            },
          ],
        },

        currentActivity: {
          formId: null,
          publicJson: null,
          json: null,
          activityId: null,
          type: null,
        }, //当前活动

        submitValue: {}, //提交数据

        props: {
          button: [],
          column: [],
          notice: [],
          event: [],
          base: {},
        },

        comment: [], //流程历史意见

        chosenPerson: {
          //流程处理人
          visible: false,
          data: [],
          value: null,
        },

        //弹出框控制属性
        refuse: {
          visible: false,
        },
        sure: {
          visible: false,
          notice: [],
          noticeChosen: [],
        },
        start: {
          visible: false,
          notice: [],
        },
        returnAndWrite: {
          visible: false,
        },
        doList: {
          visible: false,
        },

        return: {
          visible: false,
        },

        designer: {
          visible: false,
        },

        understandingStart: {
          visible: false,
        },

        understanding: {
          visible: false,
        },
        invitationread: {
          visible: false,
        },
        invitationreadSure: {
          visible: false,
        },
        invitationreadApprove: {
          visible: false,
          comment: null,
        },
        end: {
          visible: false,
          comment: null,
        },
        revoke: {
          visible: false,
        },
        add: {
          visible: false,
        },

        deleteTask: {
          visible: false,
        },

        draft: {
          title: "草稿箱",
        },

        model: {
          title: "范本夹",
        },

        //当前任务信息
        task: {
          activityId: undefined,
          activityType: undefined,
          comment: undefined,
          completedTime: undefined,
          customerData: undefined,
          doUserId: undefined,
          doUserName: undefined,
          extend: undefined,
          prevTaskId: undefined,
          processInstanceId: undefined,
          receiveTime: undefined,
          receiveUserId: undefined,
          receiveUserName: undefined,
          remark: undefined,
          sendUserId: undefined,
          sendUserName: undefined,
          status: undefined,
          taskId: undefined,

          files: [],
        },

        duBanHistory: [],
      },
    };
  },

  props: {
    /**
     * 表单配置属性
     */
    options: {
      type: Object,
    },

    /**
     * 配置
     */
    config: {
      type: Object,
      default: () => {
        return {
          configId: undefined, //配置id
        };
      },
    },

    //更新信息
    update: {
      type: Object,
      default: () => {
        return {
          RelationId: undefined, //配置id
        };
      },
    },

    //编辑信息,如记录IDId
    copy: {
      type: Boolean,
      default: false,
    },

    //禁用
    disabled: {
      type: Boolean,
      default: false,
    },

    //标题
    title: {
      type: String,
    },

    //自定义表单数据
    customerFormValue: {
      type: Object,
    },

    //来源类型:可用于通过不同来源类型,处理不同业务逻辑（用于自定义脚本判断）
    fromType: {
      type: String,
    },

    //自动化编辑字段信息
    automation: {
      type: Object,
    },

    //是否为工作流
    isWorkflow: {
      type: Boolean,
      default: false,
    },

    //流程数据
    workflowData: {
      type: Object,
      default: () => {
        return {
          processId: undefined, //流程实例Id,发起流程时用
          processInstanceId: undefined,
          taskId: undefined,
          activityId: undefined,
          type: 1, //处理类型1审核，2知会，3加签，4阅示，5流程监控，6阅示审批处理,有通过和拒绝按钮
        };
      },
    },
  },

  mounted() {
    if (this.isWorkflow) {
      this.initWorkflow();
    } else {
      this.init();
    }
  },
  methods: {
    /**
     * 日志
     */
    logContract() {
      this.drawerRightShow = !this.drawerRightShow;
      this.$emit("logContract", this.drawerRightShow);
    },

    /**
     * 初始化设计器
     */
    async initWorkflow() {
      let that = this;
      that.spinning = true;
      that.skeleton = true;
      if (that.workflowData.processInstanceId) {
        that.workflow.form.processInstanceId =
          that.workflowData.processInstanceId;
        that.update.RelationId = that.workflowData.processInstanceId;
      }
      var result;
      //是否具有流程任务Id
      if (this.workflowData.taskId) {
        result = await workflowEngineActivityByTaskId(that.workflowData.taskId);
        if (result.code == that.eipResultCode.success) {
          let data = result.data;
          if (data == null || data.length == 0) {
            that.$message.error("任务节点不存在,请确认是否被删除!!!");
            that.cancel();
          } else {
            that.workflow.currentActivity = data;
            if (that.workflow.currentActivity.json) {
              that.workflow.props = JSON.parse(
                that.workflow.currentActivity.json
              ).props;

              that.workflow.props.button = that.workflow.props.button.filter(
                (f) => f.showPc
              );
            }
            that.config.editConfigId = that.workflow.currentActivity.formId;
            that.workflow.currentActivity.type = "task";
            that.workflowCommentHistory();
          }
        }
      } else {
        if (that.workflowData.processInstanceId) {
          result = await workflowEngineDetail({
            id: that.workflowData.processInstanceId,
          });
          if (result.code == that.eipResultCode.success) {
            let data = result.data;
            if (data == null || data.length == 0) {
              that.$message.error("任务节点不存在,请确认是否被删除!!!");
              that.cancel();
            } else {
              that.workflow.currentActivity = data;
              if (that.workflow.currentActivity.json) {
                that.workflow.props = JSON.parse(
                  that.workflow.currentActivity.json
                ).props;
                that.workflow.props.button = that.workflow.props.button.filter(
                  (f) => f.showPc
                );
              }
              that.config.editConfigId = that.workflow.currentActivity.formId;
              that.workflowCommentHistory();
            }
          }
        } else {
          //开始节点
          result = await workflowEngineActivityStart(
            that.workflowData.processId
          );
          if (result.code == that.eipResultCode.success) {
            let data = result.data;
            if (data == null || data.length == 0) {
              that.$message.error("未发现流程开始节点,请完善流程设置!!!");
              that.cancel();
            } else {
              that.workflow.currentActivity = data[0];
              if (that.workflow.currentActivity.json) {
                that.workflow.props = JSON.parse(
                  that.workflow.currentActivity.json
                ).props;
                that.workflow.props.button = that.workflow.props.button.filter(
                  (f) => f.showPc
                );
              }
              that.config.editConfigId = that.workflow.currentActivity.formId;
            }
          }
        }
      }

      if (!that.config.editConfigId) {
        that.$message.error("请确认已配置开始节点流程表单!!!");
        that.cancel();
      }
      that.submitForm.configId = that.config.editConfigId;

      var systemAgile = that.systemAgile.filter(
        (f) => f.configId == that.submitForm.configId
      );
      if (systemAgile && systemAgile.length > 0) {
        that.editConfig = systemAgile[0];
      } else {
        systemAgile = await menuAgile({ configId: that.submitForm.configId });
        if (systemAgile.code === that.eipResultCode.success) {
          that.editConfig = systemAgile.data[0];
        } else {
          that.$message.error("获取配置错误");
        }
      }

      if (that.editConfig.columnJson) {
        that.columnJson = JSON.parse(that.editConfig.columnJson);
      }
      var publicJson = JSON.parse(that.editConfig.publicJson);
      that.publicJson = publicJson;
      await that.initDynamicData();
      that.initUploadConfig();
      that.skeleton = false;

      that.workflowInit();
    },

    /**
     * 按钮点击
     */
    buttonClick(item) {
      let that = this;
      if (item.triggerType == 2) {
        //动态执行script
        eval(item.script);
      } else if (item.triggerType == 3) {
        //触发接口
        var apiOption = JSON.parse(item.api);
        if (apiOption.confirmType == 1) {
          //执行接口
          that.buttonApiDo(apiOption, []);
        }
      } else {
        switch (item.method) {
          case "saveClose": //新增
            this.saveClose();
            break;
          case "cancel": //编辑
            this.cancel();
            break;
          default:
            break;
        }
      }
    },

    /**
     * 取消
     */
    cancel() {
      this.$emit("cancel");
    },

    /**
     * 所有控件值改变事件
     */
    handleChange(value, key) {
      var that = this;
      var data = { value, key };
      that.formValue[data.key] = data.value;
      //查询与当前改变组件相关组件
      that.setMap(data);
      //执行改变事件
      that.checkEventPcChange(value, key);
      //清空关联数据
      that.checkRelationData(data.key);
      //根据传入的关键字获取关联数据
      that.checkReloadSource(data.key);
      //动态变化显示值
      that.checkDataShow(data.key);
    },

    /**
     * 事件触发
     */
    checkEventPcChange(value, key) {
      var that = this;
      let publicJson = this.publicJson;
      if (publicJson.config.event.pc.change) {
        eval(publicJson.config.event.pc.change);
      }
    },

    /**
     *处理映射
     */
    setMap(data) {
      let that = this;
      var columns = this.columnJson.filter((f) => f.model == data.key);
      var options = this.$refs.kfb.getOptions();
      if (columns.length > 0) {
        var column = columns[0];
        var mapColumns = null;
        if (column.type == "dataList") {
          mapColumns = column.options.map.filter((f) => f.mapcolumn != "");
          mapColumns.forEach((item) => {
            var values = options.filter((f) => f.model == data.key)[0].options
              .selectRows;
            var selectRowsValue = [];
            values.forEach((v) => {
              selectRowsValue.push(v[item.name]);
            });
            that.formValue[item.mapcolumn] = selectRowsValue.join(",");

            var option = that.getOptionByModel(item.mapcolumn);
            if (option != null && !option.hidden) {
              this.$refs.kfb.setData({
                [item.mapcolumn]: selectRowsValue.join(","),
              });
            }
          });
        } else if (column.type == "map") {
          mapColumns = column.options.map.filter((f) => f.mapcolumn != "");
          mapColumns.forEach((item) => {
            var value = "";
            if (data.value != "") {
              var dataJson = JSON.parse(data.value);
              if (item.name == "lng") {
                value = dataJson.lng;
              }
              if (item.name == "lat") {
                value = dataJson.lat;
              }
              if (item.name == "address") {
                value = dataJson.address;
              }
            }

            that.formValue[item.mapcolumn] = value;
            var option = that.getOptionByModel(item.mapcolumn);
            if (option != null && !option.hidden) {
              that.$refs.kfb.setData({
                [item.mapcolumn]: value,
              });
            }
          });
        } else if (column.type == "select" && column.options.dynamic) {
          mapColumns = column.options.dynamicConfig.map.filter(
            (f) => f.mapcolumn != ""
          );
          mapColumns.forEach((item) => {
            // 映射类型
            var values = that.dynamicData[column.model].filter(
              (f) => f[column.options.dynamicConfig.key] == data.value
            );
            var selectRowsValue = [];
            values.forEach((v) => {
              selectRowsValue.push(v[item.name]);
            });
            that.formValue[item.mapcolumn] = selectRowsValue.join(",");
            this.$refs.kfb.setData({
              [item.mapcolumn]: selectRowsValue.join(","),
            });
          });
        } else if (column.type == "correlationRecord") {
        }
      }
    },

    /**
     * 清空关联数据
     */
    checkRelationData(key) {
      var that = this;
      var columns = this.$utils.clone(that.columnJson, true);
      var column = this.$utils.find(columns, (f) => f.model == key);
      //下拉框
      var dynamics = columns.filter(
        (f) =>
          f.options.dynamic &&
          f.options.dynamicConfig &&
          f.options.dynamicConfig.dataSourceId &&
          f.options.dynamicConfig.inParams &&
          f.options.dynamicConfig.inParams.filter((c) => c.condition == key)
            .length > 0
      );
      if (dynamics.length > 0) {
        dynamics.forEach((item) => {
          that.clearRelationData(item);
        });
      }

      //工作表
      var correlationRecords = columns.filter(
        (f) => f.type == "correlationRecord"
      );
      correlationRecords.forEach((item) => {
        var filters = JSON.stringify(item.options.filter);
        if (filters.includes(column.key)) {
          that.clearRelationData(item);
        }
      });
    },

    /**
     * 清空关联数据
     */
    clearRelationData(item) {
      this.$refs.kfb.setData({
        [item.model]: undefined,
      });
      this.checkRelationData(item.model);
    },

    /**
     * 根据传入的关键字获取关联数据
     * @param {*} key
     */
    checkReloadSource(key) {
      var that = this;
      var columns = this.$utils.clone(that.columnJson, true);
      var column = this.$utils.find(columns, (f) => f.model == key);
      var dynamics = columns.filter(
        (f) =>
          (f.options.dynamic &&
            f.options.dynamicConfig &&
            f.options.dynamicConfig.dataSourceId &&
            f.options.dynamicConfig.inParams &&
            f.options.dynamicConfig.inParams.filter((c) => c.condition == key)
              .length > 0) ||
          (f.type == "correlationRecord" &&
            JSON.stringify(f.options.filter).includes(column.key))
      );
      if (dynamics.length > 0) {
        dynamics.forEach((item) => {
          that.reloadSource(item);
        });
      }
    },

    /**
     * 加载数据
     */
    async reloadSource(item) {
      var that = this;
      that.setDynamicData(item);
      that.checkReloadSource(item.model);
    },

    /**
     * 初始化设计器
     */
    async init() {
      let that = this;
      that.spinning = true;
      that.skeleton = true;
      that.submitForm.configId = that.config.editConfigId;
      var systemAgileData = that.systemAgile.filter(
        (f) => f.configId == that.submitForm.configId
      );
      if (systemAgileData && systemAgileData.length > 0) {
        that.editConfig = systemAgileData[0];
      } else {
        systemAgileData = await menuAgile({
          configId: that.submitForm.configId,
        });
        if (systemAgileData.code === that.eipResultCode.success) {
          that.editConfig = systemAgileData.data[0];
        } else {
          that.$message.error("获取配置错误");
        }
      }

      if (that.editConfig.publicJson) {
        this.initColumnJson();
        this.initPublicJson();
        await that.initDynamicData();
        that.initUploadConfig();
      }

      that.skeleton = false;
      that.spinning = false;
    },

    /**
     * 初始化字段Json
     */
    initColumnJson() {
      let that = this;
      if (that.editConfig.columnJson) {
        that.columnJson = JSON.parse(that.editConfig.columnJson);
      }
      if (that.automation) {
        let column = [];
        that.columnJson.forEach((item) => {
          //判断是否具有
          var have = that.$utils.find(
            that.automation.editColumn,
            (f) => f.column == item.model
          );
          if (have) {
            column.push(item);
          }
        });
        that.columnJson = column;
      }
    },

    /**
     * 初始化自动化编辑页面,若有则会覆盖对应字段
     */
    initPublicJson() {
      let that = this;
      var publicJson = JSON.parse(that.editConfig.publicJson);
      if (that.automation) {
        var list = [];
        publicJson.list.forEach((item) => {
          var have = that.$utils.find(
            that.automation.editColumn,
            (f) => f.column == item.model
          );
          if (have) {
            if (have.type == "readonly") {
              item.options.disabled = true;
            } else if (have.type == "write") {
              item.options.disabled = false;
            } else if (have.type == "writeMust") {
              item.options.disabled = false;
              item.rules.push({
                required: true,
                message: "必填项",
              });
            }

            //默认值判断
            if (have.default) {
              item.options.defaultValue = have.default;
            }
            list.push(item);
          }
        });
        publicJson.list = list;
      }
      that.publicJson = publicJson;
      that.buttons = publicJson.config.buttons;
    },

    /**
     * 初始化文件上传配置
     */
    initUploadConfig() {
      let that = this;
      var uploadFile = this.getOptionsByType(["uploadFile", "uploadImg"]);
      uploadFile.forEach((item) => {
        if (!item.options.action) {
          item.options.action =
            window.SITE_CONFIG.eip.baseURL + "/system/file/uploadform";
        }

        item.options.data = JSON.stringify({
          correlationId:
            that.submitForm.relationId +
            (item.options.relationModel ? "|" + item.model : ""),
          single: item.options.limit == 1,
          pdfShow: item.options.pdfShow,
        });
        var authorization = getAuthorization();
        item.options.headers = {
          Authorization: authorization,
        };
      });
    },

    /**
     * 初始化数据源数据
     */
    async initDynamicData() {
      let that = this;
      var dynamicData = {};
      that.columnJson.forEach((item) => {
        if (item.options.dynamic) {
          dynamicData[item.model] = [];
        }
      });

      that.dynamicData = dynamicData;

      //若有自定义数据则已自定义数据为准
      if (that.customerFormValue) {
        that.defaultValue = that.customerFormValue;
        //编辑则重新赋值
        if (that.update && that.update.RelationId) {
          await that.setFormValue({
            code: that.eipResultCode.success,
            data: that.customerFormValue,
          });
        }
      } else {
        if (that.update && that.update.RelationId) {
          await that.find();
        } else {
          var value = {};
          await that.setDefaultValue(value);
          that.defaultValue = that.$utils.clone(value);
          that.formValue = value;
          await that.getDynamicData();
        }
      }
    },

    /**
     * 获取数据
     */
    async getDynamicData() {
      let that = this;
      var columnsJson = this.$utils.clone(that.columnJson, true);
      var columns = columnsJson.filter(
        (f) =>
          (f.options.dynamic &&
            f.options.dynamicConfig &&
            f.options.dynamicConfig.dataSourceId) ||
          (f.type == "correlationRecord" && f.options.showType == "select") ||
          f.type == "district" ||
          f.type == "dictionary"
      );
      for (let index = 0; index < columns.length; index++) {
        that.setDynamicData(columns[index]);
      }

      //查询子表
      var batchColumns = columnsJson.filter((f) => f.type == "batch");
      for (let index = 0; index < batchColumns.length; index++) {
        const batchColumn = batchColumns[index];
        var columnsList = batchColumn.list.filter(
          (f) =>
            f.options.dynamic &&
            f.options.dynamicConfig &&
            f.options.dynamicConfig.dataSourceId
        );
        for (let indexc = 0; indexc < columnsList.length; indexc++) {
          const item = columnsList[indexc];
          //查看映射数据
          item.modelBatch = batchColumn.model + "." + item.model;
          that.setDynamicData(item);
        }
      }
    },

    /**
     * 加载数据
     * @param {*} column
     */
    async setDynamicData(column) {
      let that = this;
      if (
        column.type == "correlationRecord" &&
        column.options.showType == "select"
      ) {
        let sidx = [];
        let columns = [];
        column.options.columns.forEach((item) => {
          if (item.isShow) {
            var column = {
              field: item.name,
              title: item.title,
              align: item.align,
              sort: item.isSort,
              width: item.width,
              style: item.style,
              format: item.format,
              mask: item.mask,
            };
            columns.push(column);
          }
          if (item.sord) {
            sidx.push(item.name + " " + item.sordType);
          }
        });
        var filters = that.analysisFilter(column.options.filter);
        queryTable({
          table: column.options.dynamicConfig.table,
          columns: JSON.stringify(columns),
          formValue: that.formValue,
          sidx: sidx.join(","),
          filters: JSON.stringify(filters),
        }).then((resultTable) => {
          let dynamicData = [];
          resultTable.data.data.forEach((res) => {
            var labels = [];
            columns.forEach((col) => {
              labels.push(res[col.field]);
            });
            dynamicData.push({
              label: labels.join(" | "),
              value: res["RelationId"],
              title: columns.length > 0 ? res[columns[0].field] : null,
            });
          });
          that.kfbSetOptions([column.model], dynamicData);
          that.dynamicData[column.model] = dynamicData;
        });
      } else if (column.type == "dictionary") {
        switch (column.options.dictionaryLevel) {
          case 1:
            dictionaryParentId(column.options.dictionaryId).then((result) => {
              var dictionaryParentResult = result.data;
              var resultDynamic = [];
              dictionaryParentResult.forEach((f) => {
                resultDynamic.push({
                  label: f.name,
                  value: f.dictionaryId,
                });
              });
              that.kfbSetOptions([column.model], resultDynamic);
              that.dynamicData[column.model] = resultDynamic;
            });
            break;
          case 2:
            dictionaryQueryByIds({
              id: column.options.dictionaryId,
            }).then((result) => {
              result = result.data;
              that.kfbSetOptions([column.model], result);
              that.dynamicData[column.model] = result;
            });
            break;
          default:
            break;
        }
      } else if (column.type == "district") {
        let jsonPath = "/data/county.json";
        switch (column.options.levelType) {
          case 1:
            jsonPath = "/data/province.json";
            break;
          case 2:
            jsonPath = "/data/city.json";
            break;
          case 3:
            jsonPath = "/data/county.json";
            break;
          case 4:
            jsonPath = "/data/town.json";
            break;
          default:
            jsonPath = "/data/all.json";
            break;
        }
        var data = await fetch(jsonPath);
        data.json().then((treeData) => {
          var dynamicData = that.setChildrenTreeData(
            that.$utils.toArrayTree(treeData, {
              key: "value",
            })
          );
          that.kfbSetOptions([column.model], dynamicData);
          that.dynamicData[column.model] = dynamicData;
        });
      } else {
        var fields = [];
        //查看映射数据
        var maps = column.options.dynamicConfig.map.filter(
          (f) => f.mapcolumn != ""
        );
        if (maps.length > 0) {
          maps.forEach((item) => {
            fields.push(item.name);
          });
        }
        that.formValue.RelationId = that.submitForm.relationId;
        businessDataFormSource({
          dataSourceId: column.options.dynamicConfig.dataSourceId,
          inParams: JSON.stringify(
            that.convertInParams(column.options.dynamicConfig.inParams)
          ),
          formValue: JSON.stringify(that.formValue),
          sidx: column.options.dynamicConfig.sidx, //排序字段
          sord: column.options.dynamicConfig.sord, //排序方式
        }).then((result) => {
          if (result.code === that.eipResultCode.success) {
            let dynamicData = [];
            if (column.type == "cascader") {
              let tree = [];
              result.data.forEach((res) => {
                tree.push({
                  value: res[column.options.dynamicConfig.key],
                  label: res[column.options.dynamicConfig.value].toString(),
                  parentId: res[column.options.dynamicConfig.parentId],
                });
              });
              dynamicData = that.setChildrenTreeData(
                that.$utils.toArrayTree(tree, {
                  key: "value",
                })
              );
            } else if (column.type == "treeSelect") {
              let tree = [];
              result.data.forEach((res) => {
                tree.push({
                  value: res[column.options.dynamicConfig.key].toString(),
                  key: res[column.options.dynamicConfig.key],
                  title: res[column.options.dynamicConfig.value],
                  parentId: res[column.options.dynamicConfig.parentId],
                });
              });
              dynamicData = that.setChildrenTreeData(
                that.$utils.toArrayTree(tree, {
                  key: "key",
                })
              );
            } else if (column.type == "autoComplete") {
              result.data.forEach((res) => {
                dynamicData.push(res[column.options.dynamicConfig.key]);
              });
            } else {
              result.data.forEach((res) => {
                dynamicData.push({
                  label: res[column.options.dynamicConfig.value],
                  value: res[column.options.dynamicConfig.key].toString(),
                });
              });
            }

            if (column.modelBatch) {
              that.kfbSetOptions([column.modelBatch], dynamicData);
              that.dynamicData[column.modelBatch] = dynamicData;
            } else {
              that.kfbSetOptions([column.model], dynamicData);
              that.dynamicData[column.model] = dynamicData;
            }
          }
        });
      }
    },

    /**
     * 转换输入参数
     */
    convertInParams(inParams) {
      let that = this;
      var output = [];
      inParams.forEach((element) => {
        if (element.condition.includes("contenteditable")) {
          var condition = element.condition
            .replaceAll("<p>", "")
            .replaceAll("</p>", "")
            .replaceAll(' class="non-editable" contenteditable="false"', "");
          const tempDiv = document.createElement("div");
          // 设置div的内容为HTML字符串
          tempDiv.innerHTML = condition;
          // 查询所有的span标签
          const spans = tempDiv.querySelectorAll("span");
          spans.forEach((span) => {
            var id = decodeURIComponent(span.id);
            condition = condition.replaceAll(span.outerHTML, id);
          });
          element.condition = condition;
        }
        output.push(element);
      });
      return output;
    },

    /**
     * 删除children为空
     * @param {*} data
     */
    setChildrenTreeData(data) {
      for (var i = 0; i < data.length; i++) {
        if (data[i].children) {
          if (data[i].children.length < 1) {
            delete data[i].children;
          } else {
            this.setChildrenTreeData(data[i].children);
          }
        }
      }
      return data;
    },

    /**
     * 默认值
     */
    async setDefaultValue(defaultValue) {
      let that = this;
      //查看已有默认值配置
      if (!that.update) {
        for (let index = 0; index < that.columnJson.length; index++) {
          const item = that.columnJson[index];
          if (item.type == "serialNo" && item.options.loadDisplay) {
            //编码
            var result = await serialNoCreate({
              rule: item.options.rule,
              loadDisplay: item.options.loadDisplay,
              userOccupation: item.options.userOccupation,
              configId: that.config.configId,
              model: item.model,
            });

            if (result.code == that.eipResultCode.success) {
              defaultValue[item.model] = result.data;
            }
          } else if (item.type == "select" && item.options.dynamic) {
            //下拉默认选中第一个
            if (
              that.dynamicData[item.model].length > 0 &&
              item.options.defaultValueFirst
            ) {
              defaultValue[item.model] = that.dynamicData[item.model][0].value;
            }
          } else if (item.type == "date") {
            debugger;
            if (item.options.range) {
            } else {
              switch (item.options.defaultValueDate) {
                case "now":
                  debugger;
                  defaultValue[item.model] = moment().format(
                    item.options.format
                  );
                  break;
              }
            }
          } else {
            if (
              item.options &&
              typeof !that.$utils.isUndefined(item.options.defaultValue)
            ) {
              defaultValue[item.model] = item.options.defaultValue;
            }
          }
        }

        this.setDataShow(defaultValue);
      }

      if (that.automation) {
        that.publicJson.list.forEach((item) => {
          var have = that.$utils.find(
            that.automation.editColumn,
            (f) => f.column == item.model
          );
          if (have) {
            //默认值判断
            if (have.default) {
              defaultValue[item.model] = have.default;
            }
          }
        });
      }
    },

    /**
     * 赋予数据显示
     */
    setDataShow(defaultValue) {
      let that = this;
      let dataShows = that.columnJson.filter((f) =>
        ["dataShow"].includes(f.type)
      );
      dataShows.forEach((item) => {
        //解析默认值
        var content = item.options.defaultValue
          .replaceAll("<p>", "")
          .replaceAll("</p>", "")
          .replaceAll(' class="non-editable" contenteditable="false"', "");
        const tempDiv = document.createElement("div");
        // 设置div的内容为HTML字符串
        tempDiv.innerHTML = content;
        // 查询所有的span标签
        const spans = tempDiv.querySelectorAll("span");
        spans.forEach((span) => {
          var id = decodeURI(span.id);
          switch (id) {
            case "RelationId":
              content = content.replaceAll(
                span.outerHTML,
                that.submitForm.relationId
              );
              break;
            case "CreateUserId":
              content = content.replaceAll(span.outerHTML, that.user.userId);
              break;
            case "CreateUserName":
              content = content.replaceAll(span.outerHTML, that.user.name);
              break;
            case "CreateOrganizationId":
              content = content.replaceAll(
                span.outerHTML,
                that.user.organizationId
              );
              break;
            case "CreateOrganizationName":
              content = content.replaceAll(
                span.outerHTML,
                that.user.organizationName
              );
              break;
            default:
              content = content.replaceAll(
                span.outerHTML,
                that.$utils.isUndefined(defaultValue[id])
                  ? ""
                  : defaultValue[id]
              );
              break;
          }
        });
        defaultValue[item.model] = content;
      });
    },

    /**
     * 动态变化显示值
     */
    checkDataShow(key) {
      let that = this;
      let dataShows = that.columnJson.filter((f) =>
        ["dataShow"].includes(f.type)
      );
      dataShows.forEach((item) => {
        //解析默认值
        var content = item.options.defaultValue
          .replaceAll("<p>", "")
          .replaceAll("</p>", "")
          .replaceAll(' class="non-editable" contenteditable="false"', "");
        const tempDiv = document.createElement("div");
        // 设置div的内容为HTML字符串
        tempDiv.innerHTML = content;
        // 查询所有的span标签
        const spans = tempDiv.querySelectorAll("span");
        let isChange = false;
        spans.forEach((span) => {
          var id = decodeURI(span.id);
          if (id == key) {
            isChange = true;
            return false;
          }
        });

        if (isChange) {
          spans.forEach((span) => {
            var id = decodeURI(span.id);
            switch (id) {
              case "RelationId":
                content = content.replaceAll(
                  span.outerHTML,
                  that.submitForm.relationId
                );
                break;
              case "CreateUserId":
                content = content.replaceAll(span.outerHTML, that.user.userId);
                break;
              case "CreateUserName":
                content = content.replaceAll(span.outerHTML, that.user.name);
                break;
              case "CreateOrganizationId":
                content = content.replaceAll(
                  span.outerHTML,
                  that.user.organizationId
                );
                break;
              case "CreateOrganizationName":
                content = content.replaceAll(
                  span.outerHTML,
                  that.user.organizationName
                );
                break;
              default:
                content = content.replaceAll(
                  span.outerHTML,
                  that.formValue[id]
                );
                break;
            }
          });
          that.formValue[item.model] = content;
          this.$refs.kfb.setData({
            [item.model]: content,
          });
        }
      });
    },

    /**
     * 查询数据
     */
    async find() {
      let that = this;
      var result = await businessDataById({
        configId: that.submitForm.configId,
        id: that.update.RelationId,
      });

      if (result.code == that.eipResultCode.success) {
        await that.setFormValue(result);
      } else {
        that.$message.destroy();
        that.$message.error(result.msg);
      }

      this.agileLogData();
    },

    /**
     * 获取敏捷开发日志
     */
    agileLogData() {
      let that = this;
      agileDataLogRelationId({
        relationId: this.update.RelationId,
      }).then((result) => {
        if (result.code == that.eipResultCode.success) {
          result.data.forEach((item) => {
            item.contentJson =
              item.type == 2
                ? JSON.parse(item.content).filter((f) => !f.Hidden)
                : [];
            item.image =
              document.location.origin +
              "/userheader/" +
              item.createUserId +
              ".png?t=1705725039";
          });
          that.dataLog = result.data;
        }
      });
    },

    /**
     * 赋予表单值
     * @param {*} result
     */
    async setFormValue(result) {
      let that = this;
      that.submitForm.relationId = that.update.RelationId;
      var value = {};
      var column = that.columnJson.filter(
        (f) => !["batch", "text", "divider"].includes(f.type)
      );
      for (let index = 0; index < column.length; index++) {
        const item = column[index];
        switch (item.type) {
          case "select":
            if (item.options.multiple) {
              value[item.model] = result.data[item.model]?.split(",");
            } else {
              value[item.model] = result.data[item.model];
            }
            break;
          case "correlationRecord":
            if (item.options.showType == "select") {
              if (item.options.multiple) {
                value[item.model] = result.data[item.model]?.split(",");
              } else {
                value[item.model] = result.data[item.model];
              }
            } else if (item.options.showType == "dialog") {
              if (item.options.multiple) {
                value[item.model] = {
                  value: result.data[item.model]?.split(","),
                  valueTxt: result.data[item.model + "_Txt"]?.split(","),
                };
              } else {
                value[item.model] = {
                  value: result.data[item.model],
                  valueTxt: result.data[item.model + "_Txt"],
                };
              }
            }
            break;
          case "cascader":
          case "treeSelect":
          case "district":
            let modelData = [];
            if (item.options.multiple) {
              result.data[item.model]?.split(",").forEach((citem) => {
                // if (!isNaN(parseInt(citem))) {
                //     citem = parseInt(citem);
                // }
                modelData.push(citem);
              });
            } else {
              modelData = result.data[item.model];
            }
            value[item.model] = modelData;
            break;
          case "dictionary":
            //为复选框，为下拉并且多选，为数行结构并且多选
            var multiple =
              item.options.dictionaryShowType == 3 ||
              (item.options.dictionaryShowType == 1 && item.options.multiple) ||
              (item.options.dictionaryLevel == 2 && item.options.multiple);

            //单选还是多选
            if (multiple) {
              let modelData = [];
              result.data[item.model]?.split(",").forEach((citem) => {
                modelData.push(citem);
              });
              value[item.model] = modelData;
            } else {
              value[item.model] = result.data[item.model];
            }
            break;
          case "checkbox":
            value[item.model] = result.data[item.model]?.split(",");
            break;
          case "switch":
            value[item.model] =
              that.eipDbType == "mysql"
                ? result.data[item.model] == 1
                : result.data[item.model] == 0;
            break;
          case "user":
            var option = that.getOptionByModel(item.model);
            var ids = result.data[item.model];
            if (ids) {
              var chosen = [];
              var values =
                result.data[item.model + that.relationTxt].split(",");
              var valuesHeadImage =
                result.data[item.model + that.relationTxt + "Header"].split(
                  ","
                );
              ids.split(",").forEach((item, index) => {
                chosen.push({
                  id: item,
                  name: values[index],
                  headImage: valuesHeadImage[index],
                });
              });
              option.options.chosen = chosen;
            }
            value[item.model] = result.data[item.model + that.relationTxt];
            break;
          case "post":
          case "group":
          case "organization":
            option = that.getOptionByModel(item.model);
            ids = result.data[item.model];
            if (ids) {
              chosen = [];
              values = result.data[item.model + that.relationTxt].split(",");
              ids.split(",").forEach((item, index) => {
                chosen.push({
                  id: item,
                  name: values[index],
                });
              });
              option.options.chosen = chosen;
            }
            value[item.model] = result.data[item.model + that.relationTxt];
            break;
          case "uploadFile":
          case "uploadImg":
            var files = result.data[item.model];
            var fileResult = [];
            var uploads = [];
            if (files && files != "") {
              fileResult = JSON.parse(files);
              fileResult.forEach((upload, index) => {
                uploads.push({
                  fileId: upload.fileId || "",
                  type: item.type == "uploadImg" ? "img" : "file",
                  name: upload.name,
                  status: "done",
                  uid: index,
                  url: upload.url || "",
                });
              });
            } else {
              fileResult = await fileCorrelationId(
                that.submitForm.relationId + "|" + item.model
              );
              fileResult.forEach((upload, index) => {
                uploads.push({
                  fileId: upload.fileId || "",
                  type: item.type == "uploadImg" ? "img" : "file",
                  name: upload.name,
                  status: "done",
                  uid: index,
                  url: upload.path || "",
                });
              });
            }
            value[item.model] = uploads;
            break;
          case "map":
            var option = that.getOptionByModel(item.model);
            var resultValue = result.data[item.model];
            if (resultValue) {
              value[item.model] = resultValue;
            }
            break;

          default:
            value[item.model] = result.data[item.model];
            break;
        }
      }
      that.setDefaultValue(value);
      that.defaultValue = that.$utils.clone(value);
      that.formValue = value;

      //得到联动数据
      await that.getDynamicData(result.data);

      //子表单
      that.columnJson
        .filter((f) => f.type == "batch")
        .forEach((item) => {
          that.$message.loading("数据加载中...", 0);
          businessDataBatch({
            configId: that.submitForm.configId,
            table: item.model,
            id: result.data["RelationId"],
          }).then(function (result) {
            that.$message.destroy();
            if (result.code == that.eipResultCode.success) {
              if (result.data && result.data.length > 0) {
                var values = [];
                result.data.forEach((ditem) => {
                  var value = {};
                  item.list.forEach((item) => {
                    switch (item.type) {
                      case "checkbox":
                        value[item.model] = ditem[item.model]?.split(",");
                        break;
                      case "select":
                        if (item.options.multiple && ditem[item.model]) {
                          value[item.model] = ditem[item.model]?.split(",");
                        } else {
                          value[item.model] = ditem[item.model];
                        }
                        break;
                      case "switch":
                        value[item.model] =
                          that.eipDbType == "mysql"
                            ? ditem[item.model] == 1
                            : ditem[item.model] == 0;
                        break;
                      case "user":
                        var users = [];
                        var ditemValue = ditem[item.model];
                        if (ditemValue) {
                          var ditemValueTxt =
                            ditem[item.model + that.relationTxt].split(",");
                          var ditemValueTxtHeader =
                            ditem[
                              item.model + that.relationTxt + "Header"
                            ].split(",");
                          ditemValue
                            .split(",")
                            .forEach((ditemSplit, ditemSplitIndex) => {
                              users.push({
                                id: ditemSplit,
                                name: ditemValueTxt[ditemSplitIndex],
                                headImage: ditemValueTxtHeader[ditemSplitIndex],
                              });
                            });
                        }
                        value[item.model] = users;
                        break;
                      case "organization":
                        var organizations = [];
                        var ditemValue = ditem[item.model];
                        if (ditemValue) {
                          var ditemValueTxt =
                            ditem[item.model + that.relationTxt].split(",");
                          ditemValue
                            .split(",")
                            .forEach((ditemSplit, ditemSplitIndex) => {
                              organizations.push({
                                id: ditemSplit,
                                name: ditemValueTxt[ditemSplitIndex],
                              });
                            });
                        }
                        value[item.model] = organizations;
                        break;
                      default:
                        value[item.model] = ditem[item.model];
                        break;
                    }
                  });
                  values.push(value);
                });
                //赋值
                that.$refs.kfb.setData({
                  [item.model]: values,
                });
              }
            }
          });
        });
    },

    /**
     * 保存流数据
     */
    async saveConfirm() {
      let that = this;
      await this.$refs.kfb
        .getData()
        .then(async (res) => {
          that.loading = true;
          that.$loading.show({ text: that.eipMsg.saveLoading });
          var data = await that.getSubmitValue(res);
          that.$message.loading("数据保存中", 0);
          businessData(data).then(async function (result) {
            if (result.code == that.eipResultCode.success) {
              that.loading = false;
              that.$loading.hide();
              that.$message.destroy();
              if (that.update) {
                that.cancel();
              } else {
                if (that.modal.button.submit.success == 0) {
                  if (that.save.continue) {
                    //不保留上次内容
                    if (!that.save.retain) {
                      that.$refs.kfb.reset();
                    }
                    that.submitForm.relationId = newGuid();
                  } else {
                    that.cancel();
                  }
                }
                if (that.modal.button.submit.success == 1) {
                  that.init();
                }
              }
              that.$emit("ok", result);
              that.$message.success(result.msg);
            } else {
              that.loading = false;
              that.$loading.hide();
              that.$message.destroy();
              that.$message.error(result.msg);
            }
          });
        })
        .catch((err) => {
          that.loading = false;
          that.$message.destroy();
          that.$loading.hide();
        });
    },

    /**
     * 点击继续添加
     */
    saveContinue() {
      this.save.continue = true;
      this.saveConfirmTip();
    },

    /**
     *保存后关闭
     */
    saveClose() {
      this.save.continue = false;
      this.saveConfirmTip();
    },

    /**
     * 保存二次提示
     */
    saveConfirmTip() {
      let that = this;
      if (this.automation && this.automation.confirm.is) {
        this.$refs.kfb.getData().then((res) => {
          that.$confirm({
            title: this.automation.confirm.title,
            content: this.automation.confirm.content,
            okText: this.automation.confirm.okText,
            okType: "danger",
            cancelText: this.automation.confirm.cancelText,
            onOk() {
              that.saveConfirm();
            },
            onCancel() {},
          });
        });
      } else {
        that.saveConfirm();
      }
    },

    /**
     *
     * @param {*} value
     */
    async eventApiConfirm(value) {
      var apiResult = await eventApi(value);
      return apiResult;
    },

    /**
     * 获取提交数据
     */
    getSubmitValue(res) {
      let that = this;
      let columns = [];
      var options = that.$refs.kfb.getOptions();
      options
        .filter((f) => !["batch", "text", "divider"].includes(f.type))
        .forEach((columnObj) => {
          var value = that.formValue[columnObj.model];
          var txt = "";
          var txtArray = [];
          var options = undefined;
          var option = undefined;
          switch (columnObj.type) {
            case "checkbox":
              if (!value) {
                value = columnObj.options.defaultValue;
              }
              options = columnObj.options.options;
              if (columnObj.options.dynamic) {
                options = that.dynamicData[columnObj.model];
              }
              if (value) {
                value.forEach((v) => {
                  option = that.$utils.find(options, (f) => value == f.value);
                  if (option) {
                    txtArray.push(option.label);
                  }
                });
                value = value.join(",");
                txt = txtArray.join(",");
              }

              columns.push({
                isSingle: false,
                name: columnObj.model,
                description: columnObj.label,
                type: columnObj.type,
                value: value,
              });

              columns.push({
                name: columnObj.model + that.relationTxt,
                description: columnObj.label,
                type: columnObj.type,
                value: txt,
              });

              break;
            case "radio":
              if (!value) {
                value = columnObj.options.defaultValue;
              }
              options = columnObj.options.options;
              if (columnObj.options.dynamic) {
                options = that.dynamicData[columnObj.model];
              }
              if (value) {
                option = that.$utils.find(options, (f) => value == f.value);
                if (option) {
                  txtArray.push(option.label);
                }
                txt = txtArray.join(",");
              }

              columns.push({
                isSingle: false,
                name: columnObj.model,
                description: columnObj.label,
                type: columnObj.type,
                value: value,
              });

              columns.push({
                name: columnObj.model + that.relationTxt,
                description: columnObj.label,
                type: columnObj.type,
                value: txt,
              });

              break;
            case "select":
              options = columnObj.options.options;
              if (columnObj.options.dynamic) {
                options = that.dynamicData[columnObj.model];
              }
              if (columnObj.options.multiple && value) {
                value.forEach((v) => {
                  option = that.$utils.find(options, (f) => v == f.value);
                  if (option) {
                    txtArray.push(option.label);
                  }
                });
                txt = txtArray.join(",");
                //变成字符串并且用逗号分割
                value = value.join(",");
              } else {
                option = that.$utils.find(options, (f) => value == f.value);
                if (option) {
                  txt = option.label;
                }
              }

              columns.push({
                isSingle: !columnObj.options.multiple,
                name: columnObj.model,
                description: columnObj.label,
                type: columnObj.type,
                value: value,
              });

              columns.push({
                name: columnObj.model + that.relationTxt,
                description: columnObj.label,
                type: columnObj.type,
                value: txt,
              });
              break;
            case "correlationRecord":
              if (columnObj.options.showType == "select") {
                options = that.dynamicData[columnObj.model];
                if (columnObj.options.multiple && value) {
                  value.forEach((v) => {
                    option = that.$utils.find(options, (f) => v == f.value);
                    if (option) {
                      txtArray.push(option.title);
                    }
                  });
                  txt = txtArray.join(",");
                  value = value.join(",");
                } else {
                  option = that.$utils.find(options, (f) => value == f.value);
                  if (option) {
                    txt = option.title;
                  }
                }
              } else if (columnObj.options.showType == "dialog") {
                if (columnObj.options.multiple && value) {
                  txt = value.valueTxt;
                  value = value.value;
                } else {
                  txt = value.valueTxt;
                }
              }

              columns.push({
                isSingle: !columnObj.options.multiple,
                name: columnObj.model,
                description: columnObj.label,
                type: columnObj.type,
                value: value,
              });

              columns.push({
                name: columnObj.model + that.relationTxt,
                description: columnObj.label,
                type: columnObj.type,
                value: txt,
              });
              break;

            case "treeSelect":
              options = columnObj.options.options;
              if (columnObj.options.dynamic) {
                options = that.$utils.toTreeArray(
                  that.dynamicData[columnObj.model]
                );
              }

              if (columnObj.options.multiple && value) {
                value.forEach((v) => {
                  option = that.$utils.find(options, (f) => v == f.value);
                  if (option) {
                    txtArray.push(option.title);
                  }
                });
                txt = txtArray.join(",");
                value = value.join(",");
              } else {
                option = that.$utils.find(options, (f) => value == f.value);
                if (option) {
                  txt = option.title;
                }
              }

              columns.push({
                name: columnObj.model,
                description: columnObj.label,
                type: columnObj.type,
                value: value,
              });

              columns.push({
                name: columnObj.model + that.relationTxt,
                description: columnObj.label,
                type: columnObj.type,
                value: txt,
              });
              break;
            case "cascader":
              options = columnObj.options.options;
              if (columnObj.options.dynamic) {
                options = that.$utils.toTreeArray(
                  that.dynamicData[columnObj.model]
                );
              }
              if (value && value.length > 0) {
                value.forEach((v) => {
                  var option = options.filter((f) => v == f.value);
                  if (option.length > 0) {
                    txtArray.push(option[0].label);
                  }
                });
                txt = txtArray.join("/");
                //变成字符串并且用逗号分割
                value = value.join(",");
              } else {
                value = "";
              }

              columns.push({
                name: columnObj.model,
                description: columnObj.label,
                type: columnObj.type,
                value: value,
              });

              columns.push({
                name: columnObj.model + that.relationTxt,
                description: columnObj.label,
                type: columnObj.type,
                value: txt,
              });
              break;
            case "district":
              options = that.$utils.toTreeArray(
                that.dynamicData[columnObj.model]
              );
              if (value && value.length > 0) {
                value.forEach((v) => {
                  var option = options.filter((f) => v == f.value);
                  if (option.length > 0) {
                    txtArray.push(option[0].label);
                  }
                });
                txt = txtArray.join("/");
                //变成字符串并且用逗号分割
                value = value.join(",");
              } else {
                value = "";
              }

              columns.push({
                name: columnObj.model,
                description: columnObj.label,
                type: columnObj.type,
                value: value,
              });

              columns.push({
                name: columnObj.model + that.relationTxt,
                description: columnObj.label,
                type: columnObj.type,
                value: txt,
              });
              break;
            case "dictionary":
              //判断是多选还是单选
              var multiple =
                columnObj.options.dictionaryShowType == 3 ||
                (columnObj.options.dictionaryShowType == 1 &&
                  columnObj.options.multiple) ||
                (columnObj.options.dictionaryLevel == 2 &&
                  columnObj.options.multiple);

              if (!multiple) {
                value = [value];
              }
              if (columnObj.options.dictionaryLevel == 2) {
                options = that.$utils.toTreeArray(
                  that.dynamicData[columnObj.model]
                );
              } else {
                options = that.dynamicData[columnObj.model];
              }
              if (value && value.length > 0) {
                value.forEach((v) => {
                  var option = options.filter((f) => v == f.value);
                  if (option.length > 0) {
                    txtArray.push(
                      columnObj.options.dictionaryLevel == 2
                        ? option[0].title
                        : option[0].label
                    );
                  }
                });
                txt = txtArray.join(",");
                //变成字符串并且用逗号分割
                value = value.join(",");
              } else {
                value = "";
              }

              columns.push({
                name: columnObj.model,
                description: columnObj.label,
                type: columnObj.type,
                value: value,
              });

              columns.push({
                name: columnObj.model + that.relationTxt,
                description: columnObj.label,
                type: columnObj.type,
                value: txt,
              });
              break;
            case "user":
            case "post":
            case "organization":
              if (value) {
                columns.push({
                  isSingle: !columnObj.options.multiple,
                  name: columnObj.model,
                  description: columnObj.label,
                  type: columnObj.type,
                  value: columnObj.options.chosen.map((m) => m.id).join(","),
                });

                columns.push({
                  name: columnObj.model + that.relationTxt,
                  description: columnObj.label,
                  type: columnObj.type,
                  value: columnObj.options.chosen.map((m) => m.name).join(","),
                });
              }
              break;
            case "map":
              if (value) {
                columns.push({
                  name: columnObj.model,
                  description: columnObj.label,
                  type: columnObj.type,
                  value: value,
                });

                columns.push({
                  name: columnObj.model + that.relationTxt,
                  description: columnObj.label,
                  type: columnObj.type,
                  value: JSON.parse(value).address,
                });
              }
              break;
            case "switch":
              var c = {
                name: columnObj.model,
                description: columnObj.label,
                type: columnObj.type,
                value: null,
              };
              if (typeof value == "undefined") {
                //读取默认值
                c.value =
                  that.eipDbType == "mysql"
                    ? columnObj.options.defaultValue
                      ? 1
                      : 0
                    : columnObj.options.defaultValue
                    ? 0
                    : 1;
              } else {
                c.value =
                  that.eipDbType == "mysql" ? (value ? 1 : 0) : value ? 0 : 1;
              }
              columns.push(c);
              break;
            case "uploadFile":
            case "uploadImg":
              columns.push({
                name: columnObj.model,
                description: columnObj.label,
                type: columnObj.type,
                value: JSON.stringify(value),
              });
              break;
            case "serialNo":
              columns.push({
                name: columnObj.model,
                description: columnObj.label,
                type: columnObj.type,
                value: value,
                rule: columnObj.options.rule,
                loadDisplay: columnObj.options.loadDisplay,
                userOccupation: columnObj.options.userOccupation,
              });
              break;
            case "dataShow":
              columns.push({
                name: columnObj.model,
                description: columnObj.label,
                type: columnObj.type,
                value: value,
                defaultValue: columnObj.options.defaultValue,
              });
              break;
            default:
              columns.push({
                name: columnObj.model,
                description: columnObj.label,
                type: columnObj.type,
                value: value,
              });
              break;
          }
        });
      that.columnJson
        .filter((f) => f.type == "batch")
        .forEach((item) => {
          var value = res[item.model];
          var column = item.list;
          var values = []; //值集合
          if (typeof value != "undefined") {
            if (column != null && column.length > 0) {
              value.forEach((vf) => {
                var batchColumns = [];
                column.forEach((batchColumnObj) => {
                  var batchValue = vf[batchColumnObj.model];
                  var txtArray = [];
                  var txt = "";
                  switch (batchColumnObj.type) {
                    case "radio":
                    case "checkbox":
                      if (batchValue) {
                        batchValue.forEach((v) => {
                          var option = batchColumnObj.options.options.filter(
                            (f) => v == f.value
                          );
                          if (option.length > 0) {
                            txtArray.push(option[0].label);
                          }
                        });
                        txt = txtArray.join(",");
                        batchValue = batchValue.join(",");
                      }

                      batchColumns.push({
                        isSingle: false,
                        name: batchColumnObj.model,
                        description: batchColumnObj.label,
                        type: batchColumnObj.type,
                        value: batchValue,
                      });

                      batchColumns.push({
                        name: batchColumnObj.model + that.relationTxt,
                        description: batchColumnObj.label,
                        type: batchColumnObj.type,
                        value: txt,
                      });

                      break;
                    case "select":
                      if (batchColumnObj.options.multiple && batchValue) {
                        batchValue.forEach((v) => {
                          var option = batchColumnObj.options.options.filter(
                            (f) => v == f.value
                          );
                          if (option.length > 0) {
                            txtArray.push(option[0].label);
                          }
                        });
                        txt = txtArray.join(",");
                        //变成字符串并且用逗号分割
                        batchValue = batchValue.join(",");
                      } else {
                        var option = batchColumnObj.options.options.filter(
                          (f) => f.value == batchValue
                        );
                        if (option.length > 0) {
                          txt = option[0].lable;
                        }
                      }

                      batchColumns.push({
                        isSingle: !batchColumnObj.options.multiple,
                        name: batchColumnObj.model,
                        description: batchColumnObj.label,
                        type: batchColumnObj.type,
                        value: batchValue,
                      });

                      batchColumns.push({
                        name: batchColumnObj.model + that.relationTxt,
                        description: batchColumnObj.label,
                        type: batchColumnObj.type,
                        value: txt,
                      });
                      break;
                    case "user":
                    case "organization":
                      batchColumns.push({
                        isSingle: !batchColumnObj.options.multiple,
                        name: batchColumnObj.model,
                        description: batchColumnObj.label,
                        type: batchColumnObj.type,
                        value: batchValue.map((m) => m.id).join(","),
                      });

                      batchColumns.push({
                        name: batchColumnObj.model + that.relationTxt,
                        description: batchColumnObj.label,
                        type: batchColumnObj.type,
                        value: batchValue.map((m) => m.name).join(","),
                      });
                      break;
                    case "post":
                    case "dictionary":
                      break;
                    case "switch":
                      batchColumns.push({
                        name: batchColumnObj.model,
                        description: batchColumnObj.label,
                        type: batchColumnObj.type,
                        value:
                          that.eipDbType == "mysql"
                            ? batchValue
                              ? 1
                              : 0
                            : batchValue
                            ? 0
                            : 1,
                      });
                      break;
                    case "dataShow":
                      columns.push({
                        name: batchColumnObj.model,
                        description: batchColumnObj.label,
                        type: batchColumnObj.type,
                        value: batchValue,
                        defaultValue: batchColumnObj.options.defaultValue,
                      });
                      break;
                    default:
                      batchColumns.push({
                        name: batchColumnObj.model,
                        description: batchColumnObj.label,
                        type: batchColumnObj.type,
                        value: batchValue,
                      });
                      break;
                  }
                });
                batchColumns.push({
                  isSingle: true,
                  name: "BatchId",
                  description: "子表记录ID",
                  type: "hidden",
                  value: newGuid(),
                });

                values.push({
                  detail: batchColumns,
                });
              });
            }
          }

          columns.push({
            name: item.model,
            description: item.label,
            type: "batch",
            value: JSON.stringify(values),
          });
        });

      return {
        configId: that.submitForm.configId,
        relationId: that.copy ? newGuid() : that.submitForm.relationId,
        columns: columns,
      };
    },

    /**
     *
     * 根据名称获取控件属性
     */
    getOptionByModel(model) {
      var option = null;
      // 递归遍历控件树
      const traverse = (array) => {
        array.forEach((element) => {
          if (model == element.model) {
            option = element;
          }
          if (
            ["grid", "tabs", "selectInputList", "collapse"].includes(
              element.type
            )
          ) {
            // 栅格布局 and 标签页
            element.columns.forEach((item) => {
              traverse(item.list);
            });
          } else if (element.type === "card" || element.type === "batch") {
            // 卡片布局 and  动态表格
            traverse(element.list);
          } else if (element.type === "table") {
            // 表格布局
            element.trs.forEach((item) => {
              item.tds.forEach((val) => {
                traverse(val.list);
              });
            });
          }
        });
      };
      traverse(this.publicJson.list);
      return option;
    },

    /**
     * 获取所有上传控件
     */
    getOptionsByType(types) {
      var files = [];
      // 递归遍历控件树
      const traverse = (array) => {
        array.forEach((element) => {
          if (types.includes(element.type)) {
            files.push(element);
          }
          if (
            ["grid", "tabs", "selectInputList", "collapse"].includes(
              element.type
            )
          ) {
            // 栅格布局 and 标签页
            element.columns.forEach((item) => {
              traverse(item.list);
            });
          } else if (element.type === "card" || element.type === "batch") {
            // 卡片布局 and  动态表格
            traverse(element.list);
          } else if (element.type === "table") {
            // 表格布局
            element.trs.forEach((item) => {
              item.tds.forEach((val) => {
                traverse(val.list);
              });
            });
          }
        });
      };
      traverse(this.publicJson.list);
      return files;
    },

    /**
     * 组件加载完成
     */
    async kfbComplate() {
      let that = this;
      that.complete = true;
      var publicJson = JSON.parse(that.editConfig.publicJson);
      //赋值
      for (let key in this.dynamicData) {
        that.kfbSetOptions([key], this.dynamicData[key]);
      }
      await this.workflowKfbComplate();
      if (publicJson.config.event.pc.onload) {
        eval(publicJson.config.event.pc.onload);
      }
    },

    /**
     * 赋值
     */
    kfbSetOptions(key, data) {
      if (this.complete) {
        this.$refs.kfb.options(key, data);
      }
    },

    /**
     *
     */
    async workflowKfbComplate() {
      let that = this;
      var kfbref = this.$refs.kfb;
      if (
        this.workflowData.type == this.eipWorkflowDoType["流程监控"] ||
        this.workflowData.type == this.eipWorkflowDoType["阅示审批"]
      ) {
        kfbref.disable(this.workflow.props.column.map((m) => m.name));

        this.workflow.props.column.forEach((item) => {
          //子表判断
          if (item.type == "batch") {
            var batchDisableds = [];
            item.batch.fields.forEach((field) => {
              batchDisableds.push(field.name);
            });

            if (batchDisableds.length > 0) {
              kfbref.batchFieldDisabled(batchDisableds, item.name);
            }
            kfbref.batchDelete([item.name]);
            kfbref.batchCopy([item.name]);
            var batchButtonHidden = [];
            //处理按钮
            item.batch.buttons.forEach((button, index) => {
              batchButtonHidden.push(index);
            });

            if (batchButtonHidden.length > 0) {
              kfbref.batchButtonHide(batchButtonHidden, item.name);
            }
          }
        });
      } else {
        this.workflowColumn();
      }
    },

    /**
     * 初始化流程
     */
    async workflowInit() {
      let that = this;
      if (that.workflowData.type == this.eipWorkflowDoType["阅示审批"]) {
        var taskResult = await workflowEngineTaskById(that.workflowData.taskId);
        if (taskResult.code == that.eipResultCode.success) {
          this.workflow.task.doUserName = taskResult.data.doUserName;
          this.workflow.task.completedTime = taskResult.data.completedTime;
          this.workflow.task.comment = taskResult.data.comment;
          this.workflow.task.status = taskResult.data.status;
          this.workflow.task.headImage = taskResult.data.headImage;
        }

        //获取相关附件
        var fileResult = await fileCorrelationId(that.workflowData.taskId);
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
        that.workflow.task.files = uploads;

        //得到历史阅示信息
        workflowInstanceProcess({
          id: this.workflowData.processInstanceId,
        }).then(async (result) => {
          var data = result.data.filter(
            (f) =>
              f.title == "督办" &&
              f.doUserId == taskResult.data.receiveUserId &&
              f.taskId != that.workflowData.taskId
          );
          data.forEach((item) => {
            item.files = [];
          });

          data
            .filter((f) => f.status == 6)
            .forEach(async (item) => {
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
          that.workflow.duBanHistory = data;
        });
      } else {
        //开始节点
        if (!that.workflowData.processInstanceId) {
          that.workflow.form.processInstanceId = that.submitForm.relationId;
        }
      }
      this.workflowNotice();
      that.spinning = false;
    },

    /**
     * 按钮点击
     */
    async workflowButtonClick(button) {
      switch (button.script) {
        case "flowSave();": //保存数据
          this.workflowSave(this.workflowSaveSuccess);
          break;
        case "flowNext();": //保存数据
          this.workflowNext();
          break;
        case "flowReturn();": //退回
          this.workflow.return.visible = true;
          break;
        case "flowRefuse();": //拒绝
          this.workflow.refuse.visible = true;
          break;
        case "flowDoList();": //过程列表
          this.workflow.doList.visible = true;
          break;
        case "flowReturnAndWrite();": //退回重填
          this.workflow.returnAndWrite.visible = true;
          break;
        case "flowDesigner();": //流程图
          this.workflow.designer.visible = true;
          break;
        case "flowUnderstanding();": //知会
          switch (this.workflow.currentActivity.type) {
            case "begin":
              this.workflow.understandingStart.visible = true;
              break;
            case "task":
              this.workflow.understanding.visible = true;
              break;
          }
          break;
        case "flowUnderstandingSure();": //知会阅示直接结束
          this.workflowUnderStandingRead();
          break;
        case "flowInvitationRead();": //邀请阅示
          this.workflow.invitationread.visible = true;
          break;
        case "flowInvitationReadSure();": //邀请阅示确定,弹出意见框
          this.workflow.invitationreadSure.visible = true;
          break;
        case "flowInvitationReadApprove();": //邀请阅示确定,弹出意见框
          this.workflow.invitationreadApprove.visible = true;
          break;
        case "flowRevoke();": //撤销
          this.workflow.revoke.visible = true;
          break;
        case "flowEnd();": //结束
          this.workflow.end.visible = true;
          break;
        case "flowAdd();": //加签
          this.workflow.add.visible = true;
          break;
        case "flowDelete();": //删除
          this.workflow.deleteTask.visible = true;
          break;
      }

      // let that = this;
      // button.script = "that." + button.script;
      // eval(button.script);
    },

    /**
     * 生成流程标题
     */
    workflowGenerateTitleRule(data) {
      var title = "";
      let that = this;
      var titleRule = that.workflow.props.base.titleRule;
      if (titleRule != null) {
        titleRule = titleRule
          .replaceAll("<p>", "")
          .replaceAll("</p>", "")
          .replaceAll(' class="non-editable" contenteditable="false"', "");
        const tempDiv = document.createElement("div");
        // 设置div的内容为HTML字符串
        tempDiv.innerHTML = titleRule;
        // 查询所有的span标签
        const spans = tempDiv.querySelectorAll("span");
        spans.forEach((span) => {
          var id = decodeURI(span.id);
          switch (id) {
            case "流程类型名称":
              titleRule = titleRule.replaceAll(
                span.outerHTML,
                currentActivity.ProcessName
              );
              break;
            case "当前时间":
              titleRule = titleRule.replaceAll(
                span.outerHTML,
                moment().format("YYYY-MM-DD HH:mm:ss")
              );
              break;
            case "当前用户组织架构名称":
              titleRule = titleRule.replaceAll(
                span.outerHTML,
                that.user.organizationName
              );
              title += that.user.organizationName;
              break;
            case "当前用户名":
              titleRule = titleRule.replaceAll(span.outerHTML, that.user.name);
              break;
            default:
              var searchColumns = data.columns.filter((f) => f.name == id);
              if (searchColumns.length > 0) {
                titleRule = titleRule.replaceAll(
                  span.outerHTML,
                  searchColumns[0].value
                );
              } else {
                titleRule = titleRule.replaceAll(span.outerHTML, "");
              }
          }
        });
      }
      this.workflow.form.title = titleRule;
    },

    /**
     *流程保存
     */
    async workflowSave(method) {
      let that = this;
      await that.$refs.kfb
        .getData()
        .then(async (res) => {
          var data = that.getSubmitValue(res);
          that.designerFormValue = res;
          that.$loading.show({ text: that.eipMsg.saveLoading });
          if (that.workflow.currentActivity.type == "begin") {
            that.workflowGenerateTitleRule(data);
            data.columns.push({
              description: "状态",
              name: "WorkflowStatus",
              type: "input",
              value: null,
            });
            data.columns.push({
              description: "标题",
              name: "WorkflowTitle",
              type: "input",
              value: that.workflow.form.title,
            });
          }
          var result = await businessData(data);
          if (result.code == that.eipResultCode.success) {
            if (method) {
              method();
            } else {
              that.$emit("ok", result);
            }
          } else {
            that.$message.error(result.msg);
            that.workflowLoadingHide();
          }
        })
        .catch((err) => {});
    },

    /**
     * 保存成功
     */
    workflowSaveSuccess() {
      this.workflowLoadingHide();
      var result = {
        code: this.eipResultCode.success,
        msg: "保存成功",
      };
      this.$message.success(result.msg);
      this.$emit("ok", result);
      this.cancel();
    },

    /**
     * 流程处理完毕关闭弹出层
     */
    workflowLoadingHide() {
      this.$message.destroy();
      this.$loading.hide();
    },

    /**
     * 下一步
     */
    workflowNext() {
      //判断是开始节点还是处理节点
      switch (this.workflow.currentActivity.type) {
        case "begin":
          this.workflowNextStart();
          break;
        case "task":
          this.workflowTaskProcessSure();
          break;
      }
    },

    /**
     * 流程开始
     */
    async workflowNextStart() {
      let that = this;
      await that.$refs.kfb.getData().then(async (res) => {
        that.workflow.form.notice = that.workflow.sure.noticeChosen.join(",");
        that.workflow.form.activityType = "start";
        that.workflow.form.activityId =
          that.workflow.currentActivity.activityId;
        that.$loading.show({ text: "处理中..." });
        var data = await that.getSubmitValue(res);
        that.workflow.form.columns = data.columns;
        var eventResult = await that.workflowEvent(0);
        if (eventResult.code == that.eipResultCode.success) {
          workflowStart(that.workflow.form).then(async (result) => {
            if (result.code == that.eipResultCode.success) {
              that.workflow.form.activities = result.data.activities;
              that.workflow.form.links = result.data.links;
              that.workflow.form.needChosenPerson =
                result.data.needChosenPerson;
              let nextTask = [], //任务处理人
                chosenPerson = false, //是否具有弹出处理人
                chosenPersons = []; //弹出选择处理人
              result.data.person.forEach((item) => {
                let activity = item.activity; //返回下个步骤处理人

                var activityObj = JSON.parse(activity.json);
                if (activityObj.props.user) {
                  //处理策略：0列表中的第一处理人，1发送给列表中的所有处理人，2弹出选择审批人
                  switch (activityObj.props.user.strategy) {
                    case 2: //弹出选择审批人
                      chosenPerson = true;
                      chosenPersons.push(item);
                      break;
                    default:
                      nextTask.push({
                        activity: {
                          title: activityObj.text.text,
                          processInstanceId: activity.processInstanceId,
                          activityId: activity.activityId,
                          type: activity.type,
                          json: activity.json,
                          // subprocessinput: activity.SubProcessInput,
                          // subprocessoutput: activity.SubProcessOutput,
                          // subProcessProcessId: activity.SubProcessProcessId
                        },
                        persons: item.persons,
                      });
                      break;
                  }
                } else {
                  nextTask.push({
                    activity: {
                      title: activityObj.text.text,
                      processInstanceId: activity.processInstanceId,
                      activityId: activity.activityId,
                      type: activity.type,
                      json: activity.json,
                      // subprocessinput: activity.SubProcessInput,
                      // subprocessoutput: activity.SubProcessOutput,
                      // subProcessProcessId: activity.SubProcessProcessId
                    },
                    persons: item.persons,
                  });
                }
              });

              //判断是否跳转到选择人人员出来界面
              if (chosenPerson) {
                that.workflowLoadingHide();
                chosenPersons.forEach((item) => {
                  item.checked = false;
                });
                that.workflow.chosenPerson.data = chosenPersons;
                that.workflow.chosenPerson.visible = true;
                that.workflow.start.visible = true;
              } else {
                that.workflow.form.nextTask = nextTask;
                that.workflow.form.nextTaskString = JSON.stringify(nextTask);
                //是否需要弹出通知方式,若不弹出则直接走下一步
                if (that.workflow.form.notice) {
                  that.workflowLoadingHide();
                  that.workflow.start.visible = true;
                } else {
                  that.workflowSave(that.workflowStartNextTask);
                }
              }
            } else {
              that.workflowLoadingHide();
              that.$message.error(result.msg);
            }
          });
        } else {
          that.workflowLoadingHide();
          that.$message.error(eventResult.msg);
        }
      });
    },

    /**
     * 选择通知后事件
     * @param {*} data
     */
    workflowNextStartNotice(data) {
      this.workflow.sure.noticeChosen = data.notice;
      this.workflow.chosenPerson.value = data.person;
      if (this.workflow.chosenPerson.visible) {
        this.workflowChosenPersonConfirm();
      } else {
        this.$loading.show({ text: this.eipMsg.saveLoading });
        this.workflowNextStart();
      }
    },

    /**
     * 开始下一步
     * @param {Object} that
     * @param {Object} data
     * @param {Object} success
     */
    async workflowStartNextTask() {
      let that = this;
      that.$loading.show({ text: "处理中..." });
      this.workflow.form.notice = this.workflow.sure.noticeChosen.join(",");
      //保存业务数据
      workflowStartRun(this.workflow.form).then(async (result) => {
        that.workflowLoadingHide();
        if (result.code == that.eipResultCode.success) {
          //事件处理
          var eventResult = await that.workflowEvent(1);
          if (eventResult.code == that.eipResultCode.success) {
            that.$message.success(result.msg);
            that.$emit("ok", result);
            that.cancel();
          } else {
            that.$message.error(eventResult.msg);
          }
        } else {
          that.$message.error(result.msg);
        }
      });
    },

    /**
     * 工作流事件
     */
    async workflowEvent(trigger) {
      let that = this;
      var result = {
        code: that.eipResultCode.success,
        msg: "",
      };
      //使用并且类型并且触发类型:下一步开始后
      var events = this.workflow.props.event.filter(
        (f) => f.use && f.trigger == trigger
      );
      for (let index = 0; index < events.length; index++) {
        const element = events[index];
        if (element.type == 0) {
          //通知类型:JS
          eval(element.config.js);
        } else if (element.type == 1) {
          //通知类型:接口
          result = await workflowEngineEvent({
            url: element.config.path,
            type: element.config.type,
            data: {
              task: {
                processId: that.workflowData.processId,
                processInstanceId: that.workflow.form.processInstanceId,
                formId: that.workflow.currentActivity.formId,
                activityId: that.workflow.form.activityId,
                type: that.workflow.currentActivity.type,
                taskId: that.workflow.form.taskId,
              },
              controls: that.workflow.form.columns,
            },
          });
        }
      }
      return result;
    },

    /**
     * 设置默认通知
     */
    workflowNotice() {
      this.workflow.sure.notice = this.workflow.props.notice.filter(
        (f) => f.use && f.trigger == 0 && f.userchosen
      );
      var notice = [];
      for (let index = 0; index < this.workflow.sure.notice.length; index++) {
        notice.push(index);
      }
      this.workflow.sure.noticeChosen = notice;
      this.workflow.form.notice = notice.join(",");
    },

    /**
     * 输入意见后提交
     * @param {*} data
     */
    workflowTaskProcessSureComment(data) {
      this.workflow.form.comment = data.comment;
      this.workflow.sure.noticeChosen = data.notice;
      this.workflow.chosenPerson.value = data.person;

      if (this.workflow.chosenPerson.visible) {
        this.workflowChosenPersonConfirm();
      } else {
        this.$loading.show({ text: this.eipMsg.saveLoading });
        this.workflowTaskProcessTask();
      }
    },

    /**
     * 确定流程走向
     */
    async workflowTaskProcessSure() {
      let that = this;
      await that.$refs.kfb.getData().then(async (res) => {
        that.loading = true;
        that.$loading.show({ text: "处理中..." });
        that.workflow.form.activityType = "task";
        that.workflow.form.activityId =
          that.workflow.currentActivity.activityId;
        that.workflow.form.taskId = that.workflowData.taskId;
        that.workflow.form.notice = that.workflow.sure.noticeChosen.join(",");
        var data = await that.getSubmitValue(res);
        that.workflow.form.columns = data.columns;
        var eventResult = await that.workflowEvent(0);

        if (eventResult.code == that.eipResultCode.success) {
          workflowTaskProcess(that.workflow.form).then(async (result) => {
            if (result.code == that.eipResultCode.success) {
              that.workflow.form.activities = result.data.activities;
              that.workflow.form.links = result.data.links;
              that.workflow.form.needChosenPerson =
                result.data.needChosenPerson;

              let nextTask = [], //任务处理人
                chosenPerson = false, //是否具有弹出处理人
                chosenPersons = []; //弹出选择处理人
              result.data.person.forEach((item) => {
                let activity = item.activity; //返回下个步骤处理人
                var activityObj = JSON.parse(activity.json);
                if (activityObj.props.user) {
                  switch (activityObj.props.user.strategy) {
                    case 2: //弹出选择审批人
                      chosenPerson = true;
                      chosenPersons.push(item);
                      break;
                    default:
                      nextTask.push({
                        activity: {
                          title: activityObj.text.text,
                          processInstanceId: activity.processInstanceId,
                          activityId: activity.activityId,
                          type: activity.type,
                          json: activity.json,
                          // subprocessinput: activity.SubProcessInput,
                          // subprocessoutput: activity.SubProcessOutput,
                          // subProcessProcessId: activity.SubProcessProcessId
                        },
                        persons: item.persons,
                      });
                      break;
                  }
                } else {
                  nextTask.push({
                    activity: {
                      title: activityObj.text.text,
                      processInstanceId: activity.processInstanceId,
                      activityId: activity.activityId,
                      type: activity.type,
                      json: activity.json,
                      // subprocessinput: activity.SubProcessInput,
                      // subprocessoutput: activity.SubProcessOutput,
                      // subProcessProcessId: activity.SubProcessProcessId
                    },
                    persons: item.persons,
                  });
                }
              });

              //判断是否跳转到选择人人员出来界面
              if (chosenPerson) {
                //弹出人员
                that.workflowLoadingHide();
                chosenPersons.forEach((item) => {
                  item.checked = false;
                });
                that.workflow.chosenPerson.data = chosenPersons;
                that.workflow.sure.visible = true;
                that.workflow.chosenPerson.visible = true;
              } else {
                that.workflow.form.nextTask = nextTask;
                that.workflow.form.nextTaskString = JSON.stringify(nextTask);
                //是否需要弹出通知方式,若不弹出则直接走下一步
                if (
                  that.workflow.form.notice &&
                  that.workflow.props.base.commentMessage
                ) {
                  that.workflowLoadingHide();
                  that.workflow.sure.visible = true;
                } else {
                  that.workflowSave(that.workflowTaskProcessTask);
                }
              }
            } else {
              that.workflowLoadingHide();
              that.$message.error(result.msg);
            }
          });
        } else {
          that.workflowLoadingHide();
          that.$message.error(eventResult.msg);
        }
      });
    },

    /**
     * 开始下一步
     * @param {Object} that
     * @param {Object} data
     * @param {Object} success
     */
    async workflowTaskProcessTask() {
      let that = this;
      this.$loading.show({ text: "处理中..." });
      workflowTaskProcessRun(this.workflow.form).then(async (result) => {
        that.workflowLoadingHide();
        if (result.code == that.eipResultCode.success) {
          var eventResult = await that.workflowEvent(1);
          if (eventResult.code == that.eipResultCode.success) {
            that.$message.success(result.msg);
            that.$emit("ok", result);
            that.cancel();
          } else {
            that.$message.error(eventResult.msg);
          }
        } else {
          that.$message.error(result.msg);
        }
      });
    },

    /**
     * 选择人员确定
     */
    async workflowChosenPersonConfirm() {
      let that = this;
      var checked = that.workflow.chosenPerson.value;
      //查看是否选择了人员
      var nextTask = [];
      this.workflow.chosenPerson.data.forEach((item) => {
        let activity = item.activity; //返回下个步骤处理人
        var activityObj = JSON.parse(activity.json);
        var task = {
          activity: {
            title: activityObj.text.text,
            processInstanceId: activity.processInstanceId,
            activityId: activity.activityId,
            type: activity.type,
            json: activity.json,
            // subprocessinput: activity.SubProcessInput,
            // subprocessoutput: activity.SubProcessOutput,
            // subProcessProcessId: activity.SubProcessProcessId
          },
          persons: [],
        };
        item.persons.forEach((pitem) => {
          var isIn = false;
          //是否为字符串
          if (typeof checked == "string") {
            isIn = checked == pitem.userId;
          } else {
            isIn = checked.filter((f) => f == pitem.userId).length > 0;
          }
          if (isIn) {
            task.persons.push({
              userId: pitem.userId,
              code: pitem.code,
              name: pitem.name,
            });
          }
        });
        if (task.persons.length > 0) {
          nextTask.push(task);
        }
      });
      if (nextTask.length == 0) {
        that.$message.error("请选择流程处理人");
        return;
      }
      that.workflow.form.nextTask = nextTask;
      that.workflow.form.nextTaskString = JSON.stringify(nextTask);
      if (this.workflow.form.activityType == "start") {
        //重新赋予处理人员
        that.workflowSave(that.workflowStartNextTask);
      } else {
        that.workflowSave(that.workflowTaskProcessTask);
      }
    },

    /**
     * 初始化历史意见
     */
    async workflowCommentHistory() {
      let that = this;
      if (that.workflow.props.base.isopinion) {
        workflowInstanceProcess({
          id: this.workflowData.processInstanceId,
        }).then(async (result) => {
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
          that.workflow.comment = result.data;
        });
      }
    },

    /**
     *流程结束意见
     */
    workflowTaskProcessEndComment(data) {
      this.workflow.end.comment = data.comment;
      this.$message.loading("数据保存中", 0);
      this.loading = true;
      this.$loading.show({ text: this.eipMsg.saveLoading });
      this.flowEnd();
    },

    /**
     *
     */
    async flowEnd() {
      let that = this;
      //事件处理
      var eventResult = await that.workflowEvent(6);
      if (eventResult.code == that.eipResultCode.success) {
        var result = await workflowEngineEnd({
          taskId: that.workflowData.taskId,
          comment: that.workflow.end.comment,
        });
        if (result.code == that.eipResultCode.success) {
          eventResult = await that.workflowEvent(7);
          if (eventResult.code == that.eipResultCode.success) {
            that.$message.success(result.msg);
            that.cancel();
            that.$emit("ok", result);
          } else {
            that.workflowLoadingHide();
            that.$message.error(eventResult.msg);
          }
        } else {
          that.workflowLoadingHide();
          that.$message.error(result.msg);
        }
      } else {
        that.workflowLoadingHide();
        that.$message.error(eventResult.msg);
      }
    },

    /**
     *加签
     */
    flowAdd(data) {
      let that = this;
      that.$loading.show({ text: "加签中.." });
      this.workflow.form.extend = data;
      switch (this.workflow.currentActivity.type) {
        case "begin":
          this.workflow.form.activityType = "start";
          break;
        case "task":
          this.workflow.form.activityType = "task";
          break;
      }
      this.workflow.form.activityId = that.workflow.currentActivity.activityId;
      this.workflow.form.taskId = that.workflowData.taskId;
      workflowEngineAdd(this.workflow.form).then((result) => {
        that.workflowLoadingHide();
        if (result.code == that.eipResultCode.success) {
          that.$message.success(result.msg);
          that.cancel();
          that.$emit("ok", result);
        } else {
          that.$message.error(result.msg);
        }
      });
    },

    /**
     *
     */
    flowAddApprove() {
      let that = this;
      this.$refs.form.validate((valid) => {
        if (valid) {
          that.$loading.show({ text: "处理中.." });
          workflowEngineAddApprove({
            taskId: that.workflowData.taskId,
            comment: that.workflow.form.comment,
          }).then((result) => {
            that.workflowLoadingHide();
            if (result.code == that.eipResultCode.success) {
              that.$message.success(result.msg);
              that.cancel();
              that.$emit("ok", result);
            } else {
              that.$message.error(result.msg);
            }
          });
        }
      });
    },

    /**
     * 拒绝
     */
    workflowRefuse(comment) {
      let that = this;
      that.$loading.show({ text: "任务拒绝中.." });
      workflowEngineRefuse({
        taskId: this.workflowData.taskId,
        comment: comment,
      }).then((result) => {
        that.workflowLoadingHide();
        if (result.code == that.eipResultCode.success) {
          that.$message.success(result.msg);
          that.cancel();
          that.$emit("ok", result);
        } else {
          that.$message.error(result.msg);
        }
      });
    },

    /**
     * 删除
     * @param {*} comment
     */
    workflowDeleteTask(comment) {
      let that = this;
      that.$loading.show({ text: "删除中.." });
      workflowEngineDeleteTask({
        taskId: this.workflowData.taskId,
        comment: comment,
      }).then((result) => {
        that.workflowLoadingHide();
        if (result.code == that.eipResultCode.success) {
          that.$message.success(result.msg);
          that.cancel();
          that.$emit("ok", result);
        } else {
          that.$message.error(result.msg);
        }
      });
    },

    /**
     * 返回重填
     * @param {*} comment
     */
    workflowReturnAndWrite(comment) {
      let that = this;
      that.$loading.show({ text: "返回重填中.." });
      workflowEngineReturnAndWrite({
        taskId: this.workflowData.taskId,
        comment: comment,
      }).then((result) => {
        that.workflowLoadingHide();
        if (result.code == that.eipResultCode.success) {
          that.$message.success(result.msg);
          that.cancel();
          that.$emit("ok", result);
        } else {
          that.$message.error(result.msg);
        }
      });
    },

    /**
     *
     */
    workflowUnderStandingStart(data) {},

    /**
     *
     */
    workflowUnderStanding(data) {
      let that = this;
      that.$loading.show({ text: "处理中.." });
      workflowEngineUnderStanding({
        taskId: this.workflowData.taskId,
        userId: data.userId,
        userName: data.userName,
      }).then((result) => {
        that.workflowLoadingHide();
        if (result.code == that.eipResultCode.success) {
          that.$message.success(result.msg);
          that.cancel();
          that.$emit("ok", result);
        } else {
          that.$message.error(result.msg);
        }
      });
    },
    /**
     *
     */
    workflowUnderStandingRead() {
      let that = this;
      that.$loading.show({ text: "处理中.." });
      workflowEngineUnderStandingRead({
        taskId: that.workflowData.taskId,
        comment: that.workflow.form.comment,
      }).then((result) => {
        that.workflowLoadingHide();
        if (result.code == that.eipResultCode.success) {
          that.$message.success(result.msg);
          that.cancel();
          that.$emit("ok", result);
        } else {
          that.$message.error(result.msg);
        }
      });
    },

    /**
     *
     */
    workflowInvitationRead(data) {
      let that = this;
      that.$loading.show({ text: "处理中.." });
      workflowEngineInvitationRead({
        taskId: this.workflowData.taskId,
        userId: data.userId,
        userName: data.userName,
      }).then((result) => {
        that.workflowLoadingHide();
        if (result.code == that.eipResultCode.success) {
          that.$message.success(result.msg);
          that.cancel();
          that.$emit("ok", result);
        } else {
          that.$message.error(result.msg);
        }
      });
    },

    /**
     *
     */
    workflowInvitationReadSure(comment) {
      let that = this;
      that.$loading.show({ text: "处理中.." });
      workflowEngineInvitationReadSure({
        taskId: that.workflowData.taskId,
        comment: comment,
      }).then((result) => {
        that.workflowLoadingHide();
        if (result.code == that.eipResultCode.success) {
          that.$message.success(result.msg);
          that.cancel();
          that.$emit("ok", result);
        } else {
          that.$message.error(result.msg);
        }
      });
    },

    /**
     *
     */
    workflowInvitationReadApprove(comment) {
      let that = this;
      that.$loading.show({ text: "处理中.." });
      workflowEngineInvitationReadApprove({
        taskId: that.workflowData.taskId,
        comment: comment,
      }).then((result) => {
        that.workflowLoadingHide();
        if (result.code == that.eipResultCode.success) {
          that.$message.success(result.msg);
          that.cancel();
          that.$emit("ok", result);
        } else {
          that.$message.error(result.msg);
        }
      });
    },

    /**
     *
     * @param {*} comment
     */
    workflowRevoke(comment) {
      let that = this;
      that.$loading.show({ text: "撤销中.." });
      workflowEngineRevoke({
        taskId: this.workflowData.taskId,
        comment: comment,
      }).then((result) => {
        that.workflowLoadingHide();
        if (result.code == that.eipResultCode.success) {
          that.$message.success(result.msg);
          that.cancel();
          that.$emit("ok", result);
        } else {
          that.$message.error(result.msg);
        }
      });
    },

    /**
     * 退回某步
     * @param {*} comment
     */
    workflowReturn(data) {
      let that = this;
      that.$loading.show({ text: "退回中.." });
      workflowEngineReturnAndWrite({
        taskId: this.workflowData.taskId,
        activityId: data.activityId,
        comment: data.comment,
      }).then((result) => {
        that.workflowLoadingHide();
        if (result.code == that.eipResultCode.success) {
          that.$message.success(result.msg);
          that.cancel();
          that.$emit("ok", result);
        } else {
          that.$message.error(result.msg);
        }
      });
    },

    /**
     * 审核通过
     */
    workflowInvitationReadApprovePass() {
      let that = this;
      that.$loading.show({ text: "通过中.." });
      workflowEngineInvitationReadApprovePass({
        taskId: this.workflowData.taskId,
        comment: this.workflow.invitationreadApprove.comment,
      }).then((result) => {
        that.workflowLoadingHide();
        if (result.code == that.eipResultCode.success) {
          that.$message.success(result.msg);
          that.cancel();
          that.$emit("ok", result);
        } else {
          that.$message.error(result.msg);
        }
      });
    },

    /**
     * 审核拒绝
     */
    workflowInvitationReadApproveRefuse() {
      let that = this;
      if (!this.workflow.invitationreadApprove.comment) {
        this.$message.error("请输入拒绝理由");
        return;
      }
      that.$loading.show({ text: "拒绝中.." });
      workflowEngineInvitationReadApproveRefuse({
        taskId: this.workflowData.taskId,
        comment: this.workflow.invitationreadApprove.comment,
      }).then((result) => {
        that.workflowLoadingHide();
        if (result.code == that.eipResultCode.success) {
          that.$message.success(result.msg);
          that.cancel();
          that.$emit("ok", result);
        } else {
          that.$message.error(result.msg);
        }
      });
    },

    /**
     * 流程字段:控制隐藏显示
     */
    workflowColumn() {
      var hides = [],
        disableds = [],
        batchCopy = [],
        batchDelete = [];
      this.workflow.props.column.forEach((item) => {
        //子表判断
        if (item.type == "batch") {
          //子项目
          var batchHides = [],
            batchDisableds = [];
          item.batch.fields.forEach((field) => {
            if (field.isHide) {
              batchHides.push(field.name);
            }
            if (!field.isWrite) {
              batchDisableds.push(field.name);
            }
          });

          if (batchHides.length > 0) {
            this.$refs.kfb.batchFieldHide(batchHides, item.name);
          }

          if (batchDisableds.length > 0) {
            this.$refs.kfb.batchFieldDisabled(batchDisableds, item.name);
          }

          var batchButtonHidden = [];
          //处理按钮
          item.batch.buttons.forEach((button, index) => {
            if (!button.isShow) {
              batchButtonHidden.push(index);
            }
          });

          if (batchButtonHidden.length > 0) {
            this.$refs.kfb.batchButtonHide(batchButtonHidden, item.name);
          }

          if (!item.isDelete) {
            batchDelete.push(item.name);
          }
          if (!item.isWrite) {
            batchCopy.push(item.name);
          }
        } else {
          if (item.isHide) {
            hides.push(item.name);
          }
          if (!item.isWrite) {
            disableds.push(item.name);
          }
        }
      });

      //判断当前步骤名称
      if (hides.length > 0) {
        this.$refs.kfb.hide(hides);
      }
      if (disableds.length > 0) {
        this.$refs.kfb.disable(disableds);
      }
      if (batchDelete.length > 0) {
        this.$refs.kfb.batchDelete(batchDelete);
      }
      if (batchCopy.length > 0) {
        this.$refs.kfb.batchCopy(batchCopy);
      }

      var rules = this.workflow.props.column.filter(
        (f) => f.validator.length > 0
      );
      if (rules.length > 0) {
        this.$refs.kfb.rule(rules);
      }
    },

    /**
     * 解析filter，返回替换后表达式
     */
    analysisFilter(filter) {
      let that = this;
      let copyFilters = this.$utils.clone(filter, true);
      let filters = {
        groupOp: copyFilters.groupOp,
        rules: [],
        groups: [],
      };
      // 递归遍历控件树
      const traverseRule = (array, filters) => {
        array.forEach((element) => {
          switch (element.type) {
            case "correlationRecord":
              filters.rules.push({
                field: element.field,
                op: element.op,
                data: that.analysisFilterData({
                  type: "correlationRecord",
                  data: element.data,
                }),
              });

              break;
            default:
              filters.rules.push({
                field: element.field,
                op: element.op,
                data: that.analysisFilterData(element),
              });
              break;
          }
        });
      };
      traverseRule(copyFilters.rules, filters);

      const traverseGroup = (array, filters) => {
        array.forEach((element) => {
          var group = {
            groupOp: element.groupOp,
            rules: [],
            groups: [],
          };

          element.rules.forEach((rule) => {
            switch (rule.type) {
              case "correlationRecord":
                if (rule.data.length > 0) {
                  var selectFilters = {
                    groupOp: "OR",
                    rules: [],
                  };
                  //循环值
                  rule.data.forEach((ditem) => {
                    selectFilters.rules.push({
                      field: rule.field.replace("_Txt", ""),
                      op: rule.op,
                      data: that.analysisFilterData({
                        type: "correlationRecord",
                        data: ditem,
                      }),
                    });
                  });
                  group.groups.push(selectFilters);
                }
                break;
              default:
                group.rules.push({
                  field: rule.field,
                  op: rule.op,
                  data: that.analysisFilterData(rule),
                });
                break;
            }
          });

          filters.groups.push(group);

          if (element.groups.length > 0) {
            traverseGroup(element.groups, filters);
          }
        });
      };
      traverseGroup(copyFilters.groups, filters);
      return filters;
    },

    /**
     * 解析数据
     * @param {*} data
     */
    analysisFilterData(rule) {
      var that = this;
      var column = this.$utils.find(that.columnJson, (f) => f.key == rule.data);
      switch (rule.type) {
        case "input":
          return rule.data;
        case "correlationRecord":
          return this.formValue[column.model];
      }
    },
  },
};
</script>

<style lang="less" scoped>
/deep/ .ant-modal-body {
  padding: 0;
}

.title {
  font-weight: 600 !important;
  font-size: 16px;
  color: #304265;
}

.menu-collapse {
  position: absolute;
  top: 50%;
  width: 10px;
  height: 40px;
  line-height: 40px;
  border-radius: 0 2px 2px 0;
  transform: translateY(-50%);
  background: #e0e3e9;

  .ico-button {
    height: 100%;
    width: 100%;
    cursor: pointer;
  }
}

.eip-form-designer-container .content.toolbars-top,
.eip-form-build .content.toolbars-top {
  padding-bottom: 6px;
  height: calc(100% - 114px);
}

.eip-form-designer-container .operating-area,
.eip-form-build .operating-area {
  border-bottom: 1px solid #e8e8e8;
}

.eip-form-designer-container .content section {
  box-shadow: 0px 0px 1px 1px #e8e8e8;
}

.eip-form-designer-container .content aside {
  box-shadow: 0px 0px 1px 1px #e8e8e8;
}

.eip-form-build {
  overflow: auto;
  height: 100%;
  padding: 0px;
}

/deep/ .right .ant-tabs-bar {
  margin: 0 !important;
}

.body {
  padding: 4px;

  .left {
    float: right;
    width: 300px;
    padding: 10px;
    margin: 0 0 10px 4px;
    background: #fff;
  }

  .right {
    overflow: auto;
  }

  .form {
    width: 98%;
    margin: 0 auto;
    border-width: 1px;
    border-style: solid;
    background-color: rgba(255, 255, 255, 1);
    border-color: rgba(233, 233, 233, 1);
    border-radius: 5px;
    -moz-box-shadow: none;
    -webkit-box-shadow: none;
    box-shadow: none;
    font-family: "微软雅黑", sans-serif;
    font-weight: 400;
    font-style: normal;
    text-align: left;
    line-height: 20px;
    padding: 30px 10px;
    margin-top: 10px;
  }
}

/deep/ table .ant-form-item {
  margin: 0 !important;
}

.invitationReadApproveDetail {
  padding: 10px;
  background-color: rgba(242, 242, 242, 1);
  box-sizing: border-box;
  border-width: 1px;
  border-style: solid;
  border-color: rgba(242, 242, 242, 1);
  border-radius: 10px;
  -moz-box-shadow: none;
  -webkit-box-shadow: none;
  box-shadow: none;
  font-weight: 400;
  font-style: normal;
  text-align: left;
  line-height: 20px;
}

.workflowCard:hover {
  box-shadow: 0 2px 10px rgb(0 0 0 / 8%);
}

/deep/.ant-timeline-item-content {
  margin: 0 0 0 30px !important;
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

.ant-modal-root::v-deep .ant-modal-body::-webkit-scrollbar {
  width: 6px;
  height: 6px;
}

.ant-modal-root::v-deep .ant-modal-body::-webkit-scrollbar-thumb {
  border-radius: 5px;
  -webkit-box-shadow: inset 0 0 5px rgb(0 0 0 / 20%);
  box-shadow: inset 0 0 5px rgb(0 0 0 / 20%);
  background: rgba(0, 0, 0, 0.2);
  scrollbar-arrow-color: red;
}

.ant-modal-root::v-deep .ant-modal-body::-webkit-scrollbar-track {
  -webkit-box-shadow: inset 0 0 5px rgb(0 0 0 / 20%);
  box-shadow: inset 0 0 5px rgb(0 0 0 / 20%);
  border-radius: 0;
  background: rgba(0, 0, 0, 0.1);
}

.close {
  color: #ff4d4f !important;
  cursor: pointer;

  .text {
    font-size: 14px;
  }
}

.content {
  background: #fff;
  height: 100%;
  border: none;
}

.logBox {
  box-sizing: border-box;
  padding: 6px 10px 0;
  padding-left: 10px !important;
}

.logBox .paddingLeft27 {
  padding-left: 7px;
}

.logBox .moreLogData {
  cursor: pointer;
  margin-left: 28px;
}

.logBox .selectTriggerChildAvatar .accountName {
  border-radius: 5px;
  cursor: pointer;
  padding: 3px 5px;
}

.logBox .selectTriggerChildAvatar .accountName:hover {
  background: #f7f7f7;
  color: #2196f3;
}

.logBox .selectTriggerChildAvatar .accountName.mobileAccountName:hover {
  background: #fff;
  color: #9e9e9e;
}

.logBox .selectTriggerChild {
  align-items: center;
  border-radius: 5px;
  cursor: pointer;
  display: flex;
  padding: 3px 5px;
}

.logBox .selectTriggerChild.hasHover:hover {
  background: #f7f7f7;
  color: #2196f3;
}

.logBox .selectTriggerChild.hasHover:hover .icon {
  color: #2196f3 !important;
}

.logBox .worksheetRocordLogCard {
  background-color: #fff;
  border-radius: 5px;
  box-shadow: 0 1px 2px 0 rgba(0, 0, 0, 0.06);
  margin-bottom: 16px;
  padding: 16px;
}

.logBox .worksheetRocordLogCard .worksheetRocordLogCardTopBox {
  display: flex;
  justify-content: space-between;
  margin-bottom: 8px;
}

.logBox .worksheetRocordLogCard .worksheetRocordLogCardTopBox span {
  line-height: 20px;
}

.logBox
  .worksheetRocordLogCard
  .worksheetRocordLogCardTopBox
  .worksheetRocordLogCardTitleAvatar {
  height: auto !important;
  margin-right: 2px;
}

.logBox
  .worksheetRocordLogCard
  .worksheetRocordLogCardTopBox
  .worksheetRocordLogCardTitleAvatar
  img {
  margin-top: -4px;
}

.logBox
  .worksheetRocordLogCard
  .worksheetRocordLogCardTopBox
  .titleAvatarText
  a {
  color: #333;
}

.logBox
  .worksheetRocordLogCard
  .worksheetRocordLogCardTopBox
  .titleAvatarText
  a:hover {
  color: #40a9ff;
}

.logBox .worksheetRocordLogCard .worksheetRocordLogCardHrTime {
  border-top: 1px solid #eaeaea;
  color: #9e9e9e;
  display: flex;
  justify-content: space-between;
  margin-left: 7px;
  margin-top: 16px;
  padding-top: 16px;
}

.logBox .worksheetRocordLogCard:last-child .worksheetRocordLogCardTopBox {
  margin-bottom: 0;
}

.logBox .loadOldLog {
  color: #9e9e9e;
  font-size: 12px;
  margin: 16px 0;
  text-align: center;
}

.logBox .loadOldLog span:hover {
  cursor: pointer;
  font-size: #1890ff;
}

.logBox .logDivider {
  color: #9e9e9e;
  font-size: 12px;
}

.logBox .worksheet-rocord-log-item {
  margin-top: 4px;
}

.logBox .worksheet-rocord-log-item .widgetTitle {
  align-items: center;
  display: flex;
  font-size: 13px;
  margin-left: 2px;
  margin-right: 8px;
}

.logBox .worksheet-rocord-log-item .widgetTitle > span:first-child {
  font-weight: 700;
}

.logBox .worksheet-rocord-log-item .widgetTitle .icon {
  display: inline-block;
  vertical-align: middle;
  width: 20px;
}

.logBox .hideEle {
  display: none;
}

.logBox.mobileLogBox {
  font-size: 13px;
  padding: 10px 10px 0 !important;
}

.logBox.mobileLogBox .worksheetRocordLogCardTopBox {
  font-size: 12px;
}

.logBox.mobileLogBox .worksheet-rocord-log-item {
  padding: 0;
  text-align: justify;
}

.worksheetRocordLog {
  background: #fafafa;
}

.worksheetRecordLog .oldBackground {
  background: #feeaec;
  text-decoration: line-through;
}

.worksheetRecordLog .newBackground {
  background: #dff8ea;
}

.worksheetRecordLog .defaultBackground {
  background: #f0f0f0;
}

.WorksheetRocordLogSelectTags {
  display: flex;
  flex-wrap: wrap;
  justify-content: left;
}

.WorksheetRocordLogSelectTags .WorksheetRocordLogSelectTag {
  -webkit-line-clamp: 5;
  -webkit-box-orient: vertical;
  word-wrap: break-word;
  border-radius: 3px;
  cursor: pointer;
  font-size: 13px;
  height: auto;
  line-height: 20px;
  margin-bottom: 4px;
  margin-right: 4px;
  overflow: hidden;
  padding: 0 8px;
  text-overflow: ellipsis;
  width: fit-content;
}

.WorksheetRocordLogSelectTags .hoverHighline {
  cursor: pointer;
}

.WorksheetRocordLogSelectTags .hoverHighline:hover {
  color: #0d47a1;
}

.WorksheetRocordLogSelectTags .noneTextLineThrough {
  text-decoration: none !important;
}

.worksheetRecordLog .flexDirectionRever {
  flex-direction: row-reverse;
}

.worksheetRecordLog .oldValue {
  background: #feeaec;
  text-decoration: line-through;
}

.worksheetRecordLog .newValue {
  background: #dff8ea;
}

.worksheetRecordLog .defaultValue {
  background: rgba(0, 0, 0, 0.06);
}

/deep/ .ant-card {
  height: auto !important;
}
</style>
