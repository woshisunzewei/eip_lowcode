<template>
  <div>
    <a-cascader :disabled="disabled" v-model="data" :options="options" :placeholder="record.options.placeholder"
      @change="onChange" />
  </div>
</template>
<script>
export default {
  props: ["value", "record", "disabled"],
  data() {
    return {
      options: [],
      data: []
    };
  }, watch: {
    value(value) {
      if (value) {
        this.data = value
      }
    },
    'record.options.options': function (value) {
      this.options = value;
    }
  },
  created() {

  },
  methods: {
    onChange(value) {
      this.$emit("change", this.data);
    },
  },
};
</script>
<style lang="less" scoped>
.eip-user-picker {
  cursor: pointer;
}

.eip-user-picker .eip-user-picker-arrow {
  font-size: 12px;
}

.eip-text-placeholder {
  color: #bfbfbf;
}

.eip-user-picker:hover .eip-user-picker-arrow .anticon-close-circle+.anticon-down,
.eip-user-picker:not(:hover) .eip-user-picker-arrow .anticon-close-circle {
  display: none;
}

.eip-user-picker-popper {
  width: 100%;
  max-width: 90vw;
  padding: 0;
}

.eip-user-picker-popper .ant-tabs-tab {
  padding: 10px 0 !important;
  margin: 0 12px !important;
}

.eip-user-picker-popper .ant-card-body {
  padding: 0;
}

.anticon-close-circle:hover {
  color: rgba(0, 0, 0, 0.45);
}
</style>
