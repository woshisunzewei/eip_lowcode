<template>
  <div>
    <a-tabs
      default-active-key="1"
      style="width: 500px"
      size="small"
      @change="function () {}"
    >
      <a-tab-pane key="1">
        <span slot="tab">
          <a-icon type="copy" />
          组合查询
        </span>
        <div style="max-height: 400px; overflow-y: auto">
          <eip-search-pro-item
            :filters="filters"
            :columns="columns"
          ></eip-search-pro-item>
        </div>
        <div class="padding-xs" style="text-align: right">
          <a-space>
            <a-button size="small" type="primary" icon="search" @click="search">
              查询
            </a-button>

            <a-button size="small" type="primary" icon="save" @click="save">
              保存
            </a-button>
          </a-space>
        </div>
      </a-tab-pane>
      <a-tab-pane key="2">
        <span slot="tab">
          <a-icon type="user" />
          我的查询
        </span>
        <vxe-table
          :loading="table.loading"
          ref="table"
          :seq-config="{
            startIndex: (table.page.param.current - 1) * table.page.param.size,
          }"
          height="200px"
          :export-config="{}"
          :print-config="{}"
          :data="table.data"
          :sort-config="{
            trigger: 'cell',
            defaultSort: { field: 'orderNo', order: 'asc' },
            orders: ['desc', 'asc'],
          }"
        >
          <template #loading>
            <a-spin></a-spin>
          </template>
          <template #empty>
            <eip-empty />
          </template>

          <vxe-column type="seq" title="序号" width="60"></vxe-column>
          <vxe-column field="name" title="名称" width="150"></vxe-column>
          <vxe-column
            field="createTime"
            title="创建时间"
            width="148"
          ></vxe-column>
          <vxe-column title="操作" align="center" width="130px">
            <template #default="{ row }">
              <a-tooltip @click="tableSearch(row)" title="查询">
                <a-button size="small" type="primary" icon="search"></a-button>
              </a-tooltip>
              <a-divider type="vertical" />
              <a-tooltip @click="tableUpdateRow(row)" title="编辑名称">
                <a-button size="small" type="primary" icon="edit"></a-button>
              </a-tooltip>
              <a-divider type="vertical" />
              <a-popconfirm
                ok-text="确认"
                cancel-text="取消"
                @confirm="tableDelRow(row)"
              >
                <template slot="title"> 删除查询 </template>
                <a-tooltip title="删除">
                  <a-button size="small" type="danger" icon="delete"></a-button>
                </a-tooltip>
              </a-popconfirm>
            </template>
          </vxe-column>
        </vxe-table>

        <a-pagination
          class="padding-top-sm float-right"
          v-model="table.page.param.current"
          show-size-changer
          show-quick-jumper
          :page-size-options="table.page.sizeOptions"
          :show-total="(total) => `共 ${total} 条`"
          :page-size="table.page.param.size"
          :total="table.page.total"
          @change="initSearchTable"
          @showSizeChange="tableSizeChange"
        ></a-pagination>
      </a-tab-pane>
    </a-tabs>
    <a-modal
      width="400px"
      :zIndex="1020"
      v-drag
      :destroyOnClose="true"
      :maskClosable="false"
      :visible="searchSave.visible"
      title="条件保存"
      @cancel="searchSave.visible = false"
      @ok="saveOk"
    >
      <a-input v-model="searchSave.name" placeholder="请输入保存条件名称" />
    </a-modal>
  </div>
</template>
<script>
import moment from "moment";
import { newGuid } from "@/utils/util";
import { query, del, save } from "@/services/system/agile/run/search";
export default {
  name: "eip-search-pro",

  props: {
    columns: {
      type: Array,
    },
    menuId: null,
  },
  data() {
    return {
      filters: {
        groupOp: "AND",
        rules: [],
        groups: [],
      },
      searchSave: {
        visible: false,
        name: "",
        searchId: newGuid(),
      },

      table: {
        page: {
          param: {
            current: 1,
            size: this.eipPage.size,
            sord: "asc",
            sidx: "id",
            id: null,
            filters: "",
            menuId: this.menuId,
          },
          total: 0,
          sizeOptions: this.eipPage.sizeOptions,
        },
        loading: true,
        data: [],
      },
    };
  },
  mounted() {
    this.initSearchTable();
  },
  methods: {
    /**
     * 快捷查询
     * @param {*} row
     */
    tableUpdateRow(row) {
      this.searchSave.visible = true;
      this.searchSave.name = row.name;
      this.searchSave.searchId = row.searchId;
    },

    /**
     *
     * @param {*} row
     */
    async tableSearch(row) {
      this.filters = JSON.parse(row.config);
      this.search();
    },
    /**
     * 删除
     */
    tableDelRow(row) {
      let that = this;
      that.$message.loading("删除中,请稍等...", 0);
      del({ id: row.searchId }).then((result) => {
        that.$message.destroy();
        if (result.code == that.eipResultCode.success) {
          that.initSearchTable();
        }
        that.$message.success(result.msg);
      });
    },

    /**
     * 保存
     */
    save() {
      this.searchSave.visible = true;
    },

    /**
     * 初始化查询表格
     */
    initSearchTable() {
      let that = this;
      query(this.table.page.param).then((result) => {
        if (result.code == that.eipResultCode.success) {
          that.table.data = result.data;
          that.table.page.total = result.total;
          that.searchSave.visible = false;
        }
        that.table.loading = false;
      });
    },
    /**
     *数量改变
     */
    tableSizeChange(current, pageSize) {
      this.table.page.param.size = pageSize;
      this.initSearchTable();
    },

    /**
     * 保存
     */
    saveOk() {
      let that = this;
      if (!this.searchSave.name) {
        this.$message.error("请输入名称");
        return false;
      }
      that.searchSave.visible = false;
      that.$message.loading("保存中,请稍等...", 0);
      save({
        name: this.searchSave.name,
        menuId: this.menuId,
        config: JSON.stringify(this.filters),
        searchId: this.searchSave.searchId,
      }).then((result) => {
        that.$message.destroy();
        if (result.code == that.eipResultCode.success) {
          that.initSearchTable();
          that.$message.success(result.msg);
        } else {
          that.$message.error(result.msg);
        }
      });
    },
    /**
     * 查询
     */
    search() {
      let filters = this.getFilters();
      this.$emit("search", JSON.stringify(filters));
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
          switch (element.type) {
            case "correlationRecord":
              if (element.value.length > 0) {
                var selectFilters = {
                  groupOp: "OR",
                  rules: [],
                };
                //循环值
                element.value.forEach((ditem) => {
                  selectFilters.rules.push({
                    field: element.field.replace("_Txt", ""),
                    op: element.op,
                    data: ditem,
                  });
                });
                filters.groups.push(selectFilters);
              }
              break;
            default:
              filters.rules.push({
                field: element.field,
                op: element.op,
                data: element.data,
              });
              break;
          }
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
            switch (rule.type) {
              case "correlationRecord":
                if (rule.value.length > 0) {
                  var selectFilters = {
                    groupOp: "OR",
                    rules: [],
                  };
                  //循环值
                  rule.value.forEach((ditem) => {
                    selectFilters.rules.push({
                      field: rule.field.replace("_Txt", ""),
                      op: rule.op,
                      data: ditem,
                    });
                  });
                  group.groups.push(selectFilters);
                }
                break;
              default:
                group.rules.push({
                  field: rule.field,
                  op: rule.op,
                  data: rule.data,
                });
                break;
            }
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
  },
};
</script>
