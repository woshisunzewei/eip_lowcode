<template>
  <a-card :hoverable="true">
    <!-- 标题 -->
    <div slot="title">
      <span class="ant-card-title">常用流程</span>
    </div>
    <!-- 流程区域 -->
    <div class="often-apply-box">
      <div class="apply-item">
        <a-avatar class="avatar-icon" :size="50"> user </a-avatar>
        <span class="apply-title">请假流程</span>
      </div>
      <div class="apply-item">
        <a-avatar class="avatar-icon" :size="50"> user </a-avatar>
        <span class="apply-title">测试实施流程</span>
      </div>
      <div class="apply-item">
        <a-avatar class="avatar-icon" :size="50"> user </a-avatar>
        <span class="apply-title">测试实施流程</span>
      </div>
      <div class="apply-item">
        <a-avatar class="avatar-icon" :size="50"> user </a-avatar>
        <span class="apply-title">测试实施流程</span>
      </div>
      <div class="apply-item">
        <a-avatar class="avatar-icon" :size="50"> user </a-avatar>
        <span class="apply-title">测试实施流程</span>
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

.often-apply-box {
  display: grid;
  grid-template-columns: repeat(1, 100%);
}

.often-apply-box .apply-item {
  display: flex;
  align-items: center;
  cursor: pointer;
  margin-bottom: 20px;
}

.apply-item .apply-title {
  margin-left: 10px;
}

.apply-item .avatar-icon {
  background-color: #2f7dcd !important;
}
</style>
