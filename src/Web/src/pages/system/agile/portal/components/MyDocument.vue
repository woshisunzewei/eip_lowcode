<template>
  <a-card :hoverable="true">
    <!-- 标题 -->
    <div slot="title">
      <span class="ant-card-title">我督办的公文</span>
    </div>
    <!-- 标签栏 -->
    <div class="mydocument-box">
      <div class="mydocument-item">
        <span>405001</span>
        <i class="iconfont icon-bell-fill"></i>
        <span>收文-关于国庆节假期规定</span>
        <span class="level-item">普通</span>
        <span>当前步骤：部门会签</span>
        <span class="name-item">王小明</span>
        <span>2020-09-10 09:51</span>
      </div>
      <div class="mydocument-item">
        <span>405001</span>
        <i class="iconfont icon-bell-fill"></i>
        <span>收文-关于国庆节假期规定</span>
        <span class="level-item">普通</span>
        <span>当前步骤：部门会签</span>
        <span class="name-item">王小明</span>
        <span>2020-09-10 09:51</span>
      </div>
      <div class="mydocument-item">
        <span>405001</span>
        <i class="iconfont icon-bell-fill"></i>
        <span>收文-关于国庆节假期规定</span>
        <span class="level-item">普通</span>
        <span>当前步骤：部门会签</span>
        <span class="name-item">王小明</span>
        <span>2020-09-10 09:51</span>
      </div>
      <div class="mydocument-item">
        <span>405001</span>
        <i class="iconfont icon-bell-fill"></i>
        <span>收文-关于国庆节假期规定</span>
        <span class="level-item">普通</span>
        <span>当前步骤：部门会签</span>
        <span class="name-item">王小明</span>
        <span>2020-09-10 09:51</span>
      </div>
      <div class="mydocument-item">
        <span>405001</span>
        <i class="iconfont icon-bell-fill"></i>
        <span>收文-关于国庆节假期规定</span>
        <span class="level-item">普通</span>
        <span>当前步骤：部门会签</span>
        <span class="name-item">王小明</span>
        <span>2020-09-10 09:51</span>
      </div>
    </div>
  </a-card>
</template>

<script>
export default {
  props: ['id'],
  data() {
    return {
      panelId: this.id,

    }
  },
  methods: {

  }
}
</script>

<style>
.ant-card {
  height: 100%;
}

.anticon-close {
  cursor: pointer;
}

.ant-card-title {
  font-weight: bold;
  font-size: 1.2em;
}

.mydocument-item {
  display: grid;
  grid-template-columns: 1fr 0.5fr 3.5fr 1fr 3fr 1fr 2fr;
  height: 40px;
  cursor: pointer;
  font-size: 14px;
  line-height: 40px;
}

.mydocument-item i {
  color: #ffb339;
}

.mydocument-item>* {
  overflow: hidden;
  white-space: nowrap;
  text-overflow: ellipsis;
}
</style>
