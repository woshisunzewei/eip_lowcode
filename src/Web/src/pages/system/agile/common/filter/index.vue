<template>
  <div>
    <vxe-table row-key ref="searchTable" :data="filter" :height="height">
      <template #loading>
        <a-spin></a-spin>
      </template>
      <template #empty>
        <eip-empty />
      </template>
      <vxe-column title="新增" width="54px" align="center">
        <template #header>
          <a-button @click="add" size="small" type="primary">
            <a-icon type="plus" />
          </a-button>
        </template>
        <template v-slot="{ row }">
          <a-popconfirm title="确定删除查询配置?" ok-text="确定" cancel-text="取消" @confirm="del(row)">
            <a-button @click.stop="" size="small" type="danger">
              <a-icon type="delete"></a-icon>
            </a-button>
          </a-popconfirm>
        </template>
      </vxe-column>
      <vxe-column title="查询字段" width="200px">
        <template v-slot="{ row }">
          <a-select placeholder="请选择查询字段" v-model="row.field" style="width: 180px">
            <a-select-option v-for="(item, i) in columns" :key="i" :value="item.name"><label v-if="!item.description">{{
      item.name }}</label>{{ item.description
              }}</a-select-option>
          </a-select>
        </template>
      </vxe-column>

      <vxe-column title="比较符" width="150px">
        <template v-slot="{ row }">
          <a-input-group compact>
            <a-select style="width: 100px" v-model="row.op" placeholder="请选择弹出">
              <a-select-option v-for="(item, i) in row.ops" :key="i" :value="item.value">{{ item.title
                }}</a-select-option>
            </a-select>
          </a-input-group>
        </template>
      </vxe-column>

      <vxe-column title="值" width="180px">
        <template v-slot="{ row }">
          <a-input placeholder="请输入值" v-model="row.value" />
        </template>
      </vxe-column>
    </vxe-table>
  </div>
</template>

<script>
export default {
  name: "filter-index",
  components: {},
  data() {
    return {
      height: window.innerHeight - 66,
      opControls: {
        //比较符对应类型
        input: [
          { value: "cn", title: "包含" },
          { value: "eq", title: "等于" },
          { value: "ne", title: "不等" },
          { value: "bw", title: "开始于" },
          { value: "bn", title: "不开始于" },
          { value: "ew", title: "结束于" },
          { value: "en", title: "不结束于" },
          { value: "nc", title: "不包含" },
          { value: "nu", title: "空值" },
          { value: "nn", title: "非空值" },
        ],
        select: [
          { value: "eq", title: "等于" },
          { value: "ne", title: "不等" },
        ],
        bool: [
          { value: "eq", title: "等于" },
          { value: "ne", title: "不等" },
        ],
        datetime: [
          { value: "date", title: "等于" },
          { value: "daterange", title: "之间" },
          { value: "lt", title: "小于" },
          { value: "le", title: "小于等于" },
          { value: "gt", title: "大于" },
          { value: "ge", title: "大于等于" },
        ],
        number: [
          { value: "eq", title: "等于" },
          { value: "lt", title: "小于" },
          { value: "le", title: "小于等于" },
          { value: "gt", title: "大于" },
          { value: "ge", title: "大于等于" },
        ],
      },

      typeItem: {
        //d当前选中类型
        ops: [],
      },
    };
  },

  props: {
    filter: {
      type: Array,
    },

    columns: {
      type: Array,
    },
  },
  mounted() { },
  methods: {
    /**
     * 新增
     */
    add() {
      this.filter.push({
        field: undefined,
        type: "input",
        typeConfig: null,
        ops: this.opControls.input,
        op: "cn",
        value: ''
      });
    },

    /**
     * 删除
     */
    del(row) {
      let spIndex = -1;
      this.filter.forEach((item, index) => {
        if (item._X_ROW_KEY == row._X_ROW_KEY) {
          spIndex = index;
        }
      });
      if (spIndex != -1) {
        this.filter.splice(spIndex, 1);
      }
    },

    /**
     * 类型点击
     */
    typeClick(row) {
      this.typeItem = row;
      switch (row.type) {
        case "select":
          this.drawer.select.visible = true;
          this.$refs.searchSelect.init(row.typeConfig);
          break;
        case "bool":
          this.drawer.bool.visible = true;
          break;
        case "datetime":
          this.drawer.datetime.visible = true;
          break;
        case "number":
          this.drawer.number.visible = true;
          break;
        default:
          this.$message.error("当前类型无配置");
          break;
      }
    },

    /**
     * 类型关闭
     */
    typeConfigClose() {
      this.drawer.select.visible = false;
      this.drawer.datetime.visible = false;
      this.drawer.number.visible = false;
      this.drawer.bool.visible = false;
    },

    /**
     * 类型下拉改变,改变比较符
     */
    typeChange(row, value) {
      this.typeItem = row;
      this.typeItem.ops = [];
      switch (value) {
        case "input":
          this.typeItem.ops = this.opControls.input;
          break;
        case "select":
          this.typeItem.ops = this.opControls.select;
          break;
        case "bool":
          this.typeItem.ops = this.opControls.bool;
          break;
        case "datetime":
          this.typeItem.ops = this.opControls.datetime;
          break;
        case "number":
          this.typeItem.ops = this.opControls.number;
          break;
        default:
          this.typeItem.ops = this.opControls.input;
          break;
      }
      this.typeItem.op = this.typeItem.ops[0].value;
    },

    /**
     * select下拉配置
     */
    typeConfigSelectOk(config) {
      this.typeItem.typeConfig = config;
      this.typeConfigClose();
    },
  },
};
</script>

<style lang="less" scoped></style>