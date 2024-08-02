<template>
  <a-drawer width="100%" placement="right" :visible="drawer.visible" :destroyOnClose="true"
    :bodyStyle="drawer.bodyStyle" @close="cancel">

  </a-drawer>
</template>

<script>

export default {
  name: "businessdatadesigner",

  data() {
    return {

    };
  },

  watch: {
    visible(val) {
      this.drawer.visible = val;
    },
  },
  props: {
    visible: {
      type: Boolean,
      default: false,
    },

    title: {
      type: String,
    },
  },

  methods: {

  },
};
</script>