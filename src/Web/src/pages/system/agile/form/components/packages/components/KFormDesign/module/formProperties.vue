<template>
  <div class="properties-centent kk-checkbox">
    <div class="properties-body">
      <a-form>
        <a-form-item label="表单布局">
          <Radio buttonStyle="solid" v-model="config.layout">
            <RadioButton value="horizontal">水平</RadioButton>
            <RadioButton value="vertical">垂直</RadioButton>
            <RadioButton value="inline">行内</RadioButton>
          </Radio>
        </a-form-item>
        <a-form-item label="标签布局（水平布局生效）">
          <Radio buttonStyle="solid" v-model="config.labelLayout">
            <RadioButton value="flex">固定</RadioButton>
            <RadioButton value="Grid">栅格</RadioButton>
          </Radio>
        </a-form-item>
        <a-form-item v-show="config.labelLayout === 'flex'" label="标签宽度（px）">
          <InputNumber v-model="config.labelWidth" class="eip-width-full" />
        </a-form-item>
        <a-form-item label="labelCol" v-show="config.labelLayout !== 'flex'">
          <div class="change-col-box">
            <Slider id="test" :max="24" :min="0" v-model="config.labelCol.xs" @change="handleChangeCol" />
            <div>
              <label>xs:</label>
              <InputNumber v-model="config.labelCol.xs" />
            </div>
            <div>
              <label>sm:</label>
              <InputNumber v-model="config.labelCol.sm" />
            </div>
            <div>
              <label>md:</label>
              <InputNumber v-model="config.labelCol.md" />
            </div>
            <div>
              <label>lg:</label>
              <InputNumber v-model="config.labelCol.lg" />
            </div>
            <div>
              <label>xl:</label>
              <InputNumber v-model="config.labelCol.xl" />
            </div>
            <div>
              <label>xxl:</label>
              <InputNumber v-model="config.labelCol.xxl" />
            </div>
          </div>
        </a-form-item>
        <a-form-item label="wrapperCol" v-show="config.labelLayout !== 'flex'">
          <div class="change-col-box">
            <div>
              <label>xs:</label>
              <InputNumber v-model="config.wrapperCol.xs" />
            </div>
            <div>
              <label>sm:</label>
              <InputNumber v-model="config.wrapperCol.sm" />
            </div>
            <div>
              <label>md:</label>
              <InputNumber v-model="config.wrapperCol.md" />
            </div>
            <div>
              <label>lg:</label>
              <InputNumber v-model="config.wrapperCol.lg" />
            </div>
            <div>
              <label>xl:</label>
              <InputNumber v-model="config.wrapperCol.xl" />
            </div>
            <div>
              <label>xxl:</label>
              <InputNumber v-model="config.wrapperCol.xxl" />
            </div>
          </div>
        </a-form-item>
        <a-form-item label="弹出类型">
          <a-select v-model="config.showFormType">
            <a-select-option value="modal">
              对话框
            </a-select-option>
            <a-select-option value="drawer">
              抽屉
            </a-select-option>
          </a-select>
        </a-form-item>
        <a-form-item label="弹出宽度">
          <a-input-group compact>
            <a-input-number style="width:174px" v-model="config.showFormWidth" placeholder="请输入弹出宽度" />
            <a-select v-model="config.showFormWidthUnit" style="width:60px">
              <a-select-option value="px">
                px
              </a-select-option>
              <a-select-option value="%">
                %
              </a-select-option>
            </a-select>
          </a-input-group>
        </a-form-item>

        <a-form-item label="弹出选项">
          <kCheckbox v-if="config.showFormType == 'modal'" v-model="config.modalCentered" label="弹出居中" />
          <kCheckbox v-model="config.maskClosable" label="点击蒙层是否允许关闭" />
        </a-form-item>
        <a-form-item label="弹出层级数" help="">
          <a-tooltip>
            <template slot="title">
              数值越大越在顶部
            </template>
            <a-input-number class="eip-width-full" :min="1000" v-model="config.zIndex" />
          </a-tooltip>
        </a-form-item>
        <a-form-item label="表单CSS">
          <a-textarea v-model="config.customStyle" placeholder="请输入表单CSS" />
        </a-form-item>
        <a-form-item label="表单属性">
          <kCheckbox v-model="config.hideRequiredMark" label="隐藏必选标记" />
          <kCheckbox v-model="config.labelBold" label="标签加粗" />
        </a-form-item>
        <a-form-item label="按钮配置">
          <a-button style="text-align: left;" @click="button.visible = true" block><a-icon type="code" />
            <a-badge :count="config.buttons.length">
              按钮配置
            </a-badge>
          </a-button>
          <buttons v-if="button.visible" @confirm="buttonConfirm" :data="config.buttons" :visible.sync="button.visible"
            title="按钮配置"></buttons>
        </a-form-item>

        <a-form-item label="事件脚本（PC）">
          <a-button v-if="isDefined(config.event.pc.onload)" style="text-align: left;" @click="eventClick(1)"
            block><a-icon type="code" />
            <a-badge :dot="config.event.pc.onload != null && config.event.pc.onload != ''">
              数据加载完成
            </a-badge>
          </a-button>
          <a-button v-if="isDefined(config.event.pc.change)" style="text-align: left;" @click="eventClick(2)"
            block><a-icon type="code" />
            <a-badge :dot="config.event.pc.change != null && config.event.pc.change != ''">
              数据改变
            </a-badge>
          </a-button>

          <a-button v-if="isDefined(config.event.pc.submitBefore)" style="text-align: left;" @click="eventClick(5)"
            block><a-icon type="code" />
            <a-badge :dot="config.event.pc.submitBefore != null && config.event.pc.submitBefore != ''">
              提交数据前
            </a-badge>
          </a-button>
          <a-button v-if="isDefined(config.event.pc.submitAfter)" style="text-align: left;" @click="eventClick(6)"
            block><a-icon type="code" />
            <a-badge :dot="config.event.pc.submitAfter != null && config.event.pc.submitAfter != ''">
              提交数据后
            </a-badge>
          </a-button>
        </a-form-item>
        <a-form-item label="事件脚本（Mobile）">
          <a-button v-if="isDefined(config.event.mobile.onload)" style="text-align: left;" @click="eventClick(3)"
            block><a-icon type="code" />
            <a-badge :dot="config.event.mobile.onload != null && config.event.mobile.onload != ''">
              数据加载完成
            </a-badge>
          </a-button>
          <a-button v-if="isDefined(config.event.mobile.change)" style="text-align: left;" @click="eventClick(4)"
            block><a-icon type="code" />
            <a-badge :dot="config.event.mobile.change != null && config.event.mobile.change != ''">
              数据改变
            </a-badge>
          </a-button>

          <a-button v-if="isDefined(config.event.mobile.submitBefore)" style="text-align: left;" @click="eventClick(7)"
            block><a-icon type="code" />
            <a-badge :dot="config.event.mobile.submitBefore != null && config.event.mobile.submitBefore != ''">
              提交数据前
            </a-badge>
          </a-button>
          <a-button v-if="isDefined(config.event.mobile.submitAfter)" style="text-align: left;" @click="eventClick(8)"
            block><a-icon type="code" />
            <a-badge :dot="config.event.mobile.submitAfter != null && config.event.mobile.submitAfter != ''">
              提交数据后
            </a-badge>
          </a-button>
        </a-form-item>

        <a-form-item label="提示"> 实际预览效果请点击预览查看 </a-form-item>
      </a-form>
    </div>
    <a-drawer width="1200px" placement="right" :bodyStyle="{ padding: '1px' }" :visible="event.visible"
      :title="event.title" @close="event.visible = false" :destroyOnClose="true">
      <div class="beauty-scroll" style="height: calc(100vh - 110px);">
        <eip-code :value.sync="event.script"></eip-code>
      </div>
      <div class="eip-drawer-toolbar">
        <a-space>
          <a-popover title="变量说明：data:当前控件值  formValue值">
            <template slot="content">
              <a-descriptions class="eip-descriptions" bordered size="small" :column="1">
                <a-descriptions-item label="隐藏字段">
                  this.$refs.kfb.hide([字段名称集合]);
                  <a-tooltip>
                    <template slot="title">
                      点击复制
                    </template>
                    <a-icon class="copy" data-clipboard-text="this.$refs.kfb.hide([字段名称集合]);" @click="copy()"
                      type="copy">
                    </a-icon>
                  </a-tooltip>
                </a-descriptions-item>
                <a-descriptions-item label="禁用字段">this.$refs.kfb.disable([字段名称集合]);
                  <a-tooltip>
                    <template slot="title">
                      点击复制
                    </template>
                    <a-icon class="copy" data-clipboard-text="this.$refs.kfb.disable([字段名称集合]);" @click="copy()"
                      type="copy">
                    </a-icon>
                  </a-tooltip>

                </a-descriptions-item>
                <a-descriptions-item label="启用字段">this.$refs.kfb.enable([字段名称集合]);
                  <a-tooltip>
                    <template slot="title">
                      点击复制
                    </template>
                    <a-icon class="copy" data-clipboard-text="this.$refs.kfb.enable([字段名称集合]);" @click="copy()"
                      type="copy">
                    </a-icon>
                  </a-tooltip>
                </a-descriptions-item>
                <a-descriptions-item label="字段验证">this.$refs.kfb.rule([验证规则]);<a-tooltip>
                    <template slot="title">
                      点击复制
                    </template>
                    <a-icon class="copy" data-clipboard-text="this.$refs.kfb.rule([验证规则]);" @click="copy()" type="copy">
                    </a-icon>
                  </a-tooltip></a-descriptions-item>
                <a-descriptions-item label="子表不可编辑">this.$refs.kfb.batchFieldDisabled([子表名],
                  字段名);<a-tooltip>
                    <template slot="title">
                      点击复制
                    </template>
                    <a-icon class="copy" data-clipboard-text="his.$refs.kfb.batchFieldDisabled([子表名],
                  字段名);" @click="copy()" type="copy">
                    </a-icon>
                  </a-tooltip></a-descriptions-item>
                <a-descriptions-item label="子表可复制">this.$refs.kfb.batchButtonHide([按钮下标], 子表名);<a-tooltip>
                    <template slot="title">
                      点击复制
                    </template>
                    <a-icon class="copy" data-clipboard-text="this.$refs.kfb.batchButtonHide([按钮下标], 子表名);"
                      @click="copy()" type="copy">
                    </a-icon>
                  </a-tooltip></a-descriptions-item>
                <a-descriptions-item label="赋值">this.$refs.kfb.setData({['字段名']: 值});<a-tooltip>
                    <template slot="title">
                      点击复制
                    </template>
                    <a-icon class="copy" data-clipboard-text="this.$refs.kfb.setData({['字段名']: 值});" @click="copy()"
                      type="copy">
                    </a-icon>
                  </a-tooltip></a-descriptions-item>
              </a-descriptions>
            </template>
            <a-button type="primary">
              <a-icon type="api" /> 变量说明
            </a-button>
          </a-popover>
          <a-popover title="字段">
            <template slot="content">
              <a-descriptions class="eip-descriptions" bordered size="small" :column="1">
                <a-descriptions-item v-for="(item, index) in list" :label="item.label" :key="index">
                  {{ item.model }}
                  <a-tooltip>
                    <template slot="title">
                      点击复制
                    </template>
                    <a-icon class="copy" :data-clipboard-text="item.model" @click="copy()" type="copy">
                    </a-icon>
                  </a-tooltip>
                </a-descriptions-item>
              </a-descriptions>
            </template>
            <a-button type="primary">
              <a-icon type="container" /> 字段
            </a-button>
          </a-popover>
          <a-button key="back" @click="event.visible = !event.visible"><a-icon type="close" />取消</a-button>
          <a-button @click="eventScriptSaveClick" type="primary"><a-icon type="save" />提交</a-button>
        </a-space>
      </div>
    </a-drawer>
  </div>
</template>
<script>
import Clipboard from "clipboard";
/*
 * author kcz
 * date 2019-11-20
 * description 表单属性设置面板组件
 */
import kCheckbox from "../../KCheckbox/index.vue";
import buttons from "../components/button/index.vue";
import { pluginManager } from "../../../utils/index";
const InputNumber = pluginManager.getComponent("number").component;
const Radio = pluginManager.getComponent("radio").component;
const RadioButton = pluginManager.getComponent("radioButton").component;
const Slider = pluginManager.getComponent("aSlider").component;


export default {
  name: "formProperties",
  components: {
    buttons,
    kCheckbox,
    InputNumber,
    Radio,
    RadioButton,
    Slider
  },
  data() {
    return {

      button: {
        visible: false,
      },
      event: {
        visible: false,
        script: "",
        type: '',
        title: '事件脚本配置'
      },
    };
  },
  props: {
    config: {
      type: Object,
      required: true
    },
    previewOptions: {
      type: Object,
      required: true
    },
    list: {
      type: Array
    }
  },
  methods: {
    /**
          * 
          */
    copy() {
      // 复制数据
      let clipboard = new Clipboard('.copy');
      clipboard.on("success", () => {
        this.$message.success("复制成功");
      });
      clipboard.on("error", () => {
        this.$message.error("复制失败");
      });
      setTimeout(() => {
        // 销毁实例
        clipboard.destroy();
      }, 122);
    },
    /**
    * 判断是否已定义
    * @param {*} value
    */
    isDefined(value) {
      return typeof value !== "undefined";
    },
    buttonConfirm(buttons) {
      this.config.buttons = buttons;
    },
    /**
    *
    * @param {*} v
    */
    eventScriptChange(v) {
      switch (this.event.type) {
        case 1:
          this.config.event.pc.onload = v;
          break;
        case 2:
          this.config.event.pc.change = v;
          break;
        case 3:
          this.config.event.mobile.onload = v;
          break;
        case 4:
          this.config.event.mobile.change = v;
          break;

        case 5:
          this.config.event.pc.submitBefore = v;
          break;
        case 6:
          this.config.event.pc.submitAfter = v;
          break;
        case 7:
          this.config.event.mobile.submitBefore = v;
          break;
        case 8:
          this.config.event.mobile.submitAfter = v;
          break;
      }

    },
    /**
     *
     */
    eventClick(type) {
      this.event.type = type;
      let title = ''
      switch (type) {
        case 1:
          title = '数据加载完成（PC）';
          this.event.script = this.config.event.pc.onload;
          break;
        case 2:
          title = '数据改变（PC）';
          this.event.script = this.config.event.pc.change;
          break;
        case 3:
          title = '数据加载完成（Mobile）';
          this.event.script = this.config.event.mobile.onload;
          break;
        case 4:
          title = '数据改变（Mobile）';
          this.event.script = this.config.event.mobile.change;
          break;

        case 5:
          title = '提交数据前（PC）';
          this.event.script = this.config.event.pc.submitBefore;
          break;
        case 6:
          title = '提交数据后（PC）';
          this.event.script = this.config.event.pc.submitAfter;
          break;
        case 7:
          title = '提交数据前（Mobile）';
          this.event.script = this.config.event.mobile.submitBefore;
          break;
        case 8:
          title = '提交数据后（Mobile）';
          this.event.script = this.config.event.mobile.submitAfter;
          break;
      }
      this.event.title = title + "脚本配置";
      this.event.visible = true;
    },

    /**
     *
     */
    eventScriptSaveClick() {
      this.event.visible = false;
    },

    handleChangeCol(e) {
      this.config.labelCol.xs = this.config.labelCol.sm = this.config.labelCol.md = this.config.labelCol.lg = this.config.labelCol.xl = this.config.labelCol.xxl = e;

      this.config.wrapperCol.xs = this.config.wrapperCol.sm = this.config.wrapperCol.md = this.config.wrapperCol.lg = this.config.wrapperCol.xl = this.config.wrapperCol.xxl =
        24 - e;
    }
  }
};
</script>
<style lang="less" scoped>
/deep/ .CodeMirror {
  height: calc(100vh - 110px);
}

.eip-descriptions /deep/ .ant-descriptions-item-label {
  width: 120px !important;
}

.eip-descriptions /deep/ .ant-descriptions-item-content {
  word-wrap: break-word;
  word-break: break-all;
}
</style>
