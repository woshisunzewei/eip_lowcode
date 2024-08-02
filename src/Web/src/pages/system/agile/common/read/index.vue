<template>
  <a-form-model :label-col="labelCol" :wrapper-col="wrapperCol">
    <a-form-model-item label="读取缓存">
      <a-switch v-model="config.cache" />
    </a-form-model-item>

    <a-form-model-item label="自动刷新" help="每隔一段时间后自动刷新当前视图">
      <a-select v-model="config.refresh" style="width: 180px">
        <a-select-option
          v-for="(item, i) in refreshs"
          :key="i"
          :value="item.key"
        >
          {{ item.value }}</a-select-option
        >
      </a-select>
    </a-form-model-item>
  </a-form-model>
</template>
<script>
export default {
  data() {
    return {
      labelCol: { span: 2 },
      wrapperCol: { span: 14 },
      refreshs: [
        { key: "", value: "关闭" },
        { key: "5", value: "5秒" },
        { key: "30", value: "30秒" },
        { key: "60", value: "1分钟" },
        { key: "120", value: "2分钟" },
        { key: "180", value: "3分钟" },
        { key: "240", value: "4分钟" },
        { key: "300", value: "5分钟" },
      ],
    };
  },
  props: {
    config: {
      type: Object,
      default: () => {
        return {
          cache: true,
          refresh: "",
        };
      },
    },
  },
  methods: {},
};
</script>