<template>
  <a-modal width="600px" v-drag :destroyOnClose="true" :maskClosable="false" :visible="visible"
    :bodyStyle="{ padding: '1px' }" :title="title" @cancel="cancel">
    <a-spin :spinning="spinning">
      <a-form-model ref="form" :model="form" :rules="rules" :label-col="config.labelCol"
        :wrapper-col="config.wrapperCol">
        <a-form-model-item label="短信服务商" prop="provide">
          <a-select placeholder="请选择短信服务商" allow-clear v-model="form.provide">
            <a-select-option value="Aliyun"> 阿里云 </a-select-option>
            <a-select-option value="Tencent"> 腾讯云 </a-select-option>
            <a-select-option value="LingKai"> 凌凯 </a-select-option>
            <a-select-option value="Other"> 其他 </a-select-option>
          </a-select>
        </a-form-model-item>
        <a-form-model-item label="名称" prop="name">
          <a-input v-model="form.name" placeholder="请输入名称" allow-clear />
        </a-form-model-item>
        <a-form-model-item label="AppId" prop="appId">
          <a-input v-model="form.appId" placeholder="请输入AppId" allow-clear />
        </a-form-model-item>
        <a-form-model-item label="AppKey" prop="appKey">
          <a-input-password v-model="form.appKey" placeholder="请输入AppKey" allow-clear />
        </a-form-model-item>
        <a-form-model-item label="禁用" prop="isFreeze">
          <a-switch v-model="form.isFreeze" />
        </a-form-model-item>
        <a-form-model-item label="备注" prop="remark">
          <a-input allow-clear v-model="form.remark" type="textarea" placeholder="请输入备注" />
        </a-form-model-item>
      </a-form-model>
    </a-spin>
    <template slot="footer">
      <div class="flex justify-between align-center">
        <div>
          <a-checkbox v-if="!smsConfigId" v-model="save.retain">
            继续创建时，保留本次提交内容
          </a-checkbox>
        </div>

        <a-space>
          <a-button v-if="!smsConfigId" key="submit" @click="saveContinue" :loading="loading"><a-icon
              type="save" />提交并继续创建</a-button>
          <a-button v-if="smsConfigId" @click="cancel"><a-icon type="close" />关闭</a-button>
          <a-button key="submit" @click="saveClose" type="primary" :loading="loading"><a-icon
              type="save" />提交</a-button>
        </a-space>
      </div>
    </template>
  </a-modal>
</template>

<script>
import { save, findById } from "@/services/system/smsconfig/edit";

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
        smsConfigId: newGuid(),
        provide: undefined,
        name: null,
        appId: null,
        appKey: null,
        isFreeze: false,
        remark: null,
      },

      rules: {
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
    smsConfigId: {
      type: String,
    },
    title: {
      type: String,
    },
    copy: {
      type: Boolean,
      default: false,
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
    saveConfirm() {
      let that = this;
      this.$refs.form.validate((valid) => {
        if (valid) {
          that.loading = true;
          that.$loading.show({ text: that.eipMsg.saveLoading });
          that.form.smsConfigId = that.copy ? newGuid() : that.form.smsConfigId;
          save(that.form).then(function (result) {
            if (result.code === that.eipResultCode.success) {
              that.$message.success(result.msg);
              if (that.save.continue) {
                //不保留上次内容
                if (!that.save.retain) {
                  that.$refs.form.resetFields();
                }
                that.form.smsConfigId = newGuid();
              } else {
                that.cancel();
              }
              that.$emit("ok", result);
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
      if (this.smsConfigId) {
        that.spinning = true;
        findById(this.smsConfigId).then(function (result) {
          if (result.code === that.eipResultCode.success) {
            that.form = result.data;
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