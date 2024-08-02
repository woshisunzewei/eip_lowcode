<template>
  <a-modal v-drag :destroyOnClose="true" :maskClosable="false" width="1100px" :title="title" :visible="visible"
    :bodyStyle="modal.bodyStyle" @cancel="cancel">
    <a-spin :spinning="spinning">
      <a-tabs default-active-key="1" @change="changeTab">
        <a-tab-pane key="1" :forceRender="true">
          <span slot="tab"> <a-icon type="form" />基本 </span>
          <a-form-model ref="form" :model="base" :rules="rules" :label-col="config.labelCol"
            :wrapper-col="config.wrapperCol">
            <a-row>
              <a-col>
                <a-form-model-item label="子流程名称" prop="title">
                  <a-input v-model="base.title" placeholder="请输入子流程名称" allow-clear />
                </a-form-model-item>
                <a-form-model-item label="表单" prop="formId">
                  <a-select placeholder="请选择表单" allow-clear v-model="base.formId" @change="formChange">
                    <a-select-option v-for="(item, i) in forms" :key="i" :value="item.configId">{{ item.name
                      }}</a-select-option>
                  </a-select>
                </a-form-model-item>
                <a-form-model-item label="子流程" prop="subProcessProcessId">
                  <a-tree-select placeholder="请选择上级" allow-clear v-model="base.subProcessProcessId" show-search
                    style="width: 100%" treeNodeFilterProp="title" :tree-data="process.data"
                    :dropdown-style="{ maxHeight: '400px', overflow: 'auto' }" @select="processSelect">
                  </a-tree-select>
                </a-form-model-item>
                <a-form-model-item label="同/异步" prop="async">
                  <a-checkbox :checked="base.async">等待子流程返回</a-checkbox>
                </a-form-model-item>
                <a-form-model-item label="开始活动" prop="autoEndStart">
                  <a-checkbox :checked="base.autoEndStart">自动结束子流程的第一个活动</a-checkbox>
                </a-form-model-item>
              </a-col>
            </a-row>
          </a-form-model>
        </a-tab-pane>

        <a-tab-pane key="2" :forceRender="true">
          <span slot="tab"> <a-icon type="user" />处理人 </span>
          <div style="padding: 10px">
            <a-tabs type="card" default-active-key="1">
              <a-tab-pane key="1" style="overflow: auto">
                <span slot="tab"> <a-icon type="team" />流程处理人 </span>
                <a-card style="margin-top: 0px" size="small">
                  <vxe-table ref="tableUser" id="workflowactivitynotice" size="small" :height="240"
                    :data="user.approve">
                    <template #loading>
                      <a-spin></a-spin>
                    </template>

                    <template #empty>
                      <eip-empty />
                    </template>
                    <vxe-column type="seq" title="序号" align="center" width="60"></vxe-column>
                    <vxe-column title="排序" width="50" align="center">

                      <template #default>
                        <span class="drag-btn" style="cursor: move">
                          <a-icon type="drag" />
                        </span>
                      </template>
                    </vxe-column>
                    <vxe-column width="140" align="center">

                      <template #header>
                        <a-button type="primary" size="small" @click="userSetAdd">
                          <a-icon type="plus" />处理人
                        </a-button>
                      </template>

                      <template #default="{ row }">
                        <a-button size="small" @click="userSetEdit(row)">
                          <a-icon type="edit" />
                        </a-button>
                        <a-divider type="vertical" />
                        <a-popconfirm title="确定删除处理人?" ok-text="确定" cancel-text="取消" @confirm="userSetDel(row)">
                          <a-button @click.stop="" size="small" type="danger">
                            <a-icon type="delete" />
                          </a-button>
                        </a-popconfirm>
                      </template>
                    </vxe-column>
                    <vxe-column field="name" title="处理人"></vxe-column>
                  </vxe-table>
                </a-card>
                <a-card style="margin-top: 10px" size="small">
                  <a-row :gutter="[24, 24]" type="flex" justify="start">
                    <a-col :span="4">处理策略：</a-col>
                    <a-col :span="20">
                      <a-radio-group v-model="user.strategy">
                        <a-radio :value="0">列表中的第一处理人</a-radio>
                        <a-radio :value="1">发送给列表中的所有处理人</a-radio>
                        <a-radio :value="2">弹出选择审批人</a-radio>
                      </a-radio-group>
                    </a-col>
                    <a-col :span="4">选人规则：</a-col>
                    <a-col :span="20">
                      <a-radio-group v-model="user.chosen">
                        <a-radio :value="0">非必选</a-radio>
                        <a-radio :value="1">必选</a-radio>
                      </a-radio-group>
                    </a-col>

                    <a-col :span="4">无处理人时：</a-col>
                    <a-col :span="20">
                      <a-radio-group v-model="user.no">
                        <a-radio :value="0">不能提交</a-radio>
                        <a-radio :value="1">跳过本步骤</a-radio>
                      </a-radio-group>
                    </a-col>

                    <a-col :span="4">自动同意策略：</a-col>
                    <a-col :span="20">
                      <a-checkbox-group v-model="user.auto">
                        <a-checkbox :value="0">处理人就是提交人</a-checkbox>
                        <a-checkbox :value="1">处理人和上一步相同</a-checkbox>
                        <a-checkbox :value="2">处理人已经审批过</a-checkbox>
                      </a-checkbox-group>
                    </a-col>

                    <a-col :span="4">通过策略：</a-col>
                    <a-col :span="20">
                      <a-radio-group v-model="user.pass">
                        <a-radio :value="0">一人同意即可</a-radio>
                        <a-radio :value="1">所有人必须同意</a-radio>
                        <a-radio :value="2">通过百分比</a-radio>
                        <a-radio :value="3">处理后不知会</a-radio>
                      </a-radio-group>
                    </a-col>

                    <a-col v-if="user.pass == 2" :span="4">通过百分比：</a-col>
                    <a-col v-if="user.pass == 2" :span="20">
                      <a-input-number :default-value="user.passConfig" :min="0" :max="100"
                        :formatter="(value) => `${value}%`" :parser="(value) => value.replace('%', '')" />
                    </a-col>
                  </a-row>
                </a-card>
              </a-tab-pane>
              <a-tab-pane key="2" force-render>
                <span slot="tab"> <a-icon type="user-add" />加签处理人 </span>
              </a-tab-pane>
            </a-tabs>
          </div>
        </a-tab-pane>

        <a-tab-pane key="3" :forceRender="true"><span slot="tab"> <a-icon type="arrow-down" />数据下发 </span>
          <div class="padding-sm">
            <a-alert message="子流程启动时，父流程数据代入子流程：" type="warning" show-icon />
          </div>
          <div class="padding-sm">
            <vxe-table ref="tableInput" id="workflowSubprocessInput" size="small" :height="height" :data="input">

              <template #loading>
                <a-spin></a-spin>
              </template>

              <template #empty>
                <eip-empty />
              </template>
              <vxe-column type="seq" title="序号" align="center" width="60"></vxe-column>
              <vxe-column field="name" title="列名" width="250">
              </vxe-column>
              <vxe-column field="description" title="备注">
              </vxe-column>
              <vxe-column title="填充值(子流程数据)" align="center" width="380">

                <template v-slot="{ row }">
                  <a-space size="small">
                    <a-input style="width: 220px" v-model="row.valueTxt" placeholder="请选择填充数据" disabled>
                    </a-input>
                    <a-badge :dot="row.valueTxt">
                      <a-button @click="inputSetting(row)" type="primary">
                        设置
                      </a-button>
                    </a-badge>

                    <a-button @click="inputDel(row)" type="danger">
                      清空
                    </a-button>
                  </a-space>
                </template>
              </vxe-column>
            </vxe-table>
          </div>
        </a-tab-pane>
        <a-tab-pane key="4" :forceRender="true"><span slot="tab"> <a-icon type="arrow-up" />数据返回 </span>
          <div class="padding-sm">
            <a-alert message="子流程结束时，子流程数据代回父流程：" type="warning" show-icon />
          </div>
          <div class="padding-sm">
            <vxe-table ref="tableOutput" id="workflowSubprocessOutput" size="small" :height="height" :data="output">

              <template #loading>
                <a-spin></a-spin>
              </template>

              <template #empty>
                <eip-empty />
              </template>
              <vxe-column type="seq" title="序号" align="center" width="60"></vxe-column>
              <vxe-column field="name" title="列名" width="250">
              </vxe-column>
              <vxe-column field="description" title="备注">
              </vxe-column>
              <vxe-column title="填充值(父流程数据)" align="center" width="380">

                <template v-slot="{ row }">
                  <a-space size="small">
                    <a-input style="width: 220px" v-model="row.valueTxt" placeholder="请选择填充数据" disabled>
                    </a-input>
                    <a-badge :dot="row.valueTxt">
                      <a-button @click="outputSetting(row)" type="primary">
                        设置
                      </a-button>
                    </a-badge>
                    <a-button @click="outputDel(row)" type="danger">
                      清空
                    </a-button>
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
    <eip-workflow-user-set ref="userset" v-if="userset.visible" :visible.sync="userset.visible" title="处理人员"
      :edit="userset.edit" :column="column.data" @ok="userSetOk"></eip-workflow-user-set>

    <eip-workflow-fill-data ref="fillData" v-if="fill.visible" :visible.sync="fill.visible" :title="fill.title"
      :data="fill.data" @ok="fillOk"></eip-workflow-fill-data>
  </a-modal>
</template>

<script>
import {
  findForm,
  findFormColumns,
  findPermissionProcess,
  findColumns,
} from "@/services/workflow/process/activity/subprocess";

import Sortable from "sortablejs";

export default {

  name: "activityTask",
  data() {
    return {
      height: "480px",
      modal: {
        bodyStyle: {
          padding: "1px",
          height: '620px'
        },
      },
      process: {
        data: [],
      },
      loading: false,
      spinning: false,
      config: {
        labelCol: {
          span: 3,
        },
        wrapperCol: {
          span: 20,
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

        subProcessProcessId: [
          {
            required: true,
            message: "请选择子流程",
            trigger: ["blur", "change"],
          },
        ],
      },
      base: {
        title: "",
        formId: undefined,
        formName: "",
        subProcessProcessId: undefined,
        subProcessName: "",
        async: true,
        autoEndStart: true,
      },
      output: [],
      input: [],
      //处理用户
      user: {
        strategy: 0, //处理策略
        chosen: 0, //选人规则
        no: 0, //无对应处理人
        auto: [], //自动同意规则
        pass: 0, //通过策略
        passConfig: 0, //通过策略配置
        approve: [], //审核处理人
        addActivity: [], //加签处理人
      },
      userset: {
        visible: false,
        edit: {},
      },
      column: {
        data: [],
      },
      fill: {
        visible: false,
        title: "",
        data: [],
        row: {},
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
    formId: {
      type: String,
    }, //默认表单Id
  },
  mounted() {
    this.processInit();
    this.userInit();
    this.formInit();
    this.inputInit();
    this.outputInit();
  },
  methods: {
    /**
     *初始化所有流程
     */
    processInit() {
      let that = this;
      findPermissionProcess().then(function (result) {
        if (result.code === that.eipResultCode.success) {
          that.process.data = result.data;
        }
      });
    },

    /**
     *子流程改变
     */
    processSelect(id) {
      let that = this;

      if (id) {
        findColumns(id).then(function (result) {
          if (result.code === that.eipResultCode.success) {
            result.data.forEach((item) => {
              item.value = null;
              item.valueTxt = null;
            });
            that.output = result.data;
          }
        });
      } else {
        this.output = [];
      }
    },

    /**
     * 表单切换
     */
    formChange(id) {
      let that = this;
      if (id) {
        findFormColumns(id).then(function (result) {
          if (result.code === that.eipResultCode.success) {
            result.data.forEach((item) => {
              item.value = null;
              item.valueTxt = null;
            });
            that.column.data = result.data;
            that.input = result.data;
          }
        });
      } else {
        this.column.data = [];
        this.input = [];
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
          that.data.props.user = that.user;
          that.data.props.output = that.output;
          that.data.props.input = that.input;
          that.data.text.text = that.base.title;
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
            if (!that.base.formId && that.formId) {
              that.base.formId = that.formId;
            }
          } else {
            that.base.formId = that.formId;
          }

          if (that.base.formId) {
            that.formChange(that.base.formId);
          }
        }
      });
    },

    /**
     * 初始化审核人员
     */
    userInit() {
      let that = this;
      if (that.data) {
        that.user = that.$utils.clone(that.data.props.user, true);
      }
    },

    /**
     * 初始化输入
     */
    inputInit() {
      let that = this;
      if (that.data) {
        that.input = that.$utils.clone(that.data.props.input, true);
      }
    },

    /**
     * 初始化输入
     */
    outputInit() {
      let that = this;
      if (that.data) {
        that.output = that.$utils.clone(that.data.props.output, true);
      }
    },

    /**
     * 处理人员新增
     */
    userSetAdd() {
      this.userset.edit = null;
      this.userset.visible = true;
    },

    /**
     * 保存完毕
     */
    userSetOk(data) {
      if (this.userset.edit) {
        this.userset.edit = data;
      } else {
        this.user.approve.push(data);
      }
      this.userSetDrop();
    },

    /**
     * 处理人编辑
     */
    userSetEdit(row) {
      this.userset.edit = row;
      this.userset.visible = true;
    },

    /**
     * 处理人编辑
     */
    userSetDel(row) {
      var delIndex = -1;
      this.user.approve.forEach((item, index) => {
        if (item._X_ROW_KEY == row._X_ROW_KEY) {
          delIndex = index;
        }
      });
      if (delIndex != -1) {
        this.user.approve.splice(delIndex, 1);
      }
      this.userSetDrop();
    },

    /**
     * 拖拽
     */
    userSetDrop() {
      let that = this;
      const xTable = this.$refs.tableUser;
      Sortable.create(
        xTable.$el.querySelector(".body--wrapper>.vxe-table--body tbody"),
        {
          handle: ".drag-btn",
          onEnd: ({ newIndex, oldIndex }) => {
            const currRow = that.user.approve.splice(oldIndex, 1)[0];
            that.user.approve.splice(newIndex, 0, currRow);
          },
        }
      );
    },

    /**
     *
     */
    inputSetting(row) {
      this.fill.visible = true;
      this.fill.row = row;
      this.fill.data = this.output;
      this.fill.title = "请选择填充值(子流程数据)";
    },

    /**
     *
     */
    inputDel(row) {
      row.value = "";
      row.valueTxt = "";
    },

    /**
     *
     */
    outputSetting(row) {
      this.fill.visible = true;
      this.fill.row = row;
      this.fill.data = this.input;
      this.fill.title = "请选择填充值(父流程数据)";
    },

    /**
     *
     * @param {*} row
     */
    outputDel(row) {
      row.value = "";
      row.valueTxt = "";
    },

    /**
     * 填充
     */
    fillOk(data) {
      this.fill.row.value = data[0].key;
      this.fill.row.valueTxt = data[0].title;
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
