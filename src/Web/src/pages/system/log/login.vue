<template>
  <splitpanes @resize="resize" class="default-theme" :style="{ height: split.height + 'px' }" :horizontal="true"
    :first-splitter="false">
    <pane :min-size="split.minHeight" :size="split.percentage">
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
        <vxe-table :loading="table.loading" ref="table" id="systemloglogin"
          :column-config="{ isCurrent: true, isHover: true }" :row-config="{ isCurrent: true, isHover: true }"
          :seq-config="{
            startIndex: (table.page.param.current - 1) * table.page.param.size,
          }" :height="split.tableHeight - 160 + 'px'" :export-config="{}" :print-config="{}" :data="table.data"
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
          <vxe-column field="createTime" title="登录时间" width="160"></vxe-column>
          <vxe-column field="createUserCode" title="登录名" width="150" showOverflow="ellipsis"></vxe-column>
          <vxe-column field="createUserName" title="名称" width="100" showOverflow="ellipsis"></vxe-column>
          <vxe-column field="loginOutTime" title="退出时间" width="160"></vxe-column>
          <vxe-column field="standingTime" title="停留时间(分钟)" width="120"></vxe-column>
          <vxe-column field="remoteIp" title="客户端IP" width="120"></vxe-column>
          <vxe-column field="remoteIpAddress" title="归属地址" width="150" showOverflow="ellipsis"></vxe-column>
          <vxe-column field="userAgent" title="浏览器代理" min-width="200" showOverflow="ellipsis"></vxe-column>
          <vxe-column field="osInfo" title="操作系统" width="100"></vxe-column>
          <vxe-column field="browser" title="浏览器" width="100"></vxe-column>
        </vxe-table>
        <a-pagination class="padding-top-sm float-right" v-model="table.page.param.current" show-size-changer
          show-quick-jumper :page-size-options="['20', '50', '100', '150', '200']"
          :show-total="(total) => `共 ${total} 条`" :page-size="table.page.param.size" :total="table.page.total"
          @change="tableInit" @showSizeChange="tableSizeChange"></a-pagination>
      </a-card>
    </pane>
    <pane :min-size="20" :size="100 - split.percentage">
      <a-card class="eip-admin-card-small">
        <div id="system-loginlog-list-analysis" :style="{ height: split.analysisHeight + 'px' }"></div>
      </a-card>
    </pane>
    <login-detail v-if="detail.visible" :visible.sync="detail.visible" :data="detail.data"></login-detail>
  </splitpanes>
</template>

<script>
import { query, analysis } from "@/services/system/log/login";
import loginDetail from "./login-detail";
import Vue from 'vue'
import { selectTableRow, deleteConfirm } from "@/utils/util";
import { Splitpanes, Pane } from 'splitpanes'
import 'splitpanes/dist/splitpanes.css'
export default {
  data() {
    return {
      split: {
        height: this.eipHeaderHeight() - 10,
        minHeight: 0,
        percentage: 50,
        tableHeight: 0,
        analysisHeight: 100,
      },

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
                field: "RemoteIpAddress",
                op: "cn",
                placeholder: "请输入归属地址",
                title: "归属地址",
                value: "",
                type: "text",
              },
              {
                field: "LoginOutTime",
                placeholder: "请输入退出时间",
                title: "退出时间",
                op: "daterange",
                type: "datetime",
                options: {
                  mode: 'date',
                  format: "YYYY-MM-DD",
                },
                value: []
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
        data: {},
        visible: false,
      },

      analysis: {
        echarts: {},
        option: {
          tooltip: {
            trigger: "axis",
          },
          legend: {
            data: ["登录次数"],
          },
          grid: {
            x: 50,
            y: 30,
            x2: 50,
            y2: 64,
          },
          toolbox: {
            show: true,
            feature: {
              mark: { show: true },
              dataView: { show: true, readOnly: false },
              magicType: { show: true, type: ["line", "bar"] },
              restore: { show: true },
              saveAsImage: { show: true },
            },
          },
          dataZoom: [
            {
              type: "slider",
              show: true,
              xAxisIndex: [0],
              start: 0,
              end: 9999, //初始化滚动条
            },
          ],
          xAxis: {
            type: "category",
            boundaryGap: false,
            data: [],
          },
          yAxis: {
            type: "value",
          },
          series: {
            data: [],
            name: "登录次数",
            type: "line",
            areaStyle: {},
          },
        },
      },
    };
  },
  components: {
    loginDetail,
    Splitpanes,
    Pane
  },
  created() {
    this.initHeight();
    setTimeout(() => {
      this.$refs.table.connect(this.$refs.toolbar);
      this.tableInit();
    }, 1000);
    
  },
  mounted() {
    this.$nextTick(function () {
      this.analysisInit();
    })
  },
  methods: {
    /**
     * 
     * @param {*} event 
     */
    resize(event) {
      this.split.percentage = event[0].size
      this.initHeight();
      this.analysis.echarts.resize();
    },
    /**
     * 初始化高度
     */
    initHeight() {
      this.split.minHeight = (350 / this.split.height) * 100;
      this.split.tableHeight = this.split.height * (this.split.percentage / 100);
      this.split.analysisHeight = (this.split.height - this.split.tableHeight);
    },
    //登录日志
    analysisInit() {
      let that = this;

      if (!this.$echarts) {
        Vue.prototype.$echarts = window.echarts;
      }

      this.analysis.echarts = this.$echarts.init(
        document.getElementById("system-loginlog-list-analysis")
      );
      analysis(that.table.page.param).then(function (result) {
        that.analysis.option.xAxis.data = result.analysis.xdata;
        that.analysis.option.series.data = result.analysis.ydata;
        that.analysis.echarts.setOption(that.analysis.option);
      });
    },

    /**
     * 列表查询
     */
    tableSearch(filters) {
      this.table.page.param.current = 1;
      this.table.page.param.filters = filters;
      this.tableInit();
      this.analysisInit();
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
      this.detail.data = row;
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
          that.detail.data = row;
          that.detail.visible = true;
        },
        that
      );
    },

    /**
     * 调整,收起展开高度
     */
    tableAdvanced(advanced) {
      var height = this.eipHeaderHeight() - (advanced ? 462 : 512);
      this.table.height = height < 300 ? 300 : height;
    },
  },
};
</script>