<template>
  <div class="exception-page">
    <a-result :status="type" :title="config[type].title" :sub-title="config[type].desc">
      <template #extra>
        <a-button type="primary" @click="backHome"> 返回首页 </a-button>
      </template>
    </a-result>
  </div>
</template>

<script>
import Config from "./typeConfig";

export default {
  name: "ExceptionPage",
  props: ["type", "homeRoute"],
  data() {
    return {
      config: Config,
    };
  },
  methods: {
    backHome() {
      if (this.homeRoute) {
        this.$router.push(this.homeRoute);
      }
      this.$emit("backHome", this.type);
    },
  },
};
</script>

<style lang="less" scoped>
.exception-page {
  border-radius: 4px;
  display: flex;
  justify-content: center;
  align-items: center;
  background-color: @base-bg-color;

  .img {
    padding-right: 52px;
    zoom: 1;

    img {
      max-width: 430px;
    }
  }

  .content {
    h1 {
      color: #434e59;
      font-size: 72px;
      font-weight: 600;
      line-height: 72px;
      margin-bottom: 24px;
    }

    .desc {
      color: @text-color-second;
      font-size: 20px;
      line-height: 28px;
      margin-bottom: 16px;
    }
  }
}
</style>
