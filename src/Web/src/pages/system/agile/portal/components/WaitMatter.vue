<template>
  <a-card :hoverable="true">
    <div slot="title">
      <span class="ant-card-title">待办事项</span>
    </div>
    <div class="wait-matter-box">
      <div class="left-box">
        <span class="wait-total-title">待办总数</span>
        <span class="wait-total-num">56</span>
      </div>
      <div class="right-box">
        <div class="right-box-item">
          <a-icon type="book" class="right-box-icon-1"></a-icon>
          <div class="item-box">
            <span>待办公文</span>
            <span>10</span>
          </div>
        </div>
        <div class="right-box-item">
          <a-icon type="book" class="right-box-icon-2"></a-icon>
          <div class="item-box">
            <span>待办流程</span>
            <span>10</span>
          </div>
        </div>
        <div class="right-box-item">
          <a-icon type="book" class="right-box-icon-3"></a-icon>
          <div class="item-box">
            <span>待办执行任务</span>
            <span>10</span>
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

.wait-matter-box {
  display: grid;
  grid-template-columns: 1fr 3fr;
}

/* 左区域 */
.wait-matter-box .left-box {
  display: flex;
  flex-direction: column;
  border-right: 1px solid #D7DDEB;
  align-items: center;
}

.wait-matter-box .left-box .wait-total-title {
  font-size: 18px;
  margin-bottom: 10px;
}

.wait-matter-box .left-box .wait-total-num {
  font-size: 36px;
  font-weight: bold;
}

/* 右区域 */
.wait-matter-box .right-box {
  display: grid;
  grid-template-columns: repeat(3, 33.3%);
}

.wait-matter-box .right-box .right-box-item {
  display: flex;
  justify-content: center;
  align-items: center;
  cursor: pointer;
}

.wait-matter-box .right-box-item .item-box {
  display: flex;
  flex-direction: column;
  margin-left: 10px;
}

.item-box span:nth-child(1) {
  font-size: 14px;
  margin-bottom: 5px;
}

.item-box span:nth-child(2) {
  font-size: 24px;
  font-weight: bold;
}

.wait-matter-box .right-box i {
  background-color: #F2F4F7;
  font-size: 1.5em;
  border-radius: 50%;
  height: 40px;
  width: 40px;
  line-height: 45px;
  text-align: center;
}

.right-box-item .right-box-icon-1 {
  color: #FF394C;
}

.right-box-item .right-box-icon-2 {
  color: #396AFF;
}

.right-box-item .right-box-icon-3 {
  color: #39A0FF;
}
</style>
