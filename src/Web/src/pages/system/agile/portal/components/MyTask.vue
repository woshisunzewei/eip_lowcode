<template>
  <a-card :hoverable="true">
    <!-- 标题 -->
    <div slot="title">
      <span class="ant-card-title">待我执行的任务</span>
    </div>
    <!-- 内容 -->
    <div class="mytask-box">
      <div class="mytask-box-item">
        <div class="item-left">
          <a-progress class="progress" color="#396AFF" :show-text="false" :percentage="50"></a-progress>
        </div>
        <div class="item-right">
          <div class="right-1">
            <span class="task-tip">督办</span>
            <a-divider direction="vertical"></a-divider>
            <span class="task-title">任务名称任务名称任务</span>
          </div>
          <div class="right-2">
            <i class="iconfont icon-time-circle-fill"></i>
            <span class="task-date">3-16</span>
            <span class="task-time">15:30-17:30</span>
          </div>
        </div>
      </div>
      <div class="mytask-box-item">
        <div class="item-left">
          <a-progress class="progress" color="#396AFF" :show-text="false" :percentage="20"></a-progress>
        </div>
        <div class="item-right">
          <div class="right-1">
            <span class="task-tip">督办</span>
            <a-divider direction="vertical"></a-divider>
            <span class="task-title">任务名称任务名称任务</span>
          </div>
          <div class="right-2">
            <i class="iconfont icon-time-circle-fill"></i>
            <span class="task-date">3-16</span>
            <span class="task-time">15:30-17:30</span>
          </div>
        </div>
      </div>
      <div class="mytask-box-item">
        <div class="item-left">
          <a-progress class="progress" color="#396AFF" :show-text="false" :percentage="30"></a-progress>
        </div>
        <div class="item-right">
          <div class="right-1">
            <span class="task-tip">督办</span>
            <a-divider direction="vertical"></a-divider>
            <span class="task-title">任务名称任务名称任务</span>
          </div>
          <div class="right-2">
            <i class="iconfont icon-time-circle-fill"></i>
            <span class="task-date">3-16</span>
            <span class="task-time">15:30-17:30</span>
          </div>
        </div>
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

.mytask-box-item {
  display: flex;
  padding: 10px 0;
  border: 1px solid #f2f4f7;
  border-radius: 4px;
  height: 80px;
  margin-bottom: 10px;
  cursor: pointer;
}

.item-left {
  flex: 0 0 15%;
  height: 100%;
  display: flex;
  align-items: center;
}

.progress {
  width: 100%;
  transform: rotate(270deg);
}

.item-right {
  height: 100%;
}

.right-1 {
  display: flex;
  align-items: center;
  flex: 1;
}

.right-1 .task-tip {
  color: #396aff;
}

.right-1 .task-title {
  overflow: hidden;
  white-space: nowrap;
  text-overflow: ellipsis;
}

.right-2 {
  margin-top: 15px;
}

.right-2 i {
  color: #ff394c;
}

.right-2 .task-date,
.task-time {
  color: #7682a8;
  font-size: 14px;
}

.right-2 .task-date {
  margin: 0 20px 0 5px;
}
</style>
