<template>
  <a-form-model :layout="form.layout">
    <a-form-model-item label="获取方式" prop="name">
      <a-radio-group v-model="drawerData.getType">
        <a-radio :value="1">
          从表单中获取
          <a-tooltip title="获取后什么也不做，在以后节点使用" placement="top">
            <a-icon type="question-circle"></a-icon>
          </a-tooltip>
        </a-radio>
        <a-radio :value="2">
          获取数组对象
          <a-tooltip title="获取发送API请求、调用已集成API、代码块、业务流程输入/输出节点的JSON数组对象" placement="top">
            <a-icon type="question-circle"></a-icon>
          </a-tooltip>
        </a-radio>
      </a-radio-group>
    </a-form-model-item>
    <a-form-model-item label="表单" v-if="drawerData.getType == 1">
      <a-select v-model="drawerData.getTypeTable" showSearch allowClear :placeholder="`请选择`"
        @change="getTypeTableChange">
        <a-select-option v-for="item in tables" :key="item.value" :value="item.configId">{{
    item.name }}</a-select-option>
      </a-select>
    </a-form-model-item>
    <a-form-model-item label="选择发送API请求节点" v-if="drawerData.getType == 2">
      <a-select v-model="drawerData.getCustomerRequest" showSearch allowClear :placeholder="`请选择`"
        @change="getCustomerRequestChange">
        <a-select-option v-for="item in configList.filter(f => [21, 22].includes(f.type) && f.data.test.array)"
          :key="item.id" :value="item.id">{{ item.title }}</a-select-option>
      </a-select>
    </a-form-model-item>

    <a-form-model-item label="筛选条件" extra="设置筛选条件，仅使满足条件的记录进入流程。">
      <div class="getMethod">
        <conditionFilter :filters.sync="drawerData.conditionFilters" :columns="drawerData.columns" />
      </div>
    </a-form-model-item>
    <a-form-model-item label="排序规则">
      <a-radio-group v-model="drawerData.sortRules" @change="sortRulesChange">
        <a-radio :value="1">不排序</a-radio>
        <a-radio :value="2">升序</a-radio>
        <a-radio :value="3">降序</a-radio>
      </a-radio-group>
    </a-form-model-item>

    <a-form-model-item label="排序字段" v-if="drawerData.sortRules != 1">
      <a-select v-model="drawerData.sortRulesSelect" showSearch allowClear :placeholder="`请选择`">
        <a-select-option v-for=" item  in  drawerData.columns" :key="item.model" :value="item.model">{{
    item.label
  }}</a-select-option>
      </a-select>
    </a-form-model-item>
  </a-form-model>

</template>

<script>
import conditionFilter from './conditionFilter'


export default {
  name: "getMultipleData",
  components: {
    conditionFilter
  },
  props: {
    tables: {
      required: true,
      type: Array,
      default: () => { return [] }
    },
    configList: {
      required: true,
      type: Array,
      default: () => { return [] }
    },
    drawerData: {
      required: true,
      type: Object
    }
  },
  data() {
    return {
      form: {
        layout: 'horizontal'
      },
    }
  },
  computed: {

  },
  mounted() {
    this.init();
  },
  methods: {
    init() {
      let that = this;
      if (this.drawerData.getType == 1) {
        var table = this.$utils.find(that.tables, item => item.configId == this.drawerData.getTypeTable);
        if (table) {
          that.drawerData.columns = JSON.parse(table.columnJson)
        }
      }
    },
    sortRulesChange(item) {

    },

    getTypeTableChange(configId) {
      var table = this.$utils.find(this.tables, item => item.configId == configId);
      this.drawerData.columns = JSON.parse(table.columnJson)
    },

    /**
     * 自定义数据源
     */
    getCustomerRequestChange(id) {
      var config = this.$utils.find(this.configList, item => item.id == id);
      if (config && config.data.test.data.length > 0) {
        //得到字段
        let columns = [];
        config.data.test.data.forEach(f => {
          for (let value in f) {
            columns.push({
              type: null,
              model: value,
              label: value
            })
          }
        })
        this.drawerData.columns = columns
      }
    }
  }
}
</script>

<style scoped>
.getSingleData {
  padding: 0 20px;
  color: #606266;
}

.getMethod {
  padding: 16px 0;
  border-bottom: 1px solid #F1F2F3;
}

.getMethod_title {
  font-size: 14px;
  color: #303233;
}

.getMethod_from {
  margin: 8px 0;
}

.getMethod_select {
  margin-top: 24px;
  font-size: 12px;
}

.getMethod_select .a-select {
  margin: 0 8px;
}

/deep/ .info {
  padding: 16px 0;
  font-size: 12px;
  width: 100% !important
}

/deep/.ant-form-item-label>label {
  font-weight: bold;
}
</style>
