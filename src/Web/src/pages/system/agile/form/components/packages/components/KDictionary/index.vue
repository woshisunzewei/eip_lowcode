<template>
  <div>
    <a-form-item label="字典">
      <a-tree-select placeholder="请选择上级" allow-clear v-model="value.dictionaryId" show-search style="width: 100%"
        treeNodeFilterProp="title" :tree-data="data" :dropdown-style="{ maxHeight: '400px', overflow: 'auto' }"
        @select="treeSelect">
      </a-tree-select>
    </a-form-item>
    <a-form-model-item label="层级">
      <a-select v-model="value.dictionaryLevel">
        <a-select-option :value="1">一级</a-select-option>
        <a-select-option :value="2">多级</a-select-option>
      </a-select>
    </a-form-model-item>
    <a-form-model-item label="显示形式" v-if="value.dictionaryLevel == 1">
      <a-select v-model="value.dictionaryShowType">
        <a-select-option :value="1">下拉</a-select-option>
        <a-select-option :value="2">单选框</a-select-option>
        <a-select-option :value="3">多选框</a-select-option>
      </a-select>
    </a-form-model-item>
  </div>
</template>

<script>
import {
  dictionaryQuery,
} from "@/services/system/dictionary/list";
/*
 * author 孙泽伟
 * date 2024-05-21
 * description KDictionary
 */
export default {
  name: "KDictionary",
  components: {},
  data() {
    return {
      data: []
    };
  },
  props: {
    value: {
      type: Object,
      required: true,
    }
  },
  created() {
    this.init();
  },
  methods: {
    /**
     * 初始化
     */
    init() {
      let that = this;
      dictionaryQuery().then((result) => {
        that.data = result.data;
      });
    },

    /**
     * 选择
     */
    treeSelect(keys, event) {

    },
  },
};
</script>

<style lang="less" scoped>
.ant-form-item {
  margin-bottom: 0;
}
</style>
