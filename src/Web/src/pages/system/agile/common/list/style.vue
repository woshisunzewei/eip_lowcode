<template>
  <a-drawer width="800px" :visible="visible" :destroyOnClose="true" :zIndex="style.zIndex"
    :body-style="{ padding: '10px' }" :title="title" @close="cancel">
    <vxe-toolbar ref="styleToolbar" :refresh="{ query: reset }">
      <template v-slot:buttons>
        <a-space>
          <a-button @click="yesNo" type="primary"> 是否 </a-button>
          <a-button @click="verifyStatus" type="primary"> 审核状态 </a-button>
          <a-button @click="publicStatus" type="primary"> 发布状态 </a-button>
          <a-button @click="taskStatus" type="primary"> 流程任务状态 </a-button>
        </a-space>
      </template>
    </vxe-toolbar>
    <vxe-table :loading="loading" row-key ref="styleTable" :data="styles" :height="height">
      <template #loading>
        <a-spin></a-spin>
      </template>
      <template #empty>
        <eip-empty />
      </template>
      <vxe-column title="新增" width="54px" align="center">
        <template #header>
          <a-button @click="add" size="small" type="primary">
            <a-icon type="plus" />
          </a-button>
        </template>
        <template v-slot="{ row }">
          <a-popconfirm title="确定删除配置?" ok-text="确定" cancel-text="取消" @confirm="del(row)">
            <a-button @click.stop="" size="small" type="danger">
              <a-icon type="delete"></a-icon>
            </a-button>
          </a-popconfirm>
        </template>
      </vxe-column>

      <vxe-column title="值" width="100px">
        <template v-slot="{ row }">
          <a-input allow-clear placeholder="值" v-model="row.value" />
        </template>
      </vxe-column>
      <vxe-column title="预设值颜色" width="120px">
        <template v-slot="{ row }">
          <a-select allow-clear placeholder="预设值颜色" v-model="row.presets" style="width: 100%">
            <a-select-option value="pink">pink</a-select-option>
            <a-select-option value="red">red</a-select-option>
            <a-select-option value="orange">orange</a-select-option>
            <a-select-option value="green">green</a-select-option>
            <a-select-option value="cyan">cyan</a-select-option>
            <a-select-option value="blue">blue</a-select-option>
            <a-select-option value="purple">purple</a-select-option>
          </a-select>
        </template>
      </vxe-column>
      <vxe-column title="自定义" width="190px" align="center">
        <template v-slot="{ row }">
          <eip-color :value="row.custom" @change="(value) => { row.custom = value }"></eip-color>
        </template>
      </vxe-column>
      <vxe-column title="内容" width="150px">
        <template v-slot="{ row }">
          <a-input allow-clear placeholder="显示内容" v-model="row.content" />
        </template>
      </vxe-column>
      <vxe-column title="预览" align="center" width="130px">
        <template v-slot="{ row }">
          <a-tag v-if="row.presets" :color="row.presets">{{
            row.content
          }}</a-tag>
          <a-tag v-else :color="row.custom">{{ row.content }}</a-tag>
        </template>
      </vxe-column>
    </vxe-table>

    <div class="eip-drawer-toolbar">
      <a-space>
        <a-button key="back" @click="cancel"><a-icon type="close" />取消</a-button>
        <a-button key="submit" @click="ok" type="primary"><a-icon type="save" />提交</a-button>
      </a-space>
    </div>
  </a-drawer>
</template>

<script>

import {
  businessDataFormSource,
} from "@/services/system/agile/run/edit";
import {
  dictionaryParentId
} from "@/services/system/agile/run/edit";
export default {
  name: "list-style",
  data() {
    return {
      style: {
        zIndex: 1020,
      },
      height: window.innerHeight - 182,
      loading: false,
      styles: [],
      title: ''
    };
  },

  props: {
    visible: {
      type: Boolean,
      default: false,
    },
    config: {
      type: Object,
    },
  },
  created() {
    this.init();
  },

  methods: {
    /**
     * 取消
     */
    cancel() {
      this.$emit("update:visible", false);
    },

    /**
     * 保存
     */
    ok() {
      this.cancel();
      this.$emit("ok", this.styles);
    },

    /**
     * 新增
     */
    add() {
      this.styles.push({
        value: null,
        presets: undefined,
        custom: "#000",
        content: "测试内容",
      });
    },

    /**
     * 任务状态
     */
    taskStatus() {
      var styles = [
        { value: null, presets: undefined, custom: "#2db7f5", content: "待发起" },
        { value: 0, presets: undefined, custom: "#3c9cff", content: "处理中" },
        { value: 2, presets: undefined, custom: "#f9ae3d", content: "终止" },
        { value: 4, presets: undefined, custom: "#909399", content: "暂停" },
        { value: 6, presets: undefined, custom: "#f56c6c", content: "即将到期" },
        { value: 8, presets: undefined, custom: "#f56c6c", content: "拒绝" },
        { value: 10, presets: undefined, custom: "#303133", content: "撤销" },
        { value: 12, presets: undefined, custom: "#606266", content: "删除" },
        { value: 14, presets: undefined, custom: "#909193", content: "退回重填" },
        { value: 16, presets: undefined, custom: "#c8c9cc", content: "取消" },
        { value: 100, presets: undefined, custom: "#5ac725", content: "完成" },
      ];
      this.styles = styles;
    },

    /**
     * 审核状态
     */
    verifyStatus() {
      var styles = [
        { value: 1, presets: undefined, custom: "#f9ae3d", content: "审核中" },
        { value: 2, presets: undefined, custom: "#108ee9", content: "通过" },
        { value: 3, presets: undefined, custom: "#f56c6c", content: "未通过" },
      ];
      this.styles = styles;
    },
    /**
         * 发布状态
         */
    publicStatus() {
      var styles = [
        { value: 1, presets: undefined, custom: "#f9ae3d", content: "发布中" },
        { value: 2, presets: undefined, custom: "#108ee9", content: "交易完成" },
        { value: 3, presets: undefined, custom: "#f56c6c", content: "用户取消" },
        { value: 4, presets: undefined, custom: "#c8c9cc", content: "强制下架" },
        { value: 5, presets: undefined, custom: "#5ac725", content: "过期下架" },
      ];
      this.styles = styles;
    },

    /**
     * 是否
     */
    yesNo() {
      var styles = [
        { value: 1, presets: undefined, custom: '#3c9cff', content: "是" },
        { value: 0, presets: undefined, custom: '#f56c6c', content: "否" },
      ];
      this.styles = styles;
    },

    /**
     * 删除
     */
    del(row) {
      let spIndex = -1;
      this.styles.forEach((item, index) => {
        if (item._X_ROW_KEY == row._X_ROW_KEY) {
          spIndex = index;
        }
      });
      if (spIndex != -1) {
        this.styles.splice(spIndex, 1);
      }
    },

    /**
     * 初始化
     */
    async init() {
      let that = this;
      var styles = [];
      that.loading = true;
      if (that.config.style.length == 0) {
        var styleCustom = [
          { custom: "#3c9cff" },
          { custom: "#f9ae3d" },
          { custom: "#909399" },
          { custom: "#2db7f5" },
          { custom: "#f56c6c" },
          { custom: "#303133" },
          { custom: "#606266" },
          { custom: "#909193" },
          { custom: "#c8c9cc" },
          { custom: "#5ac725" },
          { custom: "#BD10E0" },
          { custom: "#50E3C2" },
          { custom: "#8B572A" },
        ];
        if (!that.config.options.dynamic) {
          if (that.config.options != null
            && typeof (that.config.options) != "undefined"
            && that.config.options.options != null
            && typeof (that.config.options.options) != "undefined") {
            that.config.options.options.forEach((item, index) => {
              var custom = styleCustom.length < index ? "#3c9cff" : styleCustom[index].custom
              styles.push({ value: item.value, presets: null, custom: custom, content: item.label })
            })
          } else if (that.config.format == "Dictionary") {
            var result = (await dictionaryParentId(that.config.options.dictionaryId));
            if (result.code === that.eipResultCode.success) {
              result.data.forEach((res, index) => {
                var custom = styleCustom.length < index + 1 ? "#3c9cff" : styleCustom[index].custom
                styles.push({ value: res.dictionaryId, presets: null, custom: custom, content: res.name })
              });
            }
          }
        } else {
          var result = await businessDataFormSource({
            dataSourceId: this.config.options.dynamicConfig.dataSourceId,
            inParams: JSON.stringify(this.config.options.dynamicConfig.inParams),
            sidx: this.config.options.dynamicConfig.sidx, //排序字段
            sord: this.config.options.dynamicConfig.sord, //排序方式
          })
          if (result.code === this.eipResultCode.success) {
            result.data.forEach((res, index) => {
              var custom = styleCustom.length < index ? "#3c9cff" : styleCustom[index].custom;
              styles.push({ value: res[that.config.options.dynamicConfig.key], presets: null, custom: custom, content: res[that.config.options.dynamicConfig.value] })
            });
          }
        }
      } else {
        styles = that.$utils.clone(this.config.style, true);
      }
      this.styles = styles;
      this.title = '字段样式配置-' + this.config.description;
      this.loading = false;
    },

    reset() {
      this.config.style = [];
      this.init();
    },
  },
};
</script>

<style lang="less" scoped>
.ant-tag {
  border-radius: 40px !important;
}
</style>
