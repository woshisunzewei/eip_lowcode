<template>
  <div style="width: 100%;">
    <a-dropdown :trigger="trigger" v-model="visible">
      <div class="eip-icon-picker ant-dropdown-trigger">
        <div class="m-colorPicker">
          <div :style="{ 'background-color': colors.hex }" class="colorBtn"></div>
        </div>
      </div>
      <div slot="overlay" class="ant-dropdown-menu eip-icon-picker-popper">
        <sketch-picker v-model="colors" @input="colorChange" />
        <div @click="colorClear" v-if="showClear" style="text-align: right;" class="padding-xs">
          <a-button size="small" type="primary">清空</a-button>
        </div>
      </div>
    </a-dropdown>
  </div>
</template>
<script>
import {
  Sketch
} from "vue-color";
export default {
  name: 'eip-color',
  components: {
    // "material-picker": Material,
    // "compact-picker": Compact,
    // "swatches-picker": Swatches,
    // "slider-picker": Slider,
    // "photoshop-picker": Photoshop,
    // "chrome-picker": Chrome,
    "sketch-picker": Sketch,
    // "twitter-picker": Twitter,
    // "grayscale-picker": Grayscale
  },
  data() {
    return {
      trigger: ["click"],
      visible: false,
      /* 颜色选择器 */
      colors: {
        color: this.value,
        hex: this.value,
        hsl: { h: 150, s: 0.5, l: 0.2, a: 1 },
        hsv: { h: 150, s: 0.66, v: 0.3, a: 1 },
        rgba: { r: 25, g: 77, b: 51, a: 1 },
        a: 1
      }

    };
  },
  props: {
    value: {
      type: String,
      default: 'transparent'
    },
    showClear: {
      type: Boolean,
      default: true
    }
  },
  methods: {
    /**
     * 选择
     */
    colorChange(item) {
      this.$emit("change", item.hex);
    },
    /**
    * 清空
    */
    colorClear() {
      this.colors.hex = 'transparent'
      this.$emit("change", 'transparent');
    },
  },
};
</script>
<style lang="less" scoped>
.eip-icon-picker-popper {
  padding: 2px;
  width: 100% !important;
  max-width: 90vw;
  padding: 0;
}

.m-colorPicker {
  position: relative;
  text-align: left;
  font-size: 14px;
  display: inline-block;
  outline: none;
  width: 100%;
  vertical-align: bottom;
  padding: 5px;
  border: 1px solid #d9d9d9;
  font-size: 0;
}
</style>
