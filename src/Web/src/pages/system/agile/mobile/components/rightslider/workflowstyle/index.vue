<template>
  <div class="workflowstyle">
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
  name: "workflowstyle",
  props: {
    datas: Object,
    "data-type": String,
  },
};
</script>

<style scoped lang="less">
.workflowstyle {
  width: 100%;
  position: absolute;
  left: 0;
  top: 0;
  padding: 0 10px 20px;
  box-sizing: border-box;
}
</style>
