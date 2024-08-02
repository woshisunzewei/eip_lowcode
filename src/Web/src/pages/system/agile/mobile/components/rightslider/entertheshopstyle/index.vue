<template>
  <div class="entertheshopstyle">
    <!-- 标题 -->
    <h2>{{ datas.text }}</h2>

    <!-- 表单 -->
    <a-form :label-col="{ span: 5 }" :wrapper-col="{ span: 19 }" :model="datas" :rules="rules">
      <a-form-item label="左侧标题" :hide-required-asterisk="true" prop="shopName">
        <a-input v-model="datas.shopName" placeholder="请输入左侧标题" maxlength="10" show-word-limit />
      </a-form-item>

      <!-- 文案 -->
      <a-form-item label="右侧内容" :hide-required-asterisk="true" prop="copywriting">
        <a-input v-model="datas.copywriting" placeholder="请输入右侧内容" maxlength="8" show-word-limit />
      </a-form-item>

      <a-form-item label="左侧图标">
        <img :src="datas.icon" v-if="datas.icon" />
        <!-- 添加导航按钮 -->
        <a-button @click="upLoadimg.visible = true" class="uploadImg" type="primary" plain>
          <a-icon type="plus" />点击{{ datas.icon ? "更换" : "添加" }}图片
        </a-button>
      </a-form-item>

      <!-- 跳转页面 -->
      <a-form-item label="跳转页面">
        <div class="imgText">
          <a-select style="width: 60%" v-model="datas.type" placeholder="请选择跳转类型" size="small">
            <a-select-option v-for="(item, index) in optionsType" :key="index" :value="item.type">
              {{ item.name }}
            </a-select-option>
          </a-select>

          <!-- 输入链接 -->
          <a-input style="width: 100%" size="small" placeholder="请输入链接，输入前确保可以访问" v-model="datas.http.externalLink">
          </a-input>
        </div>
      </a-form-item>
    </a-form>

    <!-- 上传图片 -->
    <eip-material v-if="upLoadimg.visible" :visible.sync="upLoadimg.visible" @ok="materialOk" />
  </div>
</template>

<script>

export default {
  name: "entertheshopstyle",
  props: {
    datas: Object,
  },
  data() {
    return {
      upLoadimg: {
        visible: false
      },
      rules: {
        //校验表单输入
        shopName: [
          //页面名称
          { required: true, message: "请输入左侧标题", trigger: "blur" },
        ],
        copywriting: [
          //页面描述
        ],
      },
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
      emptyText: "",
    };
  },
  methods: {
    // 提交
    materialOk(res) {
      this.datas.icon = res;
    },
  },

};
</script>

<style scoped lang="less">
.entertheshopstyle {
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

  /* 图片字 */
  .imgText {
    width: 100%;
    display: flex;
    box-sizing: border-box;
    justify-content: space-between;

    .fir-sele.a-select {
      width: 40%;
    }
  }

  /* 上传图片按钮 */
  .uploadImg {
    width: 100%;
    height: 40px;
    margin-top: 20px;
  }

  img {
    display: block;
    margin: 0 auto;
    width: 56px;
    height: 56px;
  }
}
</style>
