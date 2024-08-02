<template>
  <a-drawer width="95%" :title="title" :visible="visible" :bodyStyle="{ padding: '1px' }" @close="cancel"
    :destroyOnClose="true">
    <list ref="list" :menuId="menuId"></list>
  </a-drawer>
</template>

<script>
import list from "@/pages/system/agile/print/list";

export default {
  name: "print-index",
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
    menuId: {
      type: String,
    },
    title: {
      type: String,
    },
  },
  mounted() {

  },
  methods: {
    /**
        * 取消
        */
    cancel() {
      this.$emit("update:visible", false);
    },

  },
};
</script>

<style lang="less" scoped></style>
