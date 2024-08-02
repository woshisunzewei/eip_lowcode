<template>
  <a-modal width="700px" v-drag :destroyOnClose="true" :maskClosable="false" :visible="visible"
    :bodyStyle="{ padding: '1px' }" :title="title" @cancel="cancel">
    <a-spin :spinning="spinning">
      <a-form-model ref="form" :model="form" :rules="rules" :label-col="config.labelCol"
        :wrapper-col="config.wrapperCol">
        <a-row>
          <a-col>
            <a-form-model-item label="归属模块" prop="menuId">
              <a-tree-select placeholder="请选择归属模块" allow-clear v-model="form.menuId" show-search style="width: 100%"
                treeNodeFilterProp="title" :tree-data="data" :dropdown-style="{ maxHeight: '400px', overflow: 'auto' }"
                @select="treeSelect">
              </a-tree-select>
            </a-form-model-item>
            <a-form-model-item label="名称" prop="name">
              <a-input v-model="form.name" placeholder="请输入名称" allow-clear />
            </a-form-model-item>
            <a-form-model-item label="类型" prop="style">
              <a-select default-value="primary" allow-clear v-model="form.style">
                <a-select-option value="primary">主按钮 </a-select-option>
                <a-select-option value=""> 次按钮 </a-select-option>
                <a-select-option value="dashed"> 虚线按钮 </a-select-option>
                <a-select-option value="danger"> 危险按钮 </a-select-option>
                <a-select-option value="link"> 链接按钮 </a-select-option>
                <a-select-option value="upload"> 上传按钮 </a-select-option>
              </a-select>
            </a-form-model-item>
            <a-form-model-item label="形状" prop="style">
              <a-select default-value="primary" allow-clear v-model="form.shape">
                <a-select-option value="">长方形 </a-select-option>
                <a-select-option value="circle">圆形 </a-select-option>
                <a-select-option value="round"> 椭圆形 </a-select-option>
              </a-select>
            </a-form-model-item>
            <a-form-model-item label="图标" prop="icon">
              <eip-icon :name="form.icon" :theme="form.theme" @click="iconClick" @clear="iconClear"></eip-icon>
            </a-form-model-item>
            <a-form-model-item label="触发类型" prop="triggerType">
              <a-select default-value="primary" allow-clear v-model="form.triggerType">
                <a-select-option :value="1">方法 </a-select-option>
                <a-select-option :value="2">脚本 </a-select-option>
                <a-select-option :value="3">接口 </a-select-option>
                <a-select-option :value="4">自动化 </a-select-option>
              </a-select>
            </a-form-model-item>
            <a-form-model-item v-if="form.triggerType == 1" label="方法" prop="method">
              <a-input v-model="form.method" placeholder="请输入方法" allow-clear />
            </a-form-model-item>
            <a-form-model-item v-if="form.triggerType == 2" label="脚本" prop="script">
              <a-input type="textarea" v-model="form.script" allow-clear placeholder="请输入脚本" />
            </a-form-model-item>

            <a-form-model-item v-if="form.triggerType == 3" label="Api地址" prop="script">
              <a-input v-model="api.path" placeholder="请输入Api地址" allow-clear />
            </a-form-model-item>
            <a-form-model-item v-if="form.triggerType == 3" label="请求方式" prop="script">
              <a-radio-group v-model="api.type">
                <a-radio value="post">POST </a-radio>
                <a-radio value="get">GET </a-radio>
              </a-radio-group>
            </a-form-model-item>
            <a-form-model-item v-if="form.triggerType == 3" label="确认提示语" prop="confirmMsg">
              <a-input v-model="api.confirmMsg" allow-clear placeholder="请输入确认提示语:如(是否删除?)" />
            </a-form-model-item>
            <a-form-model-item v-if="form.triggerType == 3" label="数据选择类型" prop="confirmType">
              <a-radio-group v-model="api.confirmType">
                <a-radio :value="1">可空 </a-radio>
                <a-radio :value="2">一条 </a-radio>
                <a-radio :value="3">多条 </a-radio>
              </a-radio-group>
            </a-form-model-item>
            <a-form-model-item v-if="form.triggerType == 4" label="自动化设置" prop="designer">
              <a-button type="primary" @click="automationDesigner()">
                点击配置自动化设置
              </a-button>
            </a-form-model-item>
            <a-form-model-item v-if="form.triggerType == 4" label="数据选择类型" prop="confirmType">
              <a-radio-group v-model="automation.confirmType">
                <a-radio :value="1">可空 </a-radio>
                <a-radio :value="2">一条 </a-radio>
                <a-radio :value="3">多条 </a-radio>
              </a-radio-group>
            </a-form-model-item>
            <a-form-model-item label="选项">
              <a-checkbox :checked="form.isFreeze" @change="isFreeze">
                禁用
              </a-checkbox>
              <a-checkbox :checked="form.isShowTable" @change="isShowTable">
                列表显示
              </a-checkbox>
              <a-checkbox :checked="form.isShowMobile" @change="isShowMobile">
                手机端显示
              </a-checkbox>
            </a-form-model-item>

            <a-form-model-item label="排序号" prop="orderNo">
              <a-input-number id="inputNumber" style="width: 100%" placeholder="请输入排序号" v-model="form.orderNo" :min="0"
                :max="999" />
            </a-form-model-item>
            <a-form-model-item label="备注" prop="remark">
              <a-input v-model="form.remark" type="textarea" allow-clear placeholder="请输入备注" />
            </a-form-model-item>
          </a-col>
        </a-row>
      </a-form-model>
    </a-spin>
    <template slot="footer">
      <div class="flex justify-between align-center">
        <div>
          <a-checkbox v-if="!menuButtonId" v-model="save.retain">
            继续创建时，保留本次提交内容
          </a-checkbox>
        </div>

        <a-space>
          <a-button v-if="!menuButtonId" @click="saveContinue" :loading="loading"><a-icon
              type="save" />提交并继续创建</a-button>

          <a-button v-if="menuButtonId" @click="cancel"><a-icon type="close" />关闭</a-button>

          <a-button @click="saveClose" type="primary" :loading="loading"><a-icon type="save" />提交</a-button>
        </a-space>
      </div>
    </template>
    <automation-designer ref="designer" v-if="designer.visible" :visible.sync="designer.visible" :title="designer.title"
      :configId="designer.configId" @close="designerClose"></automation-designer>
  </a-modal>
</template>

<script>
import automationDesigner from "@/pages/system/agile/automation/designer";
import { save, findById } from "@/services/system/menubutton/edit";
import { newGuid } from "@/utils/util";
export default {
  name: "edit",
  components: { automationDesigner },
  data() {
    return {
      designer: {
        visible: false,
        configId: "",
        title: "",
        bodyStyle: {
          padding: "0",
        },
      },
      config: {
        labelCol: { span: 4 },
        wrapperCol: { span: 20 },
      },
      form: {
        menuButtonId: newGuid(),
        menuId: null,
        menuName: null,
        style: "primary",
        shape: '',
        name: null,
        icon: null,
        theme: null,
        triggerType: 1,//触发执行类型:1方法，2脚本，3接口
        method: null,
        script: null,
        api: null,//接口配置
        automation: null,//自动化
        orderNo: 1,
        isFreeze: false,
        isShowTable: true,
        isShowMobile: true,
        remark: null,
      },
      api: {
        path: null,
        type: 'post',
        confirmMsg: '',//确认提示信息
        confirmType: 1,//确认类型,1可不选择数据，2单选，3多选
      },
      automation: {
        confirmType: 1,//确认类型,1可不选择数据，2单选，3多选
      },
      rules: {
        menuId: [
          {
            required: true,
            message: "请选择归属模块",
            trigger: "change",
          },
        ],
        name: [
          {
            required: true,
            message: "请输入名称",
            trigger: "blur",
          },
        ],
      },
      loading: false,
      spinning: false,
      save: {
        continue: false,
        retain: false,
      },
    };
  },

  props: {
    visible: {
      type: Boolean,
      default: false,
    },
    data: {
      type: Array,
    },
    title: {
      type: String,
    },
    copy: {
      type: Boolean,
      default: false,
    },
    menuButtonId: {
      type: String,
    },
  },
  mounted() {
    this.find();
  },
  methods: {
    /**
    * 设计
    */
    automationDesigner() {
      let that = this;
      this.$refs.form.validate((valid) => {
        if (valid) {
          that.loading = true;
          that.$loading.show({ text: that.eipMsg.saveLoading });
          that.form.menuButtonId = that.copy
            ? newGuid()
            : that.form.menuButtonId;
          //api参数
          that.form.automation = JSON.stringify(that.api);
          save(that.form).then(function (result) {
            if (result.code === that.eipResultCode.success) {
              that.designer.title = that.form.name;
              that.designer.visible = true;
              that.designer.configId = that.form.menuButtonId;
              that.loading = false;
              that.$loading.hide();
            }
          });
        } else {
          return false;
        }
      });
    },
    /**
     *
     */
    isShowTable(e) {
      this.form.isShowTable = e.target.checked;
    },
    /**
     *
     */
    isFreeze(e) {
      this.form.isFreeze = e.target.checked;
    },
    /**
         *
         */
    isShowMobile(e) {
      this.form.isShowMobile = e.target.checked;
    },
    /**
     * 树选择
     */
    treeSelect(electedKeys, e) {
      this.form.menuName = e.title;
      this.form.menuId = e.eventKey;
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
    saveConfirm() {
      let that = this;
      this.$refs.form.validate((valid) => {
        if (valid) {
          that.loading = true;
          that.$loading.show({ text: that.eipMsg.saveLoading });
          that.form.menuButtonId = that.copy
            ? newGuid()
            : that.form.menuButtonId;

          if (that.form.triggerType == 3) {
            //api参数
            that.form.api = JSON.stringify(that.api);
          }

          save(that.form).then(function (result) {
            if (result.code === that.eipResultCode.success) {
              that.$message.success(result.msg);
              if (that.save.continue) {
                //不保留上次内容
                if (!that.save.retain) {
                  that.$refs.form.resetFields();
                }
                that.form.noticeId = newGuid();
              } else {
                that.cancel();
              }
              that.$emit("save", result);
            } else {
              that.$message.error(result.msg);
            }
            that.loading = false;
            that.$loading.hide();
          });
        } else {
          return false;
        }
      });
    },
    /**
     *
     */
    saveContinue() {
      this.save.continue = true;
      this.saveConfirm();
    },

    /**
     *
     */
    saveClose() {
      this.save.continue = false;
      this.saveConfirm();
    },
    /**
     * 根据Id查找
     */
    find() {
      let that = this;
      if (this.menuButtonId) {
        that.spinning = true;
        findById(this.menuButtonId).then(function (result) {
          if (that.copy) {
            result.data.orderNo += 1;
          }
          if (result.data.triggerType == 3) {
            that.api = JSON.parse(result.data.api);
          }
          that.form = result.data;
          that.spinning = false;
        });
      }
    },
    /**
     *图标点击
     */
    iconClick(icon) {
      this.form.icon = icon.name;
      this.form.theme = icon.theme;
    },

    /**
     * 清空图标
     */
    iconClear() {
      this.form.icon = null;
      this.form.theme = null;
    },
  },
};
</script>