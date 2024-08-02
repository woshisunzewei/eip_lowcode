<template>
  <div>
    <div v-if="record.options.showType == 'dialog'" @click="onChosen"
      :style="{ 'cursor': record.options.disabled ? 'not-allowed' : '' }">
      <a-input :disabled="record.options.disabled || parentDisabled" :size="record.options.size" v-model="data"
        :placeholder="record.options.placeholder">
        <a-icon slot="addonAfter" type="search" :style="{ 'color': 'rgba(0, 0, 0, 0.25)' }" />
      </a-input>
    </div>

    <a-select show-search optionFilterProp="title" option-label-prop="title" @popupScroll="handlePopupScroll"
      v-else-if="record.options.showType == 'select'" :mode="record.options.multiple ? 'multiple' : ''"
      :disabled="record.options.disabled || parentDisabled" :size="record.options.size" v-model="data"
      :placeholder="record.options.placeholder" @change="selectChange" :allowClear="record.options.clearable">
      <a-select-option v-for="item in options" :key="item.value" :value="item.value" :title="item.title">
        {{ item.label }}
      </a-select-option>

    </a-select>

    <a-drawer v-if="modal.visible && record.options.dialog.type == 'drawer'"
      :bodyStyle="{ padding: '1px', 'background-color': '#f0f2f5' }"
      :width="record.options.dialog.width + record.options.dialog.widthUnit" :visible="modal.visible"
      :title="record.options.placeholder" :maskClosable="record.options.dialog.closable"
      :zIndex="record.options.dialog.zIndex" @close="modal.visible = false" :destroyOnClose="true">
      <div class="eip-drawer-body">
        <a-card class="margin-bottom-xs eip-admin-card-small">
          <eip-search :option="table.search.option" @search="search" v-if="record.options.search.columns.length > 0"
            @advanced="advanced"></eip-search>
          <vxe-table ref="datalistTable" :loading="table.loading" v-if="table.loaded"
            :column-config="{ isCurrent: true, isHover: true }" :row-config="{ isCurrent: true, isHover: true }"
            :seq-config="{
              startIndex:
                (table.page.param.current - 1) * table.page.param.size,
            }" :height="table.height" :export-config="{}" :print-config="{}" :data="table.data" @cell-click="cellClick"
            @cell-dblclick="cellDbClick">
            <template #loading>
              <a-spin></a-spin>
            </template>
            <template #empty>
              <eip-empty />
            </template>
            <vxe-column :type="record.options.multiple ? 'checkbox' : 'radio'" width="60" align="center" fixed="left">
              <template #header="{ checked, indeterminate }" v-if="record.options.multiple">
                <span @click.stop="$refs.datalistTable.toggleAllCheckboxRow()">
                  <a-checkbox :indeterminate="indeterminate" :checked="checked">
                  </a-checkbox>
                </span>
              </template>
              <template #checkbox="{ row, checked, indeterminate }">
                <span @click.stop="$refs.datalistTable.toggleCheckboxRow(row)">
                  <a-checkbox :indeterminate="indeterminate" :checked="checked">
                  </a-checkbox>
                </span>
              </template>
              <template #radio="{ row, checked }">
                <span @click.stop="$refs.datalistTable.toggleCheckboxRow(row)">
                  <a-radio :checked="checked">
                  </a-radio>
                </span>
              </template>
            </vxe-column>
            <vxe-column type="seq" title="序号" width="60"></vxe-column>

            <vxe-column v-for="(item, i) in table.columns" :key="i" :field="item.field" :title="item.title"
              :width="item.width" :align="item.align" :sortable="item.sort" showOverflow="ellipsis">
              <template v-slot="{ row }">
                <run-list-content :row="row" :item="item"></run-list-content>
              </template>
            </vxe-column>
          </vxe-table>

          <a-pagination class="padding-top-sm float-right" v-model="table.page.param.current" show-size-changer
            show-quick-jumper :page-size-options="table.page.sizeOptions" :show-total="(total) => `共 ${total} 条`"
            :page-size="table.page.param.size" :total="table.page.total" @change="initData"
            @showSizeChange="sizeChange"></a-pagination>
        </a-card>
      </div>

      <div class="eip-drawer-toolbar">
        <a-space>
          <a-button key="back" @click="modal.visible = false"> 取消 </a-button>
          <a-button key="submit" @click="ok" type="primary">
            确定
          </a-button></a-space>
      </div>
    </a-drawer>

    <a-modal v-if="modal.visible && record.options.dialog.type == 'modal'"
      :bodyStyle="{ padding: '1px', 'background-color': '#f0f2f5' }"
      :width="record.options.dialog.width + record.options.dialog.widthUnit" :visible="modal.visible"
      :title="record.options.placeholder" :centered="record.options.dialog.centered"
      :maskClosable="record.options.dialog.closable" :zIndex="record.options.dialog.zIndex"
      @cancel="modal.visible = false">
      <a-card class="margin-bottom-xs eip-admin-card-small">
        <eip-search :option="table.search.option" @search="search" v-if="record.options.search.columns.length > 0"
          @advanced="advanced"></eip-search>
        <vxe-table ref="datalistTable" :loading="table.loading" v-if="table.loaded"
          :column-config="{ isCurrent: true, isHover: true }" :row-config="{ isCurrent: true, isHover: true }"
          :seq-config="{
            startIndex:
              (table.page.param.current - 1) * table.page.param.size,
          }" :height="table.height" :export-config="{}" :print-config="{}" :data="table.data" @cell-click="cellClick"
          @cell-dblclick="cellDbClick">
          <template #loading>
            <a-spin></a-spin>
          </template>
          <template #empty>
            <eip-empty />
          </template>
          <vxe-column :type="record.options.multiple ? 'checkbox' : 'radio'" width="60" align="center" fixed="left">
            <template #header="{ checked, indeterminate }" v-if="record.options.multiple">
              <span @click.stop="$refs.datalistTable.toggleAllCheckboxRow()">
                <a-checkbox :indeterminate="indeterminate" :checked="checked">
                </a-checkbox>
              </span>
            </template>
            <template #checkbox="{ row, checked, indeterminate }">
              <span @click.stop="$refs.datalistTable.toggleCheckboxRow(row)">
                <a-checkbox :indeterminate="indeterminate" :checked="checked">
                </a-checkbox>
              </span>
            </template>
            <template #radio="{ row, checked }">
              <span @click.stop="$refs.datalistTable.toggleCheckboxRow(row)">
                <a-radio :checked="checked">
                </a-radio>
              </span>
            </template>
          </vxe-column>
          <vxe-column type="seq" title="序号" width="60"></vxe-column>

          <vxe-column v-for="(item, i) in table.columns" :key="i" :field="item.field" :title="item.title"
            :width="item.width" :align="item.align" :sortable="item.sort" showOverflow="ellipsis">
            <template v-slot="{ row }">
              <run-list-content :row="row" :item="item"></run-list-content>
            </template>
          </vxe-column>
        </vxe-table>

        <a-pagination class="padding-top-sm float-right" v-model="table.page.param.current" show-size-changer
          show-quick-jumper :page-size-options="table.page.sizeOptions" :show-total="(total) => `共 ${total} 条`"
          :page-size="table.page.param.size" :total="table.page.total" @change="initData"
          @showSizeChange="sizeChange"></a-pagination>
      </a-card>

      <template slot="footer">
        <a-space>
          <a-button key="back" @click="modal.visible = false"> 取消 </a-button>
          <a-button key="submit" @click="ok" type="primary">
            确定
          </a-button></a-space>
      </template>
    </a-modal>
  </div>
</template>
<script>
import { query } from "@/services/system/agile/components/control/correlationRecord";
import runListContent from "@/pages/system/agile/run/components/list/content";

export default {
  components: {
    runListContent,
  },
  data() {
    return {
      options: [],
      data: undefined,
      modal: {
        bodyStyle: {
          padding: "2px",
        },
        visible: false,
        title: "",
      },

      //表格配置信息
      table: {
        page: {
          param: {
            current: 1,
            size: this.eipPage.size,
            sord: "desc",
            sidx: "",
            filters: "",
            isPaging: true, //是否分页
            dataSourceId: undefined,
            inParams: [],
            formValue: '',
            columns: '',
          },
          total: 0,
          sizeOptions: this.eipPage.sizeOptions,
        },
        sidx: [],
        height: this.record.options.dialog.type == 'modal' ? 500 : this.eipHeaderHeight() - 76,
        loading: false,
        loaded: false, //配置是否加载完成
        data: [],
        search: {
          option: {
            num: 6,
            config: [],
          },
        },
        columns: [],
      },
    };
  },
  props: ["record", "value", "formValue", "parentDisabled"],
  watch: {
    value(value) {
      if (this.record.options.showType == 'select') {
        this.data = value
      }

      else if (this.record.options.showType == 'dialog') {
        this.data = value.valueTxt
      }
    },
    'record.options.options': function (value) {
      this.options = value;
    }
  },
  methods: {
    /**
    * 调整,收起展开高度
    */
    advanced(advanced) {
      if (this.record.options.dialog.type == 'modal') {
        this.table.height = 500
      } else {
        var height = 115;
        if (advanced) {
          //判断具有几行
          var number =
            this.table.search.option.config.length / this.table.search.option.num;
          if (number % 1 === 0) {
            number += 1;
          } else {
            number = Math.ceil(number);
          }
          height = height + (number - 1) * 40;
        }
        this.table.height = this.eipHeaderHeight() - height;
      }
    },

    /**
     * 下拉列表滚动时的回调
     * @param {*} e 
     */
    handlePopupScroll(e) {
      const { target } = e;
      const { scrollTop, scrollHeight, clientHeight } = target;
      if (scrollTop + 2 + clientHeight >= scrollHeight) {
        console.log(e)
        // 已经到达底部，触发分页逻辑00                                       
        // todo 这里就可以触发加载下一页请求  具体函数根据当前业务需求来定
      }
    },
    /**
     * 选择
     */
    onChosen() {
      if (!this.record.options.disabled) {
        this.modal.visible = true;
        this.modal.title = this.record.options.placeholder;
        this.table.page.param.table = this.record.options.dynamicConfig.table;
        this.table.page.param.formValue = JSON.stringify(this.formValue);
        this.initConfig();
      }
    },

    /**
        * 双击事件
        */
    cellClick({
      row,
      rowIndex,
      $rowIndex,
      column,
      columnIndex,
      $columnIndex,
      $event,
    }) {
      if (this.record.options.rowClickConfirm) {
        this.record.options.selectRows = [row];
        let value = row[this.table.columns[0].field];
        this.$emit("change", {
          value: row['RelationId'],
          valueTxt: value
        });
        this.modal.visible = false;
      }
    },

    /**
     * 双击事件
     */
    cellDbClick({
      row,
      rowIndex,
      $rowIndex,
      column,
      columnIndex,
      $columnIndex,
      $event,
    }) {
      if (this.record.options.rowDoubleClickConfirm) {
        this.record.options.selectRows = [row];
        let value = row[this.table.columns[0].field];
        this.$emit("change", {
          value: row['RelationId'],
          valueTxt: value
        });
        this.modal.visible = false;
      }
    },

    /**
     * 下拉改变
     */
    selectChange(value, option) {
      this.$emit("change", value);
    },

    /**
     * 确认
     */
    ok() {
      if (this.record.options.multiple) {
        this.record.options.selectRows =
          this.$refs.datalistTable.getCheckboxRecords();
        let valuesTxt = [], values = [];
        this.record.options.selectRows.forEach((item) => {
          values.push(item['RelationId']);
          valuesTxt.push(item[this.table.columns[0].field]);
        });
        this.$emit("change",
          {
            value: values.join(","),
            valueTxt: valuesTxt.join(",")
          });
        this.modal.visible = false;
      } else {
        var row = this.$refs.datalistTable.getRadioRecord();
        this.record.options.selectRows = row ? [row] : [];
        let valueTxt = "";
        if (row) {
          valueTxt = row[this.table.columns[0].field];
        }
        this.$emit("change", {
          value: row['RelationId'],
          valueTxt: valueTxt
        });
        this.modal.visible = false;
      }
    },

    /**
     * 初始化配置
     */
    initConfig() {
      let that = this;
      that.table.loading = true;
      that.initSearch();
      that.initColumns();
      that.initData();
      that.table.loaded = true;
    },

    /**
     * 列表初始化
     */
    search(filters) {
      this.table.page.param.current = 1;
      this.table.page.param.filters = filters;
      this.initData();
    },

    /**
     * 渲染列
     */
    initColumns() {
      let that = this;
      that.table.sidx = [];
      that.table.columns = [];
      var fields = [];
      that.record.options.columns.forEach((item) => {
        if (item.isShow) {
          var column = {
            R: item.name,
            title: item.title,
            align: item.align,
            sort: item.isSort,
            width: item.width,
            style: item.style,
            format: item.format,
            mask: item.mask
          };
          that.table.columns.push(column);

          fields.push(item.name);
        }
        that.table.page.param.columns = JSON.stringify(that.table.columns);
        if (item.isSearch) {
          var have = fields.filter((f) => f == item.name).length;
          if (have == 0) {
            fields.push(item.name);
          }
        }
        if (item.sord) {
          that.table.sidx.push(item.name + " " + item.sordType);
        }
      });
    },

    /**
     * 初始化查询
     */
    initSearch() {
      let that = this;
      if (this.record.options.search.num) {
        that.table.search.option.num = this.record.options.search.num;
      }
      let config = [];
      this.record.options.search.columns.forEach((item) => {
        switch (item.type) {
          case "input":
            config.push({
              field: item.fields ? item.fields : item.field,
              op: item.op,
              placeholder: item.placeholder,
              title: item.title,
              value: "",
              type: "text",
            });
            break;
          case "number":
            config.push({
              field: item.fields ? item.fields : item.field,
              op: item.op,
              placeholder: item.placeholder,
              title: item.title,
              value: "",
              type: "number",
            });
            break;
          case "bool":
            config.push({
              field: item.fields ? item.fields : item.field,
              op: item.op,
              placeholder: item.placeholder,
              title: item.title,
              value: undefined,
              type: "bool",
              options: item.options
            });
            break;
          case "select":
            //得到具体配置
            var options = item.options;
            if (options) {
              switch (options.source.type) {
                case "dynamic":
                  config.push({
                    field: item.field.replace(/_Txt*$/, ''),
                    op: item.op,
                    placeholder: item.placeholder,
                    title: item.title,
                    type: "select",
                    options: {
                      multiple: options.multiple, //单选
                      source: {
                        type: "dynamic",
                        format: [],
                        dynamicConfig: options.source,
                      },
                    }
                  });
                  break;
                case "custom":
                  config.push({
                    field: item.field.replace(/_Txt*$/, ''),
                    op: item.op,
                    placeholder: item.placeholder,
                    title: item.title,
                    type: "select",
                    options: {
                      multiple: options.multiple, //单选
                      source: {
                        type: "custom",
                        format: options.source.custom,
                      },
                    }
                  });
                  break;
              }
            }
            break;
          case "datetime":
            switch (item.options.format) {
              case "YYYY":
                item.options.mode = 'year';
                break;
              case "YYYY-MM":
                item.options.mode = 'month';
                break;
              default:
                item.options.mode = 'date';
                break;
            }
            config.push({
              field: item.fields ? item.fields : item.field,
              op: item.op,
              open: false,
              placeholder: item.placeholder,
              title: item.title,
              value: item.op == 'daterange' ? [] : '',
              type: "datetime",
              options: item.options
            });
        }
      });

      that.table.search.option.config = config;
    },

    /**
     * 初始化列表数据
     */
    async initData() {
      let that = this;
      that.table.loading = true;
      that.table.page.param.sidx = that.table.sidx.join(",");
      var result = await query(that.table.page.param);
      if (result.code == that.eipResultCode.success) {
        let queryData = result.data;
        that.table.data = queryData.data;
        if (that.table.page.param.isPaging) {
          that.table.page.total = queryData.count;
        }
      } else {
        that.$message.error(result.msg);
      }
      that.table.loading = false;
    },

    /**
     *数量改变
     */
    sizeChange(current, pageSize) {
      this.table.page.param.size = pageSize;
    },

    /**
     *提示状态信息
     */
    operateStatus(result) {
      if (result.code === this.eipResultCode.success) {
        this.reload();
      }
    },

    /**
     * 重新加载
     */
    reload() {
      this.table.page.param.current = 1;
      this.initData();
    },
  },
};
</script>
<style lang="less" scoped>
/deep/ .ant-input-search {
  cursor: pointer;
}

/deep/ .ant-input[disabled] {
  pointer-events: none;
  color: rgba(0, 0, 0, 0.65);
  background-color: #fff;
  cursor: pointer;
  opacity: 1;
}

/deep/ .ant-input-affix-wrapper .ant-input-disabled~.ant-input-suffix .anticon {
  color: rgba(0, 0, 0, 0.25);
  cursor: pointer;
}

/deep/ .ant-form-item {
  margin-bottom: 0;
}
</style>