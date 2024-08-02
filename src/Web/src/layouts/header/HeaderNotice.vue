<template>
  <div>
    <a-dropdown :trigger="['click']" v-model="show">
      <div slot="overlay">
        <a-spin :spinning="loading">
          <a-tabs
            class="dropdown-tabs"
            :tabBarStyle="{ textAlign: 'center' }"
            :style="{ width: '297px' }"
          >
            <a-tab-pane tab="待处理" key="1">
              <a-list class="tab-pane">
                <a-list-item
                  style="cursor: pointer"
                  @click="noticeDetail(item)"
                  v-for="(item, index) in data"
                  :key="index"
                >
                  <a-list-item-meta
                    :title="item.message"
                    :description="item.createTimeFormat"
                  >
                    <a-avatar
                      v-if="item.openUrl == 'workflowView'"
                      style="background-color: #f5222d"
                      slot="avatar"
                      icon="audit"
                    />

                    <a-avatar
                      v-else-if="item.openUrl == 'workflwCuiBan'"
                      style="background-color: #2db7f5"
                      slot="avatar"
                      icon="reconciliation"
                    />

                    <a-avatar
                      v-else
                      style="background-color: #108ee9"
                      slot="avatar"
                      icon="notification"
                    />
                    {{ item.openUrl }}
                  </a-list-item-meta>
                </a-list-item>
              </a-list>
            </a-tab-pane>
            <a-tab-pane tab="已处理" key="2">
              <div
                style="max-height: 400px; overflow-y: auto"
                class="beauty-scroll"
              >
                <a-list class="tab-pane">
                  <a-list-item
                    style="cursor: pointer"
                    @click="noticeDetail(item)"
                    v-for="(item, index) in haveDoData"
                    :key="index"
                  >
                    <a-list-item-meta
                      :title="item.message"
                      :description="item.createTimeFormat"
                    >
                      <a-avatar
                        style="background-color: #f5222d"
                        slot="avatar"
                        icon="mail"
                      />
                    </a-list-item-meta>
                  </a-list-item>
                </a-list>
              </div>
              <!-- <a-divider style="margin: 0" />
              <a-row type="flex">
                <a-col :span="12" @click="messageReadAll">
                  <div class="cell-content">清空通知</div>
                </a-col>
                <a-col :span="12" @click="messageLogAll">
                  <div class="cell-content">查看更多</div>
                </a-col>
              </a-row> -->
            </a-tab-pane>
            <!-- <a-tab-pane tab="消息" key="2">
            <a-list class="tab-pane"></a-list>
          </a-tab-pane>
          <a-tab-pane tab="待办" key="3">
            <a-list class="tab-pane"></a-list>
          </a-tab-pane> -->
          </a-tabs>
        </a-spin>
      </div>
      <span class="header-notice">
        <a-badge class="notice-badge" :count="total">
          <a-icon :class="['header-notice-icon']" type="bell" />
        </a-badge>
      </span>
    </a-dropdown>
    <workflow-view
      ref="workflowView"
      v-if="workflowDo.visible"
      :visible.sync="workflowDo.visible"
      :title="workflowDo.title"
      :isWorkflow="true"
      :workflowData="workflowDo.data"
      @ok="readMessage"
    ></workflow-view>

    <a-modal
      v-drag
      width="1000px"
      okText="已读"
      cancelText="关闭"
      :title="message.title"
      :visible="message.visible"
      @ok="readMessage"
      @cancel="message.visible = false"
    >
      <p>{{ message.message }}</p>
      <br />
      <div style="text-align: right">{{ message.createTime }}</div>
    </a-modal>
  </div>
</template>

<script>
import workflowView from "@/pages/system/agile/run/edit";
import {
  query,
  findById,
  read,
  menuAgile,
} from "@/services/system/messagelog/header";
import { mapMutations, mapGetters } from "vuex";
import { removeAuthorization } from "@/utils/request";
import * as signalR from "@microsoft/signalr";
export default {
  name: "HeaderNotice",
  components: {
    workflowView,
  },
  data() {
    return {
      loading: false,
      show: false,
      connection: null,
      total: 0,
      data: [],
      haveDoData: [],
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

      messageId: null,

      message: {
        visible: false,
        title: "",
        message: "",
        createTime: "",
      },

      isOffline: false, //是否单点登录踢下线
    };
  },
  computed: {
    ...mapGetters("account", ["user"]),
  },
  created() {
    this.initMenuAgile();
    this.initNotice();
    this.initSignalr();
  },
  beforeDestroy(){
    if(this.connection){
      this.connection.stop();
    }
  },
  methods: {
    ...mapMutations("account", ["setSystemAgile"]),
    /**
     *
     */
    initMenuAgile() {
      let that = this;
      menuAgile({}).then(function (result) {
        if (result.code === that.eipResultCode.success) {
          that.setSystemAgile(result.data);
        }
      });
    },

    /**
     * 初始化通知
     */
    initNotice() {
      let that = this;
      query({
        current: 1,
        size: 999,
        haveDo: false,
      }).then(function (result) {
        that.total = result.total;
        that.data = result.data;
      });

      query({
        current: 1,
        size: 999,
        haveDo: true,
      }).then(function (result) {
        that.haveDoData = result.data;
      });
    },

    /**
     * 初始化连接器
     */
    async initSignalr() {
      let that = this;
      //得到缓存连接Id
      var BASE_URL = window.SITE_CONFIG.eip.baseURL;
      this.connection = new signalR.HubConnectionBuilder()
        .withUrl(BASE_URL+"/eiphub", {
          skipNegotiation: true,
          transport: signalR.HttpTransportType.WebSockets,
        })
        .configureLogging(signalR.LogLevel.Error)
        .withAutomaticReconnect()
        .build();

      //踢下线
      this.connection.on("CompulsoryDownline", (user) => {
        localStorage.removeItem(process.env.VUE_APP_ROUTES_KEY);
        localStorage.removeItem(process.env.VUE_APP_PERMISSIONS_KEY);
        localStorage.removeItem(process.env.VUE_APP_ROLES_KEY);
        removeAuthorization();

        that.connectionStop();
        this.$error({
          keyboard: false,
          title: "下线提醒",
          content: "当前您的帐号已在其他浏览器登录,若非本人操作,请及时修改密码",
          okText: "确定",
          okType: "danger",

          onOk() {
            that.$router.push("/login");
          },
        });
      });

      this.connection.on("SendMessageAll", (user, message) => {});

      //站点消息
      this.connection.on("SendWebSiteMessage", (message) => {
        const key = `open${Date.now()}`;
        if (message) {
          var data = JSON.parse(message);
          that.$notification["info"]({
            message: data.Title,
            description: data.Message,
            placement: "bottomRight",
            btn: (h) => {
              return h(
                "a-button",
                {
                  props: {
                    type: "primary",
                    size: "small",
                  },
                  on: {
                    click: () => {
                      that.noticeDetail({
                        messageLogId: data.MessageLogId,
                      });
                    },
                  },
                },
                "立即查看"
              );
            },
            key,
            onClose: close,
          });

          that.initNotice();
        }
      });

      //启动
      await this.start();

      //(默认4次重连) 全部都失败后，调用，状态为：Disconnected。
      this.connection.onclose(() => {
        if (!that.isOffline) {
          that.$notification.destroy();
          that.$notification["error"]({
            placement: "bottomRight",
            message: "网络提示",
            icon: <a-icon type="loading" />,
            description: "连接断开#4",
          });
        }
      });

      //重连之前调用 （只有在掉线的一瞬间，只进入一次），状态为：Reconnecting
      this.connection.onreconnecting(() => {
        if (!that.isOffline) {
          this.$notification.destroy();
          this.$notification["error"]({
            placement: "bottomRight",
            message: "网络提示掉线",
            icon: <a-icon type="loading" />,
            description: "连接断开#1",
          });
        }
      });

      //(默认4次重连)，任何一次只要回调成功，调用，状态为：Connected
      this.connection.onreconnected(async () => {
        this.$notification.destroy();
        this.$notification["success"]({
          placement: "bottomRight",
          message: "网络提示",
          description: "网络已连接",
        });

        //启动
        await that.start();
      });
    },

    /**
     * 启动
     */
    async start() {
      try {
        await this.connection.start();
        if (this.connection.state === signalR.HubConnectionState.Connected) {
          this.$notification.destroy();
          this.connection.invoke("SendUserOnline", this.user);
        }
      } catch (err) {
        if (this.connection.state === signalR.HubConnectionState.Disconnected) {
          this.$notification.destroy();
          this.$notification["error"]({
            placement: "bottomRight",
            message: "网络提示",
            icon: <a-icon type="loading" />,
            description: "启动连接断开#S",
          });
        }
        if (this.connection.state === signalR.HubConnectionState.Connected) {
          this.$notification.destroy();
          this.connection.invoke("SendUserOnline", this.user);
        }
      }
    },

    /**
     * 下线
     */
    connectionStop() {
      if (this.connection) {
        this.isOffline = true;
        this.connection.stop();
      }
    },

    /**
     *
     */
    messageLogAll() {
      this.$openPage("/pages/system/messagelog/list", "所有消息");
    },

    /**
     * 阅读所有
     */
    messageReadAll() {
      let that = this;
      this.show = false;
      if (this.data.length > 0) {
        that.$loading.show({ text: "处理中..." });
        var ids = this.data.map((f) => f.messageLogId).join(",");
        read({
          messageLogIds: ids,
        }).then((result) => {
          that.initNotice();
          that.$loading.hide();
        });
      }
    },

    /**
     *
     * @param {*} messageLog
     */
    noticeDetail(messageLog) {
      let that = this;
      this.show = false;
      that.$message.loading("查询中...", 0);
      that.messageId = messageLog.messageLogId;
      //读取消息明细
      findById(that.messageId).then((result) => {
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
        } else if (row.openUrl == "workflwCuiBan") {
          that.workflowDo.title = "处理流程-" + row.title;
          var workflowData = JSON.parse(row.customerData);
          that.workflowDo.data = {
            processInstanceId: workflowData.ProcessInstanceId,
            activityId: workflowData.ActivityId,
            taskId: workflowData.TaskId,
            type: that.eipWorkflowDoType["审核"],
          };
          that.workflowDo.visible = true;
        } else {
          that.message.title = row.title;
          that.message.createTime = row.createTime;
          that.message.message = row.message;
          this.message.visible = true;
        }
        that.$message.destroy();
      });
    },

    /**
     * 流程完成
     */
    readMessage() {
      let that = this;
      that.$message.loading("处理中...", 0);
      read({
        messageLogIds: that.messageId,
      }).then((result) => {
        that.initNotice();
        that.message.visible = false;
        that.$message.destroy();
      });
    },
  },
};
</script>

<style lang="less">
.header-notice {
  display: inline-block;
  transition: all 0.3s;

  span {
    vertical-align: initial;
  }

  .ant-badge-count {
    top: 2px !important;
  }

  .notice-badge {
    color: inherit;

    .header-notice-icon {
      font-size: 16px;
      padding: 4px;
    }
  }
}

.dropdown-tabs {
  background-color: @base-bg-color;
  box-shadow: 0 2px 8px @shadow-color;
  border-radius: 4px;

  .tab-pane {
    padding: 0 24px 12px;
    min-height: 250px;
  }
}

.cell-content {
  line-height: 46px;
  text-align: center;
  transition: background-color 0.3s;
  cursor: pointer;
  color: inherit;
}

.cell-content:hover {
  background: hsla(0, 0%, 60%, 0.05);
}
</style>
