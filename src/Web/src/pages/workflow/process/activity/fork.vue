<template>
  <a-modal width="600px" v-drag :destroyOnClose="true" :maskClosable="false" :title="title" :visible="visible"
    :bodyStyle="{ padding: '1px' }" @cancel="cancel">
    <a-spin :spinning="spinning">
      <a-row>
        <a-col>
          <a-form-model-item label="条件名称" prop="title" :label-col="config.labelCol" :wrapper-col="config.wrapperCol">
            <a-input v-model="base.title" placeholder="请输入条件名称" allow-clear />
          </a-form-model-item>
        </a-col>
      </a-row>
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
  name: "activityFork",
  data() {
    return {
      config: {
        labelCol: { span: 4 },
        wrapperCol: { span: 20 },
      },
      base: { title: null },
      loading: false,
      spinning: false,
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
  },

  mounted() {
    this.init();
  },

  methods: {
    /**
     *
     */
    init() {
      if (this.data.text.text) {
        this.base.title = this.$utils.clone(this.data.text.text, true);
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
      this.data.props.base = this.base;
      this.data.text.text = this.base.title;
      this.$emit("ok", this.data);
      this.cancel();
    },
  },
};
</script>