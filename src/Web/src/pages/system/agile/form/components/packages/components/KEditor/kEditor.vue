<!--
 * @Description: 对vue-quill-editor封装
 * @Author: kcz
 * @Date: 2020-03-30 12:44:03
 * @LastEditors: kcz
 * @LastEditTime: 2020-04-26 19:21:27
 -->
<template>
  <eip-editor ref="vueQuillEditor" :disabled="record.options.disabled || parentDisabled" :value="value"
    :height="record.options.height" @input="onEditorChange"></eip-editor>
</template>
<script>

export default {
  name: "editor",
  props: ["value", "record", "parentDisabled"],
  data() {
    return {
      editorOption: {
        placeholder: this.record.options.placeholder
      }
    };
  },
  methods: {
    onEditorChange(html) {
      this.$emit("change", html);
    }
  }
};
</script>