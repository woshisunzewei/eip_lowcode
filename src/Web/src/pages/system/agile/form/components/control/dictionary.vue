<template>
  <div>

    <a-select :show-search="record.options.showSearch" :mode="record.options.multiple ? 'multiple' : 'default'"
      optionFilterProp="label" :placeholder="record.options.placeholder" allow-clear
      v-if="record.options.dictionaryLevel == 1 && record.options.dictionaryShowType == 1" v-model="data"
      @change="change">
      <a-select-option v-for="(item, index) in options" :value="item.value" :key="index" :label="item.name">
        {{ item.label }}</a-select-option>
    </a-select>

    <a-checkbox-group v-model="data"
      v-if="record.options.dictionaryLevel == 1 && record.options.dictionaryShowType == 3" :options="options"
      @change="change" />

    <a-radio-group @change="change" v-model="data"
      v-if="record.options.dictionaryLevel == 1 && record.options.dictionaryShowType == 2">
      <a-radio v-for="(item, index) in options" :value="item.value" :key="index">{{ item.label }} </a-radio>
    </a-radio-group>

    <a-tree-select v-if="record.options.dictionaryLevel == 2 && record.options.multiple"
      :placeholder="record.options.placeholder" allow-clear @change="change" tree-checkable v-model="data" show-search
      style="width: 100%" treeNodeFilterProp="title" :tree-data="options"
      :dropdown-style="{ maxHeight: '400px', overflow: 'auto' }">
    </a-tree-select>

    <a-tree-select v-if="record.options.dictionaryLevel == 2 && !record.options.multiple"
      :placeholder="record.options.placeholder" allow-clear @change="change" v-model="data" show-search
      style="width: 100%" treeNodeFilterProp="title" :tree-data="options"
      :dropdown-style="{ maxHeight: '400px', overflow: 'auto' }">
    </a-tree-select>

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
  },
  watch: {
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
    change(value) {
      if (this.record.options.dictionaryShowType == 2) {
        this.$emit("change", this.data);
      } else {
        this.$emit("change", value);
      }

    },
  },
};
</script>
<style lang="less" scoped></style>
