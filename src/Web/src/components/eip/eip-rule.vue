<template>
  <div class="groupOp-group-container tree box">
    <a-card
      class="box-card"
      style="margin-top: 4px"
      :bodyStyle="{ padding: '0 2px 0 10px' }"
    >
      <div style="padding: 4px">
        <a-row>
          <a-row class="child">
            <a-radio-group
              button-style="solid"
              v-model="filters.groupOp"
              size="small"
            >
              <a-radio-button value="AND">且</a-radio-button>
              <a-radio-button value="OR">或</a-radio-button>
            </a-radio-group>
            <a-button
              size="small"
              style="float: right; margin-left: 10px"
              type="primary"
              icon="gateway"
              @click="addGroup"
              >分组</a-button
            >
            <a-button
              size="small"
              style="float: right; margin-left: 10px"
              type="primary"
              icon="plus"
              @click="add"
              >条件</a-button
            >
          </a-row>

          <template v-for="(item, index) in filters.rules">
            <a-row :key="'condition' + index" class="child">
              <a-space>
                <a-select
                  style="width: 150px"
                  @select="paramsChange(item, $event)"
                  v-model="item.field"
                  :allowClear="true"
                  optionFilterProp="label"
                  show-search
                  placeholder="请选择"
                  size="small"
                >
                  <a-select-option
                    v-for="(citem, cindex) in columns"
                    :value="citem.field"
                    :key="cindex"
                    :label="citem.title"
                  >
                    {{ citem.title ? citem.title : citem.field }}
                  </a-select-option>
                </a-select>

                <a-select
                  v-model="item.op"
                  size="small"
                  placeholder="请选择"
                  style="width: 100px"
                >
                  <a-select-option
                    v-for="(oitem, oindex) in item.ops"
                    :value="oitem.value"
                    :key="oindex"
                  >
                    {{ oitem.title }}</a-select-option
                  >
                </a-select>

                <a-select
                  v-if="
                    ['correlationRecord', 'select'].includes(item.type) &&
                    !['nu', 'nn'].includes(item.op)
                  "
                  v-model="item.data"
                  size="small"
                  style="width: 150px"
                  placeholder="请选择"
                >
                  <a-select-option
                    v-for="(citem, cindex) in dataColumns"
                    :value="citem.field"
                    :key="cindex"
                  >
                    {{ citem.title ? citem.title : citem.field }}
                  </a-select-option>
                </a-select>

                <a-input
                  v-if="
                    ['input', 'textarea', 'switch'].includes(item.type) &&
                    !['nu', 'nn'].includes(item.op)
                  "
                  style="width: 150px"
                  size="small"
                  v-model="item.data"
                  :placeholder="'请输入关键字'"
                ></a-input>

                <a-popconfirm
                  ok-text="确认"
                  cancel-text="取消"
                  @confirm="filters.rules.splice(index, 1)"
                >
                  <template slot="title"> 删除规则 </template>
                  <a-button icon="delete" type="danger" size="small"></a-button>
                </a-popconfirm>
              </a-space>
            </a-row>
          </template>

          <template v-for="(item, index) in filters.groups">
            <a-row
              :key="'group' + index"
              class="child"
              v-if="item.rules.length > 0"
            >
              <eip-rule
                :filters="item"
                :columns="columns"
                :dataColumns="dataColumns"
              ></eip-rule>
            </a-row>
          </template>
        </a-row>
      </div>
    </a-card>
  </div>
</template>
<script>
export default {
  name: "eip-rule",
  props: {
    filters: {
      type: Object,
      default: function () {
        return {
          groupOp: "AND",
          rules: [],
          groups: [],
        };
      },
    },
    columns: {
      type: Array,
    },
    dataColumns: {
      type: Array,
    },
  },
  data() {
    return {
      type: {
        input: "input",
        select: "select",
        datetime: "datetime",
        correlationRecord: "correlationRecord",
      },
    };
  },
  methods: {
    /**
     * 新增
     */
    add() {
      this.filters.rules.push({
        field: undefined,
        op: "cn",
        data: undefined,
        ops: this.opControls.input,
        type: this.type.input,
        options: [],
      });
    },

    /**
     * 获取筛选
     */
    getFilters() {
      let copyFilters = this.$utils.clone(this.filters, true);
      var filters = {
        groupOp: copyFilters.groupOp,
        rules: [],
        groups: [],
      };
      // 递归遍历控件树
      const traverseRule = (array, filters) => {
        array.forEach((element) => {
          filters.rules.push({
            field: element.field,
            op: element.op,
            data: element.data,
          });
        });
      };
      traverseRule(copyFilters.rules, filters);

      const traverseGroup = (array, filters) => {
        array.forEach((element) => {
          var group = {
            groupOp: element.groupOp,
            rules: [],
            groups: [],
          };

          element.rules.forEach((rule) => {
            group.rules.push({
              field: rule.field,
              op: rule.op,
              data: rule.data,
            });
          });

          filters.groups.push(group);

          if (element.groups.length > 0) {
            traverseGroup(element.groups, filters);
          }
        });
      };
      traverseGroup(copyFilters.groups, filters);
      return filters;
    },
    /**
     * 新增组
     */
    addGroup() {
      this.filters.groups.push({
        groupOp: "AND",
        rules: [
          {
            field: undefined,
            op: "cn",
            data: undefined,
            ops: this.opControls.input,
            type: this.type.input,
            options: [],
          },
        ],
        groups: [],
      });
    },
    /**
     * 搜索
     * @param {*} input
     * @param {*} option
     */
    filterOption(input, option) {
      return (
        option.componentOptions.children[0].text
          .toUpperCase()
          .indexOf(input.toUpperCase()) >= 0
      );
    },

    /**
     *
     * @param {*} row
     * @param {*} event
     */
    paramsChange(row, event) {
      row.data = undefined;
      var data = this.$utils.find(this.columns, (f) => f.field == row.field);
      row.type = data.type;
    },
  },
};
</script>
<style scoped>
.box {
  width: 100%;
}

/* 只需要左边边框线 */
.child {
  width: 100%;
  position: relative;
  border: 1px solid #d9d9d9;
  border-style: none none none solid;
  padding: 2px 0;
  padding-left: 12px;
}

/* 设置一个伪元素宽2px 高50% 用于遮挡多余的左边框线 */
.child::before {
  display: block;
  content: "";
  position: absolute;
  background-color: white;
  width: 1px;
  height: 50%;
}

/* 设置第一个子元素的伪类定位 */
.box .child:first-child::before {
  left: -1px;
  top: 0;
}

/* 设置第二个子元素的伪类定位 */
.box .child:last-child::before {
  left: -1px;
  bottom: 0;
}

/* 设置子元素的横线，定位在高度的50% */
.box .child::after {
  top: 50%;
  left: 0;
  position: absolute;
  content: "";
  display: block;
  width: 10px;
  height: 1px;
  border: 1px solid #d9d9d9;
  border-style: solid none none none;
}
</style>
