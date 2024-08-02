<template>
  <div class="suspensionstyle">
    <!-- 标题 -->
    <h2>{{ datas.text }}</h2>

    <!-- 表单 -->
    <a-form :label-col="{ span: 5 }" :wrapper-col="{ span: 19 }" :model="datas">
      <!-- 跳转页面 -->
      <a-form-item label="跳转页面">
        <div class="imgText">
          <!-- 选择类型 -->
          <a-select style="width: 60%" v-model="datas.type" placeholder="请选择跳转类型" size="small">
            <a-select-option :key="index">
              {{ item.name }}
            </a-select-option>
          </a-select>

          <!-- 输入链接 -->
          <a-input style="width: 100%" size="small" placeholder="请输入链接，输入前确保可以访问" v-model="datas.http.externalLink">
          </a-input>
        </div>
      </a-form-item>
    </a-form>
  </div>
</template>
<script>
export default {
  name: "suspensionstyle",
  props: {
    datas: Object,
  },
  data() {
    return {
      optionsType: [
        {
          type: "10",
          name: "内部链接",
        },
        {
          type: "11",
          name: "外部链接",
        },
      ], // 选择跳转类型
      options: [], //后端返回的列表提供下拉选择
      emptyText: "",
    };
  },
  created() { },
};
</script>
<style lang="less" scoped>
.suspensionstyle {
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

  .imgText {
    width: 100%;
    display: flex;
    box-sizing: border-box;
    justify-content: space-between;

    .fir-sele.a-select {
      width: 40%;
    }
  }
}
</style>