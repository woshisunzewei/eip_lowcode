<template>
  <a-drawer width="500px" :body-style="{ padding: '1px' }" :destroyOnClose="true" :visible="visible"
    :zIndex="style.zIndex" title="多表头配置" @close="cancel">
    <div class="eip-drawer-body beauty-scroll">
      <a-form-model ref="form" :label-col="col.labelCol" :wrapper-col="col.wrapperCol">

        <a-form-model-item ref="name" label="名称">
          <a-input allow-clear v-model="form.title" placeholder="请输入名称" />
        </a-form-model-item>

        <a-form-model-item ref="name" label="字段">
          <a-select placeholder="请选择多表头字段" v-model="form.field" mode="multiple" style="width: 100%">
            <a-select-option v-for="( item, i ) in data " :key="i" :value="item.name" :title="item.description">{{
              item.description
              }}</a-select-option>
          </a-select>
        </a-form-model-item>
      </a-form-model>
    </div>
    <div class="eip-drawer-toolbar">
      <a-space>
        <a-button @click="cancel"> 取消 </a-button>
        <a-button @click="ok" type="primary"> 确定 </a-button>
      </a-space>
    </div>
  </a-drawer>
</template>

<script>

export default {
  name: "list-header",
  data() {
    return {
      col: {
        labelCol: { span: 4 },
        wrapperCol: { span: 20 },
      },
      style: {
        zIndex: 1030,
      },

      form: {
        title: null,
        field: []
      },
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
    data: {
      type: Array,
      default: []
    }
  },
  mounted() {
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
      this.$emit("ok", this.form);
    },

    /**
     * 初始化
     */
    init() {
      if (this.config) {
        this.form = this.$utils.clone(this.config.header, true);
      } else {
        this.form = {
          title: null,
          field: []
        };
      }
    },
  },
};
</script>