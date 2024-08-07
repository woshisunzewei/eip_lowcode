<template>
  <a-drawer title="短信日志详情" placement="right" :closable="true" :width="1080" :visible="visible"
    :bodyStyle="{ padding: '1px' }" @close="close" :destroyOnClose="true">
    <a-spin :spinning="spinning">
      <a-descriptions class="eip-descriptions" bordered size="small" :column="1">
        <a-descriptions-item label="发送时间">{{
          log.createTime
        }}</a-descriptions-item>
        <a-descriptions-item label="手机号">{{
          log.phone
        }}</a-descriptions-item>
        <a-descriptions-item label="短信服务商">
          {{ log.provide }}</a-descriptions-item>
        <a-descriptions-item label="短信代码">{{ log.code }}</a-descriptions-item>
        <a-descriptions-item label="发送信息">{{
          log.message
        }}</a-descriptions-item>
        <a-descriptions-item label="请求参数">{{
          log.request
        }}</a-descriptions-item>
        <a-descriptions-item label="返回参数">{{ log.response }}</a-descriptions-item>
        <a-descriptions-item label="发送标识">
          <a-tag color="#f50" v-if="!log.isSend">
            失败
          </a-tag>
          <a-tag color="#2db7f5" v-if="log.isSend">
            成功
          </a-tag>
        </a-descriptions-item>
      </a-descriptions>
    </a-spin>
  </a-drawer>
</template>
<script>
import { findById } from "@/services/system/log/sms-detail";
export default {
  name: "smsdetail",
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
    smsLogId: {
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
      findById(this.smsLogId).then(function (result) {
        that.log = result.data;
        that.spinning = false;
      });
    },
  },
};
</script>
<style scoped>
.eip-descriptions /deep/ .ant-descriptions-item-label {
  width: 120px !important;
}

.eip-descriptions /deep/ .ant-descriptions-item-content {
  word-wrap: break-word;
  word-break: break-all;
}
</style>
