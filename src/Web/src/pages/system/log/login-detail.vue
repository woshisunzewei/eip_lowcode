<template>
  <a-drawer title="操作日志详情" placement="right" :closable="true" :width="1080" :bodyStyle="{ padding: '1px' }"
    :visible="visible" @close="close" :destroyOnClose="true">
    <a-descriptions class="eip-descriptions" bordered size="small" :column="{ xxl: 1 }">
      <a-descriptions-item label="登录时间" style="width: 200px">
        {{ data.createTime }}
      </a-descriptions-item>
      <a-descriptions-item label="登录名">
        {{ data.createUserCode }}
      </a-descriptions-item>
      <a-descriptions-item label="名称">
        {{ data.createUserName }}
      </a-descriptions-item>
      <a-descriptions-item label="退出时间">
        {{ data.loginOutTime }}
      </a-descriptions-item>
      <a-descriptions-item label="停留时间(分钟)">
        {{ data.standingTime }}
      </a-descriptions-item>
      <a-descriptions-item label="客户端IP">
        {{ data.remoteIp }}
      </a-descriptions-item>
      <a-descriptions-item label="归属地址">
        {{ data.remoteIpAddress }}
      </a-descriptions-item>
      <a-descriptions-item label="浏览器代理">
        {{ data.userAgent }}
      </a-descriptions-item>
      <a-descriptions-item label="操作系统">
        {{ data.osInfo }}
      </a-descriptions-item>
      <a-descriptions-item label="浏览器">
        {{ data.browser }}
      </a-descriptions-item>
    </a-descriptions>
  </a-drawer>
</template>
<script>
export default {
  name: "detail",
  props: {
    data: {
      type: Object,
    },
    visible: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {};
  },
  methods: {
    close() {
      this.$emit("update:visible", false);
    },
  },
};
</script>
<style scoped>
.eip-descriptions /deep/ .ant-descriptions-item-colon {
  width: 140px !important;
}
</style>
