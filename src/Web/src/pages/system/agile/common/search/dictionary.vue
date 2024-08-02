<template>
  <a-drawer width="900px" :destroyOnClose="true" :visible="visible" :bodyStyle="{ padding: '1px' }" title="查询配置"
    @close="cancel">
    <div class="eip-drawer-body beauty-scroll">
      <a-form-model ref="form" :label-col="col.labelCol" :wrapper-col="col.wrapperCol">
        <a-form-model-item label="选项">
          <a-checkbox v-model="options.multiple"> 多选 </a-checkbox>
        </a-form-model-item>
        <a-form-model-item label="字典">
          <a-tree-select placeholder="请选择上级" allow-clear v-model="options.source.dictionaryId" show-search
            style="width: 100%" treeNodeFilterProp="title" :tree-data="data"
            :dropdown-style="{ maxHeight: '400px', overflow: 'auto' }">
          </a-tree-select>
        </a-form-model-item>
        <a-form-model-item label="层级">
          <a-select v-model="options.source.dictionaryLevel">
            <a-select-option :value="1">一级</a-select-option>
            <a-select-option :value="2">多级</a-select-option>
          </a-select>
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
import {
  dictionaryQuery,
} from "@/services/system/dictionary/list";
export default {
  name: "search-dictionary",
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
      data: [],

      //下拉查询参数配置
      options: {
        multiple: true, //多选
        source: {
          dictionaryId: undefined,
          dictionaryLevel: 1, //层级,默认下一级
        },
      },
    };
  },

  props: {
    visible: {
      type: Boolean,
      default: false,
    },
  },
  created() {
    this.initDictionary();
  },
  methods: {

    /**
        * 初始化
        */
    initDictionary() {
      let that = this;
      dictionaryQuery().then((result) => {
        that.data = result.data;
      });
    },
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
      if (options && typeof (options.source) != 'undefined') {
        this.options = options;
      } else {
        this.options = {
          multiple: true,
          source: {
            dictionaryId: undefined,
            dictionaryLevel: 1, //层级,默认下一级
          },
        };
      }
    },
  },
};
</script>