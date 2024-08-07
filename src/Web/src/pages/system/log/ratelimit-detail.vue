<template>
  <a-drawer title="限流日志详情" :destroyOnClose="true" placement="right" :closable="true" :width="1080" :visible="visible"
    :bodyStyle="{ padding: '1px' }" @close="close">
    <a-spin :spinning="spinning">
      <a-descriptions class="eip-descriptions" bordered size="small" :column="1">
        <a-descriptions-item label="操作时间">{{
          log.createTime
        }}</a-descriptions-item>
        <a-descriptions-item label="登录名">{{
          log.createUserCode
        }}</a-descriptions-item>
        <a-descriptions-item label="操作人名称">{{
          log.createUserName
        }}</a-descriptions-item>
        <a-descriptions-item label="描述">{{ log.remark }}</a-descriptions-item>
        <a-descriptions-item label="控制器">{{
          log.controllerName
        }}</a-descriptions-item>
        <a-descriptions-item label="方法">{{
          log.actionName
        }}</a-descriptions-item>
        <a-descriptions-item label="地址">{{ log.url }}</a-descriptions-item>
        <a-descriptions-item label="请求类型">{{
          log.requestType
        }}</a-descriptions-item>
        <a-descriptions-item label="客户端IP">{{
          log.remoteIp
        }}</a-descriptions-item>
        <a-descriptions-item label="请求数据">{{
          log.requestData
        }}</a-descriptions-item>
      </a-descriptions>
    </a-spin>
  </a-drawer>
</template>
<script>
import { findById } from "@/services/system/log/ratelimit-detail";
export default {
  name: "ratelimitdetail",
  data() {
    return {
      log: {},
      spinning: false,
    };
  },
  props: {
    visible: {
      type: Boolean,
      default: false,
    },
    rateLimitLogId: {
      type: String,
    },
  },
  mounted() {
    this.find();
  },

  methods: {
    close() {
      this.$emit("update:visible", false);
    },

    /**
     * 根据Id查找
     */
    find() {
      let that = this;
      that.spinning = true;
      findById(this.rateLimitLogId).then(function (result) {
        that.log = result.data;
        that.spinning = false;
      });
    },
  },
};
</script>
<style scoped>
.eip-descriptions /deep/ .ant-descriptions-item-colon {
  width: 140px !important;
}
</style>
