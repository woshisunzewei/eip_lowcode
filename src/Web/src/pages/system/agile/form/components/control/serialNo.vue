<template>
  <div>
    <div class="customFormNull" v-if="!record.options.loadDisplay && !value"></div>
    <a-input v-else disabled v-model="value" />
  </div>
</template>

<script>
export default {
  name: "serialNo",
  data() {
    return {};
  },
  props: ["value", "record", "disabled"],
  methods: {},
};
</script>
<style lang="less" scoped>
.customFormNull {
  background: #eaeaea;
  border-radius: 3px;
  height: 6px;
  margin: 15px 0;
  width: 22px;
}
</style>