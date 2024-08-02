<template>
  <div class="investigatestyle">
    <!-- 标题 -->
    <h2>{{ datas.text }}</h2>

    <!-- 表单 -->
    <a-form :label-col="{ span: 8 }" :wrapper-col="{ span: 12 }">
      <a-form-item class="lef" label="外边框">
        <a-checkbox v-model="datas.isShowBorder">显示</a-checkbox>
      </a-form-item>

      <a-form-item class="lef" label="选中的颜色">
        <!-- 颜色选择器 -->
        <eip-color :value="datas.activeColor" @change="(value) => { datas.activeColor = value }"></eip-color>
      </a-form-item>

      <a-form-item class="lef" label="未选中的颜色">
        <!-- 颜色选择器 -->
        <eip-color :value="datas.inactiveColor" @change="(value) => { datas.inactiveColor = value }"></eip-color>
      </a-form-item>

      <a-form-item class="lef" label="高亮位置">
        <a-slider v-model="datas.Highlight" :max="4" :min="0"> </a-slider>
      </a-form-item>

      <a-form-item class="lef" label="导航"> </a-form-item>
      <vuedraggable v-model="datas.iconList" v-bind="dragOptions">
        <transition-group>
          <section class="imgBanner" v-for="(item, index) in datas.iconList" :key="item + index">
            <a-icon class="a-icon-circle-close" type="close-circle" @click="deleteimg(index)" />
            <!-- 图片 -->
            <div>
              <div class="imagBox" v-for="replaceIconIndex in 2" :key="replaceIconIndex"
                @click="replaceIcon(replaceIconIndex, index)">
                <img class="imag" :src="replaceIconIndex == 1 ? item.iconPic : item.inactive" draggable="false" />
                <div>
                  {{ replaceIconIndex == 1 ? "选中时" : "未选中时" }}
                </div>
              </div>
            </div>
            <!-- 标题和链接 -->
            <div class="imgText">
              <div class="imgText-top">
                <a-input v-model="item.iconText" placeholder="导航名称" size="small" />
                <div class="imgText-top-r">
                  <span>小圆点</span>
                  <a-checkbox v-model="item.isDot"></a-checkbox>
                </div>
              </div>
              <!-- 标题和链接 -->
              <div class="imgTextChild">
                <!-- 选择类型 -->
                <a-select v-model="item.linktype" placeholder="请选择跳转类型" size="small">
                  <a-select-option v-for="iteml in optionsType" :key="iteml.name" :value="iteml.type">
                    {{ iteml.name }}
                  </a-select-option>
                </a-select>

                <!-- 输入链接 -->
                <a-input size="small" placeholder="请输入链接，输入前确保可以访问" v-model="item.http.externalLink">
                </a-input>
              </div>
            </div>
          </section>
        </transition-group>
      </vuedraggable>

      <!-- 添加导航按钮 -->
      <a-button @click="upLoadimg.visible = true" class="uploadImg" type="primary" plain
        v-if="datas.iconList.length < 5">
        <a-icon type="plus" />点击添加导航
      </a-button>
      <i class="icon-tip">*最多添加5个</i>
    </a-form>

    <!-- 上传图片 -->
    <eip-material v-if="upLoadimg.visible" :visible.sync="upLoadimg.visible" @ok="materialOk"
      @handleClose="handleClose" />
  </div>
</template>

<script>
import vuedraggable from "vuedraggable"; //拖拽组件

export default {
  name: "tabBarStyle",
  props: {
    datas: Object,
  },
  data() {
    return {
      upLoadimg: {
        visible: false
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
      dragOptions: {
        animation: 200,
      },
      replaceIconIndex: null,
      replaceIndex: null,
    };
  },

  methods: {
    // 提交
    materialOk(res) {
      if (this.replaceIconIndex == 1) {
        this.datas.iconList[this.replaceIndex].iconPic = res;
        this.replaceIconIndex = null;
        return;
      }
      if (this.replaceIconIndex == 2) {
        this.datas.iconList[this.replaceIndex].inactive = res;
        this.replaceIconIndex = null;
        return;
      }
      this.datas.iconList.push({
        /** 图标名称文字 */
        iconText: "",
        /** 图标图片 */
        iconPic: res,
        inactive: res,
        /** 是否显示小圆点 */
        isDot: false,
        /** 跳转类型 */
        linktype: "10",
        /** 跳转参数 */
        http: {},
      });
    },
    /* 取消上传 */
    handleClose() {
      this.replaceIconIndex = null;
    },
    /* 删除图片 */
    deleteimg(index) {
      this.datas.iconList.splice(index, 1);
    },
    /* 点击图片 */
    replaceIcon(replaceIconIndex, replaceIndex) {
      this.replaceIconIndex = replaceIconIndex;
      this.replaceIndex = replaceIndex;
      this.upLoadimg.visible = true;
    },
  },

  components: {
    vuedraggable,
  },
};
</script>

<style lang="less" scoped>
.investigatestyle {
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
    margin-top: 20px;
  }

  /* 卡片列表 */
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
    .imagBox {
      position: relative;
      border-radius: 5px;
      overflow: hidden;
      cursor: pointer;

      .imag {
        width: 60px;
        height: 60px;
      }

      div {
        position: absolute;
        top: 0;
        width: 60px;
        line-height: 60px;
        border-radius: 5px;
        text-align: center;
        font-size: 12px;
        color: #fff;
        background-color: rgba(0, 0, 0, 0.3);
      }
    }

    /* 图片字 */
    .imgText {
      padding-left: 20px;
      display: flex;
      flex-direction: column;
      box-sizing: border-box;
      justify-content: space-around;

      /* 图片字 */
      .imgTextChild {
        width: 100%;
        display: flex;
        box-sizing: border-box;
        justify-content: space-between;

        .fir-sele.a-select {
          width: 40%;
        }
      }

      .imgText-top {
        display: flex;
        flex-direction: row;
        justify-content: space-between;
        align-items: center;

        .imgText-top-r {
          flex: 1;
          text-align: center;

          span {
            margin-right: 10px;
          }
        }

        /deep/.a-input,
        .a-input--mini {
          flex: 1;
        }
      }
    }
  }

  .icon-tip {
    font-size: 12px;
    color: red;
  }
}
</style>
