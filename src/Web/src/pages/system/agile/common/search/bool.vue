<template>
  <a-drawer width="900px" :destroyOnClose="true" :visible="visible" :bodyStyle="{ padding: '1px' }"
    :zIndex="style.zIndex" title="是否配置" @close="cancel">
    <div class="eip-drawer-body beauty-scroll">
      <a-form-model ref="form" :label-col="col.labelCol" :wrapper-col="col.wrapperCol">
        <a-form-model-item label="'是'显示文本">
          <a-input v-model="options.yes" placeholder="请输入'是'显示文本">
          </a-input>
        </a-form-model-item>
        <a-form-model-item label="'否'显示文本">
          <a-input v-model="options.no" placeholder="请输入'否'显示文本">
          </a-input>
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
  name: "search-bool",
  components: {},
  data() {
    return {
      col: {
        labelCol: { span: 4 },
        wrapperCol: { span: 20 },
      },
      style: {
        zIndex: 1030,
      },

      options: {
        yes: '是',
        no: '否'
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
          yes: '是',
          no: '否'
        };
      }
    },
  },
};
</script>