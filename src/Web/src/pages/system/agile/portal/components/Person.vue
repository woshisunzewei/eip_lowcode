<template>
  <a-card :hoverable="true">
    <div class="personal-message-box">
      <div class="personal-top-box">
        <img style="width: 70px; height: 70px; border-radius: 50%;margin-right: 10px; align-self: center; "
          v-real-img="user.headImage" />
        <div class="personal-content-box">
          <span class="user-title">{{ user.name }}</span>
          <span>{{ user.organizationName }}</span>
        </div>
      </div>
      <div class="btn-box">
        <span class="exitBtn">个人中心</span>
      </div>
    </div>
  </a-card>
</template>

<script>
import { mapMutations, mapGetters } from "vuex";
export default {
  props: ['id'],
  data() {
    return {
      panelId: this.id,
    }
  },
  computed: {
    ...mapGetters("account", ["user"]),
  },
  methods: {

  }
}
</script>

<style>
.ant-card {
  height: 100%;
}

.personal-message-box {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  height: 100%;
}

.personal-message-box .personal-top-box {
  display: flex;
  align-items: center;
  margin-bottom: 20px;
}

.personal-message-box .personal-content-box {
  display: flex;
  margin-left: 10px;
  flex-direction: column;
}

.personal-message-box .user-title {
  margin-bottom: 10px;
  font-size: 1.5em;
  font-weight: bold;
}

.btn-box {
  display: flex;
  justify-content: center;
  width: 100%;
}

.btn-box .person-btn,
.edit-btn {
  flex: 0 0 50%;
  border: 1px solid #d7ddeb;
  border-radius: 4px;
  padding: 10px 0;
  cursor: pointer;
  display: flex;
  justify-content: center;
  align-items: center;
  font-size: 0.9em;
}

.btn-box .person-btn {
  margin-right: 5px;
}

.btn-box .edit-btn {
  margin-left: 5px;
}

.panelBg {
  background-color: #d3d6dc !important;
  border: none !important;
}

.exitBtn {
  border: 1px solid #d7ddeb;
  border-radius: 4px;
  text-align: center;
  padding: 10px 50px;
  cursor: pointer;
  background-color: #fff;
}
</style>
