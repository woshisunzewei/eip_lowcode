<template>
  <a-config-provider :locale="locale">
    <a-form v-bind="$attrs" selfUpdate v-if="typeof value.list !== 'undefined' && typeof value.config !== 'undefined'
    " class="eip-form-build" :layout="value.config.layout" :hideRequiredMark="value.config.hideRequiredMark"
      :form="form" @submit="handleSubmit" :style="value.config.customStyle">
      <buildBlocks ref="buildBlocks" @handleReset="reset" v-for="(record, index) in value.list" :record="record"
        :formValue="formValue" :dynamicData="getDynamicData" :config="config" :disabled="disabled"
        :formConfig="value.config" :validatorError="validatorError" :key="index" @change="handleChange" />
    </a-form>
  </a-config-provider>
</template>
<script>
/*
 * author kcz
 * date 2019-11-20
 * description 将json数据构建成表单
 */
import buildBlocks from "./buildBlocks";
import zhCN from "ant-design-vue/lib/locale-provider/zh_CN";
import { lazyLoadTick } from "../../utils/index";

export default {
  name: "KFormBuild",
  data() {
    return {
      locale: zhCN,
      form: this.$form.createForm(this),
      validatorError: {},
      defaultDynamicData: {}
    };
  },
  props: {
    value: {
      type: Object,
      required: true
    },
    dynamicData: {
      type: Object,
      default: () => {
        return {};
      }
    },
    config: {
      type: Object,
      default: () => ({})
    },
    disabled: {
      type: Boolean,
      default: false
    },
    outputString: {
      type: Boolean,
      default: false
    },
    defaultValue: {
      type: Object,
      default: () => ({})
    },


    formValue: {
      type: Object,
      default: () => ({})
    }
  },
  components: {
    buildBlocks
  },
  computed: {
    getDynamicData() {
      return typeof this.dynamicData === "object" &&
        Object.keys(this.dynamicData).length
        ? this.dynamicData
        : window.$kfb_dynamicData || {};
    }
  },
  methods: {
    // moment,
    handleSubmit(e) {
      // 提交按钮触发，并触发submit函数，返回getData函数
      e.preventDefault();
      this.$emit("submit", this.getData);
    },
    reset() {
      // 重置表单
      this.form.resetFields();
    },
    getData() {
      // 提交函数，提供父级组件调用
      return new Promise((resolve, reject) => {
        try {
          this.form.validateFields((err, values) => {
            if (err) {
              reject(err);
              /**
               * @author: lizhichao<meteoroc@outlook.com>
               * @Description: 多容器校验时，提供error返回给多容器进行判断。
               */
              this.validatorError = err;
              return;
            }
            this.validatorError = {};
            this.$refs.buildBlocks.forEach(item => {
              if (!item.validationSubform()) {
                reject(err);
              }
            });
            if (this.outputString) {
              // 需要所有value转成字符串
              for (const key in values) {
                const type = typeof values[key];
                if (type === "string" || type === "undefined") {
                  continue;
                } else if (type === "object") {
                  values[key] = `k-form-design#${type}#${JSON.stringify(
                    values[key]
                  )}`;
                } else {
                  values[key] = `k-form-design#${type}#${String(values[key])}`;
                }
              }

              resolve(values);
            } else {
              resolve(values);
            }
          });
        } catch (err) {
          reject(err);
        }
      });
    },
    setData(json) {
      return new Promise((resolve, reject) => {
        lazyLoadTick.nextTick(() => {
          try {
            if (this.outputString) {
              // 将非string数据还原
              for (const key in json) {
                if (!json[key].startsWith("k-form-design#")) {
                  continue;
                }
                const array = json[key].split("#");
                if (array[1] === "object") {
                  json[key] = JSON.parse(array[2]);
                } else if (array[1] === "number") {
                  json[key] = Number(array[2]);
                } else if (array[1] === "boolean") {
                  json[key] = Boolean(array[2]);
                }
              }
              this.form.setFieldsValue(json);
            } else {
              this.form.setFieldsValue(json);
            }
            resolve(true);
          } catch (err) {
            console.error(err);
            reject(err);
          }
        });
      });
    },

    // 批量设置某个option的值
    setOptions(fields, optionName, value) {
      fields = new Set(fields);

      // 递归遍历控件树
      const traverse = array => {
        array.forEach(element => {
          if (fields.has(element.model)) {
            this.$set(element.options, optionName, value);
          }
          if (["grid", "tabs", "selectInputList", "collapse"].includes(element.type)) {
            // 栅格布局 and 标签页
            element.columns.forEach(item => {
              traverse(item.list);
            });
          } else if (element.type === "card" || element.type === "batch") {
            // 卡片布局 and  动态表格
            traverse(element.list);
          } else if (element.type === "table") {
            // 表格布局
            element.trs.forEach(item => {
              item.tds.forEach(val => {
                traverse(val.list);
              });
            });
          }
        });
      };
      traverse(this.value.list);
    },

    /**
     * 赋予默认值
     * @param {*} fields 
     */
    options(fields, value) {
      this.setOptions(fields, "options", value);
    },

    // 隐藏表单字段
    hide(fields) {
      this.setOptions(fields, "hidden", true);
      this.setOptions(fields, "showLabel", false);
    },
    // 显示表单字段
    show(fields) {
      this.setOptions(fields, "hidden", false);
      this.setOptions(fields, "showLabel", true);
    },
    // 禁用表单字段
    disable(fields) {
      this.setOptions(fields, "disabled", true);
    },
    // 启用表单字段
    enable(fields) {
      this.setOptions(fields, "disabled", false);
    },
    handleChange(value, key) {
      // 触发change事件
      this.$emit("change", value, key);
    },



    /**
    * 流程中处理子表按钮隐藏
    * @param {*} fields 
    * @param {*} optionName 
    */
    batchButtonHide(fields, optionName) {
      let that = this;
      var batch = this.value.list.filter(f => f.model == optionName);
      if (batch.length > 0) {
        var batchFilter = batch[0];
        fields.forEach(f => {
          batchFilter.options.buttons[f].isShow = false;
          that.$set(batchFilter.options, "buttons", batchFilter.options.buttons);
        })
      }
    },

    /**
     * 子表字段隐藏
     * @param {*} fields 
     * @param {*} optionName 
     */
    batchFieldHide(fields, optionName) {
      let that = this;
      var batch = this.value.list.filter(f => f.model == optionName);
      if (batch.length > 0) {
        var batchFilter = batch[0];
        fields.forEach(f => {
          var field = batchFilter.list.filter(fl => fl.model == f);
          that.$set(field[0].options, "hidden", true);
        })
      }
    },

    /**
     * 子表字段不可用
     * @param {*} fields 
     * @param {*} optionName 
     */
    batchFieldDisabled(fields, optionName) {
      let that = this;
      var batch = this.value.list.filter(f => f.model == optionName);
      if (batch.length > 0) {
        var batchFilter = batch[0];
        fields.forEach(f => {
          var field = batchFilter.list.filter(fl => fl.model == f);
          that.$set(field[0].options, "disabled", true);
        })
      }
    },

    /**
     * 子表不能复制
     * @param {*} fields 
     * @param {*} optionName 
     */
    batchCopy(fields) {
      this.setOptions(fields, "copy", false);
    },

    /**
     * 子表不能删除
     * @param {*} fields 
     */
    batchDelete(fields) {
      this.setOptions(fields, "delete", false);
    },

    getOptions() {
      var fields = [];

      // 递归遍历控件树
      const traverse = (array) => {
        array.forEach((element) => {
          if (["grid", "tabs", "selectInputList", "collapse"].includes(element.type)) {
            // 栅格布局 and 标签页
            element.columns.forEach((item) => {
              traverse(item.list);
            });
          } else if (element.type === "card") {
            // 卡片布局 and  动态表格
            traverse(element.list);
          } else if (element.type === "table") {
            // 表格布局
            element.trs.forEach((item) => {
              item.tds.forEach((val) => {
                traverse(val.list);
              });
            });
          } else {
            fields.push(element);
          }
        });
      };
      traverse(this.value.list);
      return fields;
    },

    getFieldsValue() {
      return this.form.getFieldsValue();
    },

    // 设置规则
    rule(fields) {
      // 递归遍历控件树
      const traverse = (array) => {
        array.forEach((element) => {
          var have = fields.filter((f) => f.name == element.model);
          if (have.length > 0) {
            var rules = [];
            have[0].validator.forEach((h) => {
              rules.push({
                required: true,
                message: h.message,
              });
            });
            this.$set(element, "rules", rules);
          }
          if (["grid", "tabs", "selectInputList", "collapse"].includes(element.type)) {
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
      traverse(this.value.list);
    },
  },
  mounted() {
    this.$nextTick(() => {
      this.setData(this.defaultValue);
      this.$emit("complete");
    });
  },
  created() {
    lazyLoadTick.reset();
  }
};
</script>
