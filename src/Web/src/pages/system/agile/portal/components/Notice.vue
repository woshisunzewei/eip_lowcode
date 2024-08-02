<template>
  <a-card :hoverable="true">
    <!-- 标题 -->
    <div slot="title">
      <span class="ant-card-title">公告栏</span>
    </div>
    <!-- 标签栏 -->
    <a-tabs v-model="activeName" @tab-click="handleClick">
      <a-tab-pane tab="全部" key="first" class="notice-all-box">
        <!-- 内容列表 -->
        <div class="all-box-2">
          <div class="all-box-2-item">
            <span class="title-item">【通知公告】关于2020年国庆节的放假通知</span>
            <span class="name-item">王小明</span>
            <span class="time-item">2020-09-10 09:51</span>
          </div>
          <div class="all-box-2-item">
            <span class="title-item">【通知公告】关于2020年国庆节的放假通知</span>
            <span class="name-item">王小明</span>
            <span class="time-item">2020-09-10 09:51</span>
          </div>
          <div class="all-box-2-item">
            <span class="title-item">【通知公告】关于2020年国庆节的放假通知</span>
            <span class="name-item">王小明</span>
            <span class="time-item">2020-09-10 09:51</span>
          </div>
          <div class="all-box-2-item">
            <span class="title-item">【通知公告】关于2020年国庆节的放假通知</span>
            <span class="name-item">王小明</span>
            <span class="time-item">2020-09-10 09:51</span>
          </div>
        </div>
      </a-tab-pane>
      <a-tab-pane tab="通知公告" key="second">配置管理</a-tab-pane>
      <a-tab-pane tab="OA速递" key="third">角色管理</a-tab-pane>
    </a-tabs>
  </a-card>
</template>

<script>
export default {
  props: ['id'],
  data() {
    return {
      panelId: this.id,
      activeName: 'first',

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
}

.anticon-close {
  cursor: pointer;
}

.ant-card-title {
  font-weight: bold;
  font-size: 1.2em;
}

.notice-all-box {
  display: flex;
}

.all-box-1 {
  flex: 0 0 25%;
}

.all-box-1 img {
  width: 100%;
  height: 150px;
}

.all-box-2 {
  flex: 1;
  font-size: 14px;
  margin-left: 5px;
}

.all-box-2 .all-box-2-item {
  display: grid;
  grid-template-columns: 4fr 1fr 2fr;
  height: 40px;
  line-height: 40px;
  cursor: pointer;
}

.all-box-2-item .name-item {
  text-align: center;
}

.all-box-2-item .title-item,
.time-item,
.name-item {
  overflow: hidden;
  white-space: nowrap;
  text-overflow: ellipsis;
}
</style>
