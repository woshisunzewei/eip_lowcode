<template>
  <a-form-model :label-col="labelCol" :wrapper-col="wrapperCol">
    <a-divider orientation="left"> 表格样式 </a-divider>
    <a-form-model-item label="行高">
      <a-input-number
        v-model="config.rowHeight"
        :min="20"
        :formatter="(value) => `${value}px`"
        :parser="(value) => value.replace('px', '')"
      />
    </a-form-model-item>
    <a-form-model-item label="高亮列">
      <a-switch v-model="config.columnIsCurrent" />
    </a-form-model-item>
    <a-form-model-item label="高亮行">
      <a-switch v-model="config.rowIsCurrent" />
    </a-form-model-item>
    <a-form-model-item label="斑马纹">
      <a-switch v-model="config.stripe" />
    </a-form-model-item>
    <a-form-model-item label="选择">
      <a-radio-group v-model="config.select">
        <a-radio value="checkbox"> 多选 </a-radio>
        <a-radio value="radio"> 单选 </a-radio>
        <a-radio value="null"> 无 </a-radio>
      </a-radio-group>
    </a-form-model-item>
    <a-form-model-item label="分页">
      <a-switch v-model="config.paging" />
    </a-form-model-item>

    <a-form-model-item label="边框">
      <a-radio-group v-model="config.border" button-style="solid">
        <a-radio :value="'default'"> 默认 </a-radio>
        <a-radio :value="'full'"> 完整 </a-radio>
        <a-radio :value="'outer'"> 外边框 </a-radio>
        <a-radio :value="'inner'"> 内边框 </a-radio>
        <a-radio :value="'none'"> 无边框 </a-radio>
      </a-radio-group>
    </a-form-model-item>
    <a-divider orientation="left"> 筛选设置 </a-divider>
    <a-form-model-item label="筛选字段">
      <a-select
        allow-clear
        placeholder="请选择查询字段"
        v-model="config.filter"
        style="width: 180px"
      >
        <a-select-option
          v-for="(item, i) in columns.filter(
            (f) =>
              (f.isShow || f.isSearch) &&
              [
                'TreeSelect',
                'Organization',
                'User',
                'CorrelationRecord',
                'District',
                'Dictionary',
                'Switch',
              ].includes(f.format)
          )"
          :key="i"
          :value="item.name"
          ><label v-if="!item.description">{{ item.name }}</label>
          {{ item.description }}</a-select-option
        >
      </a-select>
    </a-form-model-item>
    <a-form-model-item label="筛选字段比例" v-if="config.filter">
      <a-input-number v-model="config.filterWidth"></a-input-number>
    </a-form-model-item>

    <a-form-model-item label="多选" v-if="config.filter">
      <a-switch v-model="config.filterMultiple" />
    </a-form-model-item>
    <a-form-model-item
      label="展示连接线"
      v-if="
        config.filter &&
        columns.filter((f) => f.name == config.filter).length > 0 &&
        [
          'TreeSelect',
          'Organization',
          'User',
          'District',
          'Dictionary',
        ].includes(columns.filter((f) => f.name == config.filter)[0].format)
      "
    >
      <a-switch v-model="config.filterShowLine" />
    </a-form-model-item>
    <a-form-model-item
      label="展开所有"
      v-if="
        config.filter &&
        columns.filter((f) => f.name == config.filter).length > 0 &&
        [
          'TreeSelect',
          'Organization',
          'User',
          'District',
          'Dictionary',
        ].includes(columns.filter((f) => f.name == config.filter)[0].format)
      "
    >
      <a-switch v-model="config.filterExpandAll" />
    </a-form-model-item>

    <a-form-model-item label="子表布局">
      <a-radio-group v-model="config.subtableLayout" button-style="solid">
        <a-radio :value="'tree'"> 折叠 </a-radio>
        <a-radio :value="'topBottom'"> 上下 </a-radio>
        <a-radio :value="'leftRight'"> 左右 </a-radio>
      </a-radio-group>
    </a-form-model-item>

    <a-form-model-item label="左/上比例" v-if="config.subtableLayout != 'tree'">
      <a-input-number
        v-model="config.subtableLayoutPercentage"
      ></a-input-number>
    </a-form-model-item>
    <a-divider orientation="left"> 树级显示 </a-divider>
    <a-form-model-item label="树级显示">
      <a-switch v-model="config.showTree" />
    </a-form-model-item>
    <a-form-model-item label="树连接线" v-if="config.showTree">
      <a-switch v-model="config.showTreeLine" />
    </a-form-model-item>
    <a-form-model-item label="展开所有" v-if="config.showTree">
      <a-switch v-model="config.showTreeExpandAll" />
    </a-form-model-item>
    <a-form-model-item label="同节点展开一个" v-if="config.showTree">
      <a-switch v-model="config.showTreeAccordion" />
    </a-form-model-item>
    <a-form-model-item label="子级字段" v-if="config.showTree">
      <a-select
        placeholder="请选择查询字段"
        v-model="config.showTreeSon"
        style="width: 180px"
      >
        <a-select-option
          v-for="(item, i) in columns.filter((f) => f.isShow || f.isSearch)"
          :key="i"
          :value="item.name"
          ><label v-if="!item.description">{{ item.name }}</label>
          {{ item.description }}</a-select-option
        >
      </a-select>
    </a-form-model-item>
    <a-form-model-item label="父级字段" v-if="config.showTree">
      <a-select
        placeholder="请选择查询字段"
        v-model="config.showTreeParent"
        style="width: 180px"
      >
        <a-select-option
          v-for="(item, i) in columns.filter(
            (f) => (f.isShow || f.isSearch) && ['TreeSelect'].includes(f.format)
          )"
          :key="i"
          :value="item.name"
          ><label v-if="!item.description">{{ item.name }}</label>
          {{ item.description }}</a-select-option
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
    };
  },
  props: {
    columns: {
      type: Array,
    },
    config: {
      type: Object,
      default: function () {
        return this.$utils.clone(this.eipTableConfig, true).style;
      },
    },
  },
  methods: {},
};
</script>
