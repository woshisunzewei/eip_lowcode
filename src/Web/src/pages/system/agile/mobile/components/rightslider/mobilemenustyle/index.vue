<template>
  <div class="modulestyle">
    <!-- 标题 -->
    <h2>{{ datas.text }}</h2>
    <a-form :label-col="{ span: 7 }" :wrapper-col="{ span: 17 }">
      <a-form-item label="边框">
        {{ datas.border ? "显示" : "隐藏" }}
        <a-checkbox v-model="datas.border" />
      </a-form-item>

      <a-form-item label="每行显示数">
        <a-input-number v-model="datas.columnnum" />
      </a-form-item>
    </a-form>
  </div>
</template>

<script>
export default {
  name: "mobilemenustyle",
  props: {
    datas: Object,
    "data-type": String,
  },
};
</script>

<style scoped lang="less">
.modulestyle {
  width: 100%;
  position: absolute;
  left: 0;
  top: 0;
  padding: 0 10px;
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
