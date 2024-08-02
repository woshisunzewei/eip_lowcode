<template>
  <div class="auxiliarysegmentationstyle">
    <!-- 标题 -->
    <h2>{{ datas.text }}</h2>

    <!-- 表单 -->
    <a-form :label-col="{ span: 7 }" :wrapper-col="{ span: 17 }" :model="datas">
      <!-- 空白高度 -->
      <a-form-item label="空白高度" class="lef">
        <a-slider v-model="datas.blankHeight" :max="100"> </a-slider>
      </a-form-item>

      <div style="height: 20px" />

      <!-- 分割类型 -->
      <a-form-item class="lef" label="分割类型">
        <div class="weiz">
          <a-tooltip effect="dark" :title="index - 1 === 0 ? '辅助空白' : '辅助线'" placement="bottom" v-for="index in 2"
            :key="index">
            <i class="iconfont" :class="[
              index - 1 === 0
                ? 'icon-fuzhukongbai_weixuanzhong'
                : 'icon-fuzhuxiantiao',
              datas.segmentationtype === index - 1 ? 'active' : '',
            ]" @click="datas.segmentationtype = index - 1" />
          </a-tooltip>
        </div>
      </a-form-item>

      <div style="height: 20px" />

      <!-- 选择样式 -->
      <a-form-item v-show="datas.segmentationtype === 1" class="lef" label="选择样式">
        <div class="weiz">
          <a-tooltip effect="dark" :title="item.text" placement="bottom" v-for="(item, index) in borderType" :key="index">
            <i class="iconfont" :class="[item.icon, datas.bordertp === item.type ? 'active' : '']"
              @click="datas.bordertp = item.type" />
          </a-tooltip>
        </div>
      </a-form-item>

      <div v-show="datas.segmentationtype === 1" style="height: 20px" />

      <!-- 左右边距 -->
      <a-form-item v-show="datas.segmentationtype === 1" label="左右边距">
        <div class="weiz">
          <a-tooltip effect="dark" :title="index - 1 === 0 ? '无边距' : '左右留边'" placement="bottom" v-for="index in 2"
            :key="index">
            <i class="iconfont" :class="[
              index - 1 === 0
                ? 'icon-icon_wubianju'
                : 'icon-icon_zuoyoubianju',
              datas.paddType === index - 1 ? 'active' : '',
            ]" @click="datas.paddType = index - 1" />
          </a-tooltip>
        </div>
      </a-form-item>

      <div v-show="datas.segmentationtype === 1" style="height: 20px" />

      <!-- 辅助线颜色 -->
      <a-form-item v-show="datas.segmentationtype === 1" label="辅助线颜色">
        <!-- 辅助线颜色 -->
        <eip-color :value="datas.auxliarColor" @change="(value) => { datas.auxliarColor = value }"></eip-color>
      </a-form-item>
    </a-form>
  </div>
</template>

<script>
export default {
  name: "auxiliarysegmentationstyle",
  props: {
    datas: Object,
  },
  data() {
    return {
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
      borderType: [
        //线类型
        {
          icon: "icon-icon_fengexian_shixian",
          text: "实线",
          type: "solid",
        },
        {
          icon: "icon-xuxian",
          text: "虚线",
          type: "dashed",
        },
        {
          icon: "icon-dianxian--",
          text: "点线",
          type: "dotted",
        },
      ],
    };
  },
};
</script>

<style scoped lang="less">
.auxiliarysegmentationstyle {
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
    float: right;
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
}
</style>
