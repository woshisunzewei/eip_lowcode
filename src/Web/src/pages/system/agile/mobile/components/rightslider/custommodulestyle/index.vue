<template>
  <div class="custommodulestyle">
    <!-- 标题 -->
    <h2>{{ datas.text }}</h2>
    <a-input v-model="datas.demo" placeholder="请输入公告" />
    <a-input v-model="datas.img" placeholder="图片地址" />
    {{ "aaa" + datas.type }}
  </div>
</template>

<script>
export default {
  name: "custommodulestyle",
  props: {
    datas: Object,
    "data-type": String,
  },
};
</script>

<style scoped lang="less">
.custommodulestyle {
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
