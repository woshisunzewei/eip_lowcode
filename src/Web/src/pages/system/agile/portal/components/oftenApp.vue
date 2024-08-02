<template>
  <a-card :hoverable="true">
    <!-- 标题 -->
    <div slot="title">
      <span class="ant-card-title">常用应用</span>
    </div>
    <!-- 主体区域 -->
    <div class="often-app-box">
      <div class="often-app app-1">
        <a-icon type="solution"></a-icon>
        <span class="app-title">日志</span>
      </div>
      <div class="often-app app-2">
        <a-icon type="calendar"></a-icon>
        <span class="app-title">日程</span>
      </div>
      <div class="often-app app-3">
        <a-icon type="bell"></a-icon>
        <span class="app-title">会议</span>
      </div>
      <div class="often-app app-4">
        <a-icon type="snippets"></a-icon>
        <span class="app-title">任务</span>
      </div>
      <div class="often-app app-5">
        <a-icon type="cloud"></a-icon>
        <span class="app-title">网盘</span>
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

.often-app-box {
  display: grid;
  grid-template-columns: repeat(4, 25%);
  cursor: pointer;
  justify-items: center;
}

.often-app-box .often-app {
  display: flex;
  flex-direction: column;
  width: 80px;
  justify-content: center;
  align-items: center;
  margin-bottom: 20px;
}

.often-app i {
  color: #fff;
  padding: 10px;
  font-size: 2em;
  border-radius: 4px;
}

.app-1 i {
  background: #29c814;
}

.app-2 i {
  background: #ffb339;
}

.app-3 i {
  background: #ff6b79;
}

.app-4 i {
  background: #00cde9;
}

.app-5 i {
  background: #39a0ff;
}

.app-title {
  margin-top: 10px;
}
</style>
