<template>
  <div class="getSingleData">
    <a-form-model layout="horizontal" :model="drawerData">
      <a-form-model-item label="选择工作表">
        <a-select
          v-model="drawerData.table"
          :filter-option="filterOption"
          show-search
          placeholder="请选择一个工作表，开始配置流程"
          style="width: 100%"
          @change="tableChange"
        >
          <a-select-option
            v-for="(item, index) in tables"
            :key="index"
            :value="item.configId"
            >{{ item.name }}</a-select-option
          >
        </a-select>
      </a-form-model-item>
      <a-form-model-item label="触发方式">
        <a-radio-group v-model="drawerData.triggerType">
          <a-radio :value="1"> 当新增或更新记录时 </a-radio>
          <a-radio :value="2"> 仅新增记录时 </a-radio>
          <a-radio :value="3"> 仅更新记录时 </a-radio>
          <a-radio :value="4">
            删除记录时
            <br />
          </a-radio>
        </a-radio-group>
        <a-select
          v-model="drawerData.triggerColumns"
          mode="multiple"
          :filter-option="filterOption"
          v-if="drawerData.triggerType === 1 || drawerData.triggerType === 3"
          show-search
          placeholder="当以上指定的其中一个字段更新时将触发流程，如未指定则表示任何字段更新时都会触发"
          style="width: 100%"
        >
          <a-select-option
            v-for="(item, index) in columns"
            :key="index"
            :value="item.model"
            >{{ item.label }}</a-select-option
          >
        </a-select>
      </a-form-model-item>
      <a-form-model-item
        label="筛选条件"
        extra="设置筛选条件，仅使满足条件的记录进入流程。"
      >
        <div class="getMethod">
          <conditionFilter
            :filters.sync="drawerData.conditionFilters"
            :configList="configList"
            :tables="tables"
          />
        </div>
      </a-form-model-item>
    </a-form-model>
  </div>
</template>
  
<script>
import conditionFilter from "./conditionFilter";
export default {
  name: "formChange",
  components: {
    conditionFilter,
  },
  props: {
    tables: {
      required: true,
      type: Array,
      default: () => {
        return [];
      },
    },
    configList: {
      required: true,
      type: Array,
      default: () => {
        return [];
      },
    },
    drawerData: {
      required: true,
      type: Object,
    },
  },
  data() {
    return {
      columns: [],
    };
  },

  mounted() {
    this.init();
  },

  methods: {
    /**
     *
     * @param {搜索} input
     * @param {*} option
     */
    filterOption(input, option) {
      return (
        option.componentOptions.children[0].text
          .toLowerCase()
          .indexOf(input.toLowerCase()) >= 0
      );
    },

    /**
     * 工作表切换
     */
    tableChange(e) {
      let that = this;
      let table = this.$utils.find(that.tables, (item) => item.configId == e);

      if (table && table.columnJson) {
        this.columns = JSON.parse(table.columnJson);
      }
      this.drawerData.triggerColumns = [];
      this.drawerData.conditionFilters = {
        groupOp: "AND",
        rules: [],
        groups: [],
      };
      //增加ConfigList
      this.configList[0].data = {
        tableName: table.name,
        columns: JSON.parse(table.columnJson),
      };
    },

    /**
     * 初始化
     */
    init() {
      let that = this;
      if (that.drawerData.table != undefined) {
        let data = that.tables.filter(
          (f) => f.configId == that.drawerData.table
        );
        if (data.length > 0 && data[0].columnJson) {
          that.columns = JSON.parse(data[0].columnJson);
        }
      }
      this.configList.forEach((f) => {
        if (f.type == 0) {
          var table = that.$utils.find(
            that.tables,
            (item) => item.configId == f.data.table
          );
          f.data.tableName = table.name;
          f.data.columns = JSON.parse(table.columnJson);
        }
      });
    },
  },
};
</script>
  
<style scoped>
/deep/.ant-form-item-label > label {
  font-weight: bold;
}
</style>
  