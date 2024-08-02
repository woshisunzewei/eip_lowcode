<template>
  <div class="graphicnavigationstyle">
    <!-- 标题 -->
    <h2>{{ datas.text }}</h2>

    <!-- 提示 -->
    <p style="color: #969799; font-size: 12px; margin-top: 10px">
      拖动选中的导航可对其排序
    </p>

    <!-- 图片广告 -->
    <div v-if="datas.imageList[0]">
      <vuedraggable v-model="datas.imageList" v-bind="dragOptions">
        <transition-group>
          <section class="imgList" v-for="(item, index) in datas.imageList" :key="item + index">
            <a-icon class="a-icon-circle-close" type="close-circle" @click="deleteimg(index)" />
            <!-- 图片 -->
            <div class="imag">
              <img draggable="false" :src="item.src" alt="" />
            </div>
            <!-- 标题和链接 -->
            <div class="imgText">
              <a-input v-model="item.text" placeholder="请输入标题，也可不填" size="small"></a-input>
              <!-- 选择类型 -->
              <div class="select-type">
                <a-select style="width: 60%" v-model="item.linktype" placeholder="请选择跳转类型" size="small">
                  <a-select-option v-for="(item, index) in optionsType" :key="index" :value="item.type">
                    {{ item.name }}
                  </a-select-option>
                </a-select>

                <!-- 输入链接 -->
                <a-input style="width: 100%" size="small" placeholder="请输入链接，输入前确保可以访问"
                  v-model="item.http.externalLink">
                </a-input>
              </div>
            </div>
          </section>
        </transition-group>
      </vuedraggable>
    </div>

    <!-- 上传图片 -->
    <a-button @click="showUpload('0')" class="uploadImg" type="primary" plain><a-icon type="plus" />点击添加导航</a-button>

    <div class="bor"></div>

    <!-- 表单 -->
    <a-form :label-col="{ span: 5 }" :wrapper-col="{ span: 19 }" :model="datas">
      <!-- 商品类型选择 -->
      <a-form-item class="lef" label="商品类型">
        <a-radio-group v-model="datas.navigationType">
          <a-radio style="margin-left: 35px" :value="index - 1" v-for="index in 2" :key="index">{{ index === 1 ? "图片导航"
      :
      "文字导航" }}</a-radio>
        </a-radio-group>
      </a-form-item>

      <div style="height: 10px" />

      <!-- 图片样式 -->
      <a-form-item class="lef" label="图片样式">
        <div class="weiz">
          <a-tooltip effect="dark" :title="index - 1 === 0 ? '固定' : '滑动'" placement="bottom" v-for="index in 2"
            :key="index">
            <i class="iconfont" :class="[
      index - 1 === 0 ? 'icon-guding' : 'icon-hengxianghuadong',
      datas.imgStyle === index - 1 ? 'active' : '',
    ]" @click="datas.imgStyle = index - 1" />
          </a-tooltip>
        </div>
      </a-form-item>

      <div style="height: 10px" />

      <!-- 一屏显示 -->
      <a-form-item class="lef" label="一屏显示" v-show="datas.imgStyle === 1">
        <a-select v-model="datas.showSize" placeholder="请选择活动区域">
          <a-select-option :value="index + 4" v-for="index in 7" :key="index">{{ index + 4 + '个导航' }}</a-select-option>
        </a-select>
      </a-form-item>

      <div style="height: 10px" />

      <!-- 文字高度 -->
      <a-form-item label="文字高度" class="lef">
        <a-slider v-model="datas.textHeight" :max="50" :min="24"> </a-slider>
      </a-form-item>

      <div style="height: 10px" />

      <!-- 文字大小 -->
      <a-form-item label="文字大小" prop="textSize" :hide-required-asterisk="true" class="lef">
        <a-input type="number" v-model.number="datas.textSize" placeholder="请输入文字大小" :maxlength="2" />
      </a-form-item>

      <div style="height: 10px" />

      <!-- 图片倒角 -->
      <a-form-item label="图片倒角" class="lef borrediu">
        <a-slider v-model="datas.borderRadius" :max="50"> </a-slider>
      </a-form-item>

      <div style="height: 10px" />

      <a-form-item class="lef" label="背景图片">
        <div class="shop-head-pic">
          <img class="home-bg" :src="datas.bgImg" alt="" v-if="datas.bgImg" />
          <div class="shop-head-pic-btn">
            <a-space align="center">
              <a-button @click="showUpload('1')" class="uploadImg" type="primary" plain><i
                  class="a-icon-plus" />更换图片</a-button>
              <a-button type="primary" class="uploadImg" @click="clear()">清空图片</a-button></a-space>
          </div>
        </div>
      </a-form-item>
      <div style="height: 10px" />

      <!-- 背景颜色 -->
      <a-form-item class="lef" label="背景颜色">
        <!-- 颜色选择器 -->
        <eip-color :value="datas.backgroundColor" @change="(value) => { datas.backgroundColor = value }"></eip-color>
      </a-form-item>

      <div style="height: 10px" />

      <!-- 文字颜色 -->
      <a-form-item class="lef" label="文字颜色">
        <!-- 颜色选择器 -->
        <eip-color :value="datas.textColor" @change="(value) => { datas.textColor = value }"></eip-color>
      </a-form-item>
    </a-form>

    <!-- 上传图片 -->
    <eip-material v-if="upLoadimg.visible" :visible.sync="upLoadimg.visible" @ok="materialOk" />
  </div>
</template>

<script>
import vuedraggable from "vuedraggable"; //拖拽组件

export default {
  name: "graphicnavigationstyle",
  props: {
    datas: Object,
  },
  data() {
    return {
      upLoadimg: {
        visible: false
      },
      dragOptions: {
        //拖拽配置
        animation: 200,
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
      uploadImgDataType: null,
    };
  },
  created() { },
  methods: {
    showUpload(type) {
      this.uploadImgDataType = type;
      this.upLoadimg.visible = true;
    },
    // 提交
    materialOk(res) {
      if (this.uploadImgDataType === "0") {
        this.datas.imageList.push({
          src: res,
          text: "",
          http: {},
        });
      } else if (this.uploadImgDataType === "1") {
        this.datas.bgImg = res;
      }
    },

    // 清空背景图片
    clear() {
      this.datas.bgImg = "";
    },
    /* 删除图片列表的图片 */
    deleteimg(index) {
      this.datas.imageList.splice(index, 1);
    },
  },
  components: { vuedraggable },
};
</script>

<style scoped lang="less">
.graphicnavigationstyle {
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

  /* 上传图片按钮 */
  .uploadImg {
    width: 100%;
    height: 40px;
    margin-top: 10px;
  }

  /* 商品列表 */
  .imgList {
    padding: 6px 12px;
    margin: 16px 7px;
    border-radius: 2px;
    background-color: #fff;
    box-shadow: 0 0 4px 0 rgba(10, 42, 97, 0.2);
    display: flex;
    position: relative;

    /* 删除图标 */
    .a-icon-circle-close {
      position: absolute;
      right: -10px;
      top: -10px;
      cursor: pointer;
      font-size: 19px;
    }

    /* 图片 */
    .imag {
      width: 60px;
      height: 60px;
      border-radius: 5px;
      overflow: hidden;
      position: relative;
      cursor: pointer;

      img {
        width: 100%;
        height: 100%;
        display: inline-block;
      }

      span {
        background: rgba(0, 0, 0, 0.5);
        font-size: 12px;
        position: absolute;
        left: 0px;
        bottom: 0px;
        display: inline-block;
        width: 100%;
        text-align: center;
        color: #fff;
        height: 20px;
        line-height: 20px;
      }
    }

    /* 图片字 */
    .imgText {
      width: 80%;
      display: flex;
      flex-direction: column;
      box-sizing: border-box;
      padding-left: 20px;
      justify-content: space-between;

      .select-type {
        display: flex;

        /deep/.a-select {
          .a-input {
            input {
              white-space: nowrap;
              overflow: hidden;
              text-overflow: ellipsis;
            }
          }
        }
      }
    }
  }

  /* 图片样式 */
  .weiz {
    text-align: right;

    i {
      padding: 5px 14px;
      margin-left: 10px;
      border-radius: 0;
      border: 1px solid #ebedf0;
      font-size: 20px;
      font-weight: 500;
      cursor: pointer;

      &:last-child {
        font-size: 22px;
      }

      &.active {
        color: @primary-color;
        border: 1px solid @primary-color;
        background: #e0edff;
      }
    }
  }

  .shop-head-pic {
    color: #ababab;
    display: flex;
    flex-direction: column;

    .home-bg {
      width: 100px;
      height: 100px;
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

  /* 颜色选择器 */
  .picke {
    float: right;
  }
}
</style>
