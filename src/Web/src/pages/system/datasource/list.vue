<template>
  <div style="width: 100%">
    <a-card class="eip-admin-card-small eip-admin-card-small-bottom-border" :bordered="false">
      <eip-search v-if="!table.search.loading" :option="table.search.option" @search="tableSearch"></eip-search>
    </a-card>

    <a-card class="eip-admin-card-small" :bordered="false">
      <vxe-toolbar ref="toolbar" custom print export :refresh="{ query: tableInit }">
        <template v-slot:buttons>
          <eip-toolbar @del="del" @update="update" @add="add" @copy="copy" @onload="toolbarOnload"></eip-toolbar>
        </template>
      </vxe-toolbar>
      <vxe-table :loading="table.loading" ref="table" id="workflowbuttonlist" size="small" :seq-config="{
      startIndex: (table.page.param.current - 1) * table.page.param.size,
    }" :height="table.height" :export-config="{}" :print-config="{}" :data="table.data">
        <template #loading>
          <a-spin></a-spin>
        </template>
        <template #empty>
          <eip-empty />
        </template>
        <vxe-column type="checkbox" width="40" align="center" fixed="left">
          <template #header="{ checked, indeterminate }">
            <span @click.stop="$refs.table.toggleAllCheckboxRow()">
              <a-checkbox :indeterminate="indeterminate" :checked="checked">
              </a-checkbox>
            </span>
          </template>
          <template #checkbox="{ row, checked, indeterminate }">
            <span @click.stop="$refs.table.toggleCheckboxRow(row)">
              <a-checkbox :indeterminate="indeterminate" :checked="checked">
              </a-checkbox>
            </span>
          </template>
        </vxe-column>
        <vxe-column type="seq" title="序号" width="60"></vxe-column>
        <vxe-column field="name" title="名称" min-width="200"></vxe-column>
        <vxe-column field="type" title="数据源" width="100" align="center">
          <template v-slot="{ row }">
            <a-tag v-if="row.type == 'table'" color="#108ee9"> 表 </a-tag>
            <a-tag v-if="row.type == 'view'" color="#c21401"> 视图 </a-tag>
            <a-tag v-if="row.type == 'proc'" color="#90cf5b"> 存储过程 </a-tag>
            <a-tag v-if="row.type == 'interface'" color="#00af57"> 接口 </a-tag>
            <a-tag v-if="row.type == 'excel'" color="#ff1e02"> excel </a-tag>
            <a-tag v-if="row.type == 'sql'" color="#0071be"> sql </a-tag>
            <a-tag v-if="row.type == 'custom'" color="#00215f"> 自定义 </a-tag>
          </template>
        </vxe-column>
        <vxe-column field="dataBaseName" title="系统" width="150"></vxe-column>
        <vxe-column field="dataSourceNumber" title="使用数" width="80" align="center">
          <template v-slot="{ row }">
            <a-tag @click="dataSourceAgile(row)" color="#108ee9"> {{ row.dataSourceNumber }} </a-tag>
          </template>
        </vxe-column>
        <vxe-column field="remark" title="备注" min-width="150"></vxe-column>
        <vxe-column field="orderNo" title="排序" align="center" width="80"></vxe-column>
        <vxe-column field="isFreeze" title="禁用" align="center" width="80">
          <template v-slot="{ row }">
            <a-switch :checked="row.isFreeze" />
          </template>
        </vxe-column>
        <vxe-column field="createUserName" title="创建人" width="100"></vxe-column>
        <vxe-column field="createTime" title="创建时间" width="160"></vxe-column>
        <vxe-column field="updateUserName" title="修改人" width="100"></vxe-column>
        <vxe-column field="updateTime" title="修改时间" width="160"></vxe-column>
        <vxe-column title="操作" align="center" fixed="right" width="160px">
          <template #default="{ row }">
            <a-tooltip @click="tableUpdateRow(row)" title="编辑" v-if="visible.update">
              <label class="text-eip eip-cursor-pointer">编辑</label>
            </a-tooltip>
            <a-divider type="vertical" v-if="visible.update" />
            <a-tooltip title="删除" v-if="visible.del" @click="tableDelRow(row)">
              <label class="text-red eip-cursor-pointer">删除</label>
            </a-tooltip>
          </template>
        </vxe-column>
      </vxe-table>

      <a-pagination class="padding-top-sm float-right" v-model="table.page.param.current" show-size-changer
        show-quick-jumper :page-size-options="table.page.sizeOptions" :show-total="(total) => `共 ${total} 条`"
        :page-size="table.page.param.size" :total="table.page.total" @change="tableInit"
        @showSizeChange="tableSizeChange"></a-pagination>
    </a-card>

    <edit ref="edit" v-if="edit.visible" :visible.sync="edit.visible" :title="edit.title"
      :dataSourceId="edit.dataSourceId" :copy="edit.copy" @ok="operateStatus"></edit>

    <agile ref="agile" v-if="agile.visible" :visible.sync="agile.visible" :title="agile.title"
      :dataSourceId="agile.dataSourceId">
    </agile>
  </div>
</template>

<script>
import { query, del, dataBaseQuery } from "@/services/system/datasource/list";
import edit from "./edit";
import agile from "./agile";
import { selectTableRow, deleteConfirm } from "@/utils/util";
export default {
  data() {
    return {

      table: {
        page: {
          param: {
            current: 1,
            size: this.eipPage.size,
            sord: "asc",
            sidx: "OrderNo",
            filters: "",
          },
          total: 0,
          sizeOptions: this.eipPage.sizeOptions,
        },
        loading: true,
        data: [],
        height: this.eipHeaderHeight() - 162 + "px",
        search: {
          loading: true,
          option: {
            num: 6,
            config: [
              {
                field: "Name",
                op: "cn",
                placeholder: "请输入名称",
                title: "名称",
                value: "",
                type: "text",
              },
              {
                field: "Type",
                op: "eq",
                placeholder: "请选择数据源",
                title: "数据源",
                type: "select",
                options: {
                  multiple: true, //多选
                  source: {
                    type: "custom",
                    format: [
                      { key: "table", value: "表" },
                      { key: "view", value: "视图" },
                      { key: "proc", value: "存储过程" },
                      { key: "interface", value: "接口" },
                      { key: "excel", value: "excel" },
                      { key: "sql", value: "sql" },
                      { key: "custom", value: "自定义" },
                    ],
                  },
                },
              },
              {
                field: "DataBaseId",
                op: "eq",
                placeholder: "请选择系统",
                title: "系统",
                type: "select",
                options: {
                  multiple: true, //多选
                  source: {
                    type: "custom",
                    format: [

                    ]
                  },
                },
              },
              {
                field: "Remark",
                op: "cn",
                placeholder: "请输入备注",
                title: "备注",
                value: "",
                type: "text",
              },
            ],
          },
        },
      },
      visible: {
        update: true,
        del: true,
      },
      edit: {
        visible: false,
        dataSourceId: "",
        title: "",
        copy: false,
      },
      agile: {
        visible: false,
        dataSourceId: "",
        title: "",
      },
    };
  },
  components: { edit, agile },
  created() {
    this.dataBaseInit();
    this.tableInit();
  },
  mounted() {
    this.$refs.table.connect(this.$refs.toolbar);
  },
  methods: {
    /**
     * 初始化数据
     */
    dataBaseInit() {
      let that = this;
      that.table.search.loading = true;
      dataBaseQuery({
        current: 1,
        size: this.eipPage.size,
        sord: "asc",
        sidx: "OrderNo",
        filters: "",
      }).then(function (result) {
        if (result.code == that.eipResultCode.success) {
          result.data.forEach(element => {
            that.table.search.option.config[2].options.source.format.push({
              key: element.dataBaseId, value: element.name
            })
          });
        }
        that.table.search.loading = false;
      });
    },

    /**
     * 类别查询
     */
    tableSearch(filters) {
      this.table.page.param.current = 1;
      this.table.page.param.filters = filters;
      this.tableInit();
    },

    /**
     * 
     */
    tableInit() {
      let that = this;
      that.table.loading = true;
      query(that.table.page.param).then(function (result) {
        if (result.code == that.eipResultCode.success) {
          that.table.data = result.data;
          that.table.page.total = result.total;
        }
        that.table.loading = false;
      });
    },

    /**
     *数量改变
     */
    tableSizeChange(current, pageSize) {
      this.table.page.param.size = pageSize;
      this.tableInit();
    },

    /**
     * 修改
     */
    tableUpdateRow(row) {
      this.edit.dataSourceId = row.dataSourceId;
      this.edit.title = "类型配置-" + row.name;
      this.edit.visible = true;
      this.edit.copy = false;
    },

    /**
    * 占用情况
    */
    dataSourceAgile(row) {
      this.agile.dataSourceId = row.dataSourceId;
      this.agile.title = "数据源使用情况-" + row.name;
      this.agile.visible = true;
    },

    /**
     * 删除
     */
    tableDelRow(row) {
      let that = this;
      deleteConfirm(
        "类型配置【" + row.name + "】" + that.eipMsg.delete,
        function () {
          that.$loading.show({ text: that.eipMsg.delloading });
          del({ id: row.dataSourceId }).then((result) => {
            that.$loading.hide();
            if (result.code == that.eipResultCode.success) {
              that.reload();
              that.$message.success(result.msg);
            } else {
              that.$message.error(result.msg);
            }
          });
        },
        that
      );
    },

    /**
     * 新增
     */
    add() {
      this.edit.dataSourceId = null;
      this.edit.title = "新增类型配置";
      this.edit.visible = true;
      this.edit.copy = false;
    },

    /**
     * 修改
     */
    update() {
      let that = this;
      selectTableRow(
        that.$refs.table,
        function (row) {
          that.edit.dataSourceId = row.dataSourceId;
          that.edit.title = "类型配置-" + row.name;
          that.edit.visible = true;
          that.edit.copy = false;
        },
        that
      );
    },

    /**
     * 删除
     */
    del() {
      let that = this;
      selectTableRow(
        that.$refs.table,
        function (rows) {
          //提示是否删除
          deleteConfirm(
            that.eipMsg.delete,
            function () {
              //加载提示
              that.$loading.show({ text: that.eipMsg.delloading });
              let ids = that.$utils.map(rows, (item) => item.dataSourceId);
              del({ id: ids.join(",") }).then((result) => {
                that.$loading.hide();
                if (result.code == that.eipResultCode.success) {
                  that.reload();
                  that.$message.success(result.msg);
                } else {
                  that.$message.error(result.msg);
                }
              });
            },
            that
          );
        },
        that,
        false
      );
    },
    /**
     * 复制
     */
    copy() {
      let that = this;
      selectTableRow(
        that.$refs.table,
        function (row) {
          that.edit.dataSourceId = row.dataSourceId;
          that.edit.title = "模块复制-" + row.name;
          that.edit.visible = true;
          that.edit.copy = true;
        },
        that
      );
    },
    //提示状态信息
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
      this.tableInit();
    },

    /**
     * 权限按钮加载完毕
     */
    toolbarOnload(buttons) {
      this.visible.update =
        buttons.filter((f) => f.method == "update").length > 0;
      this.visible.del = buttons.filter((f) => f.method == "del").length > 0;
    },
  },
};
</script>