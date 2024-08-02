<template>
  <a-modal width="600px" v-drag :destroyOnClose="true" :maskClosable="false" :visible="visible"
    :bodyStyle="{ padding: '1px' }" :title="title" @cancel="cancel">
    <a-spin :spinning="spinning">
      <a-form-model ref="form" :model="form" :rules="rules" :label-col="config.labelCol"
        :wrapper-col="config.wrapperCol">
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
        <a-form-model-item label="图标">
          <eip-icon :name="form.icon" :theme="form.theme" @click="iconClick" @clear="iconClear"></eip-icon>
        </a-form-model-item>
        <a-form-model-item label="脚本" prop="script">
          <a-input allow-clear v-model="form.script" type="textarea" placeholder="请输入脚本" />
        </a-form-model-item>
        <a-form-model-item label="手机端显示" prop="mobileShow">
          <a-switch v-model="form.mobileShow" />
        </a-form-model-item>
        <a-form-model-item label="排序号" prop="orderNo">
          <a-input-number id="orderNo" style="width: 100%" placeholder="请输入排序号" v-model="form.orderNo" :min="0"
            :max="999" />
        </a-form-model-item>
        <a-form-model-item label="备注" prop="remark">
          <a-input allow-clear v-model="form.remark" type="textarea" placeholder="请输入备注" />
        </a-form-model-item>
      </a-form-model>
    </a-spin>
    <template slot="footer">
      <div class="flex justify-between align-center">
        <div>
          <a-checkbox v-if="!buttonId" v-model="save.retain">
            继续创建时，保留本次提交内容
          </a-checkbox>
        </div>

        <a-space>
          <a-button v-if="!buttonId" key="submit" @click="saveContinue" :loading="loading"><a-icon
              type="save" />提交并继续创建</a-button>
          <a-button v-if="buttonId" @click="cancel"><a-icon type="close" />关闭</a-button>
          <a-button key="submit" @click="saveClose" type="primary" :loading="loading"><a-icon
              type="save" />提交</a-button>
        </a-space>
      </div>
    </template>
  </a-modal>
</template>

<script>
import { save, findById } from "@/services/workflow/button/edit";
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
        buttonId: newGuid(),
        name: null,
        orderNo: 1,
        icon: "",
        theme: "",
        script: "",
        mobileShow: true,
        remark: null,
        style: "primary",
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
    buttonId: {
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

    /**
     * 保存
     */
    saveConfirm() {
      let that = this;
      this.$refs.form.validate((valid) => {
        if (valid) {
          that.loading = true;
          that.$loading.show({ text: that.eipMsg.saveLoading });
          that.form.buttonId = that.copy ? newGuid() : that.form.buttonId;
          save(that.form).then(function (result) {
            if (result.code === that.eipResultCode.success) {
              that.$message.success(result.msg);
              if (that.save.continue) {
                //不保留上次内容
                if (!that.save.retain) {
                  that.$refs.form.resetFields();
                }
                that.form.buttonId = newGuid();
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
      if (this.buttonId) {
        that.spinning = true;
        findById(this.buttonId).then(function (result) {
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