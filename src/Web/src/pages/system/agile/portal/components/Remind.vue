<template>
  <a-card :hoverable="true">
    <div slot="title">
      <span class="ant-card-title">督办提醒</span>
    </div>
    <div class="remind-box">
      <div class="remind-box-item">
        <div class="remind-content-box">
          <span class="remind-title">未超期</span>
          <span class="remind-num">8</span>
        </div>
        <a-progress class="line-progress" strokeColor="#E4E7ED" :show-text="false" :percent="30"
          :stroke-width="8"></a-progress>
      </div>
      <div class="remind-box-item">
        <div class="remind-content-box">
          <span class="remind-title">超期1天</span>
          <span class="remind-num">2</span>
        </div>
        <a-progress class="line-progress" strokeColor="#396AFF" :show-text="false" :percent="20"
          :stroke-width="8"></a-progress>
      </div>
      <div class="remind-box-item">
        <div class="remind-content-box">
          <span class="remind-title">超期2天</span>
          <span class="remind-num">4</span>
        </div>
        <a-progress class="line-progress" strokeColor="#FFB339;" :show-text="false" :percent="30"
          :stroke-width="8"></a-progress>
      </div>
      <div class="remind-box-item">
        <div class="remind-content-box">
          <span class="remind-title">超期3天</span>
          <span class="remind-num">6</span>
        </div>
        <a-progress class="line-progress" strokeColor="#FF394C" :show-text="false" :percent="60"
          :stroke-width="8"></a-progress>
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
  font-size: 1.2em;
  cursor: pointer;
}

.ant-card-title {
  font-weight: bold;
  font-size: 1.2em;
}

.remind-box {
  display: grid;
  grid-template-columns: repeat(2, 50%);
}

.remind-box .remind-box-item {
  margin-bottom: 15px;
}

.remind-box-item .line-progress {
  width: 80%;
}

.remind-box-item .remind-content-box {
  display: flex;
  align-items: center;
  margin-bottom: 5px;
}

.remind-box-item .remind-title {
  font-size: 14px;
  color: #8C8C8C;
}

.remind-box-item .remind-num {
  font-size: 18px;
  font-weight: bold;
  color: rgba(0, 0, 0, 0.85);
  margin-left: 10px;
}
</style>
