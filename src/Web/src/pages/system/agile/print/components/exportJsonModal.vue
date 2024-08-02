<template>
  <div>
    <a-tooltip title="导出">
      <a-button type="primary" @click="show">
        <a-icon type="to-top" />
        导出
      </a-button>
    </a-tooltip>
    <a-modal :zIndex="1100" v-drag title="导出预览" :footer="null" :visible="visible" @cancel="handleCancel"
      :destroyOnClose="true" wrapClassName="code-modal-9136076486841527" style="top:20px;" width="850px">
      <div class="json-box-9136076486841527">
        <eip-code v-if="editorJson" :value.sync="editorJson" language="json" :readOnly="true"></eip-code>
      </div>

      <div class="copy-btn-box-9136076486841527">
        <a-button @click="handleCopyJson" type="primary" class="copy-btn" data-clipboard-action="copy"
          :data-clipboard-text="editorJson">
          复制数据
        </a-button>
        <a-button @click="handleExportJson" type="primary">
          导出代码
        </a-button>
      </div>
    </a-modal>
  </div>
</template>
<script>

import Clipboard from "clipboard";
export default {
  name: "exportJsonModal",
  data() {
    return {
      visible: false,
      editorJson: "",
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
      this.spinning = true
      this.editorJson = JSON.stringify(this.template.getJson(), null, "\t");
      this.spinning = false
    },

    exportData(data, fileName = `打印Json.txt`) {
      let content = "data:text/csv;charset=utf-8,";
      content += data;
      var encodedUri = encodeURI(content);
      var actions = document.createElement("a");
      actions.setAttribute("href", encodedUri);
      actions.setAttribute("download", fileName);
      actions.click();
    },
    handleExportJson() {
      // 导出JSON
      this.exportData(this.editorJson);
    },
    handleCopyJson() {
      // 复制数据
      const clipboard = new Clipboard(".copy-btn");
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
    }
  }
};
</script>
