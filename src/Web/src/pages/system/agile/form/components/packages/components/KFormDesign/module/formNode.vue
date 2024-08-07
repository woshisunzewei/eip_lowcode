<!--
 * @Description: 将数据通过k-form-item组件解析，生成控件
 * @Author: kcz
 * @Date: 2019-12-30 00:37:05
 * @LastEditTime: 2021-05-27 15:19:02
 * @LastEditors: kcz
 * @FilePath: \k-form-design\packages\KFormDesign\module\formNode.vue
 -->
<template>
  <div class="drag-move-box" @click.stop="$emit('handleSelectItem', record)"
    :class="{ active: record.key === selectItem.key }">
    <div class="form-item-box">
      <a-icon class="margin-left-xs hidden" v-if="record.options.hidden" type="eye-invisible" />
      <kFormItem :fromDesign="true" :formConfig="config" :record="record" />
    </div>
    <!-- <div v-if="!hideModel" class="show-key-box">
      <a-icon class="margin-left-xs" v-if="record.options.hidden" type="eye-invisible" />
      {{ record.label + (record.model ? '/' + record.model : '') }}
    </div> -->
    <div class="copy" :class="record.key === selectItem.key ? 'active' : 'unactivated'"
      @click.stop="$emit('handleCopy')">
      <a-icon type="copy" />
    </div>
    <div class="delete" :class="record.key === selectItem.key ? 'active' : 'unactivated'"
      @click.stop="$emit('handleDelete')">
      <a-icon type="delete" />
    </div>

  </div>
</template>
<script>
/*
 * author kcz
 * date 2019-11-20
 * description 通过json生成的单个表单节点
 */
import kFormItem from "../../KFormItem/index";
export default {
  props: {
    record: {
      type: Object,
      required: true
    },
    selectItem: {
      type: Object,
      default: () => { }
    },
    config: {
      type: Object,
      required: true
    },
    hideModel: {
      type: Boolean,
      default: false
    }
  },
  components: {
    kFormItem
  }
};
</script>
<style>
.hidden {
  position: absolute;
  left: 0;
  top: 5px;
  width: 30px;
  height: 30px;
  line-height: 30px;
  text-align: center;
}
</style>
