<template>
  <a-drawer width="95%" :title="title" :visible="visible" :bodyStyle="{ padding: '1px' }" @close="cancel"
    :destroyOnClose="true">
    <list ref="list" :tableId="tableId"></list>
  </a-drawer>
</template>

<script>
import list from "@/pages/system/agile/automation/list";

export default {
  name: "automation-index",
  data() {
    return {

    };
  },
  components: { list },
  props: {
    visible: {
      type: Boolean,
      default: false,
    },
    tableId: {
      type: String,
    },
    title: {
      type: String,
    },
  },
  mounted() {
    this.init();
  },
  methods: {
    /**
        * 取消
        */
    cancel() {
      this.$emit("update:visible", false);
    },
    /**
     * 初始化
     */
    init() {

    },
  },
};
</script>

<style lang="less" scoped></style>
