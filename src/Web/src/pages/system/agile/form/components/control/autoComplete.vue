<template>
  <div>
    <a-auto-complete :allowClear="record.options.clearable" v-model="data" @change="onChange" :data-source="dataSource"
      :filter-option="filterOption">
      <a-input :addonBefore="record.options.addonBefore" :addonAfter="record.options.addonAfter"
        :placeholder="record.options.placeholder" />
    </a-auto-complete>
  </div>
</template>
<script>

export default {
  name: "autoComplete",
  components: {

  },
  data() {
    return {
      data: null,
      dataSource: [],
    };
  },
  props: ["value", "record", "disabled"],
  watch: {
    value(value) {
      if (value) {
        this.data = value
      }
    },
    'record.options.options': function (value) {
      this.dataSource = value;
    }
  },
  created() {
  },
  methods: {
    filterOption(input, option) {
      return (
        option.componentOptions.children[0].text.toUpperCase().indexOf(input.toUpperCase()) >= 0
      );
    },

    onChange(value) {
      this.$emit("change", value);
    },
  },
};
</script>