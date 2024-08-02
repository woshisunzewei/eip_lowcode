<template>
  <a-modal :visible="datas.show" v-drag :zIndex="9997" :destroyOnClose="true" :maskClosable="false"
    :bodyStyle="{ padding: '30px 20px', overflow: 'hidden' }" :footer="null" width="416px" @cancel="datas.show = false">
    <div class="RealTimeView">
      <img src="@/pages/system/agile/mobile/assets/images/phoneTop.png" style="width: 375px" />
      <iframe v-if="datas.show" ref="iframe" class="screen" :scrolling="false"
        :src="'https://mobile.eipflow.com/#/pages/design/index?configId=' + val.id" @load="load"></iframe>
      <a-spin v-if="loading"> </a-spin>
    </div>
  </a-modal>
</template>

<script>
export default {
  name: "RealTimeView",
  props: {
    datas: {
      show: false,
    },
    val: Object,
  },
  data() {
    return {
      loading: true,
    };
  },
  methods: {
    load() {
      this.loading = false;
      this.$refs["iframe"].contentWindow.postMessage(
        this.val,
        "https://mobile.eipflow.com"
      );
    },
  },
};
</script>

<style lang="less" scoped>
.RealTimeView {
  .screen {
    width: 375px
      /*no*/
    ;
    height: 667px
      /*no*/
    ;
    border: 0;

    // 隐藏滚动条
    &::-webkit-scrollbar {
      display: none;
      /* Chrome Safari */
    }
  }
}
</style>
