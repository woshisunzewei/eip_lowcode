<template>
  <a-card :hoverable="true">
    <!-- 标签栏 -->
    <a-tabs v-model="activeName" @tab-click="handleClick">
      <a-tab-pane tab="待办流程" key="first">
        <div class="process-box">
          <div class="process-item">

            <span>王小明</span>
            <span>请假申请</span>
            <span>当前步骤：经理审批</span>
            <span>2020-09-10 09:51</span>
          </div>
        </div>
        <div class="process-box">
          <div class="process-item">
            <span>王小明</span>
            <span>请假申请</span>
            <span>当前步骤：经理审批</span>
            <span>2020-09-10 09:51</span>
          </div>
        </div>
        <div class="process-box">
          <div class="process-item">
            <span>王小明</span>
            <span>请假申请</span>
            <span>当前步骤：经理审批</span>
            <span>2020-09-10 09:51</span>
          </div>
        </div>
        <div class="process-box">
          <div class="process-item">
            <span>王小明</span>
            <span>请假申请</span>
            <span>当前步骤：经理审批</span>
            <span>2020-09-10 09:51</span>
          </div>
        </div>
      </a-tab-pane>
      <a-tab-pane tab="待签阅流程" key="second">配置管理</a-tab-pane>
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
    },

  }
}
</script>

<style>
.ant-card {
  height: 100%;
  position: relative;
}

.ant-card .process-close-icon {
  position: absolute;
  top: 20px;
  right: 20px;
  z-index: 200;
}

.process-item {
  display: grid;
  grid-template-columns: 1fr 1fr 2fr 3fr 3fr;
  height: 40px;
  margin-bottom: 20px;
  font-size: 14px;
  cursor: pointer;
  line-height: 40px;
}

.process-item>* {
  overflow: hidden;
  white-space: nowrap;
  text-overflow: ellipsis;
}
</style>
