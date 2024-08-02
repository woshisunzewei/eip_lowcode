<template>
  <a-drawer width="900px" :destroyOnClose="true" :visible="visible" :bodyStyle="{ padding: '1px' }"
    :zIndex="style.zIndex" title="时间配置" @close="cancel">
    <div class="eip-drawer-body beauty-scroll">
      <a-form-model ref="form" :label-col="col.labelCol" :wrapper-col="col.wrapperCol">
        <a-form-model-item label="格式">
          <a-radio-group name="type" default-value="" v-model="config.format">
            <a-radio value="YYYY">
              年
            </a-radio>
            <a-radio value="YYYY-MM">
              年月
            </a-radio>
            <a-radio value="YYYY-MM-DD">
              年月日
            </a-radio>
            <a-radio value="YYYY-MM-DD HH">
              年月日时
            </a-radio>
            <a-radio value="YYYY-MM-DD HH:mm">
              年月日时分
            </a-radio>
            <a-radio value="YYYY-MM-DD HH:mm:ss">
              年月日时分秒
            </a-radio>
            <a-radio value="">
              自定义
            </a-radio>
          </a-radio-group>
        </a-form-model-item>

        <a-form-model-item label="自定义格式" v-if="config.format == ''">
          <a-input placeholder="请输入自定义格式 如 YYYY-MM-DD HH:mm:ss" v-model="config.formatCustomer">
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
  name: "search-datetime",
  data() {
    return {
      col: {
        labelCol: { span: 4 },
        wrapperCol: { span: 20 },
      },
      style: {
        zIndex: 1030,
      },

      config: {
        format: 'YYYY-MM-DD HH:mm:ss',
        formatCustomer: ''
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
      this.$emit("ok", this.config);
    },

    /**
     * 初始化
     */
    init(config) {
      if (config) {
        this.config = config;
      } else {
        this.config = {
          format: 'YYYY-MM-DD HH:mm:ss',
          formatCustomer: ''
        };
      }
    },
  },
};
</script>