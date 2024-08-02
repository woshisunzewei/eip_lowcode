<template>
  <a-modal width="1100px" v-drag :destroyOnClose="true" :maskClosable="false" :title="title" :visible="visible"
    :bodyStyle="{ padding: '1px', height: '600px' }" @cancel="cancel">
    <a-spin :spinning="spinning">
      <a-tabs default-active-key="1" @change="changeTab">
        <a-tab-pane key="1" :forceRender="true">
          <span slot="tab"> <a-icon type="form" />基本 </span>
          <div class="tab-margin">
            <a-form-model ref="form" :model="base" :rules="rules" :label-col="config.labelCol"
              :wrapper-col="config.wrapperCol">
              <a-row>
                <a-col>
                  <a-form-model-item label="名称" prop="title">
                    <a-input v-model="base.title" placeholder="请输入名称" allow-clear />
                  </a-form-model-item>
                  <!-- <a-form-model-item label="表单" prop="formId">
                      <a-select placeholder="请选择表单" allow-clear v-model="base.formId" @change="formChange">
                        <a-select-option v-for="(item, i) in forms" :key="i" :value="item.configId">{{ item.name
                        }}</a-select-option>
                      </a-select>
                    </a-form-model-item> -->
                </a-col>
              </a-row>
            </a-form-model>
          </div>
        </a-tab-pane>

        <a-tab-pane key="5" :forceRender="true">
          <span slot="tab"> <a-icon type="sound" />通知 </span>
          <div class="tab-margin">
            <vxe-table ref="tableNotice" id="workflowactivitynotice" size="small" :height="notice.height"
              :data="notice.data">
              <vxe-column type="seq" title="序号" align="center" width="60"></vxe-column>
              <vxe-column width="80" align="center">
                <template #header>
                  <a-button type="primary" size="small" @click="noticeAdd">
                    <a-icon type="plus" />
                  </a-button>
                </template>

                <template #default="{ row }">
                  <a-popconfirm title="确定删除通知?" ok-text="确定" cancel-text="取消" @confirm="noticeDel(row)">
                    <a-button @click.stop="" size="small" type="danger">
                      <a-icon type="delete" />
                    </a-button>
                  </a-popconfirm>
                </template>
              </vxe-column>
              <vxe-column field="remark" title="备注" width="150">

                <template v-slot="{ row }">
                  <a-input placeholder="请输入备注" v-model="row.remark" />
                </template>
              </vxe-column>
              <vxe-column field="use" title="启用" width="60" align="center">

                <template v-slot="{ row }">
                  <a-switch checked-children="是" un-checked-children="否" v-model="row.use" />
                </template>
              </vxe-column>
              <vxe-column field="trigger" title="触发类型" align="center" width="180">

                <template v-slot="{ row }">
                  <a-select v-model="row.trigger">
                    <a-select-option :value="0">下一步成功</a-select-option>
                    <a-select-option :value="1">退回成功</a-select-option>
                    <a-select-option :value="2">拒绝成功</a-select-option>
                  </a-select>
                </template>
              </vxe-column>
              <vxe-column title="设置" align="center">

                <template v-slot="{ row }">
                  <a-space size="small">
                    <a-select style="width: 160px" v-model="row.type" @change="noticeTypeChange(row)">
                      <a-select-option :value="0">邮件</a-select-option>
                      <a-select-option :value="1">站内</a-select-option>
                      <a-select-option :value="2">短信</a-select-option>
                      <a-select-option :value="3">微信公众号</a-select-option>
                      <a-select-option :value="4">微信小程序</a-select-option>
                      <a-select-option :value="5">企业微信</a-select-option>
                      <a-select-option :value="7">钉钉</a-select-option>
                      <a-select-option :value="8">App</a-select-option>
                    </a-select>
                    <a-badge :dot="row.config">
                      <a-button @click="noticeSetting(row)">
                        <a-icon type="setting" />
                      </a-button>
                    </a-badge>

                  </a-space>
                </template>
              </vxe-column>
            </vxe-table>
          </div>
        </a-tab-pane>

        <a-tab-pane key="7" :forceRender="true">
          <span slot="tab"> <a-icon type="file-done" />事件 </span>
          <div class="tab-margin">
            <vxe-table ref="tableEvent" id="workflowactivitynotice" size="small" :height="event.height"
              :data="event.data">
              <vxe-column type="seq" title="序号" align="center" width="60"></vxe-column>
              <vxe-column width="80" align="center">

                <template #header>
                  <a-button size="small" type="primary" @click="eventAdd">
                    <a-icon type="plus" />
                  </a-button>
                </template>

                <template #default="{ row }">
                  <a-popconfirm title="确定删除事件?" ok-text="确定" cancel-text="取消" @confirm="eventDel(row)">
                    <a-button @click.stop="" size="small" type="danger">
                      <a-icon type="delete" />
                    </a-button>
                  </a-popconfirm>
                </template>
              </vxe-column>
              <vxe-column field="remark" title="备注" width="220">

                <template v-slot="{ row }">
                  <a-input placeholder="请输入事件备注信息" v-model="row.remark" />
                </template>
              </vxe-column>
              <vxe-column field="use" title="启用" width="60" align="center">

                <template v-slot="{ row }">
                  <a-switch checked-children="是" un-checked-children="否" v-model="row.use" />
                </template>
              </vxe-column>
              <vxe-column title="触发类型" align="center" width="210">

                <template v-slot="{ row }">
                  <a-space size="small">
                    <a-select style="width: 180px" v-model="row.type">
                      <a-select-option :value="0">下一步前</a-select-option>
                      <a-select-option :value="1">下一步后</a-select-option>
                      <a-select-option :value="2">退回前</a-select-option>
                      <a-select-option :value="3">退回后</a-select-option>
                      <a-select-option :value="4">拒绝前</a-select-option>
                      <a-select-option :value="5">拒绝后</a-select-option>
                      <a-select-option :value="6">强制结束前</a-select-option>
                      <a-select-option :value="7">强制结后后</a-select-option>
                    </a-select>
                  </a-space>
                </template>
              </vxe-column>
              <vxe-column title="设置" align="center" width="200">

                <template v-slot="{ row }">
                  <a-space size="small">
                    <a-select @change="eventTypeChange(row)" style="width: 120px" v-model="row.type">
                      <a-select-option :value="0">JS</a-select-option>
                      <a-select-option :value="1">接口</a-select-option>
                    </a-select>
                    <a-badge :dot="row.config">
                      <a-button @click="eventSetting(row)">
                        <a-icon type="setting" />
                      </a-button>
                    </a-badge>

                  </a-space>
                </template>
              </vxe-column>
            </vxe-table>
          </div>
        </a-tab-pane>
      </a-tabs>
    </a-spin>

    <template slot="footer">
      <a-space>
        <a-button key="back" @click="cancel" :disabled="loading"><a-icon type="close" />取消</a-button>
        <a-button key="submit" @click="save" type="primary" :loading="loading"><a-icon type="save" />提交</a-button>
      </a-space>
    </template>

    <eip-workflow-event-js ref="eventjs" v-if="event.setting.js.visible" :visible.sync="event.setting.js.visible"
      :title="event.setting.js.title" :edit="event.setting.value" @ok="eventOk"></eip-workflow-event-js>

    <eip-workflow-event-interface ref="eventinterface" v-if="event.setting.interface.visible"
      :visible.sync="event.setting.interface.visible" :title="event.setting.interface.title" :edit="event.setting.value"
      @ok="eventOk"></eip-workflow-event-interface>

    <eip-workflow-notice-email ref="noticeEmail" v-if="notice.setting.email.visible"
      :visible.sync="notice.setting.email.visible" :column="column.data" :edit="notice.setting.value" title="邮件通知"
      @ok="noticeOk"></eip-workflow-notice-email>

    <eip-workflow-notice-ding-talk ref="noticeDingTalk" v-if="notice.setting.dingtalk.visible"
      :visible.sync="notice.setting.dingtalk.visible" :column="column.data" :edit="notice.setting.value" title="钉钉通知"
      @ok="noticeOk"></eip-workflow-notice-ding-talk>

    <eip-workflow-notice-system ref="noticeSystem" v-if="notice.setting.system.visible"
      :visible.sync="notice.setting.system.visible" :column="column.data" :edit="notice.setting.value" title="站内通知"
      @ok="noticeOk"></eip-workflow-notice-system>

    <eip-workflow-notice-sms ref="noticeSms" v-if="notice.setting.sms.visible"
      :visible.sync="notice.setting.sms.visible" :column="column.data" :edit="notice.setting.value" title="短信通知"
      @ok="noticeOk"></eip-workflow-notice-sms>

    <eip-workflow-notice-wei-xin-work ref="noticeWeiXinOffiAccount" v-if="notice.setting.weixinwork.visible"
      :visible.sync="notice.setting.weixinwork.visible" :column="column.data" :edit="notice.setting.value"
      title="企业微信通知" @ok="noticeOk"></eip-workflow-notice-wei-xin-work>

    <eip-workflow-notice-wei-xin-mini-program ref="noticeWeiXinMiniProgram"
      v-if="notice.setting.weixinminiprogram.visible" :visible.sync="notice.setting.weixinminiprogram.visible"
      :column="column.data" :edit="notice.setting.value" title="微信小程序"
      @ok="noticeOk"></eip-workflow-notice-wei-xin-mini-program>

    <eip-workflow-notice-wei-xin-mp ref="noticeWeiXinMp" v-if="notice.setting.weixinmp.visible"
      :visible.sync="notice.setting.weixinmp.visible" :column="column.data" :edit="notice.setting.value" title="微信公众号通知"
      @ok="noticeOk"></eip-workflow-notice-wei-xin-mp>

    <eip-workflow-notice-app ref="noticeApp" v-if="notice.setting.app.visible"
      :visible.sync="notice.setting.app.visible" :column="column.data" :edit="notice.setting.value" title="App通知"
      @ok="noticeOk"></eip-workflow-notice-app>

  </a-modal>
</template>

<script>
import eipWorkflowEventJs from "../components/event/js";
import eipWorkflowEventInterface from "../components/event/interface";
import eipWorkflowNoticeEmail from "../components/notice/email";
import eipWorkflowNoticeSystem from "../components/notice/system";
import eipWorkflowNoticeSms from "../components/notice/sms";
import eipWorkflowNoticeApp from "../components/notice/app";
import eipWorkflowNoticeWeiXinWork from "../components/notice/weixinwork";
import eipWorkflowNoticeWeiXinMiniProgram from "../components/notice/weixinminiprogram";
import eipWorkflowNoticeWeiXinMp from "../components/notice/weixinmp";
import eipWorkflowNoticeDingTalk from "../components/notice/dingtalk";
import {
  findForm,
  findFormColumns
} from "@/services/workflow/process/activity/task";


export default {
  name: "activityEnd",
  components: {
    eipWorkflowEventJs,
    eipWorkflowEventInterface,
    eipWorkflowNoticeEmail,
    eipWorkflowNoticeSystem,
    eipWorkflowNoticeSms,
    eipWorkflowNoticeApp,
    eipWorkflowNoticeWeiXinWork,
    eipWorkflowNoticeWeiXinMiniProgram,
    eipWorkflowNoticeWeiXinMp,
    eipWorkflowNoticeDingTalk,
  },
  data() {
    return {
      modal: {
        bodyStyle: {
          padding: "1px",
        },
      },
      loading: false,
      spinning: false,
      config: {
        labelCol: {
          span: 3,
        },
        wrapperCol: {
          span: 19,
        },
      },
      rules: {
        title: [
          {
            required: true,
            message: "请输入名称",
            trigger: "blur",
          },
        ],
        formId: [
          {
            required: true,
            message: "请选择表单",
            trigger: ["blur", "change"],
          },
        ],
      },
      base: {
        title: "",
        formId: undefined,
        formName: "",
      },

      column: {
        height: "500px",
        data: [],
        row: {},
      },
      notice: {
        height: "500px",
        data: [],
        setting: {
          value: null,
          email: {
            visible: false,
            title: "",
          },
          system: {
            visible: false,
            title: "",
          },
          sms: {
            visible: false,
            title: "",
          },
          weixinminiprogram: {
            visible: false,
            title: "",
          },
          weixinmp: {
            visible: false,
            title: "",
          },
          weixinwork: {
            visible: false,
            title: "",
          },
          app: {
            visible: false,
            title: "",
          },
          dingtalk: {
            visible: false,
            title: "",
          },
        },
      },

      event: {
        height: "500px",
        data: [],
        setting: {
          value: null,
          js: {
            visible: false,
            title: "",
          },
          interface: {
            visible: false,
            title: "",
          },
        },
      },
      forms: [], //节点配置表单
      obj: {},
    };
  },
  props: {
    visible: {
      type: Boolean,
      default: false,
    },
    data: {
      type: Object,
    },
    title: {
      type: String,
    },
  },
  mounted() {
    this.columnInit();
    this.eventInit();
    this.noticeInit();
    this.formInit();
  },
  methods: {
    /**
     * 表单切换
     */
    formChange(id) {
      let that = this;
      if (id) {
        findFormColumns(id).then(function (result) {
          if (result.code === that.eipResultCode.success) {
            result.data.forEach((item) => {
              item.isRead = true;
              item.isWrite = true;
              item.isDelete = true;
              item.isDefault = false;
              item.validator = [];
            });
            that.column.data = result.data;
          }
        });
      } else {
        this.column.data = [];
      }
    },
    /**
     *选项卡改变事件
     */
    changeTab(activeKey) { },

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
      this.$refs.form.validate((valid, error) => {
        if (valid) {
          that.loading = true;
          that.spinning = true;
          that.data.props.base = that.base;
          that.data.text.text = that.base.title;
          that.data.props.event = that.event.data;
          that.data.props.notice = that.notice.data;
          this.$emit("ok", that.data);
          this.$emit("update:visible", false);
        } else {
          let messages = [];
          for (const key in error) {
            messages.push(error[key][0].message);
          }
          that.$message.error(messages.join(","));
        }
      });
    },

    /**
     * 初始化表单
     */
    formInit() {
      let that = this;
      findForm({
        configType: 2,
      }).then(function (result) {
        if (result.code === that.eipResultCode.success) {
          that.forms = result.data;
          //其他信息
          if (that.data) {
            that.base = that.$utils.clone(that.data.props.base, true);
          }
        }
      });
    },

    /**
     * 字段初始化
     */
    columnInit() {
      let that = this;
      if (that.data) {
        //默认勾选
        that.column.data = that.$utils.clone(that.data.props.column, true);
      }
    },

    /**
     * 通知初始化
     */
    noticeInit() {
      let that = this;
      if (that.data) {
        that.notice.data = that.$utils.clone(that.data.props.notice, true);
      }
    },

    /**
     * 添加通知
     */
    noticeAdd() {
      this.notice.data.push({
        remark: "",
        use: true, //是否启用
        trigger: 0, //触发类型:下一步成功,退回成功,拒绝成功
        type: 0, //通知类型:邮件,站内,短信,微信,钉钉,QQ,
        config: null, //具体配置
      });
    },
    /**
     *删除
     */
    noticeDel(row) {
      var delIndex = -1;
      this.notice.data.forEach((item, index) => {
        if (item._X_ROW_KEY == row._X_ROW_KEY) {
          delIndex = index;
        }
      });
      if (delIndex != -1) {
        this.notice.data.splice(delIndex, 1);
      }
    },

    /**
     *
     */
    noticeTypeChange(row) {
      row.config = null;
    },

    /**
     * 消息设置
     */
    noticeSetting(row) {
      this.notice.setting.value = row;
      switch (row.type) {
        case 0:
          this.notice.setting.email.visible = true;
          break;
        case 1:
          this.notice.setting.system.visible = true;
          break;
        case 2:
          this.notice.setting.sms.visible = true;
          break;
        case 8:
          this.notice.setting.app.visible = true;
          break;
      }
    },

    /**
     * 消息设置成功
     * @param {*} value
     */
    noticeOk(value) {
      this.notice.setting.value.config = value;
    },
    /**
     * 事件通知初始化
     */
    eventInit() {
      let that = this;
      if (that.data) {
        that.event.data = that.$utils.clone(that.data.props.event, true);
      }
    },

    /**
     * 事件通知
     */
    eventAdd() {
      this.event.data.push({
        remark: "",
        use: true, //是否启用
        trigger: 0, //触发类型:下一步开始前,下一步开始后,退回开始前,退回开始后,拒绝开始前,拒绝开始后
        type: 0, //通知类型:JS,接口
        config: null, //具体配置
      });
    },

    /**
     *
     */
    eventOk(value) {
      this.event.setting.value.config = value;
    },

    /**
     * 删除
     */
    eventDel(row) {
      var delIndex = -1;
      this.event.data.forEach((item, index) => {
        if (item._X_ROW_KEY == row._X_ROW_KEY) {
          delIndex = index;
        }
      });
      if (delIndex != -1) {
        this.event.data.splice(delIndex, 1);
      }
    },

    /**
     *
     */
    eventTypeChange(row) {
      row.config = null;
    },

    /**
     * 事件配置
     */
    eventSetting(row) {
      this.event.setting.value = row;
      switch (row.type) {
        case 0:
          this.event.setting.js.title = "事件配置-js";
          this.event.setting.js.visible = true;
          break;
        case 1:
          this.event.setting.interface.title = "事件配置-接口";
          this.event.setting.interface.visible = true;
          break;
      }
    },
  },
};
</script>

<style lang="less" scoped>
.tab-margin {
  margin: 1px;
}


/deep/ .ant-tabs .ant-tabs-left-content {
  border-left: 0 !important;
  padding-left: 0 !important;
}
</style>
