<template>
  <a-form-model layout="horizontal">
    <a-row>
      <a-col :md="24 / option.num" :sm="24" :key="index" v-show="index < advancedIndex"
        v-for="(item, index) in option.config">
        <a-form-model-item :label="item.title" :labelCol="{ span: option.labelCol ? option.labelCol : 8 }" :wrapperCol="{
          span: option.wrapperCol ? option.wrapperCol : 16,
          offset: 0,
        }">
          <!-- 用户 -->
          <a-input :placeholder="item.placeholder" @click="userClick(item)" v-if="item.type == 'user'"
            v-model="item.value" allow-clear>
            <a-icon slot="suffix" type="user" />
          </a-input>

          <!-- 组织架构 -->
          <a-input :placeholder="item.placeholder" @click="organizationClick(item)" v-if="item.type == 'organization'"
            v-model="item.value" allow-clear>
            <a-icon slot="suffix" type="apartment" />
          </a-input>

          <!-- 文本框 -->
          <a-input @change="changeSearch" :placeholder="item.placeholder" v-if="item.type == 'text'"
            v-model="item.value" allow-clear />

          <!-- 数字 -->
          <a-input-number @change="changeSearch" :placeholder="item.placeholder" v-if="item.type == 'number'"
            v-model="item.value" style="width: 100%;" />

          <!-- 是否下拉 -->
          <a-select @change="changeSearch" :placeholder="item.placeholder" allow-clear v-model="item.value"
            v-if="item.type == 'bool'">
            <a-select-option value="1"> {{ item.options.yes }} </a-select-option>
            <a-select-option value="0"> {{ item.options.no }} </a-select-option>
          </a-select>

          <!-- 时间 -->
          <a-date-picker @change="changeSearch"
            :show-time="{ disabled: item.options.format.indexOf('HH') > -1, format: item.options.format.replace('YYYY-MM-DD ') }"
            :placeholder="item.placeholder" :format="item.options.format" allow-clear v-model="item.value"
            v-if="item.type == 'datetime' && item.op != 'daterange' && item.options.mode == 'date'" />

          <a-date-picker mode="year" :open="item.open" :placeholder="item.placeholder" :format="item.options.format"
            allow-clear v-model="item.value"
            v-if="item.type == 'datetime' && item.op != 'daterange' && item.options.mode == 'year'" @change="e => {
              item.value = e;
              item.open = false
              changeSearch();
            }" @panelChange="e => {
              item.value = e;
              item.open = false
            }" @openChange="e => {
              item.open = e
            }" />

          <a-date-picker v-if="item.type == 'datetime' && item.op != 'daterange' && item.options.mode == 'month'"
            mode="month" :open="item.open" :placeholder="item.placeholder" :format="item.options.format" allow-clear
            v-model="item.value" @change="e => {
              item.value = e;
              item.open = false
              changeSearch();
            }" @panelChange="e => {
              item.value = e;
              item.open = false
            }" @openChange="e => {
              item.open = e
            }" />

          <a-range-picker :mode="[item.options.mode, item.options.mode]"
            :show-time="{ disabled: item.options.format.indexOf('HH') > -1, format: item.options.format.replace('YYYY-MM-DD ') }"
            :placeholder="[item.placeholder, item.placeholder]" :format="item.options.format" allow-clear
            v-model="item.value" v-if="item.type == 'datetime' && item.op == 'daterange'" @change="e => {
              item.value = e;
              item.open = false
              changeSearch();
            }" @panelChange="e => {
              item.value = e;
              item.open = false
            }" @openChange="e => {
              item.open = e
            }" />

          <!-- 数据源下拉 -->
          <a-select v-if="item.type == 'select'" @change="changeSearch" optionFilterProp="label"
            @focus="selectFocus(item)" :mode="item.options.multiple ? 'multiple' : ''" :placeholder="item.placeholder"
            allow-clear v-model="item.value">
            <div slot="notFoundContent" style="text-align: center;">
              <a-empty :image="simpleImage" />
            </div>
            <a-select-option v-for="(sitem, sindex) in item.options.source.format" :key="sindex" :value="sitem.key"
              :label="sitem.value">
              {{ sitem.value }}
            </a-select-option>
          </a-select>

          <!-- 关联记录下拉 -->
          <a-select v-if="item.type == 'correlationRecord'" @change="changeSearch" optionFilterProp="label"
            @focus="selectCorrelationRecordFocus(item)" :mode="item.options.multiple ? 'multiple' : ''"
            :placeholder="item.placeholder" allow-clear v-model="item.value">
            <div slot="notFoundContent" style="text-align: center;">
              <a-empty :image="simpleImage" />
            </div>
            <a-select-option v-for="(sitem, sindex) in item.options.source.format" :key="sindex" :value="sitem.key"
              :label="sitem.value">
              {{ sitem.value }}
            </a-select-option>
          </a-select>

          <!-- 字典查询 -->
          <a-select v-if="item.type == 'dictionary' && item.options.source.dictionaryLevel == 1"
            :mode="item.options.multiple ? 'multiple' : ''" @focus="selectDictionaryFocus(item)" @change="changeSearch"
            optionFilterProp="label" :placeholder="item.placeholder" allow-clear v-model="item.value">
            <a-select-option v-for="(item, index) in item.options.source.format" :value="item.value" :key="index"
              :label="item.name">
              {{ item.label }}</a-select-option>
          </a-select>

          <a-tree-select
            v-if="item.type == 'dictionary' && item.options.source.dictionaryLevel == 2 && item.options.multiple"
            tree-checkable @focus="selectDictionaryTreeFocus(item)" :placeholder="item.placeholder" allow-clear
            @change="changeSearch" v-model="item.value" show-search style="width: 100%" treeNodeFilterProp="title"
            :tree-data="item.options.source.format" :dropdown-style="{ maxHeight: '400px', overflow: 'auto' }">
          </a-tree-select>

          <a-tree-select
            v-if="item.type == 'dictionary' && item.options.source.dictionaryLevel == 2 && !item.options.multiple"
            @focus="selectDictionaryTreeFocus(item)" :placeholder="item.placeholder" allow-clear @change="changeSearch"
            v-model="item.value" show-search style="width: 100%" treeNodeFilterProp="title"
            :tree-data="item.options.source.format" :dropdown-style="{ maxHeight: '400px', overflow: 'auto' }">
          </a-tree-select>

        </a-form-model-item>
      </a-col>
      <div v-if="option.config.length > 0" class="ant-col ant-col-sm-24" :class="'ant-col-md-' + 24 / option.num">
        <div class="ant-row ant-form-item" style="line-height: 40px">
          <div class="ant-col ant-col-4 ant-form-item-label"></div>
          <div class="ant-col ant-col-20 ant-form-item-control-wrapper">
            <a-space>
              <a-button v-if="option.searchButton" type="primary" @click="search"><a-icon type="search" /> 查询</a-button>
              <a-button v-if="option.searchButton" @click="reset"><a-icon type="undo" />重置</a-button>
              <a-tooltip>
                <template slot="title">
                  {{ advanced ? "收起" : "展开" }}
                </template>
                <a @click="toggleAdvanced" style="margin-left: 8px" v-if="(option.config.length + 1) / option.num > 1">
                  <a-icon :type="advanced ? 'up' : 'down'" />
                </a>
              </a-tooltip>
            </a-space>
          </div>
        </div>
      </div>
    </a-row>

    <eip-user v-if="user.visible" :visible.sync="user.visible" :chosen="user.chosen" :multiple="user.multiple"
      :title="user.title" @ok="chosenUserOk"></eip-user>

    <eip-organization v-if="organization.visible" :visible.sync="organization.visible" :chosen="organization.chosen"
      :multiple="organization.multiple" :title="organization.title" @ok="chosenOrganizationOk"></eip-organization>
  </a-form-model>
</template>

<script>
import moment from "moment";
import { businessDataFormSource } from "@/services/system/agile/run/edit";
import {
  dictionaryQueryByIds,
  dictionaryParentId
} from "@/services/system/agile/run/edit";
import {
  queryTable
} from "@/services/system/agile/run/list";
import { Empty } from 'ant-design-vue';
export default {
  name: "eip-search",
  data() {
    return {
      advanced: false,
      advancedIndex: this.option.config.length,
      user: {
        item: null,
        chosen: [],
        multiple: true,
        visible: false,
        title: '',
      },
      organization: {
        item: null,
        chosen: [],
        multiple: true,
        visible: false,
        title: '',
      }
    };
  },
  props: {
    option: {
      type: Object,
      default: function () {
        return {
          searchButton: false,
          labelCol: 8,
          wrapperCol: 16,
          num: 6,
          config: [],
        }
      },
    }
  },
  beforeCreate() {
    this.simpleImage = Empty.PRESENTED_IMAGE_SIMPLE;
  },
  created() {
    if (this.$utils.isUndefined(this.option.searchButton)) {
      this.option.searchButton = true;
    }
    this.toggleAdvanced();
  },
  methods: {
    /**
     * 
     * @param {*} item 
     */
    userClick(item) {
      if (!item.value) {
        item.data = [];
      }
      this.user.item = item;
      this.user.title = item.title;
      this.user.chosen = item.data ? item.data : [];
      this.user.multiple = item.options.multiple;
      this.user.visible = true;
    },

    /**
     * 用户选择
     * @param {} data 
     */
    chosenUserOk(data) {
      this.user.item.data = [];
      this.user.item.value = data.map(m => m.name).join(',')
      data.forEach(f => {
        this.user.item.data.push({
          id: f.userId
        });
      })
      this.user.visible = false;

      this.changeSearch();
    },

    /**
    * 
    * @param {*} item 
    */
    organizationClick(item) {
      if (!item.value) {
        item.data = [];
      }
      this.organization.item = item;
      this.organization.title = item.title;
      this.organization.chosen = item.data ? item.data : [];
      this.organization.multiple = item.options.multiple;
      this.organization.visible = true;
    },
    /**
        * 选中组织
        */
    chosenOrganizationOk(data) {
      this.organization.item.data = [];
      this.organization.item.value = data.map(m => m.title).join(',')
      data.forEach(f => {
        this.organization.item.data.push({
          id: f.key
        });
      })
      this.organization.visible = false;

      this.changeSearch();
    },
    /**
     * 关闭
     */
    toggleAdvanced() {
      this.advancedIndex = this.advanced
        ? this.option.num - 1
        : this.option.config.length;
      this.advanced = !this.advanced;
      this.$emit("advanced", this.advanced);
    },
    /**
     * 重置
     */
    reset() {
      this.option.config.forEach((element) => {
        element.value = undefined;
      });
      this.search();
    },

    /**
     * 改变后触发查询
     */
    changeSearch() {
      if (!this.option.searchButton) {
        this.search();
      }
    },

    /**
     * 改变查询字典值
     */
    changeSearchDictionary() { },

    /**
     * 查询
     */
    search() {
      var filters = this.getFilters();
      this.$emit("search", JSON.stringify(filters));
    },

    /**
     * 获取筛选
     */
    getFilters() {
      let filters = {
        groupOp: "AND",
        rules: [],
        groups: []
      };
      let that = this;
      this.option.config.forEach((element) => {
        var field = element.field,
          op = element.op,
          data = element.value;
        if (field && op && data) {
          switch (element.type) {
            case 'datetime':
              if (element.op == 'daterange') {
                if (data.length > 0) {
                  let dateBegin = moment(data[0]).format(element.options.format);
                  let dateEnd = moment(data[1]).format(element.options.format);
                  data = dateBegin + "|" + dateEnd;
                  filters.rules.push(that.getRules(field, op, data))
                }
              } else {
                data = moment(data).format(element.options.format);
                filters.rules.push(that.getRules(field, op, data))
              }

              break;
            case 'select':
            case 'dictionary':
            case 'correlationRecord':
              if (element.options.multiple) {
                var selectFilters = {
                  groupOp: "OR",
                  rules: [],
                }
                //循环值
                data.forEach(ditem => {
                  selectFilters.rules.push({
                    field,
                    op,
                    data: ditem
                  })
                })
                filters.groups.push(selectFilters)
              } else {
                filters.rules.push(that.getRules(field, op, data))
              }
              break;
            case 'user':
            case 'organization':
              if (element.options.multiple) {
                var selectFilters = {
                  groupOp: "OR",
                  rules: [],
                }
                //循环值
                element.data.forEach(ditem => {
                  selectFilters.rules.push({
                    field,
                    op,
                    data: ditem.id
                  })
                })
                filters.groups.push(selectFilters)
              } else {
                data = element.data[0].id
                filters.rules.push(that.getRules(field, op, data))
              }
              break;
            default:
              filters.rules.push(that.getRules(field, op, data))
              break;
          }
        }
      });
      return filters;
    },

    /**
     * 下拉查询
     */
    selectFocus(item) {
      let that = this;
      if (item.options.source.type == "dynamic" && item.options.source.format.length == 0) {
        var dynamicConfig = item.options.source.dynamicConfig;
        let formValue = {};
        this.option.config.forEach((element) => {
          var field = element.field,
            op = element.op,
            data = element.value;
          if (field && op && data) {
            switch (element.type) {
              case 'datetime':
                if (element.op == 'daterange') {
                  if (data.length > 0) {
                    let dateBegin = moment(data[0]).format(element.options.format);
                    let dateEnd = moment(data[1]).format(element.options.format);
                    data = dateBegin + "|" + dateEnd;
                  }
                } else {
                  data = moment(data).format(element.options.format);
                }
                break;
              default:
                break;
            }
            formValue[field] = data
          }
        });
        businessDataFormSource({
          dataSourceId: dynamicConfig.dataSourceId,
          inParams: JSON.stringify(dynamicConfig.inParams),
          formValue: JSON.stringify(formValue),
          sidx: dynamicConfig.sidx, //排序字段
          sord: dynamicConfig.sord, //排序方式
        }).then(result => {
          if (result.code === that.eipResultCode.success) {
            let dynamicData = [];
            result.data.forEach((res) => {
              dynamicData.push({
                value: res[dynamicConfig.value],
                key: res[dynamicConfig.key],
              });
            });
            item.options.source.format = dynamicData
          }
        });
      }
      else if (item.options.source.type == "api" && item.options.source.format.length == 0) { }
    },

    /**
    * 字典下拉
    */
    async selectDictionaryFocus(item) {
      let that = this;
      var result = (await dictionaryParentId(item.options.source.dictionaryId));
      if (result.code === that.eipResultCode.success) {
        let dynamicData = [];
        result.data.forEach((res) => {
          dynamicData.push({
            value: res.dictionaryId,
            name: res.name,
            label: res.name,
          });
        });
        item.options.source.format = dynamicData
      }
    },
    /**
   * 字典树下拉
   */
    async selectDictionaryTreeFocus(item) {
      var result = (await dictionaryQueryByIds({
        id: item.options.source.dictionaryId
      }));
      item.options.source.format = result.data
    },

    /**
     * 关联记录下拉
     */
    selectCorrelationRecordFocus(item) {
      let that = this;
      var columns = [{
        field: item.options.source.key,
      }, {
        field: item.options.source.value,
      }];
      queryTable({
        table: item.options.source.table,
        columns: JSON.stringify(columns),
        sidx: item.options.source.sidx,
        sord: item.options.source.sord,
      }).then(result => {
        if (result.code === that.eipResultCode.success) {
          let dynamicData = [];
          result.data.data.forEach((res) => {
            dynamicData.push({
              value: res[item.options.source.value],
              key: res[item.options.source.key],
            });
          });
          item.options.source.format = dynamicData
        }
      });
    },

    /**
     * 
     */
    getWhereFilter(conditions) {
      var rules = "";
      conditions.forEach((item) => {
        if (item.pcrelation) {
          var searchColumn = item.name,
            searchOper = item.op,
            searchString = item.condition;
          if (searchColumn && searchOper) {
            rules +=
              ',{"field":"' +
              searchColumn +
              '","op":"' +
              searchOper +
              '","data":"' +
              searchString +
              '"}';
          }
        }
      });

      rules && (rules = rules.slice(1));
      var filtersStr = '{"groupOp":"AND","rules":[' + rules + "]}";
      return filtersStr;
    },
    /**
     * 返回规则
     * @param {*} field 
     * @param {*} op 
     * @param {*} data 
     */
    getRules(field, op, data) {
      return {
        field,
        op,
        data
      }
    }
  },
};
</script>

<style lang="less" scoped>
.ant-form-item {
  margin-bottom: 0;
}

.ant-calendar-picker {
  min-width: 100% !important;
  width: 100% !important;
}

.ant-form-item {
  margin: 0 5px !important;
}
</style>
