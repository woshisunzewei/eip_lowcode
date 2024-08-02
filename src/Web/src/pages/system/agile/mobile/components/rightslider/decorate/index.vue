<template>
  <div class="decorate">
    <!-- 标题 -->
    <h2>页面设置</h2>

    <!-- 表单 -->
    <a-form :label-col="{ span: 5 }" :wrapper-col="{ span: 19 }" :model="datas" :rules="rules">
      <a-form-item label="页面名称" prop="name">
        <a-input v-model="datas.name" placeholder="页面标题" maxlength="25" show-word-limit />
      </a-form-item>

      <a-form-item label="页面描述" prop="details">
        <a-input v-model="datas.details" placeholder="用户通过微信分享给朋友时，会自动显示页面描述" />
      </a-form-item>

      <!-- 个人中心 -->
      <a-form-item label="个人中心">
        {{ datas.isPerson ? "显示" : "隐藏" }}
        <a-checkbox style="margin-left: 196px" v-model="datas.isPerson" />
      </a-form-item>

      <!-- 返回 -->
      <a-form-item label="返回按钮">
        {{ datas.isBack ? "显示" : "隐藏" }}
        <a-checkbox style="margin-left: 196px" v-model="datas.isBack" />
      </a-form-item>

      <!-- 高度 -->
      <a-form-item label="高度">
        <a-slider v-model="datas.titleHeight" :max="100" :min="35"> </a-slider>
      </a-form-item>

      <!-- 背景颜色 -->
      <a-form-item label="背景颜色">
        <!-- 单选框 -->
        <a-radio-group @change="colourActionChange" v-model="colourAction">
          <a-radio :value="'默认颜色'">默认颜色</a-radio>
          <a-radio :value="'自定义颜色'">自定义颜色</a-radio>
        </a-radio-group>
        <br />
        <!-- 颜色选择器 -->
        <eip-color v-if="pickeShow" :value="datas.bgColor" @change="(value) => { datas.bgColor = value }"></eip-color>
      </a-form-item>

      <a-form-item label="背景图片">
        <div class="shop-head-pic">
          <img class="home-bg" :src="datas.bgImg" alt="" v-if="datas.bgImg" />
          <div class="shop-head-pic-btn">
            <a-space align="center">
              <a-button type="dashed" @click="showUpload('2')" class="uploadImg"><a-icon type="plus" />更换图片</a-button>
              <a-button type="primary" @click="clear()"><a-icon type="close" />清空图片</a-button></a-space>
          </div>
        </div>
      </a-form-item>
    </a-form>

    <!-- 上传图片 -->
    <eip-material v-if="upLoadimg.visible" :visible.sync="upLoadimg.visible" @ok="materialOk" />
  </div>
</template>

<script>

export default {
  name: "decorate",
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
        name: [{ required: true, message: "请输入页面名称", trigger: "blur" }],
        details: [
          { required: true, message: "请输入页面描述", trigger: "blur" },
        ],
        classification: [
          { required: true, message: "请选择页面分类", trigger: "blur" },
        ],
      },
      colourAction: "默认颜色", // 颜色选择
      pickeShow: false, //颜色选择器是否显示
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
      uploadImgDataType: null, // 获取到的图片地址属于哪一类别   0 修改底部logo   1 修改店铺图标 2 页面背景图
    };
  },

  methods: {
    // 显示上传图片组件   type :  2 页面背景图
    showUpload(type) {
      this.uploadImgDataType = type;
      this.upLoadimg.visible = true;
    },

    // 上传图片
    materialOk(res) {
      if (this.uploadImgDataType === "2") {
        this.datas.bgImg = res;
      }
    },
    colourActionChange() {
      if (this.colourAction === "默认颜色") {
        this.datas.bgColor = "#F9F9F9";
        this.pickeShow = false;
        return;
      } else return (this.pickeShow = true);
    },
    // 清空背景图片
    clear() {
      this.datas.bgImg = "";
    },
  },

};
</script>

<style scoped lang="less">
/* 页面设置 */
.decorate {
  width: 100%;
  position: absolute;
  left: 0;
  top: 0;
  padding: 0 10px;
  box-sizing: border-box;

  h2 {
    padding: 24px 16px 24px 0;
    margin-bottom: 15px;
    border-bottom: 1px solid #f2f4f6;
    font-size: 18px;
    font-weight: 600;
    color: #323233;
  }

  /* 选择器添加和刷新 */
  .ification {
    color: @primary-color;
    font-size: 14px;
    padding: 0 15px;
    cursor: pointer;
  }

  /* 颜色选择器 */
  .picke {
    margin-left: 15px;
    vertical-align: top;
  }

  .home-bg {
    width: 100px;
    height: 300px;
  }

  // 底部logo
  .bottomLogo {
    display: flex;
    flex-direction: column;

    img {
      display: block;
      width: 220px;
      margin: 10px auto;
    }
  }

  // 店铺信息修改
  .shop-info {
    .shop-name {
      display: flex;
      flex-direction: row;
      color: #ababab;

      .a-input {
        flex: 1;
      }
    }

    .shop-head-pic {
      color: #ababab;
      display: flex;
      flex-direction: column;

      img {
        width: 70px;
        height: 70px;
        margin: 10px auto;
      }

      .shop-head-pic-btn {
        display: flex;
        flex-direction: row;

        .a-button {
          flex: 1;
        }
      }
    }
  }
}
</style>
