<template>
  <a-drawer width="900px" :destroyOnClose="true" :visible="visible" :bodyStyle="{ padding: '1px' }" title="下拉框配置"
    @close="cancel">
    <div class="eip-drawer-body beauty-scroll">
      <a-form-model ref="form" :label-col="col.labelCol" :wrapper-col="col.wrapperCol">
        <a-form-model-item label="选项">
          <a-checkbox v-model="options.multiple"> 多选 </a-checkbox>
        </a-form-model-item>
        <a-form-model-item label="关联工作表">
          <a-select optionFilterProp="label" show-search v-model="options.source.table" placeholder="请选择一个工作表"
            @change="tableChange">
            <a-select-option v-for="(item, index) in tables" :key="index" :value="item.configId" :label="item.name">{{
              item.name
            }}</a-select-option>
          </a-select>
        </a-form-model-item>

        <a-form-model-item label="键">
          <a-select optionFilterProp="label" show-search placeholder="请选择类型" v-model="options.source.key">
            <a-select-option v-for="(item, i) in columns" :key="i" :value="item.name"
              :label="item.name + item.description">
              {{ item.name }} {{ item.description }}
            </a-select-option>
          </a-select>
        </a-form-model-item>
        <a-form-model-item label="值">
          <a-select optionFilterProp="label" show-search placeholder="请选择类型" v-model="options.source.value">
            <a-select-option v-for="(item, i) in columns" :key="i" :value="item.name"
              :label="item.name + item.description">
              {{ item.name }} {{ item.description }}
            </a-select-option>
          </a-select>
        </a-form-model-item>
        <a-form-model-item label="排序字段">
          <a-select optionFilterProp="label" show-search placeholder="请选择类型" v-model="options.source.sidx">
            <a-select-option v-for="(item, i) in columns" :key="i" :value="item.name"
              :label="item.name + item.description">
              {{ item.name }} {{ item.description }}
            </a-select-option>
          </a-select>
        </a-form-model-item>
        <a-form-model-item label="排序方式">
          <a-select show-search placeholder="请选择类型" v-model="options.source.sord">
            <a-select-option value="DESC"> 降序 </a-select-option>
            <a-select-option value="ASC"> 升序 </a-select-option>
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
import { table } from "@/services/system/agile/form/designer";
export default {
  name: "search-select",
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
      columns: [],
      height: window.innerHeight - 322,
      tables: [],
      //下拉查询参数配置
      options: {
        multiple: true, //多选
        source: {
          table: undefined,//数据源类型
          value: [],//值
          sidx: undefined,//排序字段
          sord: "DESC",//排序方式
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
    this.initTable();
  },
  methods: {
    /**
        * 初始化所有的工作表
        */
    initTable() {
      let that = this;
      table().then(function (result) {
        that.tables = result.data;
      });
    },
    /**
        * 工作表切换
        */
    tableChange(e) {
      let that = this;
      let data = this.tables.filter(f => f.configId == e);
      if (data.length > 0 && data[0].columnJson) {

        let columns = JSON.parse(data[0].columnJson)

        columns.forEach(item => {
          that.columns.push({
            name: item.model,
            description: item.label,
          })
        })

        that.eipSystemColumns.forEach(systemColumn => {
          that.columns.push({
            name: systemColumn.name,
            description: systemColumn.description,
          })
        })
      }
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
        if (options.source.table) {
          this.tableChange(options.source.table)
        }

      } else {
        this.options = {
          multiple: true,
          source: {
            table: undefined,//数据源类型
            key: [],
            value: [],//值
            sidx: undefined,//排序字段
            sord: "DESC",//排序方式
          },
        };
      }
    },
  },
};
</script>