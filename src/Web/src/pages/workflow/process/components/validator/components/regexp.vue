<template>
  <a-modal width="400px" v-drag centered :destroyOnClose="true" :maskClosable="false" :visible="visible"
    :bodyStyle="{ paddingTop: 0 }" @close="cancel" title="正则表达式设置">
    <a-input allow-clear v-model="validator" :autoSize="true" :length="1024" type="textarea" placeholder="请输入正则表达式" />
    <template slot="footer">
      <a-space>
        <a-button key="back" @click="cancel" :disabled="loading"><a-icon type="close" />取消</a-button>
        <a-button key="submit" @click="save" type="primary"><a-icon type="save" />提交</a-button>
      </a-space>
    </template>
  </a-modal>
</template>

<script>
export default {
  name: "regexp",
  data() {
    return {
      config: {
        labelCol: { span: 4 },
        wrapperCol: { span: 20 },
      },
      validator: this.value,
    };
  },

  props: {
    visible: {
      type: Boolean,
      default: false,
    },
    value: {
      type: String,
    },
  },
  mounted() { },
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
      this.$emit("ok", this.validator);
      this.cancel();
    },
  },
};
</script>