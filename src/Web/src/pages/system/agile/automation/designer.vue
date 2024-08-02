<template>
  <a-drawer
    width="100%"
    placement="right"
    :visible="visible"
    :closable="false"
    :bodyStyle="{ padding: '0' }"
    @close="cancel"
    :destroyOnClose="true"
  >
    <div class="eip-form-designer-container">
      <div class="operating-area">
        <div class="left-btn-box">
          <a-tooltip title="后退">
            <a @click="revoke">
              <a-icon type="arrow-left" />
              <span>后退</span>
            </a>
          </a-tooltip>
          <a-divider type="vertical" />
          <a-tooltip title="前进">
            <a @click="forward">
              <a-icon type="arrow-right" />
              <span>前进</span>
            </a>
          </a-tooltip>
          <a-divider type="vertical" />
          <a-tooltip title="打印数据">
            <a @click="importData">
              <a-icon type="printer" />
              <span>打印数据</span>
            </a>
          </a-tooltip>
          <a-divider type="vertical" />
          <a-tooltip title="流程参数">
            <a @click="importData">
              <a-icon type="printer" />
              <span>流程参数</span>
            </a>
          </a-tooltip>
          <a-divider type="vertical" />
          <a-tooltip title="保存">
            <a @click="automationSaveClick(true)">
              <a-icon type="save" />
              <span>保存</span>
            </a>
          </a-tooltip>
          <a-divider type="vertical" />
          <a-tooltip title="发布">
            <a @click="automationPublicClick">
              <a-icon type="build" />
              <span>发布</span>
            </a>
          </a-tooltip>
        </div>
        <div class="right-btn-box">
          <a-tooltip :title="title"
            ><span class="text-bold">{{ title }}</span></a-tooltip
          >
          <a-divider type="vertical" />
          <a-tooltip title="关闭">
            <a @click="cancel" style="color: #ff4d4f">
              <a-icon type="close" style="font-size: 14px" />
              <span>关闭</span>
            </a>
          </a-tooltip>
        </div>
      </div>
      <!-- 操作区域 end -->
      <div class="content toolbars-top">
        <div class="workflow">
          <div class="workflow_container">
            <flowMain
              v-if="config.childNode"
              :config.sync="config"
              @changeNode="changeNode"
              :contentFocus="contentFocus"
            />
            <flowEnd />
          </div>
        </div>
      </div>
    </div>
    <a-drawer
      :headerStyle="{ padding: '1px' }"
      :bodyStyle="{ padding: '4px' }"
      width="700px"
      :destroyOnClose="true"
      :visible="drawer"
      @close="cancelConfig"
    >
      <div class="drawer_title" slot="title">
        <img
          :src="
            require(`@/assets/img/${drawerData.type ? drawerData.type : 0}.png`)
          "
        />
        <span class="drawer_title_text" v-show="!changeTitle">{{
          drawerData.title
        }}</span>
        <input
          ref="input"
          class="ant-input"
          type="text"
          v-show="changeTitle"
          v-model="drawerData.title"
          placeholder="请输入标题"
          @blur="changeTitle = false"
        />
        <a-icon
          type="edit"
          v-show="!changeTitle"
          @click="changeTitleFun"
        ></a-icon>
      </div>
      <div class="eip-drawer-body">
        <div class="drawer_content">
          <formChange
            v-if="drawerData.type === 0"
            :drawerData.sync="drawerData.data"
            :tables="tables"
            :configList="configList"
          />

          <timeTrigger
            v-if="drawerData.type === 101"
            :drawerData.sync="drawerData.data"
          />

          <addData
            v-if="drawerData.type === 1"
            :drawerData.sync="drawerData.data"
            :configList="configList"
            :tables="tables"
          />

          <updateData
            v-if="drawerData.type === 2"
            :drawerData.sync="drawerData.data"
            :configList="configList"
          />

          <getSingleData
            v-if="drawerData.type === 3"
            :drawerData.sync="drawerData.data"
            :tables="tables"
          />

          <getMultipleData
            v-if="drawerData.type === 4"
            :drawerData.sync="drawerData.data"
            :tables="tables"
            :configList="configList"
          />

          <deleteData
            v-if="drawerData.type === 5"
            :drawerData.sync="drawerData.data"
            :tables="tables"
            :configList="configList"
          />

          <noticeSite
            v-if="drawerData.type === 6"
            :drawerData.sync="drawerData.data"
            :configList="configList"
            :tables="tables"
          />
          <condition
            v-if="drawerData.type === 8 || drawerData.type === 9"
            :drawerData.sync="drawerData.data"
            :configList="configList"
            :tables="tables"
          />
          <customerRequest
            v-if="drawerData.type === 21"
            :drawerData.sync="drawerData.data"
            :tables="tables"
            :configList="configList"
          />

          <codeBlock
            v-if="drawerData.type === 22"
            :drawerData.sync="drawerData.data"
            :configList="configList"
            :title="drawerData.title"
          />

          <subProcess
            v-if="drawerData.type === 31"
            :drawerData.sync="drawerData.data"
            :configList="configList"
            :title="drawerData.title"
          />
        </div>
      </div>
      <!-- <div class="eip-drawer-toolbar">
        <div class="flex justify-between align-center">
          <div>
          </div>
          <a-space>
            <a-button @click="cancel">取消</a-button>
            <a-button type="primary" @click="save">保存</a-button>
          </a-space>
        </div>
      </div> -->
    </a-drawer>

    <k-json-modal ref="jsonModal" />
  </a-drawer>
</template>

<script>
import "@/pages/system/agile/form/components/styles/form-design.less";
import {
  automationSave,
  automationPublic,
  findById,
} from "@/services/system/agile/automation/designer";
import { mapMutations, mapGetters } from "vuex";
import kJsonModal from "@/pages/system/agile/form/components/packages/components/KFormDesign/module/jsonModal";
import flowMain from "./components/flowMain.vue";
import flowEnd from "./components/flowEnd.vue";
import deleteData from "./components/form/deleteData";
import getMultipleData from "./components/form/getMultipleData";
import getSingleData from "./components/form/getSingleData";
import noticeSite from "./components/form/noticeSite";
import addData from "./components/form/addData";
import updateData from "./components/form/updateData";
import subProcess from "./components/form/subProcess";
import condition from "./components/form/condition";
import formChange from "./components/form/formChange";
import timeTrigger from "./components/form/timeTrigger";
import codeBlock from "./components/form/codeBlock";
import customerRequest from "./components/form/customerRequest";
import { memoryData } from "./components/data/WorkFlowData";
import { newGuid } from "@/utils/util";
import { table } from "@/services/system/agile/automation/designer";
export default {
  name: "automationdesigner",
  components: {
    flowMain,
    flowEnd,
    getSingleData,
    getMultipleData,
    deleteData,
    formChange,
    kJsonModal,
    noticeSite,
    addData,
    updateData,
    timeTrigger,
    customerRequest,
    subProcess,
    codeBlock,
    condition,
  },
  computed: {
    ...mapGetters("account", ["systemAgile"]),
  },
  data() {
    return {
      loading: false,
      bodyStyle: {
        padding: "0",
      },
      config: {},
      configList: [],
      revokeNum: 1,
      noPush: false,
      drawer: false,
      drawerData: {},
      contentFocus: "",
      changeTitle: false,
      tables: [],
    };
  },
  watch: {
    config: {
      handler(newData) {
        if (this.noPush) {
          this.noPush = false;
        } else {
          if (this.revokeNum === 1) {
            memoryData.push(JSON.parse(JSON.stringify(newData)));
            if (memoryData.length > 20) {
              memoryData.shift();
            }
          } else {
            memoryData.splice(
              memoryData.length - (this.revokeNum - 1),
              this.revokeNum - 1
            );
            memoryData.push(JSON.parse(JSON.stringify(newData)));
            this.revokeNum = 1;
          }
        }
      },
      deep: true,
    },
  },
  mounted() {
    this.find();
    this.initTable();
  },
  created() {},

  props: {
    visible: {
      type: Boolean,
      default: false,
    },
    configId: String,
    tableId: String,
    title: {
      type: String,
    },
  },

  methods: {
    /**
     * 初始化所有的工作表
     */
    initTable() {
      let that = this;
      table().then(function (result) {
        that.tables = result.data;
      });
    },
    /**
     * 根据Id查找
     */
    find() {
      let that = this;
      that.$message.loading("加载中...", 0);
      findById(this.configId).then(function (result) {
        let form = result.data;
        if (form.saveJson) {
          that.config = JSON.parse(form.saveJson);
        } else {
          switch (form.configType) {
            case 1:
              that.config = {
                type: 0,
                id: newGuid(),
                title: "表单事件触发",
                data: {
                  table: that.tableId, //选择工作表
                  triggerType: 1, //触发方式
                  triggerColumns: [], //触发列
                  conditionFilters: {
                    groupOp: "AND",
                    rules: [],
                    groups: [],
                  },
                },
                conditionNodes: [],
                childNode: {},
              };
              break;
            case 2:
              that.config = {
                type: 101,
                id: newGuid(),
                title: "定时触发",
                data: {
                  table: that.tableId, //选择工作表
                  beginTime: "", //开始时间
                  endTime: "", //结束时间
                  cron: "", //Cron循环时间
                },
                conditionNodes: [],
                childNode: {},
              };
              break;
            case 4:
              that.config = {
                type: 201,
                id: newGuid(),
                title: "按钮触发",
                data: {
                  table: that.tableId, //选择工作表
                  beginTime: "", //开始时间
                  endTime: "", //结束时间
                  cron: "", //Cron循环时间
                },
                conditionNodes: [],
                childNode: {},
              };
              break;
          }
        }
        that.$message.destroy();
      });
    },

    /**
     *
     */
    automationPublicClick() {
      let that = this;
      this.$confirm({
        title: "发布提示?",
        content: "确认发布",
        okText: "确定",
        okType: "danger",
        cancelText: "取消",
        onOk() {
          let param = {
            configId: that.configId,
            saveJson: JSON.stringify(that.config),
            publicJson: JSON.stringify(that.config),
          };
          //如果是表单触发
          if (that.config.type == 0) {
            param.tableId = that.config.data.table;
            param.tableTriggerType = that.config.data.triggerType;
          } else if (that.config.type == 101) {
            param.tableId = that.config.data.table;
          }
          that.$loading.show({ text: "发布中,请稍等..." });
          automationPublic(param).then(function (result) {
            that.$loading.hide();
            if (result.code === that.eipResultCode.success) {
              that.$message.success(result.msg);
            } else {
              that.$message.error(result.msg);
            }
          });
        },
        onCancel() {},
      });
    },
    /**
     * 保存表单设计
     */
    automationSaveClick(tip) {
      let that = this;
      let param = {
        configId: this.configId,
        saveJson: JSON.stringify(this.config),
      };
      if (tip) {
        that.$loading.show({ text: "保存中,请稍等..." });
      }
      automationSave(param).then(function (result) {
        that.$loading.hide();
        if (result.code === that.eipResultCode.success) {
          if (tip) {
            that.$message.success(result.msg);
          }
        } else {
          that.$message.error(result.msg);
        }
      });
    },
    importData() {
      this.$refs.jsonModal.jsonData = this.config;
      this.$refs.jsonModal.visible = true;
    },
    revoke() {
      if (this.revokeNum >= memoryData.length) return;
      this.revokeNum += 1;
      this.config = JSON.parse(
        JSON.stringify(memoryData[memoryData.length - this.revokeNum])
      );
      this.noPush = true;
    },
    forward() {
      if (this.revokeNum <= 1) return;
      this.revokeNum -= 1;
      this.config = JSON.parse(
        JSON.stringify(memoryData[memoryData.length - this.revokeNum])
      );
      this.noPush = true;
    },
    changeNode(data, index) {
      this.drawerData = data;
      this.drawer = true;
      this.contentFocus = data.id;
      this.getConfigList(data);
      this.automationSaveClick(false);
    },
    /**
     * 所有组件
     */
    getConfigList(data) {
      let list = [];
      const traverse = (item, index) => {
        if (data.id == item.id && index != 0) {
          return;
        }
        if ([0, 21, 22, 4, 301].includes(item.type)) {
          list.push({
            conditionNodes: item.conditionNodes,
            data: item.data,
            id: item.id,
            title: item.title,
            type: item.type,
          });
        }
        if (item.childNode.hasOwnProperty("type")) {
          traverse(item.childNode, 1);
        }
      };

      traverse(this.config, 0);
      this.configList = list;
    },
    beforeClose(done) {
      this.contentFocus = "";
      done();
    },
    cancelConfig() {
      this.contentFocus = "";
      this.drawer = false;
    },
    cancel() {
      this.$emit("update:visible", false);
    },
    save() {
      this.contentFocus = "";
      this.drawer = false;
    },
    changeTitleFun() {
      this.changeTitle = true;
      this.$nextTick(() => {
        this.$refs.input.focus();
      });
    },
  },
};
</script>

<style lang="less" scoped>
.close-box {
  bottom: 100px !important;
}

.drag-btn {
  cursor: move;
}

.eip-form-designer-container .content section {
  -webkit-box-flex: 1;
  -ms-flex: 1;
  flex: 1;
  max-width: calc(100% - 270px);
  -webkit-user-select: none;
  -moz-user-select: none;
  -ms-user-select: none;
  user-select: none;
  margin: 0 8px 0;
  -webkit-box-shadow: 0px 0px 1px 1px #ccc;
  box-shadow: 0px 0px 1px 1px #ccc;
}

.eip-form-designer-container .content aside.right .ant-form-item {
  margin-bottom: 0;
  padding: 6px 0;
  border-bottom: 1px solid #ccc;
}

.tab-margin {
  margin: 5px;
}

/deep/ .ant-tabs-bar {
  margin: 0 0 4px 0 !important;
}

.workflow {
  box-sizing: border-box;
  position: relative;
  width: 100%;
  height: 100%;
  overflow: auto;
  padding: 80px 60px;
  display: flex;
  background-color: #fafafa;
  background-image: linear-gradient(#fafafa 11px, transparent 0),
    linear-gradient(90deg, #9ea6b7 1px, transparent 0);
  background-size: 15px 12px, 12px 15px;
}

.control_box {
  z-index: 999;
  position: fixed;
  top: 20px;
  right: 36px;
}

.workflow_container {
  margin: 0 auto;
}

.workflow .a-drawer__body {
  display: flex;
  flex-direction: column;
}

.drawer_title,
.drawer_footer {
  padding: 0 20px;
}

.drawer_title {
  height: 50px;
  color: #303233;
  display: flex;
  align-items: center;
  border-bottom: 1px solid #f1f2f3;
}

.drawer_title > img {
  width: 24px;
  height: 24px;
  border-radius: 50%;
  margin-right: 10px;
}

.a-icon-edit {
  margin-left: 8px;
}

.drawer_title_text {
  padding: 8px 0;
  font-size: 16px;
  font-weight: bold;
  border: 1px solid #fff;
}

.input_1 {
  padding: 8px 0;
  flex: 1;
  border: 1px solid #999;
  outline: none;
  font-size: 16px;
  font-weight: bold;
  color: #303233;
}

.drawer_footer {
  height: 60px;
  display: flex;
  justify-content: flex-end;
  align-items: center;
  border-top: 1px solid #f1f2f3;
}

.drawer_content {
  flex: 1;
}
</style>
