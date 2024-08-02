<template>
  <div class="pictureadsstyle">
    <!-- 标题 -->
    <h2>图片广告</h2>

    <!-- 表单 -->
    <a-form :label-col="{ span: 5 }" :wrapper-col="{ span: 19 }" :model="datas">
      <!-- 标题内容 -->
      <a-form-item label="选择模板" class="lef">
        <p style="color: #000">{{ styleText }}</p>
      </a-form-item>

      <!-- 轮播图选择 -->
      <div class="swiperType">
        <a-tooltip class="item" effect="dark" content="一行一个" placement="bottom">
          <span class="iconfont icon-yihangyige" style="font-size: 21px" :class="datas.swiperType === 0 ? 'active' : ''"
            @click="datas.swiperType = 0" />
        </a-tooltip>

        <a-tooltip class="item" effect="dark" content="轮播海报" placement="bottom">
          <span class="iconfont icon-icon_tupian_lunbohaibao" style="font-size: 20px"
            :class="datas.swiperType === 1 ? 'active' : ''" @click="datas.swiperType = 1" />
        </a-tooltip>

        <a-tooltip class="item" effect="dark" content="多图单行" placement="bottom">
          <span class="iconfont icon-daohanghengxianghuadong" style="font-size: 24px"
            :class="datas.swiperType === 2 ? 'active' : ''" @click="datas.swiperType = 2" />
        </a-tooltip>

        <a-tooltip class="item" effect="dark" content="立体轮播" placement="bottom">
          <span class="iconfont icon-xiaotuhengxianghuadong" style="font-size: 24px"
            :class="datas.swiperType === 3 ? 'active' : ''" @click="datas.swiperType = 3" />
        </a-tooltip>
      </div>

      <!-- 下划线 -->
      <div class="bor" />

      <h5 style="color: #000; font-size: 14px">添加图片</h5>
      <p style="color: #969799; font-size: 12px; margin-top: 10px">
        拖动选中的导航可对其排序
      </p>

      <!-- 图片广告 -->
      <div v-if="datas.imageList[0]">
        <vuedraggable v-model="datas.imageList" v-bind="dragOptions">
          <transition-group>
            <section class="imgBanner" v-for="(item, index) in datas.imageList" :key="item + index">
              <a-icon class="a-icon-circle-close" type="close-circle" @click="deleteimg(index)" />
              <!-- 图片 -->
              <div class="imag">
                <img :src="item.src" alt draggable="false" />
              </div>
              <!-- 标题和链接 -->
              <div class="imgText">
                <a-input v-model="item.text" placeholder="请输入标题，也可不填" size="small"></a-input>

                <!-- 选择类型 -->
                <div class="select-type">
                  <a-select style="width: 60%" v-model="item.linktype" placeholder="请选择跳转类型" size="small">
                    <a-select-option v-for="(item, index) in optionsType" :key="index" :value="item.type">
                      {{ item.name }}</a-select-option>
                  </a-select>

                  <!-- 输入链接 -->
                  <a-input style="width: 100%" size="small" placeholder="请输入链接，输入前确保可以访问"
                    v-model="item.http.externalLink"></a-input>
                </div>
              </div>
            </section>
          </transition-group>
        </vuedraggable>
      </div>
      <!-- 上传图片 -->
      <a-button @click="upLoadimg.visible = true" class="uploadImg" type="primary" plain>
        <a-icon type="plus" />点击选择图片
      </a-button>

      <!-- 下划线 -->
      <div class="bor"></div>

      <a-form-item class="lef" label="一行个数" v-show="datas.swiperType === 2">
        <!-- 单选框 -->
        <a-radio-group v-model="datas.rowindividual" class="radi">
          <a-radio :value="2">2个</a-radio>
          <a-radio :value="3">3个</a-radio>
          <a-radio :value="4">4个</a-radio>
          <a-radio :value="5">5个</a-radio>
          <a-radio :value="6">6个</a-radio>
        </a-radio-group>
      </a-form-item>

      <div style="height: 10px" />

      <!-- 图片倒角 -->
      <a-form-item label="分页类型" class="lef borrediu">
        <a-radio-group v-model="datas.pagingType" class="radi1">
          <a-radio :value="0">不显示</a-radio>
          <a-radio :value="'bullets'">样式一</a-radio>
          <a-radio :value="'fraction'">样式二</a-radio>
          <a-radio :value="'progressbar'">样式三</a-radio>
        </a-radio-group>
      </a-form-item>

      <div style="height: 10px" />

      <!-- 图片倒角 -->
      <a-form-item label="图片倒角" class="lef borrediu">
        <a-slider v-model="datas.borderRadius" :max="30"></a-slider>
      </a-form-item>

      <div style="height: 10px" />

      <!-- 页面边距 -->
      <a-form-item class="lef" label="页面边距" v-show="datas.swiperType === 0">
        <a-slider v-model="datas.pageMargin" :max="20"></a-slider>
      </a-form-item>

      <div style="height: 10px" />

      <!-- 图片边距 -->
      <a-form-item class="lef" label="图片边距" v-show="datas.swiperType === 0 || datas.swiperType === 2">
        <a-slider v-model="datas.imageMargin" :max="20"></a-slider>
      </a-form-item>
    </a-form>

    <!-- 上传图片 -->
    <eip-material v-if="upLoadimg.visible" :visible.sync="upLoadimg.visible" ref="upload" @ok="materialOk" />
  </div>
</template>

<script>
import vuedraggable from "vuedraggable"; //拖拽组件

export default {
  name: "pictureadsstyle",
  components: { vuedraggable },
  props: {
    datas: Object,
  },
  data() {
    return {
      upLoadimg: {
        visible: false
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
      dragOptions: {
        animation: 200,
      },
      emptyText: "",
    };
  },

  created() { },

  methods: {
    // 提交
    materialOk(res) {
      this.datas.imageList.push({
        src: res,
        text: "",
        http: {},
      });
    },

    /* 删除图片 */
    deleteimg(index) {
      this.datas.imageList.splice(index, 1);
    },
  },

  computed: {
    styleText() {
      let data;
      if (this.datas.swiperType === 0) data = "一行一个";
      if (this.datas.swiperType === 1) data = "轮播海报";
      if (this.datas.swiperType === 2) data = "多图单行";
      if (this.datas.swiperType === 3) data = "立体模式";
      if (this.datas.swiperType === 4) data = "导航横向滑动";

      return data;
    },
  },
};
</script>

<style scoped lang="less">
.pictureadsstyle {
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

  .lef {
    /deep/.ant-form-item-label {
      text-align: left;
    }
  }

  /* 轮播图样式 */
  .swiperType {
    display: flex;
    justify-content: space-around;
    align-items: center;

    span {
      display: inline-block;
      width: 58px;
      height: 32px;
      text-align: center;
      line-height: 32px;
      background: #ebedf0;
      color: #979797;
      border: 1px solid #fff;
      cursor: pointer;
      transition: all 0.5s;

      &:hover {
        border: 1px solid @primary-color;
        color: @primary-color;
      }

      &.active {
        border: 1px solid @primary-color;
        background-color: #e0edff;
        color: @primary-color;
      }
    }
  }

  /* 圆角 */
  .borrediu {
    span {
      display: inline-block;
      width: 48px;
      height: 26px;
      text-align: center;
      line-height: 26px;
      background: #ebedf0;
      color: #979797;
      border: 1px solid #fff;
      cursor: pointer;
      transition: all 0.5s;

      &:hover {
        border: 1px solid @primary-color;
        color: @primary-color;
      }

      &.active {
        border: 1px solid @primary-color;
        background-color: #e0edff;
        color: @primary-color;
      }
    }
  }

  /deep/.radi .a-radio {
    margin-right: 8px;
  }

  /deep/.radi1 .a-radio {
    margin-right: 7px;

    .a-radio__label {
      padding-left: 5px;
    }
  }

  /* 上传图片按钮 */
  .uploadImg {
    width: 100%;
    height: 40px;
    margin-top: 20px;
  }

  // 上传弹框内容部分
  /deep/.uploadIMG .a-modal__body {
    height: 280px;
    display: flex;
    flex-direction: column;
    align-items: center;
    position: relative;
    justify-content: center;
  }

  .disable {
    /deep/.a-upload {
      display: none !important;
    }
  }

  /* 图片广告列表 */
  .imgBanner {
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
}
</style>
