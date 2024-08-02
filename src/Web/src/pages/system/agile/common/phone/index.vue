<template>
  <a-form-model :label-col="labelCol" :wrapper-col="wrapperCol">
    <a-form-model-item label="卡片模版">
      <a-radio-group v-model="phone.style" button-style="solid">
        <a-radio :value="1"> 一行一列标题内容并排显示 </a-radio>
        <a-radio :value="2"> 一行一列标题在上内容在下 </a-radio>
        <a-radio :value="3"> 一行二列 </a-radio>
        <a-radio :value="4"> 一行三列 </a-radio>
      </a-radio-group>
    </a-form-model-item>
    <a-form-model-item label="标题">
      <a-select :allowClear="true" v-model="phone.title" placeholder="请选择显示标题">
        <a-select-option v-for="(item, i) in columns" :key="i" :value="item.name">{{ item.description }}</a-select-option>
      </a-select>
    </a-form-model-item>
    <a-form-model-item label="摘要">
      <a-select :allowClear="true" v-model="phone.subTitle" placeholder="请选择显示摘要">
        <a-select-option v-for="(item, i) in columns" :key="i" :value="item.name">{{ item.description }}</a-select-option>
      </a-select>
    </a-form-model-item>
    <a-form-model-item label="显示字段">
      <a-select :allowClear="true" v-model="phone.columns" placeholder="请选择显示字段" mode="multiple">
        <a-select-option v-for="(item, i) in columns" :key="i" :value="item.name">{{ item.description }}</a-select-option>
      </a-select></a-form-model-item>
    <a-form-model-item label="封面">
      <a-select :allowClear="true" v-model="phone.cover" placeholder="请选择封面">
        <a-select-option value="hide"> 隐藏 </a-select-option>
        <a-select-option value="show"> 显示 </a-select-option>
      </a-select>
    </a-form-model-item>
    <a-form-model-item label="显示位置">
      <a-select :allowClear="true" v-model="phone.coverPosition" placeholder="请选择封面显示位置">
        <a-select-option value="left"> 左 </a-select-option>
        <a-select-option value="right"> 右 </a-select-option>
      </a-select>
    </a-form-model-item>
    <a-form-model-item label="显示方式">
      <a-select :allowClear="true" v-model="phone.coverShowType" placeholder="请选择封面显示方式">
        <a-select-option value="fill"> 填满 </a-select-option>
        <a-select-option value="full"> 完整显示 </a-select-option>
      </a-select></a-form-model-item>
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
    phone: {
      type: Object,
      default: () => {
        return {
          style: 1, //卡片模版
          title: undefined, //标题
          subTitle: undefined, //摘要
          columns: [], //显示字段
          cover: undefined, //封面
          coverPosition: undefined, //显示位置
          coverShowType: undefined, //显示方式
        };
      },
    },
  },
  methods: {},
};
</script>
      