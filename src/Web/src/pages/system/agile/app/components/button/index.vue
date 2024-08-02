<template>
  <a-drawer width="1120px" :title="title" :visible="visible" :bodyStyle="{ padding: '1px' }" @close="cancel"
    :destroyOnClose="true">
    <div class="eip-drawer-body beauty-scroll padding">
      <vxe-toolbar ref="styleToolbar">
        <template v-slot:buttons>
          <a-space>
            <a-dropdown>
              <a-menu slot="overlay" @click="commonUse">
                <a-menu-item key="buttonCrud"> <a-icon type="form" />增删改查复制 </a-menu-item>
                <a-menu-item key="buttonWorkflow"> <a-icon type="deployment-unit" />流程相关按钮 </a-menu-item>
              </a-menu>
              <a-button> 常用按钮 <a-icon type="down" /> </a-button>
            </a-dropdown>

            <a-button type="primary"><a-icon type="copy" /> 复制已有 </a-button>
          </a-space>
        </template>
      </vxe-toolbar>
      <vxe-table :loading="table.loading" ref="table" row-key :data="button" :height="height">

        <template #loading>
          <a-spin></a-spin>
        </template>

        <template #empty>
          <eip-empty />
        </template>
        <vxe-column title="排序" width="48" align="center">

          <template>
            <span class="drag-btn">
              <a-icon type="drag" />
            </span>
          </template>
        </vxe-column>
        <vxe-column title="新增" width="54px" align="center">

          <template #header>
            <a-button @click="add" size="small" type="primary">
              <a-icon type="plus" />
            </a-button>
          </template>

          <template v-slot="{ row }">
            <a-popconfirm title="确定删除字段配置?" ok-text="确定" cancel-text="取消" @confirm="del(row)">
              <a-button @click.stop="" size="small" type="danger">
                <a-icon type="delete"></a-icon>
              </a-button>
            </a-popconfirm>
          </template>
        </vxe-column>

        <vxe-column title="名称" width="150px">

          <template v-slot="{ row }">
            <a-input placeholder="请输入名称" v-model="row.name" />
          </template>
        </vxe-column>
        <vxe-column title="触发类型" width="100px">

          <template v-slot="{ row }">
            <a-select style="width:80px" default-value="primary" allow-clear v-model="row.triggerType">
              <a-select-option :value="1">方法</a-select-option>
              <a-select-option :value="2">脚本</a-select-option>
              <a-select-option :value="3">接口</a-select-option>
              <a-select-option :value="4">自动化</a-select-option>
              <a-select-option :value="5">打印</a-select-option>
              <a-select-option :value="6">导出</a-select-option>
            </a-select>
          </template>
        </vxe-column>

        <vxe-column title="触发配置" width="80px" align="center">

          <template v-slot="{ row }">
            <a-badge>
              <a-button size="small" @click="triggerTypeEdit(row)" type="primary">
                <a-icon type="edit" />
              </a-button>
            </a-badge>
          </template>
        </vxe-column>

        <vxe-column title="图标" width="120px" align="center">
          <template v-slot="{ row }">
            <eip-icon :name="row.icon" :theme="row.theme" @click="iconClick(row, $event)"
              @clear="iconClear(row)"></eip-icon>
          </template>
        </vxe-column>
        <vxe-column title="类型" width="120px" align="center">
          <template v-slot="{ row }">
            <a-select style="width: 100px" default-value="primary" allow-clear v-model="row.style">
              <a-select-option value="primary">主按钮 </a-select-option>
              <a-select-option value=""> 次按钮 </a-select-option>
              <a-select-option value="dashed"> 虚线按钮 </a-select-option>
              <a-select-option value="danger"> 危险按钮 </a-select-option>
              <a-select-option value="link"> 链接按钮 </a-select-option>
              <a-select-option value="upload"> 上传按钮 </a-select-option>
            </a-select>
          </template>
        </vxe-column>
        <vxe-column title="形状" width="120px" align="center">
          <template v-slot="{ row }">
            <a-select style="width: 100px" default-value="primary" allow-clear v-model="row.shape">
              <a-select-option value="">长方形 </a-select-option>
              <a-select-option value="circle">圆形 </a-select-option>
              <a-select-option value="round"> 椭圆形 </a-select-option>
            </a-select>
          </template>
        </vxe-column>
        <vxe-column title="冻结" width="60px" align="center">

          <template v-slot="{ row }">
            <a-switch v-model="row.isFreeze" />
          </template>
        </vxe-column>
        <vxe-column title="列表显示" width="90px" align="center">

          <template v-slot="{ row }">
            <a-switch v-model="row.isShowTable" />
          </template>
        </vxe-column>
        <vxe-column title="备注" width="140px">

          <template v-slot="{ row }">
            <a-input placeholder="请输入备注" v-model="row.remark" />
          </template>
        </vxe-column>
      </vxe-table>
    </div>
    <div class="eip-drawer-toolbar">
      <a-space>
        <a-button key="back" @click="cancel" :disabled="loading"><a-icon type="close" />取消</a-button>
        <a-button key="submit" @click="save" type="primary" :loading="loading"><a-icon type="save" />提交</a-button>
      </a-space>
    </div>
    <a-drawer width="900px" v-if="item" placement="right" :bodyStyle="{ padding: '1px' }" :visible="trigger.visible"
      :title="'触发配置：' + item.name" @close="trigger.visible = false" :destroyOnClose="true">
      <div class="beauty-scroll" style="height: calc(100vh - 110px);">
        <eip-code v-if="trigger.type == 2" :value.sync="trigger.script"></eip-code>

        <a-form-model :label-col="config.labelCol" :wrapper-col="config.wrapperCol">
          <a-form-model-item v-if="trigger.type == 1" label="方法" prop="method">
            <a-input v-model="trigger.method" placeholder="请输入方法" allow-clear />
          </a-form-model-item>
          <a-form-model-item v-if="trigger.type == 3" label="Api地址" prop="path">
            <a-input v-model="trigger.api.path" placeholder="请输入Api地址" allow-clear />
          </a-form-model-item>
          <a-form-model-item v-if="trigger.type == 3" label="请求方式" prop="type">
            <a-radio-group v-model="trigger.api.type">
              <a-radio value="post">POST </a-radio>
              <a-radio value="get">GET </a-radio>
            </a-radio-group>
          </a-form-model-item>

          <a-form-model-item v-if="trigger.type == 3" label="数据选择类型" prop="confirmType">
            <a-radio-group v-model="trigger.api.confirmType">
              <a-radio :value="1">可空 </a-radio>
              <a-radio :value="2">一条 </a-radio>
              <a-radio :value="3">多条 </a-radio>
            </a-radio-group>
          </a-form-model-item>

          <a-form-model-item v-if="trigger.type == 3" label="点击按钮时" prop="action">
            <a-checkbox v-model="trigger.api.confirm.is">需要二次确认</a-checkbox>
          </a-form-model-item>

          <a-form-model-item v-if="trigger.type == 3 && trigger.api.confirm.is" label="确认按钮" prop="okText">
            <a-input v-model="trigger.api.confirm.okText" allow-clear placeholder="请输入确认提示语" />
          </a-form-model-item>
          <a-form-model-item v-if="trigger.type == 3 && trigger.api.confirm.is" label="取消按钮" prop="cancelText">
            <a-input v-model="trigger.api.confirm.cancelText" allow-clear placeholder="请输入取消提示语" />
          </a-form-model-item>
          <a-form-model-item v-if="trigger.type == 3 && trigger.api.confirm.is" label="标题" prop="title">
            <a-input v-model="trigger.api.confirm.title" allow-clear placeholder="请输入标题" />
          </a-form-model-item>
          <a-form-model-item v-if="trigger.type == 3 && trigger.api.confirm.is" label="说明" prop="content">
            <a-input v-model="trigger.api.confirm.content" allow-clear placeholder="请输入说明" />
          </a-form-model-item>

          <a-form-model-item v-if="trigger.type == 4" label="动作" prop="action">
            <a-radio-group v-model="trigger.automation.action">
              <a-radio :value="1">执行工作流 </a-radio>
              <a-radio :value="2">填写表单字段 </a-radio>
            </a-radio-group>
          </a-form-model-item>

          <a-form-model-item v-if="trigger.type == 4 && trigger.automation.action == 2" label="表单字段" prop="formColumns">
            <vxe-table row-key ref="formColumns" height="400px" :data="trigger.automation.editColumn">
              <template #loading>
                <a-spin></a-spin>
              </template>
              <template #empty>
                <eip-empty />
              </template>
              <vxe-column title="新增" width="54px" align="center">
                <template #header>
                  <a-button @click="addFormColumns" size="small" type="primary">
                    <a-icon type="plus" />
                  </a-button>
                </template>
                <template v-slot="{ row }">
                  <a-popconfirm title="确定删除查询配置?" ok-text="确定" cancel-text="取消" @confirm="delFormColumns(row)">
                    <a-button @click.stop="" size="small" type="danger">
                      <a-icon type="delete"></a-icon>
                    </a-button>
                  </a-popconfirm>
                </template>
              </vxe-column>
              <vxe-column title="字段" width="200px">
                <template v-slot="{ row }">
                  <a-select show-search optionFilterProp="label" placeholder="请选择字段" v-model="row.column"
                    style="width: 180px" @change="addFormColumnsChange(row, $event)">
                    <a-select-option v-for="(item, i) in columns" :key="i" :value="item.model"
                      :label="item.label"><label>{{
                        item.label }}</label></a-select-option>
                  </a-select>
                </template>
              </vxe-column>

              <vxe-column title="属性" width="170px">
                <template v-slot="{ row }">
                  <a-select show-search style="width: 100px" v-model="row.type" placeholder="请选择类型">
                    <a-select-option value="readonly">只读</a-select-option>
                    <a-select-option value="write">填写</a-select-option>
                    <a-select-option value="writeMust">必填</a-select-option>
                  </a-select>
                </template>
              </vxe-column>

              <vxe-column title="默认值" width="280px">
                <template v-slot="{ row }">
                  <a-input-number v-if="['number'].includes(row.columnType)" placeholder="请输入" v-model="row.default" />

                  <a-input v-else-if="['input', 'textarea'].includes(row.columnType)" placeholder="请输入"
                    v-model="row.default" />

                  <a-select v-else-if="['radio'].includes(row.columnType)" show-search optionFilterProp="label"
                    placeholder="请选择" v-model="row.default" style="width: 260px">
                    <a-select-option v-for="(item, i) in row.defaultOptions" :key="i" :value="item.value"
                      :label="item.label"><label>{{
                        item.label }}</label></a-select-option>
                  </a-select>

                  <a-date-picker v-else-if="['date'].includes(row.columnType)" v-model="row.default" />

                  <a-switch v-else-if="['switch'].includes(row.columnType)" v-model="row.default" />

                  <label v-else>等待实现</label>

                </template>
              </vxe-column>
            </vxe-table>
          </a-form-model-item>

          <a-form-model-item v-if="trigger.type == 4" label="数据选择类型" prop="confirmType">
            <a-radio-group v-model="trigger.automation.confirmType">
              <a-radio :value="1">可空 </a-radio>
              <a-radio :value="2">一条 </a-radio>
              <a-radio :value="3">多条 </a-radio>
            </a-radio-group>
          </a-form-model-item>

          <a-form-model-item v-if="trigger.type == 4" :label="trigger.automation.action == 1 ? '点击按钮时' : '提交时'"
            prop="action">
            <a-checkbox v-model="trigger.automation.confirm.is">需要二次确认</a-checkbox>
          </a-form-model-item>

          <a-form-model-item v-if="trigger.type == 4 && trigger.automation.confirm.is" label="确认按钮" prop="okText">
            <a-input v-model="trigger.automation.confirm.okText" allow-clear placeholder="请输入确认提示语" />
          </a-form-model-item>
          <a-form-model-item v-if="trigger.type == 4 && trigger.automation.confirm.is" label="取消按钮" prop="cancelText">
            <a-input v-model="trigger.automation.confirm.cancelText" allow-clear placeholder="请输入取消提示语" />
          </a-form-model-item>
          <a-form-model-item v-if="trigger.type == 4 && trigger.automation.confirm.is" label="标题" prop="title">
            <a-input v-model="trigger.automation.confirm.title" allow-clear placeholder="请输入标题" />
          </a-form-model-item>
          <a-form-model-item v-if="trigger.type == 4 && trigger.automation.confirm.is" label="说明" prop="content">
            <a-input v-model="trigger.automation.confirm.content" allow-clear placeholder="请输入说明" />
          </a-form-model-item>

          <a-form-model-item v-if="trigger.type == 4 && trigger.automation.action == 2" label="提交后"
            prop="saveFormAndDoWorkflowEvent">
            <a-checkbox v-model="trigger.automation.saveFormAndDoWorkflowEvent">继续执行工作流</a-checkbox>
          </a-form-model-item>

          <a-form-model-item
            v-if="trigger.type == 4 && (trigger.automation.action == 1 || (trigger.automation.action == 2 && trigger.automation.saveFormAndDoWorkflowEvent))"
            label="自动化设置" prop="designer">
            <a-button type="primary" @click="automationDesigner()">
              点击配置自动化设置
            </a-button>
          </a-form-model-item>

          <a-form-model-item v-if="trigger.type == 5" label="打印模版设置" prop="designer">
            <a-button type="primary" @click="printDesigner()">
              点击配置打印模版设置
            </a-button>
          </a-form-model-item>
          <a-form-model-item v-if="trigger.type == 6" label="模版文件" prop="printPreview">
            <a-upload name="file" :fileList="trigger.export.file" accept=".xlsx,.docx" :beforeUpload="beforeUpload"
              @change="handleChange" :customRequest="uploadCustomRequest" :multiple="false">
              <a-button type="primary" v-if="trigger.export.file.length == 0">
                <a-icon type="file-text" /> 上传导出模版文件
              </a-button>
            </a-upload>
          </a-form-model-item>

          <a-form-model-item
            v-if="trigger.type == 6 && (trigger.export.name.indexOf('.doc') > -1 || trigger.export.name.indexOf('.docx') > -1)"
            label="导出PDF" prop="printPreview">
            <a-switch v-model="trigger.export.pdf" />
          </a-form-model-item>
        </a-form-model>
      </div>
      <div class="eip-drawer-toolbar">
        <a-space>
          <a-button key="back" @click="trigger.visible = false"><a-icon type="close" />取消</a-button>
          <a-button key="submit" @click="triggerTypeOk" type="primary"><a-icon type="save" />提交</a-button>
        </a-space>
      </div>
    </a-drawer>

    <automation-designer v-if="designer.visible" :visible.sync="designer.visible" :title="designer.title"
      :configId="designer.configId" @close="designerClose"></automation-designer>

    <print-designer v-if="prints.visible" :visible.sync="prints.visible" :title="prints.title"
      :configId="prints.configId" @close="designerClose"></print-designer>
  </a-drawer>
</template>

<script>
import automationDesigner from "@/pages/system/agile/automation/designer";
import printDesigner from "@/pages/system/agile/print/designer";
import {
  saveAutomation,
  saveAgile,
  findColumnJson,
  uploadFile
} from "@/services/system/agile/components/button/index";
import { newGuid } from "@/utils/util";
import { query } from "@/services/system/menubutton/list";
import { saveAll } from "@/services/system/menubutton/edit";
import Sortable from "sortablejs";
export default {
  name: "button-index",
  components: { automationDesigner, printDesigner },
  data() {
    return {
      height: window.innerHeight - 191,

      designer: {
        visible: false,
        configId: "",
        title: "",
        bodyStyle: {
          padding: "0",
        },
      },

      prints: {
        visible: false,
        configId: "",
        title: "",
        bodyStyle: {
          padding: "0",
        },
      },
      table: {
        loading: false,
      },

      config: {
        labelCol: { span: 4 },
        wrapperCol: { span: 20 },
      },

      button: [],

      loading: false,

      trigger: {
        visible: false,
        type: 1,
        script: "",
        method: "",
        api: {
          path: null,
          type: 'post',
          confirmType: 1,//确认类型,1可不选择数据，2单选，3多选
          confirm: {
            is: false,
            okText: "确认",
            cancelText: "取消",
            title: "你确认执行此操作吗？",
            content: ""
          },
        },
        automation: {
          confirmType: 1,//确认类型,1可不选择数据，2单选，3多选
          action: 1,//动作，1执行工作流，2填写表单字段
          confirm: {
            is: false,
            okText: "确认",
            cancelText: "取消",
            title: "你确认执行此操作吗？",
            content: ""
          },
          editColumn: [],//填写字段
          saveFormAndDoWorkflowEvent: false//填写完表单后继续执行工作流
        },

        prints: {
          preview: true
        },

        export: {
          pdf: false,
          name: '',
          file: []
        }

      },
      item: null,

      columns: [],
      dataFromName: null,
      dataFrom: null,
      dataBaseId: null,
    };
  },

  props: {
    visible: {
      type: Boolean,
      default: false,
    },
    menuId: {
      type: String,
    },
    title: {
      type: String,
    },
  },
  mounted() {
    this.init();
  },
  methods: {
    async beforeUpload(file) {
      if (this.trigger.export.file.length > 0) {
        this.$message.error('只能上传一个文件模版')
        return false;
      }
      return true;

    },
    handleChange({ fileList }) {
      this.trigger.export.name = fileList.length > 0 ? fileList[0].name : '';
      this.trigger.export.file = fileList;
    },
    /**
    * 上传改变
    */
    uploadCustomRequest(file) {
      let that = this;
      const formData = new FormData();
      formData.append("Files", file.file);
      formData.append("CorrelationId", this.item.menuButtonId);
      formData.append("Single", true);
      that.$message.loading({
        content: "上传中...",
        duration: 0,
      });
      uploadFile(formData)
        .then((result) => {
          that.$message.destroy();
          if (result.code == that.eipResultCode.success) {
            that.$message.success(result.msg);
            file.onSuccess(result.data, file.file)
          } else {
            that.$message.error(result.msg);
          }
        })
        .catch(() => {
          that.$message.error("上传出错");
        });
    },

    /**
     * 添加字段
     */
    addFormColumns() {
      this.trigger.automation.editColumn.push({
        column: undefined,
        columnType: 'input',
        type: undefined,
        defaultOptions: [],
        default: undefined
      })
    },

    /**
     * 
     */
    addFormColumnsChange(item, e) {
      let column = this.$utils.find(this.columns, f => f.model == e);
      if (column != null) {
        //判断类型
        item.columnType = column.type;
        switch (column.type) {
          case "radio":
            if (column.options.dynamic) {
              //获取接口数据
            } else {
              item.defaultOptions = column.options.options;
            }
            break;
        }
      }
    },

    /**
     * 删除
     * @param {} row 
     */
    delFormColumns(row) {
      let spIndex = -1;
      this.trigger.automation.editColumn.forEach((item, index) => {
        if (item._X_ROW_KEY == row._X_ROW_KEY) {
          spIndex = index;
        }
      });
      if (spIndex != -1) {
        this.trigger.automation.editColumn.splice(spIndex, 1);
      }
    },
    /**
         *
         */
    designerClose() {
      this.designer.visible = false;
      this.prints.visible = false;
    },
    /**
     * 常用
     * @param {*} e 
     */
    commonUse(e) {
      switch (e.key) {
        case "buttonCrud":
          this.buttonCrud();
          break;
        case "buttonWorkflow":
          this.buttonWorkflow();
          break;
      }
    },
    /**
     * 增删改查按钮
     */
    buttonCrud() {
      var button = [
        {
          triggerType: 1,//触发执行类型:1方法，2脚本，3接口
          style: "primary",
          shape: "",
          name: "新增",
          icon: "plus",
          theme: "outlined",
          method: "add",
          script: null,
          isFreeze: false,
          isShowTable: false,
          remark: null,
        },
        {
          triggerType: 1,//触发执行类型:1方法，2脚本，3接口
          style: "primary",
          shape: "",
          name: "编辑",
          icon: "edit",
          theme: "outlined",
          method: "update",
          script: null,
          isFreeze: false,
          isShowTable: false,
          remark: null,
        },
        {
          triggerType: 1,//触发执行类型:1方法，2脚本，3接口
          style: "danger",
          shape: "",
          name: "批量删除",
          icon: "delete",
          theme: "outlined",
          method: "del",
          script: null,
          isFreeze: false,
          isShowTable: false,
          remark: null,
        },
        {
          triggerType: 1,//触发执行类型:1方法，2脚本，3接口
          style: "primary",
          shape: "",
          name: "复制",
          icon: "copy",
          theme: "outlined",
          method: "copy",
          script: null,
          isFreeze: false,
          isShowTable: false,
          remark: null,
        },
        {
          triggerType: 1,//触发执行类型:1方法，2脚本，3接口
          style: "upload",
          shape: "",
          name: "导入",
          icon: "import",
          theme: "outlined",
          method: "importBusinessData",
          script: null,
          isFreeze: false,
          isShowTable: false,
          remark: null,
        },
        {
          triggerType: 1,//触发执行类型:1方法，2脚本，3接口
          style: "primary",
          shape: "",
          name: "Excel在线导入",
          icon: "import",
          theme: "outlined",
          method: "importExcel",
          script: null,
          isFreeze: false,
          isShowTable: false,
          remark: null,
        },
        {
          triggerType: 1,//触发执行类型:1方法，2脚本，3接口
          style: "primary",
          shape: "",
          name: "导入模板下载",
          icon: "download",
          theme: "outlined",
          method: "importTemplate",
          script: null,
          isFreeze: false,
          isShowTable: false,
          remark: null,
        },
        {
          triggerType: 1,//触发执行类型:1方法，2脚本，3接口
          style: "primary",
          shape: "",
          name: "导出",
          icon: "file-excel",
          theme: "outlined",
          method: "reportBusinessData",
          script: null,
          isFreeze: false,
          isShowTable: false,
          remark: null,
        },
      ]

      let that = this;
      button.forEach(f => {
        that.button.push(f)
      })
    },
    /**
    * 流程相关按钮
    */
    buttonWorkflow() {
      var button = [
        {
          triggerType: 1,//触发执行类型:1方法，2脚本，3接口
          style: "primary",
          shape: "",
          name: "发起",
          icon: "plus",
          theme: "outlined",
          method: "workflowStart",
          script: null,
          isFreeze: false,
          isShowTable: false,
          remark: null,
        },
        {
          triggerType: 1,//触发执行类型:1方法，2脚本，3接口
          style: "primary",
          shape: "",
          name: "编辑",
          icon: "edit",
          theme: "outlined",
          method: "workflowEdit",
          script: null,
          isFreeze: false,
          isShowTable: false,
          remark: null,
        },
        {
          triggerType: 1,//触发执行类型:1方法，2脚本，3接口
          style: "primary",
          shape: "",
          name: "取消",
          icon: "close-circle",
          theme: "outlined",
          method: "workflowRevoke",
          script: null,
          isFreeze: false,
          isShowTable: false,
          remark: null,
        },
        {
          triggerType: 1,//触发执行类型:1方法，2脚本，3接口
          style: "danger",
          shape: "",
          name: "批量删除",
          icon: "delete",
          theme: "outlined",
          method: "del",
          script: null,
          isFreeze: false,
          isShowTable: false,
          remark: null,
        },
      ]

      let that = this;
      button.forEach(f => {
        that.button.push(f)
      })
    },
    /**
     * 取消
     */
    cancel() {
      this.$emit("update:visible", false);
    },
    /**
     * 新增
     */
    add() {
      this.button.push({
        menuButtonId: newGuid(),
        icon: "",
        isFreeze: false,
        isShowTable: false,
        triggerType: 1,//触发执行类型:1方法，2脚本，3接口
        method: null,
        script: null,
        api: null,//接口配置
        automation: null,
        theme: "outlined",
        style: "primary",
        shape: "",
        remark: "",
        name: ""
      });
      this.rowDrop();
    },
    /**
     * 拖拽
     */
    rowDrop() {
      let that = this;
      this.$nextTick(() => {
        const xTable = this.$refs.table;
        if (xTable) {
          this.sortable1 = Sortable.create(
            xTable.$el.querySelector(".body--wrapper>.vxe-table--body tbody"),
            {
              handle: ".drag-btn",
              onEnd: ({ newIndex, oldIndex }) => {
                const currRow = that.button.splice(oldIndex, 1)[0];
                that.button.splice(newIndex, 0, currRow);
              },
            }
          );
        }
      })
    },
    /**
     * 删除
     */
    del(row) {
      let spIndex = -1;
      this.button.forEach((item, index) => {
        if (item._X_ROW_KEY == row._X_ROW_KEY) {
          spIndex = index;
        }
      });
      if (spIndex != -1) {
        this.button.splice(spIndex, 1);
      }
    },

    /**
     * 初始化
     */
    init() {
      let that = this;
      this.table.loading = true;
      query({
        current: 1,
        size: 100,
        sord: "asc",
        sidx: "",
        id: this.menuId,
        filters: "",
      }).then((result) => {
        that.table.loading = false;
        if (result.code == that.eipResultCode.success) {
          that.button = result.data;
          that.rowDrop();
        }
      });
      //获取配置
      findColumnJson({ menuId: this.menuId }).then(result => {
        if (result.code == this.eipResultCode.success) {
          that.columns = JSON.parse(result.data.columnJson);
          that.dataFromName = result.data.dataFromName;
          that.dataFrom = result.data.dataFrom;
          that.dataBaseId = result.data.dataBaseId;
        }
      })
    },

    /**
     *
     */
    triggerTypeEdit(item) {
      this.trigger.type = item.triggerType;
      if (item.triggerType == 1) {
        this.trigger.method = item.method ? item.method : "";

      }
      else if (item.triggerType == 2) {
        this.trigger.script = item.script ? item.script : "";
      }
      //api
      else if (item.triggerType == 3) {
        if (item.api) {
          this.trigger.api = JSON.parse(item.api);
        } else {
          this.trigger.api = {
            path: null,
            type: 'post',
            confirm: {
              is: false,
              okText: "确认",
              cancelText: "取消",
              title: "你确认执行此操作吗？",
              content: ""
            },
            confirmType: 1,//确认类型,1可不选择数据，2单选，3多选
          }
        }
      }
      //自动化
      else if (item.triggerType == 4) {
        if (item.automation) {
          this.trigger.automation = JSON.parse(item.automation);
        } else {
          this.trigger.automation = {
            confirmType: 1,//确认类型,1可不选择数据，2单选，3多选
            action: 1,//动作，1执行工作流，2填写表单字段
            confirm: {
              is: false,
              okText: "确认",
              cancelText: "取消",
              title: "你确认执行此操作吗？",
              content: ""
            },
            editColumn: [],//填写字段
            saveFormAndDoWorkflowEvent: false//填写完表单后继续执行工作流
          }
        }
      }
      //打印
      else if (item.triggerType == 5) {
        if (item.prints) {
          this.trigger.prints = JSON.parse(item.prints);
        } else {
          this.trigger.prints = {
            preview: false
          }
        }
      }

      //导出
      else if (item.triggerType == 6) {
        if (item.export) {
          this.trigger.export = JSON.parse(item.export);
        } else {
          this.trigger.export = {
            pdf: false,
            name: '',
            file: []
          }
        }
      }
      this.trigger.visible = true;
      this.item = item;
    },

    /**
     * 设计
     */
    automationDesigner() {
      let that = this;
      let formData = {
        configId: this.item.menuButtonId,
        dataFromName: null,
        name: this.item.name,
        orderNo: 1,
        isFreeze: true,
        remark: null,
        configType: 4,
      }
      that.$loading.show({ text: that.eipMsg.saveLoading });
      saveAutomation(formData).then(function (result) {
        that.$loading.hide();
        if (result.code === that.eipResultCode.success) {
          that.designer.title = formData.name;
          that.designer.visible = true;
          that.designer.configId = formData.configId;
        }
      });
    },
    /**
         * 设计
         */
    printDesigner() {
      let that = this;
      let formData = {
        configId: this.item.menuButtonId,
        dataFromName: this.dataFromName,
        dataFrom: this.dataFrom,
        dataBaseId: this.dataBaseId,
        name: this.item.name,
        orderNo: 1,
        isFreeze: true,
        remark: null,
        configType: 6,
      }
      that.$loading.show({ text: that.eipMsg.saveLoading });
      saveAgile(formData).then(function (result) {
        that.$loading.hide();
        if (result.code === that.eipResultCode.success) {
          that.prints.title = formData.name;
          that.prints.visible = true;
          that.prints.configId = formData.configId;
        }
      });
    },
    /**
     * 
     */
    triggerTypeOk() {
      if (this.trigger.type == 1) {
        this.item.method = this.trigger.method;
      }
      else if (this.trigger.type == 2) {
        this.item.script = this.trigger.script;
      }
      else if (this.trigger.type == 3) {
        this.item.api = JSON.stringify(this.trigger.api);
      }

      else if (this.trigger.type == 4) {
        this.item.automation = JSON.stringify(this.trigger.automation);
      }
      else if (this.trigger.type == 5) {
        this.item.prints = JSON.stringify(this.trigger.prints);
      }
      else if (this.trigger.type == 6) {
        //判断是否选择了模版
        if (this.trigger.export.file.length == 0) {
          this.$message.error('请选择导出模版');
        } else {
          this.item.export = JSON.stringify(this.trigger.export);
          this.trigger.visible = false
        }
      }
    },

    /**
     *图标点击
     */
    iconClick(row, data) {
      row.icon = data.name;
      row.theme = data.theme;
    },

    /**
     * 清空图标
     */
    iconClear(row) {
      row.icon = "";
      row.theme = "";
    },
    /**
     * 保存
     */
    save() {
      let that = this;
      that.$loading.show({ text: that.eipMsg.saveLoading });
      saveAll({
        menuId: that.menuId,
        menuButton: JSON.stringify(that.button),
      }).then(function (result) {
        if (result.code === that.eipResultCode.success) {
          that.$message.success(result.msg);
          that.cancel();
          that.$emit("confirm");
        } else {
          that.$message.error(result.msg);
        }
        that.$loading.hide();
      });
    },
  },
};
</script>

<style lang="less" scoped>
/deep/ .CodeMirror {
  height: calc(100vh - 110px);
}
</style>
