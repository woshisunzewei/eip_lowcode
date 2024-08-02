<template>
  <a-card :hoverable="true">
    <!-- 标题 -->
    <div slot="title">
      <span class="ant-card-title">快捷操作</span>
    </div>
    <!-- 新建按钮 -->
    <div class="often-control-box">
      <div class="often-control-item">
        <a-icon type="calendar"></a-icon>
        <span>新建日志</span>
      </div>
      <div class="often-control-item">
        <a-icon type="snippets"></a-icon>
        <i class="iconfont icon-snippets"></i>
        <span>新建任务</span>
      </div>
      <div class="often-control-item">
        <a-icon type="bell"></a-icon>
        <span>新建会议</span>
      </div>
      <div class="often-control-item">
        <a-icon type="bell"></a-icon>
        <span>新建公告</span>
      </div>
      <div class="often-control-item">
        <a-icon type="book"></a-icon>
        <span>新建收文</span>
      </div>
      <div class="often-control-item">
        <a-icon type="book"></a-icon>
        <span>新建发文</span>
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

.often-control-box {
  display: grid;
  grid-template-columns: repeat(2, 50%);
}

.often-control-box .often-control-item {
  height: 50px;
  display: flex;
  align-items: center;
  cursor: pointer;
  color: #424e5c;
}

.often-control-item>i {
  color: #396aff;
  margin-right: 10px;
  font-size: 1.5em;
}
</style>
