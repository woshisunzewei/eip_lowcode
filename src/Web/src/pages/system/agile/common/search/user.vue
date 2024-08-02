<template>
  <a-drawer width="900px" :destroyOnClose="true" :visible="visible" :bodyStyle="{ padding: '1px' }" title="人员选择配置"
    @close="cancel">
    <div class="eip-drawer-body beauty-scroll">
      <a-form-model ref="form" :label-col="col.labelCol" :wrapper-col="col.wrapperCol">
        <a-form-model-item label="选项">
          <a-checkbox v-model="options.multiple"> 多选 </a-checkbox>
        </a-form-model-item>

      </a-form-model>
    </div>
    <div class="eip-drawer-toolbar">
      <a-space>
        <a-button key="back" @click="cancel"> 取消 </a-button>
        <a-button key="submit" @click="ok" type="primary"> 确定 </a-button>
      </a-space>
    </div>

  </a-drawer>
</template>

<script>
export default {
  name: "search-user",
  components: {
  },
  data() {
    return {
      col: {
        labelCol: { span: 4 },
        wrapperCol: { span: 20 },
      },
      style: {
        zIndex: 1030,
      },
      height: window.innerHeight - 322,

      //下拉查询参数配置
      options: {
        multiple: true, //多选
      },
    };
  },

  props: {
    visible: {
      type: Boolean,
      default: false,
    },
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
      this.$emit("ok", this.options);
    },


    /**
     * 初始化
     */
    init(options) {
      if (options) {
        this.options = options;
      } else {
        this.options = {
          multiple: true,
        };
      }
    },
  },
};
</script>