<template>
  <div class="storenotecardstyle">
    <!-- 标题 -->
    <h2>{{ datas.text }}</h2>

    <!-- 表单 -->
    <a-form :label-col="{ span: 5 }" :wrapper-col="{ span: 19 }" :model="datas" :rules="rules">
      <a-form-item label="活动名称" class="lef">
        <a-input v-model="datas.name"></a-input>
      </a-form-item>

      <div class="bor" />

      <!-- 标题内容 -->
      <a-form-item class="lef" label="选择模板">
        <p style="color: #000">{{ styleText }}</p>
      </a-form-item>

      <!-- 卡片样式选择 -->
      <div class="commodityType">
        <a-tooltip class="item" effect="dark" :title="item.content" placement="bottom"
          v-for="(item, index) in commodityTypes" :key="index">
          <span class="iconfont" style="font-size: 21px" :class="[
      item.type === datas.commodityType ? 'active' : '',
      item.icon,
    ]" @click="datas.commodityType = index" />
        </a-tooltip>
      </div>

      <div class="bor" />

      <h5 style="color: #000; font-size: 14px">添加卡片</h5>
      <p style="color: #969799; font-size: 12px; margin-top: 10px">
        鼠标拖拽调整卡片顺序
      </p>

      <!-- 图片广告 -->
      <div v-if="datas.imageList[0]">
        <vuedraggable v-model="datas.imageList" v-bind="dragOptions">
          <transition-group>
            <section class="imgBanner" v-for="(item, index) in datas.imageList" :key="item + index">
              <a-icon class="a-icon-circle-close" type="close-circle" @click="deleteimg(index)" />
              <!-- 图片 -->
              <div class="imag">
                <img :src="item.src" alt="" draggable="false" />
              </div>
              <!-- 标题和链接 -->
              <div class="imgText">
                <a-input v-model="item.text" placeholder="笔记标题" size="small" />
                <!-- 标题和链接 -->
                <div class="imgTextChild">
                  <!-- 选择类型 -->
                  <a-select v-model="item.linktype" placeholder="请选择跳转类型" size="small" @change="selectType(index)">
                    <a-select-option v-for="iteml in optionsType" :key="iteml.name" :value="iteml.type">
                      {{ iteml.name }}
                    </a-select-option>
                  </a-select>

                  <!-- 输入外部链接 -->
                  <a-input size="small" placeholder="请输入链接，输入前确保可以访问" v-model="item.http.externalLink">
                  </a-input>
                </div>
              </div>
            </section>
          </transition-group>
        </vuedraggable>
      </div>

      <!-- 上传图片 -->
      <a-button @click="upLoadimg.visible = true" class="uploadImg" type="primary" plain><i
          class="a-icon-plus" />点击添加卡片</a-button>

      <div style="height: 10px" />

      <!-- 卡片样式 -->
      <a-form-item label="卡片样式" class="lef" />
      <!-- 卡片样式选择 -->
      <div class="moditystyle">
        <span v-for="(item, index) in moditystyles" :key="index" :class="item.type == datas.moditystyle ? 'active' : ''"
          @click="datas.moditystyle = index">
          {{ item.text }}
        </span>
      </div>

      <div class="bor" />

      <!-- 文本粗细 -->
      <a-form-item class="lef" label="文本粗细" prop="textWeight" :hide-required-asterisk="true">
        <a-input type="number" v-model.number="datas.textWeight" placeholder="请输入文本粗细" />
      </a-form-item>

      <div style="height: 10px" />

      <!-- 图片倒角 -->
      <a-form-item label="图片倒角" class="lef borrediu">
        <a-slider v-model="datas.borderRadius" :max="30"> </a-slider>
      </a-form-item>

      <div style="height: 10px" v-if="datas.commodityType === 0" />

      <!-- 显示位置 -->
      <a-form-item label="标题位置" class="lef" v-if="datas.commodityType === 0">
        <div class="weiz">
          <a-tooltip class="item" effect="dark" content="上方" placement="bottom">
            <i :class="datas.positions === 'top' ? 'active' : ''" class="iconfont icon-shang"
              @click="datas.positions = 'top'" />
          </a-tooltip>
          <a-tooltip class="item" effect="dark" content="下方" placement="bottom">
            <i :class="datas.positions === 'bottom' ? 'active' : ''" class="iconfont icon-jiantou"
              @click="datas.positions = 'bottom'" />
          </a-tooltip>
        </div>
      </a-form-item>

      <div class="bor" />

      <!--笔记标签 -->
      <a-form-item class="lef" label="笔记标签">
        {{ datas.noteLabels ? "显示" : "隐藏" }}
        <a-checkbox style="margin-left: 196px" v-model="datas.noteLabels" />
      </a-form-item>

      <!--阅读数 -->
      <a-form-item class="lef" label="阅读数">
        {{ datas.readingNumber ? "显示" : "隐藏" }}
        <a-checkbox style="margin-left: 196px" v-model="datas.readingNumber" />
      </a-form-item>

      <!--点赞数 -->
      <a-form-item class="lef" label="点赞数">
        {{ datas.praisePoints ? "显示" : "隐藏" }}
        <a-checkbox style="margin-left: 196px" v-model="datas.praisePoints" />
      </a-form-item>

      <!--查看更多 -->
      <a-form-item class="lef" label="查看更多">
        <!-- {{datas.viewMore1 ? '显示' : '隐藏'}} -->
        <a-checkbox v-model="datas.viewMore1">头部{{ datas.viewMore1 ? "显示" : "隐藏" }}</a-checkbox>
        <a-checkbox v-model="datas.viewMore2">尾部{{ datas.viewMore2 ? "显示" : "隐藏" }}</a-checkbox>
        <div class="imgText1" v-show="datas.viewMore1 || datas.viewMore2">
          <!-- 选择类型 -->
          <a-select style="width: 60%" v-model="datas.linktype" placeholder="请选择跳转类型" size="small">
            <a-select-option v-for="item in optionsType1" :key="item.name" :value="item.type">
              {{ item.name }}
            </a-select-option>
          </a-select>

          <!-- 输入外部链接 -->
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
import vuedraggable from "vuedraggable"; //拖拽组件

export default {
  name: "storenotecardstyle",
  props: {
    datas: Object,
  },
  components: { vuedraggable },
  data() {
    let kon = (rule, value, callback) => {
      if (value.length === 0) callback(new Error("请输入有效数字"));
    };
    return {
      upLoadimg: {
        visible: false
      },
      optionsType1: [
        {
          type: "10",
          name: "内部链接",
        },
        {
          type: "11",
          name: "外部链接",
        },
      ], // 选择跳转类型
      options1: [],
      moditystyles: [
        /* 卡片样式 */
        {
          text: "无边白底",
          type: 0,
        },
        {
          text: "卡片投影",
          type: 1,
        },
        {
          text: "描边白底",
          type: 2,
        },
        {
          text: "无边透明底",
          type: 3,
        },
      ],
      commodityTypes: [
        {
          icon: "icon-datumoshi",
          type: 0,
          content: "大图模式",
        },
        {
          icon: "icon-commodity-yihangliangge",
          type: 1,
          content: "一行两个",
        },
        {
          icon: "icon-xuanzemokuai-daohanghengxianghuadong",
          type: 2,
          content: "横向滑动",
        },
        {
          icon: "icon-sanlan",
          type: 3,
          content: "一大两小",
        },
        {
          icon: "icon-commodity-xiangxiliebiao",
          type: 4,
          content: "详细列表",
        },
      ],
      rules: {
        textWeight: [{ required: true, validator: kon, trigger: "blur" }],
      },
      marker: ["新品", "热卖", "NEW", "HOT"],
      dragOptions: {
        animation: 200,
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
      options: [], //后端返回的列表提供下拉选择
      emptyText: "",
    };
  },
  created() { },
  methods: {
    selectType(index) {
      // 每次切换类型之前 清空之前选中跳转
      this.datas.imageList[index].http = {};
      // 清空 options
      this.options = [];
    },
    // 提交
    materialOk(res) {
      this.datas.imageList.push({
        src: res,
        text: "这里显示笔记标题最多显示2行",
        http: {},
        type: "1",
      });
    },

    /* 删除图片 */
    deleteimg(index) {
      this.datas.imageList.splice(index, 1);
    },
  },
  computed: {
    // eslint-disable-next-line vue/return-in-computed-property
    styleText() {
      if (this.datas.commodityType === 0) return "大图模式";
      if (this.datas.commodityType === 1) return "一行两个";
      if (this.datas.commodityType === 2) return "横向滑动";
      if (this.datas.commodityType === 3) return "详细列表";
      if (this.datas.commodityType === 4) return "一大两小";
      if (this.datas.commodityType === 5) return "横向滑动";
    },
  },
};
</script>

<style scoped lang="less">
.storenotecardstyle {
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

  /* 图片字 */
  .imgText1 {
    width: 100%;
    display: flex;
    box-sizing: border-box;
    justify-content: space-between;

    .fir-sele.a-select {
      width: 40%;
    }
  }

  /* 列表样式 */
  .commodityType {
    display: flex;
    justify-content: space-around;
    align-items: center;

    span {
      display: inline-block;
      width: 58px;
      height: 32px;
      text-align: center;
      line-height: 32px;
      background: #fff;
      border: 1px solid #ebedf0;
      color: #979797;
      margin: 0 1px;
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

  /* 卡片样式 */
  .moditystyle {
    font-size: 12px;
    width: 100%;
    display: flex;

    span {
      width: 86px;
      height: 32px;
      display: inline-block;
      text-align: center;
      line-height: 32px;
      cursor: pointer;
      border: 1px solid #ebedf0;

      &.active {
        border: 1px solid @primary-color;
        background-color: #e0edff;
        color: @primary-color;
      }
    }
  }

  /* 位置 */
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

  /* 单选框 */
  /deep/.radi1 {
    border-top: 1px solid #f7f8fa;
    border-bottom: 1px solid #f7f8fa;
    padding: 12px 0;

    .a-radio {
      margin: 10px 25px 7px 0;
    }
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

  .tit {
    /deep/.a-input__inner {
      text-align: center;
    }
  }
}
</style>
