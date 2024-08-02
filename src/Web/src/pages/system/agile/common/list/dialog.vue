<template>
  <a-drawer width="500px" :body-style="{ padding: '1px' }" :visible="visible" :destroyOnClose="true"
    :zIndex="style.zIndex" title="弹出配置" @close="cancel">
    <div class="eip-drawer-body beauty-scroll">
      <a-form-model ref="form" :label-col="col.labelCol" :wrapper-col="col.wrapperCol">

        <a-form-model-item ref="name" label="高度">
          <a-input allow-clear v-model="form.height" placeholder="请输入高度" type="number">
            <a-select slot="addonAfter" v-model="form.heightUnit">
              <a-select-option value="px">px</a-select-option>
              <a-select-option value="%"> % </a-select-option>
            </a-select>
          </a-input>
        </a-form-model-item>

        <a-form-model-item ref="name" label="宽度">
          <a-input allow-clear v-model="form.width" placeholder="请输入宽度" type="number">
            <a-select slot="addonAfter" v-model="form.widthUnit">
              <a-select-option value="px">px</a-select-option>
              <a-select-option value="%"> % </a-select-option>
            </a-select>
          </a-input>
        </a-form-model-item>
        <a-form-model-item ref="name" label="弹出表">
          <a-select show-search optionFilterProp="label" placeholder="请选择弹出表" v-model="form.id"
            style="width: calc(100%)">
            <a-select-option v-for="( item, i ) in data " :key="i" :value="item.configId"
              :label="(item.remark ? item.remark + '-' : '') + item.name"
              :title="(item.remark ? item.remark + '-' : '') + item.name">{{ (item.remark ? item.remark + '-' : '') }}
              {{
                item.name
              }}</a-select-option>
          </a-select>
        </a-form-model-item>
        <a-form-model-item label="表达式" prop="condition">
          <a-textarea allow-clear style="height: 200px" v-model="form.condition"
            placeholder="字段使用{xxx}表示。如 id='{KeyId}'' and name='{Name}'" />
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
  name: "list-dialog",
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
        id: undefined,
        condition: null,
        height: 200,
        heightUnit: ["px"],
        width: 300,
        widthUnit: ["px"],
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
        this.form = this.$utils.clone(this.config.dialog, true);
      } else {
        this.form = {
          id: undefined,
          condition: null,
          height: 200,
          heightUnit: ["px"],
          width: 300,
          widthUnit: ["px"],
        };
      }
    },
  },
};
</script>