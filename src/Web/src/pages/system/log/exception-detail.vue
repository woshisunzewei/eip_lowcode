<template>
  <a-drawer title="异常日志详情" placement="right" :closable="true" :width="1080" :bodyStyle="{ padding: '1px' }"
    :visible="visible" @close="close" :destroyOnClose="true">
    <a-spin :spinning="spinning">
      <a-descriptions bordered class="eip-descriptions" size="small" :column="1">
        <a-descriptions-item label="错误时间">{{
          log.createTime
        }}</a-descriptions-item>
        <a-descriptions-item label="错误信息">{{
          log.message
        }}</a-descriptions-item>
        <a-descriptions-item label="请求Url">{{
          log.requestUrl
        }}</a-descriptions-item>
        <a-descriptions-item label="内部信息">{{
          log.innerException
        }}</a-descriptions-item>
        <a-descriptions-item label="登录名">{{
          log.createUserCode
        }}</a-descriptions-item>
        <a-descriptions-item label="名称">{{
          log.createUserName
        }}</a-descriptions-item>
        <a-descriptions-item label="请求方式">{{
          log.httpMethod
        }}</a-descriptions-item>
        <a-descriptions-item label="请求数据">{{
          log.requestData
        }}</a-descriptions-item>
        <a-descriptions-item label="客户端IP">
          {{ log.remoteIp }}
        </a-descriptions-item>
        <a-descriptions-item label="归属地址">
          {{ log.remoteIpAddress }}
        </a-descriptions-item>
        <a-descriptions-item label="浏览器代理">
          {{ log.userAgent }}
        </a-descriptions-item>
        <a-descriptions-item label="操作系统">
          {{ log.osInfo }}
        </a-descriptions-item>
        <a-descriptions-item label="浏览器">
          {{ log.browser }}
        </a-descriptions-item>
        <a-descriptions-item label="堆栈信息">{{
          log.stackTrace
        }}</a-descriptions-item>
      </a-descriptions>
    </a-spin>
  </a-drawer>
</template>
<script>
import { findById } from "@/services/system/log/exception-detail";
export default {
  name: "detail",
  data() {
    return { log: {}, spinning: false };
  },
  props: {
    visible: {
      type: Boolean,
      default: false,
    },
    exceptionLogId: {
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
      findById(this.exceptionLogId).then(function (result) {
        that.log = result.data;
        that.spinning = false;
      });
    },
  },
};
</script>
