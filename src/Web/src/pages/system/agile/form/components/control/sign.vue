<template>
  <div>
    <div v-if="!disabled">
      <div v-if="valueCheck == null">
        <vue-esign
          ref="esign"
          style="border: 1px dashed #c2c1c1"
          :isCrop="isCrop"
          :lineWidth="lineWidth"
          :lineColor="lineColor"
          :bgColor.sync="bgColor"
        />
        <a-space v-if="!disabled">
          <a-button type="primary" @click="handleGenerate">确定签名</a-button>
          <a-button @click="handleReset">重新签字</a-button>
        </a-space>
      </div>
      <div v-else>
        <img style="max-width: 100%; max-height: 100%" :src="value" />
        <a-space>
          <a-button @click="valueCheck = null">重新签字</a-button>
        </a-space>
      </div>
    </div>
    <img v-else style="max-width: 100%; max-height: 100%" :src="value" />
  </div>
</template>

<script>
import vueEsign from "vue-esign";
export default {
  components: {
    vueEsign,
  },
  data() {
    return {
      valueCheck: null,
      lineWidth: 6,
      lineColor: "#000000",
      bgColor: "#fff",
      isCrop: false,
    };
  },
  props: ["value", "record", "disabled"],
  watch: {
    value(value) {
      if (value) {
        this.valueCheck = value;
      }
    },
  },
  methods: {
    /**
     * 重画
     */
    handleReset() {
      this.$refs.esign.reset();
    },

    /**
     * 获取图片
     */
    handleGenerate() {
      let that = this;
      this.$refs.esign
        .generate()
        .then((res) => {
          that.$emit("change", res);
          that.$message.success("确认签名成功");
        })
        .catch((err) => {
          that.$message.error(err);
        });
    },
  },
};
</script>