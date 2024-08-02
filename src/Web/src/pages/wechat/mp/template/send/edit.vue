<template>
  <a-drawer width="600px" :visible="visible" :bodyStyle="{ padding: '1px' }" :title="title" @close="cancel"
    :destroyOnClose="true">
    <div class="eip-drawer-body beauty-scroll">
      <a-spin :spinning="spinning">
        <k-form-build ref="kfb" :value="template.design" /></a-spin>
    </div>

    <div class="eip-drawer-toolbar">
      <a-space>
        <a-button key="back" @click="cancel" :disabled="loading"><a-icon type="close" />取消</a-button>
        <a-button key="submit" @click="save" type="primary" :loading="loading"><a-icon type="save" />提交</a-button>
      </a-space>
    </div>
  </a-drawer>
</template>

<script>
import Vue from "vue";
import KFormBuild from "@/pages/system/agile/form/components/packages";
import "@/pages/system/agile/form/components/styles/form-design.less";
Vue.use(KFormBuild);

import { findById } from "@/services/wechat/mp/template/edit";
import {
  findByTemplateSendId,
  save,
} from "@/services/wechat/mp/template/send/edit";
import { newGuid } from "@/utils/util";
export default {
  name: "edit",
  data() {
    return {
      config: {
        labelCol: { span: 4 },
        wrapperCol: { span: 20 },
      },
      form: {
        templateSendId: newGuid(),
        templateId: this.templateId,
        data: null,
      },
      template: {
        design: {},
      },
      rules: {
        code: [
          {
            required: true,
            message: "请输入代码",
            trigger: "blur",
          },
        ],
        title: [
          {
            required: true,
            message: "请输入名称",
            trigger: "blur",
          },
        ],
      },

      loading: false,
      spinning: false,
    };
  },

  props: {
    visible: {
      type: Boolean,
      default: false,
    },
    templateSendId: {
      type: String,
    },
    templateId: {
      type: String,
    },
    title: {
      type: String,
    },
  },
  mounted() {
    this.find();
  },
  methods: {
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
      // 使用getData函数获取数据
      this.$refs.kfb
        .getData()
        .then((values) => {
          that.loading = true;
          that.spinning = true;
          that.form.data = JSON.stringify(values);
          save(that.form).then(function (result) {
            if (result.code === that.eipResultCode.success) {
              that.$message.success(result.msg);
              that.cancel();
              that.$emit("ok", result);
            } else {
              that.$message.error(result.msg);
            }
            that.loading = false;
            that.spinning = false;
          });
        })
        .catch(() => {
          console.log("验证未通过，获取失败");
        });
    },

    /**
     * 根据Id查找
     */
    find() {
      let that = this;
      if (this.templateId) {
        that.spinning = true;
        findById(this.templateId).then(function (result) {
          if (result.code === that.eipResultCode.success) {
            if (result.data.design) {
              that.template.design = JSON.parse(result.data.design);
            }
            that.findSend();
          } else {
            that.$message.error(result.msg);
          }
          that.spinning = false;
        });
      }
    },

    /**
     *
     */
    findSend() {
      let that = this;
      if (this.templateSendId) {
        that.spinning = true;
        findByTemplateSendId(this.templateSendId).then(function (result) {
          if (result.code === that.eipResultCode.success) {
            that.form = result.data;
            that.$refs.kfb.setData(JSON.parse(result.data.data));
          } else {
            that.$message.error(result.msg);
          }
          that.spinning = false;
        });
      }
    },
  },
};
</script>