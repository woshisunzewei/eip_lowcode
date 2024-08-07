<template>
  <div style="width: 100%">
    <a-card class="eip-admin-card-small eip-admin-card-small-bottom-border" :bordered="false">
      <eip-search :option="table.search.option" @search="tableSearch" @advanced="tableAdvanced"></eip-search>
    </a-card>

    <a-card class="eip-admin-card-small" :bordered="false">
      <vxe-toolbar ref="toolbar" custom print export :refresh="{ query: tableInit }">
        <template v-slot:buttons>
          <a-space>
            <a-button type="primary" @click="detailClick">
              <a-icon type="eye" />查看
            </a-button>
            <a-button type="danger"> <a-icon type="delete" />删除 </a-button>
          </a-space>
        </template>
      </vxe-toolbar>
      <vxe-table :loading="table.loading" ref="table" id="systemlogoperation"
        :column-config="{ isCurrent: true, isHover: true }" :row-config="{ isCurrent: true, isHover: true }"
        :seq-config="{
          startIndex: (table.page.param.current - 1) * table.page.param.size,
        }" :height="table.height" :export-config="{}" :print-config="{}" :data="table.data"
        @cell-dblclick="showDetail">
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
        <vxe-column field="createTime" title="操作时间" width="160"></vxe-column>
        <vxe-column field="createUserCode" title="登录名" width="100" showOverflow="ellipsis"></vxe-column>
        <vxe-column field="createUserName" title="名称" width="100" showOverflow="ellipsis"></vxe-column>
        <vxe-column field="remark" title="描述" showOverflow="ellipsis" width="250"></vxe-column>
        <vxe-column field="url" title="地址" showOverflow="ellipsis" min-width="120"></vxe-column>
        <vxe-column field="actionExecutionTime" title="方法时间" min-width="120"></vxe-column>
        <vxe-column field="resultExecutionTime" title="页面展示时间" min-width="120"></vxe-column>
        <vxe-column field="responseStatus" title="响应状态" width="80"></vxe-column>
        <vxe-column field="requestType" title="请求类型" width="80"></vxe-column>
        <vxe-column field="controllerName" title="控制器" width="80" showOverflow="ellipsis"></vxe-column>
        <vxe-column field="actionName" title="方法" min-width="120" showOverflow="ellipsis"></vxe-column>
        <vxe-column field="remoteIp" title="客户端IP" width="120"></vxe-column>
        <vxe-column field="remoteIpAddress" title="归属地址" width="150" showOverflow="ellipsis"></vxe-column>
        <vxe-column field="userAgent" title="浏览器代理" min-width="200" showOverflow="ellipsis"></vxe-column>
        <vxe-column field="osInfo" title="操作系统" width="100"></vxe-column>
        <vxe-column field="browser" title="浏览器" width="100"></vxe-column>
      </vxe-table>
      <a-pagination class="padding-top-sm float-right" v-model="table.page.param.current" show-size-changer
        show-quick-jumper :page-size-options="table.page.sizeOptions" :show-total="(total) => `共 ${total} 条`"
        :page-size="table.page.param.size" :total="table.page.total" @change="tableInit"
        @showSizeChange="tableSizeChange"></a-pagination>
    </a-card>
    <operation-detail ref="detail" v-if="detail.visible" :visible.sync="detail.visible"
      :operationLogId="detail.operationLogId"></operation-detail>
  </div>
</template>

<script>
import { query } from "@/services/system/log/operation";
import operationDetail from "./operation-detail";

import { selectTableRow, deleteConfirm } from "@/utils/util";
export default {
  data() {
    return {
      table: {
        page: {
          param: {
            current: 1,
            size: this.eipPage.size,
            sord: "desc",
            sidx: "",
            filters: "",
          },
          total: 0,
          sizeOptions: this.eipPage.sizeOptions,
        },
        loading: true,
        data: [],
        height: window.innerHeight - 382,

        search: {
          option: {
            num: 6,
            config: [
              {
                field: "CreateUserCode",
                op: "cn",
                placeholder: "请输入登录名",
                title: "登录名",
                value: "",
                type: "text",
              },
              {
                field: "CreateUserName",
                op: "cn",
                placeholder: "请输入名称",
                title: "名称",
                value: "",
                type: "text",
              },
              {
                field: "ControllerName",
                op: "cn",
                placeholder: "请输入控制器",
                title: "控制器",
                value: "",
                type: "text",
              },
              {
                field: "ActionName",
                op: "cn",
                placeholder: "请输入方法",
                title: "方法",
                value: "",
                type: "text",
              },
              {
                field: "Url",
                op: "cn",
                placeholder: "请输入地址",
                title: "地址",
                value: "",
                type: "text",
              },
              {
                field: "ResponseStatus",
                op: "eq",
                placeholder: "请选择响应状态",
                title: "响应状态",
                type: "select",
                options: {
                  multiple: true, //多选
                  source: {
                    type: "custom",
                    format: [
                      { key: "200", value: "200" },
                      { key: "404", value: "404" },
                      { key: "500", value: "500" },
                    ],
                  },
                },
              },
              {
                field: "ResultExecutionTime",
                op: "cn",
                placeholder: "请输入页面展示时间",
                title: "页面展示时间",
                value: "",
                type: "text",
              },
              {
                field: "ResponseData",
                op: "cn",
                placeholder: "请输入响应数据",
                title: "响应数据",
                value: "",
                type: "text",
              },
              {
                field: "RequestType",
                op: "eq",
                title: "请求类型",
                placeholder: "请选择请求类型",
                type: "select",
                options: {
                  multiple: true, //多选
                  source: {
                    type: "custom",
                    format: [
                      { key: "POST", value: "POST" },
                      { key: "GET", value: "GET" },
                    ],
                  },
                },
              },
              {
                field: "RequestData",
                op: "cn",
                placeholder: "请输入请求数据",
                title: "请求数据",
                value: "",
                type: "text",
              },
              {
                field: "CreateTime",
                op: "daterange",
                placeholder: "请选择操作时间",
                title: "操作时间",
                type: "datetime",
                options: {
                  mode: 'date',
                  format: "YYYY-MM-DD",
                },
                value: []
              },
            ],
          },
        },
      },
      detail: {
        operationLogId: "",
        visible: false,
      },
    };
  },
  mounted() {
    this.$refs.table.connect(this.$refs.toolbar);
  },
  components: { operationDetail },
  created() {
    this.tableInit();
  },
  methods: {
    /**
     * 列表查询
     */
    tableSearch(filters) {
      this.table.page.param.current = 1;
      this.table.page.param.filters = filters;
      this.tableInit();
    },

    //初始化列表数据
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
    //详情
    showDetail({
      row,
      rowIndex,
      $rowIndex,
      column,
      columnIndex,
      $columnIndex,
      $event,
    }) {
      this.detail.operationLogId = row.operationLogId;
      this.detail.visible = true;
    },

    /**
     * 修改
     */
    detailClick() {
      let that = this;
      selectTableRow(
        that.$refs.table,
        function (row) {
          that.detail.operationLogId = row.operationLogId;
          that.detail.visible = true;
        },
        that
      );
    },

    /**
     * 调整,收起展开高度
     */
    tableAdvanced(advanced) {
      this.table.height = this.eipHeaderHeight() - (advanced ? 214 : 164);
    },
  },
};
</script>