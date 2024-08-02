<template>
  <div>
    <!-- 文本 -->
    <label :style="{
      'font-size': record.options.label.fontSize,
      'color': record.options.label.color,
      'font-family': record.options.label.fontFamily
    }" v-if="record.options.type == 'label'" v-html="value"></label>

    <!-- 图片 -->
    <img style="width:100px;height:100px" v-if="record.options.type == 'img'" :src="value" />

    <!-- 条形码 -->
    <vue-barCode v-if="record.options.type == 'barCode'" :fontOptions="record.options.barCode.fontOptions"
      :font="record.options.barCode.font" :textPosition="record.options.barCode.textPosition"
      :format="record.options.barCode.format" :displayValue="record.options.barCode.displayValue"
      :height="record.options.barCode.height" :textAlign="record.options.barCode.textAlign"
      :width="record.options.barCode.width" :value="value" :textMargin="record.options.barCode.textMargin"
      :fontSize="record.options.barCode.fontSize" :background="record.options.barCode.background"
      :lineColor="record.options.barCode.lineColor" :margin="record.options.barCode.margin" />

    <!-- 二维码 -->
    <vue-qr v-if="record.options.type == 'qrCode'" :correctLevel="record.options.qrCode.correctLevel"
      :colorLight="record.options.qrCode.colorLight" :colorDark="record.options.qrCode.colorDark"
      :backgroundColor="record.options.qrCode.backgroundColor" :text="value" :size="record.options.qrCode.size"
      :margin="record.options.qrCode.margin" />
  </div>
</template>
<script>
// 条形码
import VueBarCode from 'vue-barcode';
//二维码
import VueQr from 'vue-qr'
export default {
  name: "dataShow",
  components: {
    VueQr,
    VueBarCode
  },
  data() {
    return {};
  },
  props: ["value", "record"],
  created() {
  },
  methods: {

  },
};
</script>