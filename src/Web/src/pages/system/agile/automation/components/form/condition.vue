<template>
  <div class="getSingleData">
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
    