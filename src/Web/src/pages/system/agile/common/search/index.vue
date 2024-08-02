<template>
  <div>
    <div style="margin-bottom: 4px">
      <a-form-model layout="inline">
        <a-form-model-item label="每行显示个数">
          <a-select
            placeholder="请选择每行显示个数"
            v-model="config.num"
            style="width: 180px"
          >
            <a-select-option :value="3">3 </a-select-option>
            <a-select-option :value="4">4 </a-select-option>
            <a-select-option :value="6">6</a-select-option>
          </a-select>
        </a-form-model-item>
        <a-form-model-item label="标签布局">
          <a-slider
            style="width: 150px"
            v-model="config.labelCol"
            :min="1"
            :max="24"
        /></a-form-model-item>
        <a-form-model-item label="输入控件布局">
          <a-slider
            style="width: 150px"
            v-model="config.wrapperCol"
            :min="1"
            :max="24"
          />
        </a-form-model-item>
        <a-form-model-item label="配置">
          <a-checkbox v-model="config.searchButton"
            >启用查询按钮
            <a-tooltip title="启用按钮后，点击查询按钮执行筛选。">
              <a-icon type="question-circle-o" /> </a-tooltip
          ></a-checkbox>
        </a-form-model-item>
      </a-form-model>
    </div>

    <vxe-table
      row-key
      ref="searchTable"
      :data="config.columns"
      :height="height"
    >
      <template #loading>
        <a-spin></a-spin>
      </template>
      <template #empty>
        <eip-empty />
      </template>
      <vxe-column
        type="seq"
        title="序号"
        width="50"
        align="center"
      ></vxe-column>
      <vxe-column title="排序" width="53px" align="center">
        <template>
          <span class="drag-btn">
            <a-icon type="drag" />
          </span>
        </template>
      </vxe-column>
      <vxe-column title="新增" width="54px" align="center">
        <template #header>
          <a-button @click="add" size="small" type="primary">
            <a-icon type="plus" />
          </a-button>
        </template>
        <template v-slot="{ row }">
          <a-popconfirm
            title="确定删除查询配置?"
            ok-text="确定"
            cancel-text="取消"
            @confirm="del(row)"
          >
            <a-button @click.stop="" size="small" type="danger">
              <a-icon type="delete"></a-icon>
            </a-button>
          </a-popconfirm>
        </template>
      </vxe-column>
      <vxe-column title="查询字段" width="200px">
        <template v-slot="{ row }">
          <a-select
            placeholder="请选择查询字段"
            v-model="row.field"
            style="width: 180px"
            @change="fieldChange(row)"
          >
            <a-select-option
              v-for="(item, i) in columns.filter((f) => f.isShow || f.isSearch)"
              :key="i"
              :value="item.name"
              ><label v-if="!item.description">{{ item.name }}</label>
              {{ item.description }}</a-select-option
            >
          </a-select>
        </template>
      </vxe-column>

      <vxe-column title="标题" width="130px">
        <template v-slot="{ row }">
          <a-input placeholder="标题" v-model="row.title" />
        </template>
      </vxe-column>
      <vxe-column title="提示说明" width="180px">
        <template v-slot="{ row }">
          <a-input placeholder="提示说明" v-model="row.placeholder" />
        </template>
      </vxe-column>

      <vxe-column title="类型" width="170px">
        <template v-slot="{ row }">
          <a-input-group compact>
            <a-select
              show-search
              style="width: 100px"
              v-model="row.type"
              placeholder="请选择类型"
              @change="typeChange(row, $event)"
            >
              <a-select-option value="input">文本框</a-select-option>
              <a-select-option value="number">数字</a-select-option>
              <a-select-option value="select">下拉框</a-select-option>
              <a-select-option value="datetime">时间</a-select-option>
              <a-select-option value="data">自定义数据源</a-select-option>
              <a-select-option value="bool">是否</a-select-option>
              <a-select-option value="user">用户</a-select-option>
              <a-select-option value="organization">组织架构</a-select-option>
              <a-select-option value="correlationRecord"
                >关联记录</a-select-option
              >
              <a-select-option value="dictionary">字典</a-select-option>
            </a-select>
            <a-button @click="typeClick(row)">
              <a-icon type="setting"></a-icon>
            </a-button>
          </a-input-group>
        </template>
      </vxe-column>
      <vxe-column title="比较符" width="150px">
        <template v-slot="{ row }">
          <a-input-group compact>
            <a-select
              style="width: 100px"
              v-model="row.op"
              placeholder="请选择弹出"
            >
              <a-select-option
                v-for="(item, i) in row.ops"
                :key="i"
                :value="item.value"
                >{{ item.title }}</a-select-option
              >
            </a-select>
          </a-input-group>
        </template>
      </vxe-column>
      <vxe-column title="查询字段" width="280px">
        <template v-slot="{ row }">
          <a-input
            placeholder="查询字段(若有值则以此字段为准)"
            v-model="row.fields"
          />
        </template>
      </vxe-column>
    </vxe-table>
    <!-- 查询下拉配置 -->
    <search-select
      ref="searchSelect"
      :visible.sync="drawer.select.visible"
      @cancel="optionsClose"
      @ok="optionsOk"
    ></search-select>

    <!-- 类型是否配置 -->
    <search-bool
      ref="searchBool"
      :visible.sync="drawer.bool.visible"
      @cancel="optionsClose"
      @ok="optionsOk"
    ></search-bool>

    <!-- 类型时间配置 -->
    <search-datetime
      ref="searchDatetime"
      :visible.sync="drawer.datetime.visible"
      @cancel="optionsClose"
      @ok="optionsOk"
    ></search-datetime>

    <!-- 类型用户配置 -->
    <search-user
      ref="searchUser"
      :visible.sync="drawer.user.visible"
      @cancel="optionsClose"
      @ok="optionsOk"
    ></search-user>

    <!-- 类型组织架构配置 -->
    <search-organization
      ref="searchOrganization"
      :visible.sync="drawer.organization.visible"
      @cancel="optionsClose"
      @ok="optionsOk"
    ></search-organization>

    <!-- 查询下拉配置 -->
    <search-correlationRecord
      ref="searchCorrelationRecord"
      :visible.sync="drawer.correlationRecord.visible"
      @cancel="optionsClose"
      @ok="optionsOk"
    ></search-correlationRecord>

    <!-- 查询下拉配置 -->
    <search-dictionary
      ref="searchDictionary"
      :visible.sync="drawer.dictionary.visible"
      @cancel="optionsClose"
      @ok="optionsOk"
    ></search-dictionary>
  </div>
</template>

<script>
import { query } from "@/services/system/datasource/list";
import Sortable from "sortablejs";
import searchSelect from "./select";
import searchBool from "./bool";
import searchDatetime from "./datetime";
import searchUser from "./user";
import searchOrganization from "./organization";
import searchCorrelationRecord from "./correlationRecord";
import searchDictionary from "./dictionary";
export default {
  name: "search-index",
  components: {
    searchSelect,
    searchBool,
    searchDatetime,
    searchUser,
    searchOrganization,
    searchCorrelationRecord,
    searchDictionary,
  },
  data() {
    return {
      height: window.innerHeight - 110,
      typeItem: {
        //d当前选中类型
        ops: [],
      },

      drawer: {
        style: {
          zIndex: 1020,
        },
        select: {
          visible: false,
        },
        correlationRecord: {
          visible: false,
        },
        datetime: {
          visible: false,
        },
        bool: {
          visible: false,
        },
        user: {
          visible: false,
        },
        organization: {
          visible: false,
        },
        dictionary: {
          visible: false,
        },
      },
    };
  },

  props: {
    columns: {
      type: Array,
    },
    config: {
      type: Object,
      default: function () {
        return {
          labelCol: 8,
          wrapperCol: 16,
          num: 4, //显示个数
          columns: [], //查询条件
        };
      },
    },
  },
  methods: {
    /**
     * 字段改变
     * @param {*} row
     */
    fieldChange(row) {
      let that = this;
      var columnFilter = this.columns.filter((f) => f.name == row.field);
      if (columnFilter.length > 0) {
        var column = columnFilter[0];
        row.title = column.description;
        //判断类型
        switch (column.type) {
          case "select":
            row.placeholder = "请选择" + row.title;
            row.ops = this.opControls.select;
            row.type = "select";
            row.op = "eq";
            if (column.options.dynamic) {
              query({ size: 9999 }).then((result) => {
                var dataSource = result.data.filter(
                  (f) =>
                    f.dataSourceId == column.options.dynamicConfig.dataSourceId
                );
                let title = dataSource[0].name;
                var config = JSON.parse(dataSource[0].config);
                let outParams = [];
                config.outParams.forEach((item) => {
                  outParams.push({
                    name: item.key,
                    description: item.title,
                    title: item.title,
                  });
                });
                row.options = {
                  multiple: true,
                  source: {
                    type: "dynamic", //数据源类型
                    title: title,
                    dataSourceId: column.options.dynamicConfig.dataSourceId,
                    inParams: column.options.dynamicConfig.inParams,
                    outParams: outParams,
                    key: column.options.dynamicConfig.key,
                    value: column.options.dynamicConfig.value,
                    sidx: column.options.dynamicConfig.sidx,
                    sord: column.options.dynamicConfig.sord,
                    custom: [], //自定义数据
                  },
                };
              });
            } else {
              var custom = [];
              column.options.options.forEach((item) => {
                custom.push({
                  key: item.value,
                  value: item.label,
                });
              });
              row.options = {
                multiple: true,
                source: {
                  type: "custom", //数据源类型
                  dataSourceId: null,
                  title: "",
                  inParams: [],
                  outParams: [],
                  key: [], //键
                  value: [], //值
                  sidx: undefined, //排序字段
                  sord: "DESC", //排序方式
                  custom: custom, //自定义数据
                },
              };
            }
            break;

          case "dataList":
            row.placeholder = "请选择" + row.title;
            row.ops = this.opControls.select;
            row.type = "select";
            row.op = "eq";

            query({ size: 9999 }).then((result) => {
              var dataSource = result.data.filter(
                (f) =>
                  f.dataSourceId == column.options.dynamicConfig.dataSourceId
              );
              let title = dataSource[0].name;
              var config = JSON.parse(dataSource[0].config);
              let outParams = [];
              config.outParams.forEach((item) => {
                outParams.push({
                  name: item.key,
                  description: item.title,
                  title: item.title,
                });
              });
              row.options = {
                multiple: true,
                source: {
                  type: "dynamic", //数据源类型
                  title: title,
                  dataSourceId: column.options.dynamicConfig.dataSourceId,
                  inParams: column.options.dynamicConfig.inParams,
                  outParams: outParams,
                  key: column.options.dynamicConfig.key,
                  value: column.options.dynamicConfig.value,
                  sidx: column.options.dynamicConfig.sidx,
                  sord: column.options.dynamicConfig.sord,
                  custom: [], //自定义数据
                },
              };
            });
            break;

          case "switch":
            row.placeholder = "请选择" + row.title;
            row.ops = this.opControls.select;
            row.type = "select";
            row.op = "eq";
            row.options = {
              multiple: true,
              source: {
                type: "custom", //数据源类型
                dataSourceId: null,
                title: "",
                inParams: [],
                outParams: [],
                key: [], //键
                value: [], //值
                sidx: undefined, //排序字段
                sord: "DESC", //排序方式
                custom: [
                  {
                    key: 1,
                    value: "是",
                  },
                  {
                    key: 0,
                    value: "否",
                  },
                ],
              },
            };
            break;

          case "date":
            row.options = {
              format: "YYYY-MM-DD HH:mm:ss",
              formatCustomer: "",
            };
            break;

          case "user":
            row.placeholder = "请选择" + row.title;
            row.options = {
              multiple: true,
            };
            break;

          case "organization":
            row.placeholder = "请选择" + row.title;
            row.options = {
              multiple: true,
            };
            break;

          case "correlationRecord":
            row.type = "correlationRecord";
            row.op = "eq";
            row.placeholder = "请选择" + row.title;
            row.options = {
              multiple: true, //多选
              source: {
                table: column.options.dynamicConfig.table, //数据源类型
                key: "RelationId",
                value: column.options.columns
                  .filter((f) => f.isShow)
                  .map((m) => m.name)
                  .join(","), //值
                sidx: column.options.columns
                  .filter((f) => f.sord)
                  .map((m) => m.name)
                  .join(","), //排序字段
                sord: column.options.columns
                  .filter((f) => f.sord)
                  .map((m) => m.sordType)
                  .join(","), //排序字段,//排序方式
              },
            };
            break;

          case "dictionary":
            row.type = "dictionary";
            row.op = "eq";
            row.placeholder = "请选择" + row.title;
            row.options = {
              multiple: true, //多选
              source: {
                dictionaryId: column.options.dictionaryId,
                dictionaryLevel: column.options.dictionaryLevel, //层级,默认下一级
              },
            };
            break;
          default:
            row.placeholder = "请输入" + row.title;
            if (column.style.length > 0) {
              row.placeholder = "请选择" + row.title;
              row.ops = this.opControls.select;
              row.type = "select";
              row.op = "eq";
              var custom = [];
              column.style.forEach((item) => {
                custom.push({
                  key: item.value,
                  value: item.content,
                });
              });
              row.options = {
                multiple: true,
                source: {
                  type: "custom", //数据源类型
                  title: "",
                  dataSourceId: null,
                  inParams: [],
                  outParams: [],
                  key: [], //键
                  value: [], //值
                  sidx: undefined, //排序字段
                  sord: "DESC", //排序方式
                  custom: custom, //自定义数据
                },
              };
            }
            break;
        }
      }
    },

    /**
     * 拖拽
     */
    rowDrop() {
      let that = this;
      this.$nextTick(() => {
        const xTable = this.$refs.searchTable;
        if (xTable) {
          this.sortable1 = Sortable.create(
            xTable.$el.querySelector(".body--wrapper>.vxe-table--body tbody"),
            {
              handle: ".drag-btn",
              onEnd: ({ newIndex, oldIndex }) => {
                const currRow = that.config.columns.splice(oldIndex, 1)[0];
                that.config.columns.splice(newIndex, 0, currRow);
              },
            }
          );
        }
      });
    },

    /**
     * 新增
     */
    add() {
      this.config.columns.push({
        title: null,
        placeholder: "请输入",
        field: undefined,
        fields: undefined, //真正查询字段
        type: "input",
        options: null,
        ops: this.opControls.input,
        op: "cn",
      });
      this.rowDrop();
    },

    /**
     * 删除
     */
    del(row) {
      let spIndex = -1;
      this.config.columns.forEach((item, index) => {
        if (item._X_ROW_KEY == row._X_ROW_KEY) {
          spIndex = index;
        }
      });
      if (spIndex != -1) {
        this.config.columns.splice(spIndex, 1);
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
          this.$refs.searchSelect.init(row.options);
          break;
        case "correlationRecord":
          this.drawer.correlationRecord.visible = true;
          this.$refs.searchCorrelationRecord.init(row.options);
          break;
        case "bool":
          this.drawer.bool.visible = true;
          this.$refs.searchBool.init(row.options);
          break;
        case "user":
          this.drawer.user.visible = true;
          this.$refs.searchUser.init(row.options);
          break;
        case "organization":
          this.drawer.organization.visible = true;
          this.$refs.searchOrganization.init(row.options);
          break;
        case "datetime":
          this.drawer.datetime.visible = true;
          this.$refs.searchDatetime.init(row.options);
          break;
        case "dictionary":
          this.drawer.dictionary.visible = true;
          this.$refs.searchDictionary.init(row.options);
          break;
        default:
          this.$message.error("当前类型无配置");
          break;
      }
    },

    /**
     * 类型关闭
     */
    optionsClose() {
      this.drawer.select.visible = false;
      this.drawer.correlationRecord.visible = false;
      this.drawer.datetime.visible = false;
      this.drawer.bool.visible = false;
      this.drawer.user.visible = false;
      this.drawer.organization.visible = false;
      this.drawer.dictionary.visible = false;
    },

    /**
     * 类型下拉改变,改变比较符
     */
    typeChange(row, value) {
      this.typeItem = row;
      this.typeItem.ops = [];
      this.typeItem.options = {};
      switch (value) {
        case "input":
          this.typeItem.ops = this.opControls.input;
          break;
        case "dictionary":
          this.typeItem.ops = this.opControls.select;
          this.typeItem.options = {
            multiple: true, //多选
            source: {
              dictionaryId: undefined,
              dictionaryLevel: 1, //层级,默认下一级
            },
          };
          break;
        case "select":
          this.typeItem.ops = this.opControls.select;
          this.typeItem.options = {
            multiple: true, //多选
            source: {
              type: "customer", //数据源类型
              title: "",
              dataSourceId: null,
              inParams: [],
              outParams: [],
              key: [], //键
              value: [], //值
              sidx: undefined, //排序字段
              sord: "DESC", //排序方式
            },
          };
          break;
        case "correlationRecord":
          this.typeItem.ops = this.opControls.select;
          this.typeItem.options = {
            multiple: true, //多选
            source: {
              table: undefined, //数据源类型
              value: [], //值
              sidx: undefined, //排序字段
              sord: "DESC", //排序方式
            },
          };
          break;
        case "user":
          this.typeItem.ops = this.opControls.user;
          this.typeItem.options = {
            multiple: true,
          };
          break;
        case "organization":
          this.typeItem.ops = this.opControls.organization;
          this.typeItem.options = {
            multiple: true,
          };
          break;
        case "bool":
          this.typeItem.ops = this.opControls.bool;
          this.typeItem.options = {
            yes: "是",
            no: "否",
          };
          break;
        case "datetime":
          this.typeItem.ops = this.opControls.datetime;
          this.typeItem.options = {
            format: "YYYY-MM-DD HH:mm:ss",
            formatCustomer: "",
          };
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
    optionsOk(options) {
      this.typeItem.options = options;
      this.optionsClose();
    },
  },
};
</script>

<style lang="less" scoped></style>
