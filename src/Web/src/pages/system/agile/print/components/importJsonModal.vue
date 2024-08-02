<template>
  <div>
    <a-tooltip title="导入">
      <a-button type="primary" @click="show">
        <a-icon type="to-top" />
        导入
      </a-button>
    </a-tooltip>
    <a-modal :zIndex="1100" v-drag title="导入打印Json数据" :visible="visible" @ok="handleImportJson" @cancel="handleCancel"
      cancelText="关闭" :destroyOnClose="true" wrapClassName="code-modal-9136076486841527" style="top:20px;"
      width="850px">
      <div class="json-box-9136076486841527">
        <eip-code :value.sync="jsonFormat" language="json"></eip-code>
      </div>
    </a-modal>
  </div>
</template>
<script>

export default {
  name: "importJsonModal",
  data() {
    return {
      visible: false,
      jsonFormat: ""
    };
  },
  props: {
    template: {
      type: Object,
    }
  },

  methods: {
    handleCancel() {
      this.visible = false;
    },

    show() {
      this.visible = true
    },

    handleImportJson() {
      try {
        const editorJsonData = JSON.parse(this.jsonFormat);
        this.template.update(editorJsonData)
        this.handleCancel();
        this.$message.success("导入成功");
      } catch (error) {
        console.error(error);
        this.$message.error("导入失败，数据格式不对");
      }
    }
  }
};
</script>
