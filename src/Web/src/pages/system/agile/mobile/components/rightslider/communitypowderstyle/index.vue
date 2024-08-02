<template>
  <div class="communitypowderstyle">
    <!-- 标题 -->
    <h2>{{ datas.text }}</h2>

    <div style="height: 20px" />

    <!-- 提示 -->
    <!-- <a-tooltip class="item" effect="dark" content="" placement="bottom">
      <div slot="content">活码是用户推广个人微信号或粉丝群的二维码，适用于线上线下，吸引用户<br/>添加好友或粉丝群进行长期维护的场景</div>
      <i class="a-icon-question" style="cursor: pointer;"></i>
    </a-tooltip> -->

    <!-- 表单 -->
    <a-form :label-col="{ span: 5 }" :wrapper-col="{ span: 19 }" :model="datas" :rules="rules">
      <div style="height: 10px" />

      <!-- 描述 -->
      <a-form-item label="入口图片" :hide-required-asterisk="true">
        <div class="backgroundImg" @click="showImg('mainImg')">
          <img draggable="false" v-if="!datas.mainImg" src="../../../assets/images/powder.png" alt="" />
          <img draggable="false" v-else :src="datas.mainImg" alt="" />
          <p>更换图片</p>
        </div>
      </a-form-item>

      <!-- 二维码 -->
      <a-form-item label="二维码" :hide-required-asterisk="true">
        <div class="backgroundImg" @click="showImg('qrcodeImg')">
          <i class="a-icon-plus" v-if="!datas.qrcodeImg" size="30"></i>
          <img draggable="false" v-else :src="datas.qrcodeImg" alt="" />
          <p>更换图片</p>
        </div>
      </a-form-item>

      <!-- 标题 -->
      <a-form-item label="标题" prop="title" :hide-required-asterisk="true">
        <a-input v-model="datas.title" placeholder="个人微信号, 群名称或活动标题" show-word-limit />
      </a-form-item>

      <div style="height: 10px" />

      <!-- 描述 -->
      <a-form-item label="描述" prop="describe" :hide-required-asterisk="true">
        <a-input v-model="datas.describe" placeholder="请添加描述" show-word-limit />
      </a-form-item>

      <div style="height: 10px" />

      <!-- 按钮名称 -->
      <a-form-item label="按钮名称" prop="buttonName" :hide-required-asterisk="true">
        <a-input v-model="datas.buttonName" placeholder="请输入按钮名称" show-word-limit maxlength="8" />
      </a-form-item>

      <div style="height: 10px" />

      <!-- 背景颜色 -->
      <a-form-item label="背景颜色" class="color-select">
        <!-- 颜色选择器 -->
        <eip-color :value="datas.backColor" @change="(value) => { datas.backColor = value }"></eip-color>
      </a-form-item>
    </a-form>

    <!-- 上传图片 -->
    <eip-material v-if="upLoadimg.visible" :visible.sync="upLoadimg.visible" @ok="materialOk" />
  </div>
</template>

<script>

export default {
  name: "communitypowderstyle",
  props: {
    datas: Object,
  },
  data() {
    return {
      upLoadimg: {
        visible: false
      },
      rules: {
        title: [
          //页面名称
          { required: true, message: "请输入标题", trigger: "blur" },
        ],
        describe: [
          // 描述
          { required: true, message: "请输入描述", trigger: "blur" },
        ],
        buttonName: [
          // 按钮名称
          { required: true, message: "请输入按钮名称", trigger: "blur" },
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
      imgText: "", //哪一个图片
    };
  },
  methods: {
    // 提交
    materialOk(res) {
      this.datas[this.imgText] = res;
    },
    showImg(res) {
      this.imgText = res;
      this.upLoadimg.visible = true;
    },
  },
};
</script>

<style scoped lang="less">
.communitypowderstyle {
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

  /* 颜色选择器 */
  .picke {
    margin-left: 15px;
    vertical-align: top;
  }

  /* 背景图 */
  .backgroundImg {
    display: inline-flex;
    justify-content: center;
    align-items: center;
    cursor: pointer;
    width: 60px;
    height: 60px;
    position: relative;
    background: #f2f4f6;

    img {
      width: 100%;
      height: auto;
    }

    p {
      position: absolute;
      left: 0;
      bottom: 0;
      width: 100%;
      height: 20px;
      font-size: 12px;
      color: #fff;
      background: rgba(0, 0, 0, 0.5);
      text-align: center;
      line-height: 20px;
    }
  }

  .color-select {
    /deep/.a-form-item__content {
      float: right;
    }
  }
}
</style>
