<template>
  <a-drawer width="800px" :body-style="{ padding: '1px' }" :destroyOnClose="true" :visible="visible"
    :zIndex="style.zIndex" title="脚本配置" @close="cancel">
    <div class="eip-drawer-body beauty-scroll">
      <eip-code :value.sync="form.script"></eip-code>
    </div>
    <div class="eip-drawer-toolbar">
      <a-space>
        <a-button @click="cancel"> 取消 </a-button>
        <a-button @click="ok" type="primary"> 确定 </a-button>
      </a-space>
    </div>
  </a-drawer>
</template>

<script>

export default {
  name: "list-dialog",
  data() {
    return {
      col: {
        labelCol: { span: 4 },
        wrapperCol: { span: 20 },
      },
      style: {
        zIndex: 1030,
      },

      form: {
        script: undefined
      },
    };
  },

  props: {
    visible: {
      type: Boolean,
      default: false,
    },
    config: {
      type: String,
    }
  },
  mounted() {
    this.init();
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
    ok() {
      this.cancel();
      this.$emit("ok", this.form);
    },

    /**
     * 初始化
     */
    init() {
      if (this.config) {
        this.form.script = this.$utils.clone(this.config, true);
      } else {
        this.form = {
          script: ''
        };
      }
    },
  },
};
</script>