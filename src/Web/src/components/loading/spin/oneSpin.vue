<template>
  <div class="loading">
    <i></i><i></i><i></i><i></i>
  </div>
</template>
<script>
export default {
  name: "oneSpin",
};
</script>
<style lang="less" scoped>
.loading,
.loading>div {
  position: relative;
  box-sizing: border-box;
}

.loading {
  display: block;
  font-size: 0;
  color: @primary-color;
  display: inline-block;
}

.loading {
  animation: antRotate 1.2s infinite linear;
  transform: rotate(45deg);
  position: relative;
  display: inline-block;
  font-size: 40px;
  width: 40px;
  height: 40px;
  box-sizing: border-box
}

.loading i {
  width: 20px;
  height: 20px;
  position: absolute;
  display: block;
  background-color: #1890ff;
  border-radius: 100%;
  transform: scale(.75);
  transform-origin: 50% 50%;
  opacity: .3;
  animation: antSpinMove 1s infinite linear alternate
}

.loading i:nth-child(1) {
  top: 0;
  left: 0
}

.loading i:nth-child(2) {
  top: 0;
  right: 0;
  -webkit-animation-delay: .4s;
  animation-delay: .4s
}

.loading i:nth-child(3) {
  right: 0;
  bottom: 0;
  -webkit-animation-delay: .8s;
  animation-delay: .8s
}

.loading i:nth-child(4) {
  bottom: 0;
  left: 0;
  -webkit-animation-delay: 1.2s;
  animation-delay: 1.2s
}

@keyframes antRotate {
  to {
    -webkit-transform: rotate(405deg);
    transform: rotate(405deg)
  }
}

@-webkit-keyframes antRotate {
  to {
    -webkit-transform: rotate(405deg);
    transform: rotate(405deg)
  }
}

@keyframes antSpinMove {
  to {
    opacity: 1
  }
}

@-webkit-keyframes antSpinMove {
  to {
    opacity: 1
  }
}
</style>