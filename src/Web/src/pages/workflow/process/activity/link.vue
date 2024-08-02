<template>
  <a-modal width="600px" v-drag :destroyOnClose="true" :maskClosable="false" :title="title" :visible="visible"
    :bodyStyle="{ padding: '1px' }" @cancel="cancel">
    <a-spin :spinning="spinning">
      <a-form-model layout="horizontal">
        <a-form-model-item label="连线说明" :label-col="config.labelCol" :wrapper-col="config.wrapperCol">
          <a-input v-model="form.remark" placeholder="请输入连线说明" allow-clear />
        </a-form-model-item>
      </a-form-model>
    </a-spin>
    <template slot="footer">
      <a-space>
        <a-button key="back" @click="cancel" :disabled="loading"><a-icon type="close" />取消</a-button>
        <a-button key="submit" @click="save" type="primary" :loading="loading"><a-icon type="save" />提交</a-button>
      </a-space>
    </template>
  </a-modal>
</template>

<script>

export default {
  name: "activityLink",
  data() {
    return {
      config: {
        labelCol: { span: 4 },
        wrapperCol: { span: 20 },
      },
      form: { remark: null },
      loading: false,
      spinning: false,
    };
  },

  props: {
    visible: {
      type: Boolean,
      default: false,
    },
    remark: {
      type: String,
    },
    title: {
      type: String,
    },
  },

  mounted() {
    this.initReamrk();
  },

  methods: {
    /**
     *
     */
    initReamrk() {
      if (this.remark) {
        this.form.remark = this.$utils.clone(this.remark, true);
      }
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
    save() {
      this.$emit("ok", this.form.remark);
      this.cancel();
    },
  },
};
</script>