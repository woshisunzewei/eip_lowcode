<template>
  <a-card :hoverable="true">
    <!-- 标签栏 -->
    <a-tabs v-model="activeName" @tab-click="handleClick">
      <a-tab-pane tab="待办公文" key="first">
        <div class="document-list-box">
          <div class="document-list-item">
            <span>405001</span>
            <i class="iconfont icon-bell-fill"></i>
            <span>收文-关于国庆节假期规定</span>
            <span class="level-item">普通</span>
            <span>当前步骤：部门会签</span>
            <span class="name-item">王小明</span>
            <span class="time-item">2020-09-10 09:51</span>
          </div>
          <div class="document-list-item">
            <span>405001</span>
            <i class="iconfont icon-bell-fill"></i>
            <span>收文-关于国庆节假期规定</span>
            <span class="level-item">普通</span>
            <span>当前步骤：部门会签</span>
            <span class="name-item">王小明</span>
            <span>2020-09-10 09:51</span>
          </div>
          <div class="document-list-item">
            <span>405001</span>
            <i class="iconfont icon-bell-fill"></i>
            <span>收文-关于国庆节假期规定</span>
            <span class="level-item">普通</span>
            <span>当前步骤：部门会签</span>
            <span class="name-item">王小明</span>
            <span>2020-09-10 09:51</span>
          </div>
          <div class="document-list-item">
            <span>405001</span>
            <i class="iconfont icon-bell-fill"></i>
            <span>收文-关于国庆节假期规定</span>
            <span class="level-item">普通</span>
            <span>当前步骤：部门会签</span>
            <span class="name-item">王小明</span>
            <span>2020-09-10 09:51</span>
          </div>
          <div class="document-list-item">
            <span>405001</span>
            <i class="iconfont icon-bell-fill"></i>
            <span>收文-关于国庆节假期规定</span>
            <span class="level-item">普通</span>
            <span>当前步骤：部门会签</span>
            <span class="name-item">王小明</span>
            <span>2020-09-10 09:51</span>
          </div>
        </div>
      </a-tab-pane>
      <a-tab-pane tab="待签阅公文" key="second">配置管理</a-tab-pane>
    </a-tabs>
  </a-card>
</template>

<script>
export default {
  props: ['id'],
  data() {
    return {
      activeName: 'first',
      panelId: this.id,
    }
  },
  methods: {
    handleClick(tab, event) {
      console.log(tab, event)
    },
  }
}
</script>

<style>
.ant-card {
  height: 100%;
  position: relative;
}

.ant-card .document-close-icon {
  position: absolute;
  top: 20px;
  right: 20px;
  z-index: 200;
}

.document-list-box {
  display: grid;
  grid-template-columns: repeat(1, 100%);
}

.document-list-box .document-list-item {
  display: grid;
  grid-template-columns: 1fr 0.5fr 3.5fr 1fr 3fr 1fr 2fr;
  height: 40px;
  cursor: pointer;
  font-size: 14px;
  line-height: 40px;
}

.document-list-item i {
  color: #ff394c;
}

.document-list-item>* {
  overflow: hidden;
  white-space: nowrap;
  text-overflow: ellipsis;
}
</style>
