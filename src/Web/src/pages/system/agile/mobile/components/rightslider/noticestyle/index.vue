<template>
  <div class="noticestyle">
    <!-- 标题 -->
    <h2>{{ datas.text }}</h2>

    <!-- 公告 -->
    <a-form :label-col="{ span: 5 }" :wrapper-col="{ span: 19 }" :model="datas" :rules="rules" class="lef">
      <a-form-item label="公告" :hide-required-asterisk="true" prop="noticeText">
        <a-input v-model="datas.noticeText" placeholder="请输入公告" />
      </a-form-item>

      <div style="height: 10px" />

      <!-- 背景颜色 -->
      <a-form-item label="背景颜色" class="lef">
        <!-- 颜色选择器 -->
        <eip-color :value="datas.backColor" @change="(value) => { datas.backColor = value }"></eip-color>
      </a-form-item>

      <div style="height: 10px" />

      <!-- 文字颜色 -->
      <a-form-item label="文字颜色" class="lef">
        <!-- 颜色选择器 -->
        <eip-color :value="datas.textColor" @change="(value) => { datas.textColor = value }"></eip-color>
      </a-form-item>
    </a-form>
  </div>
</template>

<script>
export default {
  name: "noticestyle",
  props: {
    datas: Object,
  },
  data() {
    return {
      rules: {
        //校验表单输入
        noticeText: [
          //页面名称
          { required: true, message: "请输入公告", trigger: "blur" },
        ],
      },
      predefineColors: [
        // 颜色选择器预设
        "#ff4500",
        "#ff8c00",
        "#ffd700",
        "#90ee90",
        "#00ced1",
        "#1e90ff",
        "#c71585",
        "#409EFF",
        "#909399",
        "#C0C4CC",
        "rgba(255, 69, 0, 0.68)",
        "rgb(255, 120, 0)",
        "hsv(51, 100, 98)",
        "hsva(120, 40, 94, 0.5)",
        "hsl(181, 100%, 37%)",
        "hsla(209, 100%, 56%, 0.73)",
        "#c7158577",
      ],
    };
  },
};
</script>

<style scoped lang="less">
.noticestyle {
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

  .lef {
    /deep/.ant-form-item-label {
      text-align: left;
    }
  }

  /* 颜色选择器 */
  .picke {
    float: right;
  }
}
</style>
