<template>
  <div class="loading-container" style=" background: rgba(0,0,0,0.7)" v-if="show">
    <div class="loading-wrapper">
      <svg-icon v-if="spin == 'loading'" icon="loading" color="#3ff9dc" :size="25" class="loading-icon" />
      <component :is="spin + '-spin'" v-else />
      <div class="text">{{ text }}</div>
    </div>
  </div>
</template>

<script>
import {
  pulseSpin,
  rectSpin,
  planeSpin,
  cubeSpin,
  preloaderSpin,
  chaseSpin,
  oneSpin,
} from "./spin";
export default {
  props: {
    show: {
      type: Boolean,
      default: false,
    },
    text: {
      type: String,
      default: "处理中...",
    },
    textColor: {
      type: String,
      default: "#3ff9dc",
    },
    background: {
      type: String,
      default: "rgba(0,0,0,0.7)",
    },
    spin: {
      type: String,
      default: "one",
    },
  },
  components: {
    pulseSpin,
    rectSpin,
    planeSpin,
    cubeSpin,
    preloaderSpin,
    chaseSpin,
    oneSpin,
  },
};
</script>
<style lang="less" scoped>
.loading-container {
  position: absolute;
  top: 0;
  left: 0;
  bottom: 0;
  right: 0;
  z-index: 9999;

  .loading-wrapper {
    position: absolute;
    top: 50%;
    margin-top: -21px;
    text-align: center;
    width: 100%;

    .text {
      margin: 8px 0;
      width: 100%;
      color: @primary-color;
    }

    .loading-icon {
      animation: rotating 1.5s linear infinite;
    }
  }
}

@keyframes rotating {
  0% {
    transform: rotate(0);
  }

  100% {
    transform: rotate(1turn);
  }
}
</style>
