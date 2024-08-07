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
      <vxe-table :loading="table.loading" ref="table" highlight-hover-rowid="systemlogexception"
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
        <vxe-column field="createTime" title="错误时间" width="160"></vxe-column>
        <vxe-column field="message" title="错误信息" min-width="120" showOverflow="ellipsis"></vxe-column>
        <vxe-column field="requestUrl" title="请求Url" width="150" showOverflow="ellipsis"></vxe-column>
        <vxe-column field="createUserCode" title="登录名" width="80"></vxe-column>
        <vxe-column field="createUserName" title="名称" width="100"></vxe-column>
        <vxe-column field="httpMethod" title="请求方式" width="80"></vxe-column>
        <vxe-column field="requestData" title="请求数据" width="150" showOverflow="ellipsis"></vxe-column>
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
    <exception-detail ref="detail" v-if="detail.visible" :visible.sync="detail.visible"
      :exceptionLogId="detail.exceptionLogId"></exception-detail>
  </div>
</template>

<script>
import { query } from "@/services/system/log/exception";
import exceptionDetail from "./exception-detail";

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
                field: "Message",
                op: "cn",
                placeholder: "请输入错误信息",
                title: "错误信息",
                value: "",
                type: "text",
              },
              {
                field: "RequestUrl",
                op: "cn",
                placeholder: "请输入请求Url",
                title: "请求Url",
                value: "",
                type: "text",
              },
              {
                field: "StackTrace",
                op: "cn",
                placeholder: "请输入堆栈信息",
                title: "堆栈信息",
                value: "",
                type: "text",
              },
              {
                field: "InnerException",
                op: "cn",
                placeholder: "请输入内部信息",
                title: "内部信息",
                value: "",
                type: "text",
              },
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
                field: "HttpMethod",
                op: "eq",
                title: "请求方式",
                placeholder: "请选择请求方式",
                type: "select",
                options: {
                  multiple: true,
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
                op: "eq",
                placeholder: "请输入请求数据",
                title: "请求数据",
                value: "",
                type: "text",
              },
              {
                field: "RemoteIpAddress",
                op: "cn",
                placeholder: "请输入地址",
                title: "地址",
                value: "",
                type: "text",
              },
              {
                field: "Navigator",
                op: "cn",
                placeholder: "请输入浏览器",
                title: "浏览器",
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
        exceptionLogId: "",
        visible: false,
      },
    };
  },
  components: { exceptionDetail },
  created() {
    this.tableInit();
  },
  mounted() {
    this.$refs.table.connect(this.$refs.toolbar);
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
    /**
     * 详情
     * @param {*} param0
     */
    showDetail({
      row,
      rowIndex,
      $rowIndex,
      column,
      columnIndex,
      $columnIndex,
      $event,
    }) {
      this.detail.exceptionLogId = row.exceptionLogId;
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
          that.detail.exceptionLogId = row.exceptionLogId;
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