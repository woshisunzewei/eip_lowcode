<template>
  <div class="richtextstyle">
    <!-- 标题 -->
    <h2>{{ datas.text }}</h2>

    <a-form :label-col="{ span: 5 }" :wrapper-col="{ span: 19 }" :model="datas">
      <!-- 背景颜色 -->
      <a-form-item label="背景颜色">
        <!-- 背景颜色 -->
        <eip-color :value="datas.backColor" @change="(value) => { datas.backColor = value }"></eip-color>
      </a-form-item>
    </a-form>
    <div class="edit">
      <eip-editor ref="sqleditor" v-if="tinymce.show" v-model="datas.myValue" :height="tinymce.height"
        :toolbar="tinymce.toolbar" :plugins="tinymce.plugins" :menubar="tinymce.menubar"></eip-editor>
    </div>
  </div>
</template>

<script>

export default {
  name: 'richtextstyle',
  props: {
    datas: Object,
  },
  data() {
    return {
      tinymce: {
        plugins: "preview  fullscreen code",
        toolbar: "undo redo |fullscreen|code",
        height: 350,
        show: false,
        menubar: "",
      },
      predefineColors: [
        // 颜色选择器预设
        '#ff4500',
        '#ff8c00',
        '#ffd700',
        '#90ee90',
        '#00ced1',
        '#1e90ff',
        '#c71585',
        '#409EFF',
        '#909399',
        '#C0C4CC',
        'rgba(255, 69, 0, 0.68)',
        'rgb(255, 120, 0)',
        'hsv(51, 100, 98)',
        'hsva(120, 40, 94, 0.5)',
        'hsl(181, 100%, 37%)',
        'hsla(209, 100%, 56%, 0.73)',
        '#c7158577',
      ],
    }
  },
}
</script>

<style scoped lang="less">
.richtextstyle {
  width: 100%;
  position: absolute;
  left: 0;
  top: 0;
  padding: 0 10px 20px;
  box-sizing: border-box;

  /* 标题 */
  h2 {
    padding: 24px 16px 24px 0;
    margin-bottom: 15px;
    border-bottom: 1px solid #f2f4f6;
    font-size: 18px;
    font-weight: 600;
    color: #323233;
  }
}
</style>