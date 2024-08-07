<template>
  <div style="width: 100%">
    <a-card class="eip-admin-card-small eip-admin-card-small-bottom-border" :bordered="false">
      <eip-search :option="table.search.option" @search="tableSearch" @advanced="tableAdvanced"></eip-search>
    </a-card>

    <a-card class="eip-admin-card-small" :bordered="false">
      <vxe-toolbar ref="toolbar" custom print export :refresh="{ query: tableInit }">
        <template v-slot:buttons>
          <a-space>
            <a-button type="danger"> <a-icon type="delete" />删除 </a-button>
          </a-space>
        </template>
      </vxe-toolbar>
      <vxe-table :loading="table.loading" ref="table" id="systemlogoperation"
        :column-config="{ isCurrent: true, isHover: true }" :row-config="{ isCurrent: true, isHover: true }" :seq-config="{
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
        <vxe-column field="createTime" title="发送时间" width="160"></vxe-column>
        <vxe-column field="phone" title="手机号" width="120" showOverflow="ellipsis"></vxe-column>
        <vxe-column field="provide" title="短信服务商" width="100">
          <!-- <template #default="{ row }">
            <a-tag color="#f50" v-if="row.provide == 0">
              阿里云
            </a-tag>
            <a-tag color="#2db7f5" v-if="row.provide == 2">
              腾讯云
            </a-tag>
            <a-tag color="#87d068" v-if="row.provide == 4">
              凌凯
            </a-tag>
          </template> -->
        </vxe-column>
        <vxe-column field="code" title="短信代码" showOverflow="ellipsis" width="150"></vxe-column>
        <vxe-column field="message" title="发送信息" min-width="120" showOverflow="ellipsis"></vxe-column>
        <vxe-column field="isSend" title="发送标识" align="center" width="80" showOverflow="ellipsis">
          <template #default="{ row }">
            <a-tag color="#f50" v-if="!row.isSend">
              失败
            </a-tag>
            <a-tag color="#2db7f5" v-if="row.isSend">
              成功
            </a-tag>
          </template>
        </vxe-column>
      </vxe-table>
      <a-pagination class="padding-top-sm float-right" v-model="table.page.param.current" show-size-changer
        show-quick-jumper :page-size-options="table.page.sizeOptions" :show-total="(total) => `共 ${total} 条`"
        :page-size="table.page.param.size" :total="table.page.total" @change="tableInit"
        @showSizeChange="tableSizeChange"></a-pagination>
    </a-card>
    <sms-detail ref="detail" v-if="detail.visible" :visible.sync="detail.visible"
      :smsLogId="detail.smsLogId"></sms-detail>
  </div>
</template>

<script>
import { query } from "@/services/system/log/sms";
import smsDetail from "./sms-detail";
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
                field: "phone",
                op: "cn",
                placeholder: "请输入手机号",
                title: "手机号",
                value: "",
                type: "text",
              },

              {
                field: "provide",
                op: "eq",
                title: "短信服务商",
                placeholder: "请选择短信服务商",
                type: "select",
                options: {
                  multiple: true, //多选
                  source: {
                    type: "custom",
                    format: [
                      { key: "Aliyun", value: "阿里云" },
                      { key: "Tencent", value: "腾讯云" },
                      { key: "LingKai", value: "凌凯" },
                    ],
                  },
                },
              },
              {
                field: "isSend",
                op: "eq",
                title: "发送状态",
                placeholder: "请选择发送状态",
                type: "select",
                options: {
                  multiple: true, //多选
                  source: {
                    type: "custom",
                    format: [
                      { key: "true", value: "成功" },
                      { key: "false", value: "失败" },
                    ],
                  },
                },
              },
              // {
              //   field: "CreateTime",
              //   op: "cn",
              //   placeholder: "请选择操作时间",
              //   title: "操作时间",
              //   type: "datetime",
              //   value: "",
              //   format: "yyyy-MM-dd",
              // },
            ],
          },
        },
      },
      detail: {
        smsLogId: "",
        visible: false,
      },
    };
  },
  mounted() {
    this.$refs.table.connect(this.$refs.toolbar);
  },
  components: { smsDetail },
  created() {
    this.tableInit();
  },
  methods: {
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
      this.detail.smsLogId = row.smsLogId;
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
          that.detail.smsLogId = row.smsLogId;
          that.detail.visible = true;
        },
        that
      );
    },

    /**
     * 列表查询
     */
    tableSearch(filters) {
      this.table.page.param.current = 1;
      this.table.page.param.filters = filters;
      this.tableInit();
    },

    /**
     * 初始化列表数据
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
     * 调整,收起展开高度
     */
    tableAdvanced(advanced) {
      this.table.height = this.eipHeaderHeight() - (advanced ? 159 : 164);
    },
  },
};
</script>