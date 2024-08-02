<template>
  <div class="table-panel">
    <splitpanes
      @resize="resizeFilterSearch"
      class="default-theme"
      :style="{ height: split.height }"
      :first-splitter="false"
    >
      <pane
        :min-size="10"
        v-if="agile.config.style.filter"
        :size="table.filterSearch.width"
        class="padding-xs"
        :style="{ height: split.height + 'px', 'background-color': '#ffffff' }"
      >
        <a-spin :spinning="table.filterSearch.spinning">
          <a-input-search
            allow-clear
            :placeholder="table.filterSearch.title + '关键字'"
            v-model="table.filterSearch.key"
          />

          <a-menu
            @select="filterSearchSelect"
            @deselect="filterSearchSelect"
            :default-selected-keys="['0']"
            mode="inline"
            v-if="
              filterSearchList.length > 0 &&
              !['Organization', 'TreeSelect'].includes(
                table.filterSearch.format
              )
            "
            :multiple="agile.config.style.filterMultiple"
          >
            <a-menu-item key="0">
              <div class="flex justify-between align-center menu-item">
                <span>全部</span>
                <a-badge
                  :count="
                    filterSearchList.reduce((sum, num) => sum + num.count, 0)
                  "
                >
                </a-badge>
              </div>
            </a-menu-item>
            <a-menu-item :key="item.field" v-for="item in filterSearchList">
              <div class="flex justify-between align-center menu-item">
                <span>{{ item.field_Txt }}</span>
                <a-badge :count="item.count"> </a-badge>
              </div>
            </a-menu-item>
          </a-menu>

          <div
            class="padding-xs"
            v-if="
              filterSearchList.length > 0 &&
              ['Organization', 'TreeSelect'].includes(table.filterSearch.format)
            "
          >
            <a-directory-tree
              :checkable="agile.config.style.filterMultiple"
              :showLine="agile.config.style.filterShowLine"
              :defaultExpandAll="agile.config.style.filterExpandAll"
              :defaultCheckedKeys="table.filterSearch.select"
              style="overflow: auto"
              :style="{ height: split.height - 54 + 'px' }"
              :tree-data="filterSearchList"
              :expandAction="false"
              @check="filterSearchCheckOrganization"
              @select="filterSearchCheckOrganization"
              ref="filterTree"
            ></a-directory-tree>
          </div>
          <eip-empty v-if="filterSearchList.length == 0" />
        </a-spin>
      </pane>

      <pane :min-size="50" :size="100 - table.filterSearch.width">
        <a-card
          class="eip-admin-card-small eip-admin-card-small-bottom-border"
          :bordered="false"
          v-if="table.search.option.config.length > 0"
        >
          <eip-search
            ref="eipSearch"
            :option="table.search.option"
            @search="search"
            @advanced="advanced"
          ></eip-search>
        </a-card>

        <splitpanes
          @resize="resize"
          class="default-theme"
          :horizontal="agile.config.style.subtableLayout == 'topBottom'"
          :style="{ height: split.height, 'background-color': '#ffffff' }"
          :first-splitter="false"
        >
          <pane :min-size="split.minHeight" :size="split.percentage">
            <a-card
              class="eip-admin-card-small"
              :loading="card.loading"
              :bordered="false"
            >
              <vxe-toolbar
                ref="runToolbar"
                custom
                print
                :refresh="{ query: initData }"
              >
                <template v-slot:buttons>
                  <a-space>
                    <template v-for="(button, index) in buttons">
                      <a-tooltip :title="button.remark">
                        <a-button
                          :shape="button.shape"
                          v-if="button.style != 'upload' && button.shape"
                          :type="button.style"
                          :key="index"
                          @click="buttonClick(button)"
                        >
                          <a-icon
                            v-if="button.icon"
                            :type="button.icon"
                            :theme="button.theme"
                          />
                          {{ button.name }}</a-button
                        >

                        <a-button
                          v-if="button.style != 'upload' && !button.shape"
                          :type="button.style"
                          :key="index"
                          @click="buttonClick(button)"
                        >
                          <a-icon :type="button.icon" :theme="button.theme" />
                          {{ button.name }}</a-button
                        >
                        <a-upload
                          v-else-if="button.style == 'upload'"
                          :key="index"
                          accept="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel"
                          :showUploadList="false"
                          @change="uploadChange(button)"
                          :customRequest="uploadCustomRequest"
                        >
                          <a-button type="primary" v-if="button.shape">
                            <a-icon :type="button.icon" :theme="button.theme" />
                            {{ button.name }}
                          </a-button>

                          <a-button type="primary" v-else>
                            <a-icon :type="button.icon" :theme="button.theme" />
                            {{ button.name }}
                          </a-button>
                        </a-upload>
                      </a-tooltip>
                    </template>
                  </a-space>
                </template>
                <template #tools>
                  <a-space>
                    <a-popover trigger="click" placement="bottomRight">
                      <template slot="content">
                        <eip-search-pro
                          ref="eipSearchPro"
                          :menuId="menuId"
                          :columns="table.columns"
                          @search="search"
                        ></eip-search-pro>
                      </template>
                      <a-tooltip title="组合查询">
                        <a-button shape="circle" icon="search" />
                      </a-tooltip>
                    </a-popover>

                    <a-tooltip title="回收站">
                      <a-button
                        shape="circle"
                        @click="recycleBin.visible = true"
                        icon="delete"
                      />
                    </a-tooltip>
                    <a-dropdown class="margin-right-sm">
                      <a-menu slot="overlay" @click="exportMenuClick">
                        <a-menu-item key="1">
                          <a-icon type="file-excel" />默认导出
                        </a-menu-item>
                        <a-menu-item key="2">
                          <a-icon type="file-excel" />导出选中</a-menu-item
                        >
                        <a-menu-item key="3">
                          <a-icon type="file-excel" />高级导出
                        </a-menu-item>
                        <a-menu-item key="4">
                          <a-icon type="file-excel" />全量导出
                        </a-menu-item>
                      </a-menu>
                      <a-button shape="circle" icon="download" />
                    </a-dropdown>
                  </a-space>
                </template>
              </vxe-toolbar>

              <vxe-table
                v-if="table.loaded"
                :loading="table.loading"
                ref="runTable"
                :border="agile.config.style.border"
                :column-config="{
                  isCurrent: agile.config.style.columnIsCurrent,
                  isHover: true,
                }"
                :show-footer="table.columns.filter((f) => f.total).length > 0"
                :row-config="{
                  isCurrent: agile.config.style.rowIsCurrent,
                  isHover: true,
                  height: agile.config.style.rowHeight,
                  keyField: 'RelationId',
                }"
                :seq-config="{
                  startIndex:
                    (table.seqConfig.startIndex - 1) * table.page.param.size,
                }"
                :stripe="agile.config.style.stripe"
                :export-config="{
                  filename: table.title,
                }"
                :height="table.height"
                :print-config="{}"
                :data="table.data"
                :footer-method="returnDataFooter"
                :expand-config="{
                  accordion: true,
                  lazy: true,
                  toggleMethod: toggleExpand,
                  loadMethod: loadExpand,
                }"
                :tree-config="
                  agile.config.style.showTree
                    ? {
                        expandAll: agile.config.style.showTreeExpandAll,
                        accordion: agile.config.style.showTreeAccordion,
                        showLine: agile.config.style.showTreeLine,
                        transform: true,
                        rowField: agile.config.style.showTreeSon,
                        parentField: agile.config.style.showTreeParent.replace(
                          '_Txt',
                          ''
                        ),
                        reserve: true,
                        trigger: 'row',
                      }
                    : {}
                "
                :tooltip-config="tooltipConfig"
                @cell-dblclick="cellDbClick"
                @cell-click="cellClick"
                @resizable-change="resizableChange"
                @sort-change="tableSort"
              >
                <template #loading>
                  <a-spin></a-spin>
                </template>

                <template #empty>
                  <eip-empty />
                </template>
                <vxe-column
                  v-if="agile.config.style.select != 'null'"
                  :type="agile.config.style.select"
                  width="40"
                  align="center"
                  fixed="left"
                >
                  <template #header="{ checked, indeterminate }">
                    <span
                      v-if="agile.config.style.select == 'checkbox'"
                      @click.stop="$refs.runTable.toggleAllCheckboxRow()"
                    >
                      <a-checkbox
                        :indeterminate="indeterminate"
                        :checked="checked"
                      >
                      </a-checkbox>
                    </span>
                  </template>

                  <template #checkbox="{ row, checked, indeterminate }">
                    <span @click.stop="$refs.runTable.toggleCheckboxRow(row)">
                      <a-checkbox
                        :indeterminate="indeterminate"
                        :checked="checked"
                      >
                      </a-checkbox>
                    </span>
                  </template>

                  <template #radio="{ row, checked }">
                    <span @click.stop="$refs.runTable.toggleCheckboxRow(row)">
                      <a-radio :checked="checked"> </a-radio>
                    </span>
                  </template>
                </vxe-column>
                <vxe-column type="seq" width="50" align="center">
                  <template #header="{}">
                    <a-tooltip v-if="haveCard">
                      <template slot="title"> 改变列宽后保存 </template>
                      <a-icon @click="saveResizableChange" type="save"></a-icon>
                    </a-tooltip>
                    <span v-else>序号</span>
                  </template>
                </vxe-column>

                <template v-for="(item, i) in table.columns">
                  <vxe-colgroup
                    :key="i"
                    align="center"
                    v-if="item.header.columns.length > 0"
                    :title="item.header.title"
                  >
                    <vxe-column
                      :key="headerIndex"
                      v-for="(headerItem, headerIndex) in item.header.columns"
                      :field="headerItem.field"
                      :title="headerItem.title"
                      :width="headerItem.width"
                      :align="headerItem.align"
                      :sortable="headerItem.sort"
                      showOverflow="ellipsis"
                      :type="
                        headerItem.subtable.length > 0 &&
                        agile.config.style.subtableLayout == 'tree'
                          ? 'expand'
                          : ''
                      "
                    >
                      <template #header="{}">
                        <a-tooltip>
                          <template slot="title"> 改变列宽后保存 </template>
                          <a-icon type="save"></a-icon>
                        </a-tooltip>
                      </template>

                      <template v-slot="{ row }">
                        <run-list-content
                          :row="row"
                          :item="headerItem"
                          @batchDetail="batchDetail"
                        ></run-list-content>
                      </template>

                      <template #content="{ row }">
                        <run-list-subtable
                          :data="row"
                          :item="headerItem"
                          :table="table"
                        ></run-list-subtable>
                      </template>
                    </vxe-column>
                  </vxe-colgroup>

                  <vxe-column
                    :key="i"
                    v-else
                    :field="item.field"
                    :tree-node="agile.config.style.showTree ? i == 0 : false"
                    :title="item.title"
                    :width="item.width"
                    :align="item.align"
                    :sortable="item.sort"
                    showOverflow="ellipsis"
                    :type="
                      item.subtable.length > 0 &&
                      agile.config.style.subtableLayout == 'tree'
                        ? 'expand'
                        : ''
                    "
                  >
                    <template #header>
                      <a-tooltip v-if="item.remark">
                        <template slot="title">
                          {{ item.remark }}
                        </template>
                        {{ item.title }}
                        <a-icon type="question-circle" />
                      </a-tooltip>
                      <span v-else>{{ item.title }}</span>
                    </template>

                    <template v-slot="{ row }">
                      <run-list-content
                        :row="row"
                        :item="item"
                        @batchDetail="batchDetail"
                      ></run-list-content>
                    </template>

                    <template #content="{ row }">
                      <run-list-subtable
                        v-if="
                          item.subtable.length > 0 &&
                          agile.config.style.subtableLayout == 'tree'
                        "
                        :data="row"
                        :item="item"
                        :table="table"
                      ></run-list-subtable>
                    </template>
                  </vxe-column>
                </template>
                <vxe-column
                  title="操作"
                  v-if="buttonsTable.data.length > 0"
                  align="center"
                  fixed="right"
                  :width="buttonsTable.width"
                >
                  <template #default="{ row }">
                    <a-tooltip
                      v-for="(button, index) in buttonsTable.data"
                      :title="button.name"
                      @click="tableButtonClick(button, row)"
                      :key="index"
                    >
                      <label
                        :class="
                          button.style == 'danger' ? 'text-red' : 'text-eip'
                        "
                        class="eip-cursor-pointer"
                        >{{ button.name }}</label
                      >
                      <a-divider
                        type="vertical"
                        v-if="index != buttonsTable.data.length - 1"
                      />
                    </a-tooltip>
                  </template>
                </vxe-column>
              </vxe-table>
              <a-pagination
                class="padding-top-sm float-right"
                v-model="table.page.param.current"
                v-if="agile.config.style.paging"
                show-size-changer
                show-quick-jumper
                :page-size-options="table.page.sizeOptions"
                :show-total="(total) => `共 ${total} 条`"
                :page-size="table.page.param.size"
                :total="table.page.total"
                @change="initData"
                @showSizeChange="sizeChange"
              ></a-pagination>
            </a-card>
          </pane>

          <pane
            :min-size="split.minHeight"
            :size="100 - split.percentage"
            v-if="
              agile.config.style.subtableLayout &&
              agile.config.style.subtableLayout != 'tree'
            "
          >
            <div
              v-for="(item, i) in table.columns.filter(
                (f) => f.subtable.length > 0
              )"
              :key="i"
            >
              <run-list-subtable
                :data="subtable.data"
                :item="item"
                :table="table"
                :height="split.subtableHeight"
              ></run-list-subtable>
            </div>
          </pane>
        </splitpanes>
      </pane>
    </splitpanes>

    <edit
      ref="edit"
      v-if="edit.visible"
      :visible.sync="edit.visible"
      :title="edit.title"
      :config="edit.config"
      :customerFormValue="customerFormValue"
      :automation="edit.automation"
      :fromType="fromType"
      :update="edit.update"
      :copy="edit.copy"
      :options="edit.options"
      :isWorkflow="edit.isWorkflow"
      :workflowData="workflow.data"
      :disabled="edit.disabled"
      @ok="operateStatus"
    ></edit>

    <run-list-batch
      ref="batch"
      v-if="batch.visible"
      :visible.sync="batch.visible"
      :title="batch.title"
      :configId="batch.configId"
      :relationId="batch.relationId"
      :model="batch.model"
    ></run-list-batch>

    <run-list-import-excel-comfirm
      ref="importExcelComfirm"
      v-if="importExcelComfirm.visible"
      @importBusinessData="importBusinessData"
      :visible.sync="importExcelComfirm.visible"
      :title="importExcelComfirm.title"
    ></run-list-import-excel-comfirm>

    <run-list-import-excel
      ref="importExcel"
      v-if="importExcel.visible"
      :visible.sync="importExcel.visible"
      :title="importExcel.title"
      :configId="importExcel.configId"
      :relationId="importExcel.relationId"
      :model="importExcel.model"
      :columns="table.columns"
      @reload="reload"
    ></run-list-import-excel>

    <run-list-recycle-bin
      ref="recycleBin"
      v-if="recycleBin.visible"
      :visible.sync="recycleBin.visible"
      :haveDataPermission="haveDataPermission"
      :configId="configId"
      :menuId="menuId"
      :name="name"
      @ok="operateStatus"
    ></run-list-recycle-bin>
  </div>
</template>

<script>
import { Splitpanes, Pane } from "splitpanes";
import "splitpanes/dist/splitpanes.css";

import Vue from "vue";
import Viewer from "v-viewer";
import "viewerjs/dist/viewer.css";
Vue.use(Viewer);
Viewer.setDefaults({
  Options: {
    inline: true, // 启用 inline 模式
    button: true, // 显示右上角关闭按钮
    navbar: true, // 显示缩略图导航
    title: true, // 显示当前图片的标题
    toolbar: true, // 显示工具栏
    tooltip: true, // 显示缩放百分比
    movable: true, // 图片是否可移动
    zoomable: true, // 图片是否可缩放
    //'rotatable': true, // 图片是否可旋转
    //'scalable': true, // 图片是否可翻转
    transition: true, // 使用 CSS3 过度
    //'fullscreen': true, // 播放时是否全屏
    keyboard: true, // 是否支持键盘
    url: "data-source", // 设置大图片的 url
  },
});
import { businessDataFormSource } from "@/services/system/agile/run/edit";
import moment from "moment";
import eipSearchPro from "@/components/eip/eip-search-pro";
import runListSubtable from "@/pages/system/agile/run/components/list/subtable";
import runListContent from "@/pages/system/agile/run/components/list/content";
import runListBatch from "@/pages/system/agile/run/components/list/batch";
import runListRecycleBin from "@/pages/system/agile/run/components/list/recycleBin";
import runListImportExcel from "@/pages/system/agile/run/components/import/excel";
import runListImportExcelComfirm from "@/pages/system/agile/run/components/list/importExcel";
import { menubutton } from "@/services/common/usercontrol/toolbar";
import edit from "./edit";
import { hiprint } from "../print/components/print";
import { chosenOrganization } from "@/services/common/usercontrol/chosenorganization";
import {
  workflowEngineRevokeByCreateUser,
  query,
  queryFooter,
  queryConfig,
  queryFilterSearch,
  del,
  eventApi,
  automationRun,
  reportData,
  reportDataTemplate,
  importData,
  importTemplate,
  menuAgile,
  businessDataById,
  businessDataBatch,
  findAgileById,
} from "@/services/system/agile/run/list";
import { mapMutations, mapGetters } from "vuex";
import { listPublic } from "@/services/system/agile/list/designer";
import {
  selectTableRow,
  deleteConfirm,
  selectTableRowRadio,
} from "@/utils/util";

export default {
  data() {
    return {
      split: {
        height: this.eipHeaderHeight() - (this.haveCard ? 40 : 0),
        minHeight: 0,
        percentage: 50, //比例
        subtableHeight: 0,
      },
      tooltipConfig: {
        showAll: true,
        enterable: true,
        contentMethod: ({ type, column, row, items, _columnIndex }) => {
          if (type == "header") {
            return "";
          }
          const { field } = column;
          if (row) {
            var columnFilter = this.table.columns.filter(
              (f) => f.field == field
            );
            if (columnFilter != null && columnFilter.length > 0) {
              var data = columnFilter[0];
              switch (data.format) {
                case "Map":
                  if (row[field]) {
                    return JSON.parse(row[field]).address;
                  }
                  break;
                case "File":
                case "Image":
                  if (row[field]) {
                    return "";
                  }
                  break;
              }
              if (data.style.length > 0) {
                var style = data.style.filter(
                  (f) => f.value == row[field.replace("_Txt", "")]
                );
                return style.length > 0 ? style[0].content : "";
              }
            }
            if (column.title == "操作") {
              return "";
            }

            var value = row[field];
            if (value) {
              return value;
            }
          }
          // 其余的单元格使用默认行为
          return null;
        },
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
            type: 0,
            rote: {
              menuId: this.menuId,
            },
            configId: this.configId,
            menuId: this.menuId,
            haveDataPermission: this.haveDataPermission, //是否具有数据权限
            isPaging: true,
            cols: [],
            reportName: "",
            timeStamp: "", //时间戳:可通过设置该值达到不读取缓存数据效果
          },

          total: 0,
          sizeOptions: this.eipPage.sizeOptions,
        },
        height: this.eipHeaderHeight() - (this.haveCard ? 158 : 108) + "px",
        loading: false,
        loaded: false, //配置是否加载完成
        data: [],
        footerData: [], //表尾数据
        search: {
          option: {
            labelCol: 8,
            wrapperCol: 16,
            num: 6,
            config: [],
          },
        },
        columns: [],
        columnExpand: {}, //点击展开字段信息
        columnLoadComplate: false, //是否加载配置完毕
        columnResizableChange: false, //是否重新改变了宽度

        subtableNames: {}, //子表名称
        title: "",

        seqConfig: {
          startIndex: 1,
        },
        advanced: true,

        filterSearch: {
          title: "",
          format: "",
          key: "", //查询关键字
          spinning: true,
          width: 10, //宽度
          select: [], //选中项目
          data: [], //筛选查询数据源
        },
      },
      //子表
      subtable: {
        data: {
          subtableDatas: [],
          subtableColumns: [],
        },
      },
      //敏捷开发配置信息
      agile: {
        configId: null, //配置Id
        config: this.$utils.clone(this.eipTableConfig, true),
      },

      buttons: [], //按钮
      buttonsTable: {
        data: [],
        width: "100px",
      }, //列表按钮宽度

      upload: {
        //上传按钮触发
        file: null,
      },

      //编辑组件
      edit: {
        configId: undefined,
        visible: false,
        title: null,
        copy: false,
        config: null, //编辑页面配置Id
        isWorkflow: false,
        automation: null,
        options: {}, //表单配置属性,如弹出宽度等配置
      },

      route: {
        name: this.name,
      },

      card: {
        loading: true,
      },

      //流程数据
      workflow: {
        data: {
          processId: this.menuId,
          processInstanceId: null,
          activityId: null,
          taskId: null,
          type: this.eipWorkflowDoType["审核"],
        },
      },

      //子表
      batch: {
        visible: false,
        title: null,
        configId: null, //编辑页面配置Id
        relationId: null, //关联Id
        model: null, //子表名称
      },

      interval: null,

      customerFormValue: null, //编辑自定义数据
      fromType: "", //来源类型:可用于通过不同来源类型,处理不同业务逻辑
      drawerWidth: "", //弹出框宽度

      //导入excel
      importExcel: {
        visible: false,
        title: null,
        configId: null, //编辑页面配置Id
        relationId: null, //关联Id
        model: null, //子表名称
      },

      //导入Excel确定
      importExcelComfirm: {
        visible: false,
        title: null,
      },

      //回收站
      recycleBin: {
        visible: false,
      },

      refreshDataInterval: null,
    };
  },
  props: {
    configId: {
      type: String,
    },
    menuId: {
      type: String,
    },

    name: {
      type: String,
    },

    haveCard: {
      type: Boolean,
      default: false,
    }, //是否为设计时界面
    haveDataPermission: {
      type: Boolean,
      default: false,
    },
  },
  beforeDestroy() {
    if (this.interval) {
      clearInterval(this.interval);
    }
    if (this.refreshDataInterval) {
      clearInterval(this.refreshDataInterval);
    }
  },
  watch: {
    //若有改变则赋予属性
    "edit.config.editConfigId"(val) {
      if (val) {
        this.setOptions();
      }
    },
  },
  components: {
    edit,
    runListSubtable,
    runListImportExcelComfirm,
    runListContent,
    runListBatch,
    runListImportExcel,
    runListRecycleBin,
    Splitpanes,
    Pane,
    eipSearchPro,
  },

  created() {
    const route = this.$route;
    if (!this.menuId) {
      this.route.name = route.name;
      this.table.page.param.menuId = route.meta.menuId;
      this.table.page.param.configId = route.meta.params.id;
    }
    this.initButton();
    this.initConfig();
  },
  computed: {
    ...mapGetters("account", ["systemAgile"]),
    filterSearchList() {
      if (
        ["Organization", "TreeSelect"].includes(this.table.filterSearch.format)
      ) {
        return this.$utils.searchTree(
          this.table.filterSearch.data,
          (item) => {
            if (this.table.filterSearch.key) {
              var titlePinyin = pinyin.convert(item.title).toLowerCase();
              if (
                item.title
                  .toLowerCase()
                  .indexOf(this.table.filterSearch.key.toLowerCase()) > -1
              ) {
                return true;
              } else if (
                titlePinyin.indexOf(this.table.filterSearch.key.toLowerCase()) >
                -1
              ) {
                return true;
              } else {
                return false;
              }
            } else {
              return true;
            }
          },
          { children: "children" }
        );
      } else {
        return this.table.filterSearch.data.filter((item) => {
          var titlePinyin = pinyin.convert(item.field_Txt).toLowerCase();
          if (
            item.field_Txt
              .toLowerCase()
              .indexOf(this.table.filterSearch.key.toLowerCase()) > -1
          ) {
            return true;
          } else if (
            titlePinyin.indexOf(this.table.filterSearch.key.toLowerCase()) > -1
          ) {
            return true;
          }
        });
      }
    },
  },
  methods: {
    /**
     * 列表排序改变
     */
    tableSort({ sortList }) {
      let that = this;
      //是否分页
      if (that.agile.config.style.paging) {
        that.table.page.param.current = 1;
        that.table.loading = true;
        that.table.page.param.isPaging = true;
        if (sortList.length > 0) {
          that.table.page.param.sidx = sortList[0].property;
          that.table.page.param.sord = sortList[0].order;
        } else {
          var sortDefault = that.agile.config.columns.filter(
            (f) => f.isSortDefalut
          );
          that.table.page.param.sidx = sortDefault.map((m) => m.name).join(",");
          that.table.page.param.sord = sortDefault
            .map((m) => (m.isSortAsc ? "asc" : "desc"))
            .join(",");
        }

        that.initData();
      }
    },
    /**
     * 列宽改变
     * @param {*} param0
     */
    resizableChange({ $rowIndex, column, columnIndex, $columnIndex, $event }) {
      if (this.haveCard) {
        this.table.columnResizableChange = true;
        var columnFilter = this.agile.config.columns.filter(
          (f) => f.name == column.field
        );
        columnFilter[0].width = column.resizeWidth;
      }
    },
    ...mapMutations("account", ["setSystemAgile"]),
    /**
     * 发布调整后表格
     */
    saveResizableChange() {
      let that = this;
      let form = {
        configId: that.agile.configId,
        saveJson: JSON.stringify(that.agile.config),
        publicJson: JSON.stringify(that.agile.config),
      };
      that.$message.loading("保存中,请稍等...", 0);
      listPublic(form).then(function (result) {
        that.$message.destroy();
        if (result.code === that.eipResultCode.success) {
          var systemAgileData = that.systemAgile.filter(
            (f) => f.configId == that.agile.configId
          );
          if (systemAgileData && systemAgileData.length > 0) {
            systemAgileData[0].publicJson = form.publicJson;
            that.setSystemAgile(that.systemAgile);
          }

          that.$message.success(result.msg);
        } else {
          that.$message.error(result.msg);
        }
      });
    },

    /**
     * 按钮初始化
     */
    initButton() {
      let that = this;
      menubutton({
        menuId: this.table.page.param.menuId,
      }).then((result) => {
        if (result.code == that.eipResultCode.success) {
          that.buttons = result.data;
          that.getTableButtonsWidth();
        }
      });
    },

    /**
     * 获取列表按钮宽度
     */
    getTableButtonsWidth() {
      let baseWidth = 22;
      let divider = 13;
      let totalWidth = 20;
      this.buttonsTable.data = this.buttons.filter((f) => f.isShowTable);
      if (this.buttonsTable.data.length > 1) {
        this.buttonsTable.data.forEach((item) => {
          totalWidth += baseWidth * item.name.length;
        });
        totalWidth += this.buttonsTable.data.length - 1 * divider;
        this.buttonsTable.width = totalWidth + "px";
      }
    },

    /**
     *
     */
    uploadCustomRequest(data) {
      this.upload.file = data;
    },
    /**
     *
     * @param {*} value
     */
    async eventApiConfirm(value) {
      var apiResult = await eventApi(value);
      return apiResult;
    },

    /**
     * 按钮api执行
     * @param {*} apiOption
     * @param {*} rows
     */
    async buttonApiDo(apiOption, rows) {
      let that = this;
      var apiResult;
      if (apiOption.confirm.is) {
        that.$confirm({
          title: apiOption.confirm.title,
          content: apiOption.confirm.content,
          okText: apiOption.confirm.okText,
          okType: "danger",
          cancelText: apiOption.confirm.cancelText,
          async onOk() {
            //执行接口
            that.$loading.show({ text: "处理中,请稍等..." });
            apiResult = await that.eventApiConfirm({
              url: apiOption.path,
              type: apiOption.type,
              param: JSON.stringify(rows),
            });
            that.$loading.hide();
            if (apiResult.code == that.eipResultCode.success) {
              that.reload();
              that.$message.success(apiResult.msg);
            } else {
              that.$message.error(apiResult.msg);
            }
          },
          onCancel() {},
        });
      } else {
        //执行接口
        that.$loading.show({ text: "处理中,请稍等..." });
        apiResult = await that.eventApiConfirm({
          url: apiOption.path,
          type: apiOption.type,
          param: JSON.stringify(rows),
        });
        that.$loading.hide();
        if (apiResult.code == that.eipResultCode.success) {
          that.reload();
          that.$message.success(apiResult.msg);
        } else {
          that.$message.error(apiResult.msg);
        }
      }
    },

    /**
     * 按钮自动化执行
     * @param {*} apiOption
     * @param {*} rows
     */
    async automationDo(automationOption, rows, item) {
      let that = this;
      //执行工作流
      this.edit.automation = automationOption;

      //执行工作流
      if (automationOption.action == 1) {
        var automationResult;
        //二次确认
        if (automationOption.confirm.is) {
          that.$confirm({
            title: automationOption.confirm.title,
            content: automationOption.confirm.content,
            okText: automationOption.confirm.okText,
            okType: "danger",
            cancelText: automationOption.confirm.cancelText,
            async onOk() {
              //执行接口
              that.$loading.show({ text: "处理中,请稍等..." });
              automationResult = await automationRun({
                id: item.menuButtonId,
              });
              that.$loading.hide();
              if (automationResult.code == that.eipResultCode.success) {
                that.reload();
                that.$message.success(automationResult.msg);
              } else {
                that.$message.error(automationResult.msg);
              }
            },
            onCancel() {},
          });
        } else {
          that.$loading.show({ text: "处理中,请稍等..." });
          automationResult = await automationRun({
            id: item.menuButtonId,
          });
          that.$loading.hide();
          if (automationResult.code == that.eipResultCode.success) {
            that.reload();
            that.$message.success(automationResult.msg);
          } else {
            that.$message.error(automationResult.msg);
          }
        }
      }
      //填写表单
      else {
        if (!this.edit.config) {
          this.$message.error("未配置编辑页面表单");
        } else {
          that.updateSure(rows);
        }
      }
    },

    /**
     * 按钮点击触发
     * @param {*} item
     * @param {*} from
     * @param {*} row
     */
    async buttonClickTrigger(item, from = "button", row = {}) {
      let that = this;
      this.buttonClickReset();
      this.edit.title = item.name;
      //脚本
      if (item.triggerType == this.eipButtonTriggerType.script) {
        eval(item.script);
      }

      //接口
      else if (item.triggerType == this.eipButtonTriggerType.api) {
        var apiOption = JSON.parse(item.api);
        //是否需要选中数据
        if (apiOption.confirmType == this.eipButtonDataConfirmType.none) {
          //执行接口
          await that.buttonApiDo(apiOption, []);
        } else {
          selectTableRow(
            that.$refs.runTable,
            async function (rows) {
              //是否具有提示语句
              await that.buttonApiDo(apiOption, rows);
            },
            that,
            apiOption.confirmType == this.eipButtonDataConfirmType.single //是否单选
          );
        }
      }

      //自动化
      else if (item.triggerType == this.eipButtonTriggerType.automation) {
        //触发接口
        var automationOption = JSON.parse(item.automation);
        //是否需要选中数据
        if (
          automationOption.confirmType == this.eipButtonDataConfirmType.none
        ) {
          await that.automationDo(automationOption, [], item);
        } else {
          //表格按钮
          if (from == "tableButton") {
            await that.automationDo(automationOption, row, item);
          } else {
            if (this.eipButtonDataConfirmType.single) {
              selectTableRowRadio(
                that.$refs.runTable,
                async function (rows) {
                  await that.automationDo(automationOption, rows, item);
                },
                that
              );
            } else {
              selectTableRow(
                that.$refs.runTable,
                async function (rows) {
                  await that.automationDo(automationOption, rows, item);
                },
                that,
                false
              );
            }
          }
        }
      } else if (item.triggerType == this.eipButtonTriggerType.print) {
        selectTableRow(
          that.$refs.runTable,
          async function (rows) {
            that.print(item.menuButtonId, rows);
          },
          that,
          false
        );
      } else if (item.triggerType == this.eipButtonTriggerType.export) {
        //判断是否具有值
        var exportData = item.export;
        if (exportData) {
          var exportConfig = JSON.parse(exportData);
          selectTableRow(
            that.$refs.runTable,
            async function (rows) {
              //后台导出
              that.$message.loading("导出中", 0);
              reportDataTemplate({
                buttonId: item.menuButtonId,
                configId: that.edit.configId,
                relationId: rows[0].RelationId,
                reportName: that.route.name,
                pdf: exportConfig.pdf,
                contentType: exportConfig.pdf
                  ? "application/pdf"
                  : exportConfig.file[0].type,
              }).then((result) => {
                that.$message.destroy();
                that.$message.success("导出成功");
              });
            },
            that,
            false
          );
        }
      }

      //内置方法
      else {
        this.customerFormValue = null;
        switch (item.method) {
          case "add": //新增
            this.add();
            break;
          case "update": //编辑
            if (from == "button") {
              this.update();
            } else {
              this.updateSure(row);
            }
            break;
          case "del": //删除
            if (from == "button") {
              this.del();
            } else {
              this.delSure([row]);
            }
            break;
          case "copy": //复制
            if (from == "button") {
              this.copy();
            } else {
              this.copySure(row);
            }
            break;
          case "import": //导入数据
            break;
          case "importTemplate": //导入数据模板下载
            this.importTemplateDown();
            break;
          case "reportBusinessData": //导出数据模板下载
            this.reportBusinessData();
            break;
          case "workflowStart": //新增
            that.edit.isWorkflow = true;
            this.add();
            break;
          case "workflowEdit": //编辑
            that.edit.isWorkflow = true;
            this.update();
            break;
          case "workflowRevoke": //取消
            this.workflowRevoke();
            break;
          case "workflowMonitor": //流程监控
            that.edit.isWorkflow = true;
            this.workflowMonitor();
            break;
          case "importExcel":
            that.importExcel.title = this.route.name;
            that.importExcel.visible = true;
            that.importExcel.configId = this.table.page.param.configId;
            break;
          default:
            break;
        }
      }
    },

    /**
     *
     * @param {*} id
     * @param {*} rows
     */
    async print(configId, rows) {
      let that = this;
      var systemAgileData = await findAgileById(configId);
      if (systemAgileData.code === that.eipResultCode.success) {
        var config = JSON.parse(systemAgileData.data.publicJson);
        let tables = [];
        config.panels[0].printElements.forEach((element) => {
          switch (element.printElementType.type) {
            case "text":
              break;
            case "table":
              tables.push(element);
              break;
          }
        });
        //是否具有表格
        that.$loading.show({ text: "处理中..." });
        let hiprintTemplate = new hiprint.PrintTemplate({ template: config });
        for (let index = 0; index < rows.length; index++) {
          let row = that.$utils.clone(rows[index]);
          if (tables.length > 0) {
            for (let index = 0; index < tables.length; index++) {
              let element = tables[index];

              if (element.options.config) {
                var elementConfig = JSON.parse(element.options.config);
                var resultBusiness = await businessDataById({
                  configId: that.edit.configId,
                  id: row.RelationId,
                });
                row = resultBusiness.data;
                var result = await businessDataBatch({
                  configId: that.edit.configId,
                  table: elementConfig.model,
                  id: row.RelationId,
                });
                row[elementConfig.model] = result.data;
              }
            }
          }
        }
        hiprintTemplate.print(rows);
        that.$loading.hide();
      } else {
        that.$message.error("获取配置错误");
      }
    },

    /**
     * 按钮点击数据重置
     */
    buttonClickReset() {
      this.edit.isWorkflow = false;
      this.edit.config.editConfigId = this.edit.configId;
      this.edit.automation = null; //设置为空
    },

    /**
     * 点击事件
     */
    async buttonClick(item) {
      let that = this;
      let from = "button";
      await that.buttonClickTrigger(item, from);
    },

    /**
     * 点击事件
     */
    async tableButtonClick(item, row) {
      let that = this;
      let from = "tableButton";
      await that.buttonClickTrigger(item, from, row);
    },

    /**
     * 上传改变
     */
    uploadChange(button) {
      switch (button.method) {
        case "importBusinessData":
          this.importExcelComfirm.title = this.route.name + "导入提示";
          this.importExcelComfirm.visible = true;
          break;
        default:
          break;
      }
    },

    /**
     * 赋予编辑页面属性
     */
    setOptions() {
      var systemAgileData = this.systemAgile.filter(
        (f) => f.configId == this.edit.config.editConfigId
      );
      if (systemAgileData && systemAgileData.length > 0) {
        let editConfig = systemAgileData[0];
        var publicJson = JSON.parse(editConfig.publicJson);
        this.edit.options = publicJson.config;
      }
    },

    /**
     * 添加
     */
    add() {
      if (!this.edit.config) {
        this.$message.error("未配置编辑页面表单");
      } else {
        this.edit.update = null;
        this.edit.copy = false;
        this.edit.visible = true;
        this.edit.disabled = false;
        this.edit.title = this.route.name;
      }
    },

    /**
     * 编辑
     */
    update() {
      this.edit.title = this.route.name;
      if (!this.edit.config) {
        this.$message.error("未配置编辑页面表单");
      } else {
        let that = this;
        if (this.agile.config.style.select == "checkbox") {
          selectTableRow(
            that.$refs.runTable,
            function (row) {
              that.updateSure(row);
            },
            that
          );
        } else {
          selectTableRowRadio(
            that.$refs.runTable,
            function (row) {
              that.updateSure(row);
            },
            that
          );
        }
      }
    },

    /**
     * 具有编辑数据
     * @param {*} row
     */
    updateSure(row) {
      let that = this;
      //是否来自工作流
      if (that.edit.isWorkflow) {
        //判断状态
        if (row.Status != null && row.Status != -1) {
          that.$message.error("流程已发起，请取消后重新发起");
          return;
        }
      }
      that.edit.update = row;
      that.edit.disabled = false;

      that.edit.visible = true;
      that.edit.copy = false;
    },

    /**
     * 删除
     */
    del() {
      let that = this;
      if (this.agile.config.style.select == "checkbox") {
        selectTableRow(
          that.$refs.runTable,
          function (rows) {
            that.delSure(rows);
          },
          that,
          false
        );
      } else {
        selectTableRowRadio(
          that.$refs.runTable,
          function (rows) {
            that.delSure([rows]);
          },
          that
        );
      }
    },

    /**
     * 删除确定
     */
    delSure(rows) {
      let that = this;
      //提示是否删除
      deleteConfirm(
        that.eipMsg.delete,
        function () {
          that.$loading.show({ text: that.eipMsg.delloading });
          let ids = that.$utils.map(rows, (item) => item.RelationId);
          del({
            configId: that.agile.configId,
            id: ids.join(","),
          }).then((result) => {
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
     * 复制
     */
    copy() {
      if (!this.edit.config) {
        this.$message.error("未配置编辑页面表单");
      } else {
        let that = this;
        if (this.agile.config.style.select == "checkbox") {
          selectTableRow(
            that.$refs.runTable,
            function (rows) {
              that.copySure(rows[0]);
            },
            that,
            false
          );
        } else {
          selectTableRowRadio(
            that.$refs.runTable,
            function (rows) {
              that.copySure(rows);
            },
            that
          );
        }
      }
    },

    /**
     * 复制确认
     */
    copySure(row) {
      let that = this;
      that.edit.update = row;
      that.edit.disabled = false;
      that.edit.title = "复制-" + that.route.name;
      that.edit.visible = true;
      that.edit.copy = true;
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
      //判断是否打开详情
      this.action = this.agile.config.action;
      if (!this.$utils.isUndefined(this.action)) {
        if (this.action.cellDbClick) {
          this.setOptions();
          this.edit.update = row;
          this.edit.title = "查看-" + this.route.name;
          this.edit.visible = true;
          this.edit.disabled = true;
          this.edit.copy = false;
        }
      }
    },

    /**
     * 查看
     */
    detail() {
      if (!this.edit.config) {
        this.$message.error("未配置编辑页面表单");
      } else {
        let that = this;
        selectTableRow(
          that.$refs.runTable,
          function (row) {
            that.edit.update = row;
            that.edit.disabled = true;
            that.edit.title = "查看-" + that.route.name;
            that.edit.visible = true;
            that.edit.copy = false;
          },
          that
        );
      }
    },

    /**
     * 列表初始化
     */
    search() {
      this.table.page.param.current = 1;
      this.initData();
    },

    /**
     * 导出
     */
    reportBusinessData() {
      let that = this;
      that.table.page.param.cols = this.table.columns;
      that.table.page.param.reportName = that.route.name;
      that.$message.loading("导出中", 0);
      reportData(that.table.page.param).then((result) => {
        that.$message.destroy();
        that.$message.success("导出成功");
      });
    },

    /**
     *导入
     */
    importBusinessData(type) {
      let that = this;
      this.importExcelComfirm.visible = false;
      const formData = new FormData();
      that.$message.loading({
        content: "导入中...",
        duration: 0,
      });

      formData.append("Files", this.upload.file.file);
      formData.append("cols", JSON.stringify(this.table.columns));
      formData.append("configId", this.table.page.param.configId);
      formData.append("type", type);
      importData(formData)
        .then((result) => {
          that.$message.destroy();
          if (result.code == that.eipResultCode.success) {
            that.$message.success(result.msg);
            that.reload();
          } else {
            var msg = [];
            try {
              var datas = JSON.parse(result.msg);
              datas.forEach((item) => {
                var m = "";
                m += "第" + item.RowIndex + "行，";
                for (let key in item.FieldErrors) {
                  m += item.FieldErrors[key] + ",";
                }
                msg.push(m);
              });
              that.upload.msg = msg.join("<br/>");
              that.upload.visible = true;
            } catch (error) {
              result.data.forEach((item) => {
                msg.push(item);
              });
              that.upload.msg = msg.join("<br/>");
              that.upload.visible = true;
            }
          }
        })
        .catch((e) => {
          that.$message.destroy();
          that.$message.error("上传出错");
        });
    },

    /**
     * 导出模板下载
     */
    importTemplateDown() {
      let that = this;
      that.table.page.param.cols = this.table.columns;
      that.table.page.param.reportName = that.route.name + "导入模板";
      that.$message.loading("下载中", 0);
      importTemplate(that.table.page.param).then((result) => {
        that.$message.destroy();
        that.$message.success("下载成功");
      });
    },

    /**
     * 初始化配置
     */
    async initConfig() {
      let that = this;
      that.agile.configId = this.table.page.param.configId;
      that.agile.config.columns = [];
      that.table.loading = true;
      var systemAgileData = that.systemAgile.filter(
        (f) => f.configId == that.agile.configId
      );
      if (!(systemAgileData && systemAgileData.length > 0)) {
        var menuAgileData = await menuAgile({ configId: that.agile.configId });
        if (menuAgileData.code === that.eipResultCode.success) {
          systemAgileData = menuAgileData.data;
        } else {
          that.$message.error("获取配置错误");
        }
      }
      if (systemAgileData[0].publicJson) {
        that.agile.config = JSON.parse(systemAgileData[0].publicJson);
        that.advanced();
        await that.initSearch();
        that.initColumns();
        that.initData();
        that.initConnect();
        that.initBaseConfig();
        that.table.title =
          (systemAgileData[0].remark ? systemAgileData[0].remark + "-" : "") +
          systemAgileData[0].name;
        that.card.loading = false;
        that.initSubTable();
        that.initHeight();
        that.initDataInterval();
      } else {
        that.$message.error("请配置视图");
      }
      that.table.loaded = true;
    },

    /**
     * 初始化数据刷新
     */
    initDataInterval() {
      let that = this;
      if (this.agile.config.read.refresh) {
        that.refreshDataInterval = setInterval(function () {
          that.initData();
        }, this.agile.config.read.refresh * 1000);
      }
    },

    /**
     * 初始化配置信息
     */
    initBaseConfig() {
      let that = this;
      queryConfig({ configId: that.agile.configId, menuIdNull: false }).then(
        (result) => {
          if (result.code == that.eipResultCode.success) {
            that.edit.config = result.data[0];
            that.edit.configId = result.data[0].editConfigId;
          }
        }
      );
    },

    /**
     * 定时器绑定操作栏
     */
    initConnect() {
      let that = this;
      that.interval = setInterval(function () {
        if (!that.$utils.isUndefined(that.$refs.runTable)) {
          that.$refs.runTable.connect(that.$refs.runToolbar);
          clearInterval(that.interval);
        }
      }, 1000);
    },

    /**
     * 渲染列
     */
    initColumns() {
      let that = this;
      var headerFields = [];
      that.agile.config.columns
        .filter((f) => f.isShow && f.header.field.length > 0)
        .forEach((item) => {
          headerFields = headerFields.concat(item.header.field);
        });
      that.agile.config.columns
        .filter((f) => f.isShow)
        .forEach((item) => {
          var column = {
            subtable: [],
            header: {
              title: null,
              columns: [],
            },
          };
          if (item.header.field.length > 0) {
            item.header.field.forEach((fitem) => {
              var columnFilter = that.agile.config.columns.filter(
                (f) => f.name == fitem
              );
              if (columnFilter.length > 0) {
                var columnObj = columnFilter[0];
                var columnHeader = {
                  field: columnObj.name,
                  fieldReplaceTxt: columnObj.name.replace(/_Txt*$/, ""),
                  title: columnObj.description,
                  align: columnObj.align,
                  sort: columnObj.isSort,
                  width: columnObj.width,
                  style: columnObj.style,
                  total: columnObj.isTotal,
                  format: columnObj.format,
                  subtable: columnObj.subtable,
                  remark: columnObj.remark,
                  options: columnObj.options,
                  dialog: columnObj.dialog,
                  script: columnObj.script,
                  header: {
                    title: null,
                    columns: [],
                  },
                };
                column.header.columns.push(columnHeader);
              }
            });
            column.header.title = item.header.title;
          } else {
            //判断字段是否在头部字段里面
            if (headerFields.filter((f) => f == item.name) == 0) {
              column = {
                field: item.name,
                fieldReplaceTxt: item.name.replace(/_Txt*$/, ""),
                title: item.description,
                align: item.align,
                sort: item.isSort,
                width: item.width,
                style: item.style,
                total: item.isTotal,
                format: item.format,
                subtable: item.subtable,
                remark: item.remark,
                options: item.options,
                dialog: item.dialog,
                script: item.script,
                header: {
                  title: null,
                  columns: [],
                },
              };
            }
          }
          that.table.columns.push(column);
        });
    },

    /**
     * 初始化查询
     */
    async initSearch() {
      let that = this;
      if (that.agile.config.search) {
        if (that.agile.config.search.num) {
          that.table.search.option.num = that.agile.config.search.num;
        }
        if (that.agile.config.search.labelCol) {
          that.table.search.option.labelCol = that.agile.config.search.labelCol;
        }
        if (that.agile.config.search.wrapperCol) {
          that.table.search.option.wrapperCol =
            that.agile.config.search.wrapperCol;
        }
        that.table.search.option.searchButton =
          that.agile.config.search.searchButton;
        let config = [];

        for (
          let index = 0;
          index < that.agile.config.search.columns.length;
          index++
        ) {
          const item = that.agile.config.search.columns[index];
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
            case "user":
              var options = item.options;
              config.push({
                field: item.fields ? item.fields : item.field,
                op: item.op,
                placeholder: item.placeholder,
                title: item.title,
                value: "",
                type: "user",
                options: {
                  multiple: options.multiple, //单选
                },
              });
              break;
            case "organization":
              var options = item.options;
              config.push({
                field: item.fields ? item.fields : item.field,
                op: item.op,
                placeholder: item.placeholder,
                title: item.title,
                value: "",
                type: "organization",
                options: {
                  multiple: options.multiple, //单选
                },
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
                options: item.options,
              });
              break;
            case "select":
              //得到具体配置
              var options = item.options;
              if (options) {
                switch (options.source.type) {
                  case "dynamic":
                    config.push({
                      field: item.field.replace(/_Txt*$/, ""),
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
                      },
                    });
                    break;
                  case "custom":
                    config.push({
                      field: item.field.replace(/_Txt*$/, ""),
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
                      },
                    });
                    break;
                }
              }
              break;
            case "dictionary":
              var options = item.options;
              config.push({
                field: item.field.replace(/_Txt*$/, ""),
                op: item.op,
                placeholder: item.placeholder,
                title: item.title,
                type: "dictionary",
                options: {
                  multiple: options.multiple, //单选
                  source: {
                    format: [],
                    dictionaryId: options.source.dictionaryId,
                    dictionaryLevel: options.source.dictionaryLevel,
                  },
                },
              });
              break;
            case "correlationRecord":
              //得到具体配置
              var options = item.options;
              if (options) {
                config.push({
                  field: item.field.replace(/_Txt*$/, ""),
                  op: item.op,
                  placeholder: item.placeholder,
                  title: item.title,
                  type: "correlationRecord",
                  options: {
                    multiple: options.multiple, //单选
                    source: {
                      table: options.source.table, //数据源类型
                      key: options.source.key,
                      value: options.source.value, //值
                      sidx: options.source.sidx, //排序字段
                      sord: options.source.sord, //排序方式
                      format: [],
                    },
                  },
                });
              }
              break;
            case "datetime":
              switch (item.options.format) {
                case "YYYY":
                  item.options.mode = "year";
                  break;
                case "YYYY-MM":
                  item.options.mode = "month";
                  break;
                default:
                  item.options.mode = "date";
                  break;
              }
              config.push({
                field: item.fields ? item.fields : item.field,
                op: item.op,
                open: false,
                placeholder: item.placeholder,
                title: item.title,
                value: item.op == "daterange" ? [] : "",
                type: "datetime",
                options: item.options,
              });
          }
        }
        that.table.search.option.config = config;
      }
    },

    /**
     * 初始化列表数据
     */
    async initData() {
      let that = this;
      that.table.loading = true;
      that.table.page.param.isPaging = that.agile.config.style.paging;
      //默认排序字段
      if (!that.table.page.param.sidx) {
        var sortDefault = that.agile.config.columns.filter(
          (f) => f.isSortDefalut
        );
        that.table.page.param.sidx = sortDefault.map((m) => m.name).join(",");
        that.table.page.param.sord = sortDefault
          .map((m) => (m.isSortAsc ? "asc" : "desc"))
          .join(",");
      }
      if (that.agile.config.read != null && !that.agile.config.read.cache) {
        that.table.page.param.timeStamp = this.$utils.timestamp();
      }
      //默认过滤条件
      that.initDefaultFilter();

      const route = this.$route;
      //参数是否带查询（两种传参数：若url里面有filters则用，否则用meta里面的参数）
      var filtersQuery = route.query.filters;
      if (!filtersQuery) {
        filtersQuery =
          route.meta && route.meta.params && route.meta.params.filters;
      }
      if (filtersQuery) {
        let filters = that.table.page.param.filters;
        let searchFilter = {};
        if (filters) {
          searchFilter = JSON.parse(filters);
        } else {
          searchFilter = { groupOp: "AND", rules: [], groups: [] };
        }
        var filtersJson = JSON.parse(unescape(filtersQuery));
        searchFilter.rules.push(filtersJson);
        that.table.page.param.filters = JSON.stringify(searchFilter);
      }
      query(that.table.page.param).then(async function (result) {
        that.table.loading = false;
        if (result.code == that.eipResultCode.success) {
          let queryData = result.data;
          //得到表尾数据
          that.table.footerData = await that.initDataFooter();
          queryData.data.forEach((d) => {
            d.subtableColumns = [];
            d.subtableDatas = {};
          });
          that.table.data = queryData.data;
          if (that.agile.config.style.paging) {
            that.table.page.total = queryData.count;
          }

          that.setStartIndex();
        } else {
          that.$message.error(result.msg);
        }
      });

      that.initFilter();
    },

    /**
     * 重置序列化
     */
    setStartIndex() {
      let page = this.$utils.clone(this.table.page.param, true);
      this.table.seqConfig.startIndex = page.current;
    },

    /**
     * 左侧选中
     */
    filterSearchSelect({ item, key, selectedKeys }) {
      this.table.filterSearch.select = selectedKeys;
      this.search();
    },

    /**
     * 选择
     */
    filterSearchCheckOrganization(checkedKeys, event) {
      this.table.filterSearch.select = checkedKeys;
      this.search();
    },
    /**
     * 初始化默认过滤条件
     */
    initDefaultFilter() {
      let filters = {
        groupOp: "AND",
        rules: [],
        groups: [],
      };

      //组合查询
      let getFilters = {};
      if (this.$refs.eipSearchPro) {
        getFilters = this.$refs.eipSearchPro.getFilters();
        if (getFilters.rules.length > 0 || getFilters.groups.length > 0) {
          filters.groups.push(getFilters);
        }
      }

      //顶部查询
      if (this.$refs.eipSearch) {
        getFilters = this.$refs.eipSearch.getFilters();
        if (getFilters.rules.length > 0 || getFilters.groups.length > 0) {
          filters.groups.push(getFilters);
        }
      }

      //左侧菜单
      var filterSearchSelect = this.table.filterSearch.select.filter(
        (f) => f != "0"
      );
      var filterSearchSelectAll = this.table.filterSearch.select.filter(
        (f) => f == "0"
      );
      if (filterSearchSelect.length > 0 && filterSearchSelectAll.length == 0) {
        var filterSearchFilters = {
          groupOp: "OR",
          rules: [],
          groups: [],
        };
        filterSearchSelect.forEach((element) => {
          var field = this.agile.config.style.filter.replace("_Txt", "");
          if (element != "-1") {
            filterSearchFilters.rules.push({
              field: field,
              op: "eq",
              data: element,
            });
          } else {
            filterSearchFilters.rules.push({ field: field, op: "nu" });
          }
        });
        filters.groups.push(filterSearchFilters);
      }

      this.table.page.param.filters = JSON.stringify(filters);
    },

    /**
     * 初始化表尾数据
     */
    async initDataFooter() {
      let that = this;
      var footer = [];
      var totalColumns = that.table.columns.filter((f) => f.total);
      if (totalColumns.length > 0) {
        footer.push("");
        footer.push("合计");
        var result = await queryFooter(that.table.page.param);
        if (result.code == that.eipResultCode.success) {
          that.table.columns.forEach((item) => {
            footer.push(item.total ? result.data[0][item.field] : "");
          });
        }
      }
      return [footer];
    },

    /**
     *返回底部数据
     */
    returnDataFooter() {
      return this.table.footerData;
    },

    /**
     * 该方法在展开或关闭触发之前调用，可以通过返回值来决定是否允许继续执行
     */
    toggleExpand({ expanded, column, columnIndex, row, rowIndex }) {
      this.table.columnExpand = column;
      return true;
    },

    /**
     * 加载点开
     */
    async loadExpand({ row }) {
      let that = this;
      //得到属性个数
      var count = 0;
      var columnsExpand = this.table.columns.filter(
        (f) => f.field == this.table.columnExpand.field
      );
      if (columnsExpand.length > 0) {
        return new Promise((resolve) => {
          var column = columnsExpand[0];
          column.subtable.forEach(async (item) => {
            var configId = item.id;
            var columns = [];

            var config = null;
            var systemAgileData = that.systemAgile.filter(
              (f) => f.configId == configId
            );

            if (systemAgileData && systemAgileData.length > 0) {
              config = JSON.parse(systemAgileData[0].publicJson);
            } else {
              systemAgileData = await findAgileById(configId);
              if (systemAgileData.code === that.eipResultCode.success) {
                config = JSON.parse(systemAgileData.data.publicJson);
              } else {
                that.$message.error("获取配置错误");
              }
            }
            that.table.subtableNames[configId] = item.name;
            // 显示列
            config.columns
              .filter((f) => f.isShow)
              .forEach((citem) => {
                var column = {
                  field: citem.name,
                  fieldReplaceTxt: citem.name.replace(/_Txt*$/, ""),
                  title: citem.description,
                  align: citem.align,
                  sort: citem.isSort,
                  width: citem.width,
                  style: citem.style,
                  total: citem.isTotal,
                  format: citem.format,
                };
                columns.push(column);
              });
            row.subtableColumns.push(columns);

            //关联条件
            var where = item.condition;
            if (where) {
              var sqlparams = [];
              var condition = where.split("{");
              condition.forEach((sitem) => {
                if (sitem.includes("}")) {
                  var value = row[sitem.split("}")[0]];
                  sqlparams.push({
                    key: "{" + sitem.split("}")[0] + "}",
                    value: value,
                  });
                }
              });
              sqlparams.forEach((pitem) => {
                where = where.replace(pitem.key, pitem.value);
              });
            }

            //查询数据
            query({
              isPaging: false,
              configId: configId,
              where: where,
            }).then(async function (subtableResult) {
              if (subtableResult.code == that.eipResultCode.success) {
                row.subtableDatas[configId] = subtableResult.data.data;
                that.subtable.data.subtableDatas[configId] =
                  subtableResult.data.data;
              }
              for (var i in row.subtableDatas) {
                count++;
              }
              if (column.subtable.length == count) {
                that.table.columnLoadComplate = true;
                resolve();
              }
            });
          });
        });
      }
    },

    /**
     * 单元格点击
     * @param {*} row
     */
    cellClick(cell) {
      let that = this;
      if (this.agile.config.style.subtableLayout != "tree") {
        var columns = this.table.columns.filter((f) => f.subtable.length > 0);
        if (columns.length > 0) {
          columns.forEach((column) => {
            column.subtable.forEach(async (item) => {
              var configId = item.id;
              //关联条件
              var where = item.condition;
              if (where) {
                var sqlparams = [];
                var condition = where.split("{");
                condition.forEach((sitem) => {
                  if (sitem.includes("}")) {
                    var value = cell.row[sitem.split("}")[0]];
                    sqlparams.push({
                      key: "{" + sitem.split("}")[0] + "}",
                      value: value,
                    });
                  }
                });
                sqlparams.forEach((pitem) => {
                  where = where.replace(pitem.key, pitem.value);
                });
              }

              //查询数据
              query({
                isPaging: false,
                configId: configId,
                where: where,
              }).then(async function (subtableResult) {
                if (subtableResult.code == that.eipResultCode.success) {
                  that.subtable.data.subtableDatas[configId] =
                    subtableResult.data.data;
                }
                that.table.columnLoadComplate = false;
                that.table.columnLoadComplate = true;
              });
            });
          });
        }
      }
    },
    /**
     * 初始化子表
     */
    initSubTable() {
      let that = this;
      var subtableLayout = this.agile.config.style.subtableLayout;

      if (subtableLayout != "tree") {
        var columns = this.table.columns.filter((f) => f.subtable.length > 0);
        if (columns.length > 0) {
          columns.forEach((column) => {
            column.subtable.forEach(async (item) => {
              var configId = item.id;
              var columns = [];

              var config = null;
              var systemAgileData = that.systemAgile.filter(
                (f) => f.configId == configId
              );

              if (systemAgileData && systemAgileData.length > 0) {
                config = JSON.parse(systemAgileData[0].publicJson);
              } else {
                systemAgileData = await findAgileById(configId);
                if (systemAgileData.code === that.eipResultCode.success) {
                  config = JSON.parse(systemAgileData.data.publicJson);
                } else {
                  that.$message.error("获取配置错误");
                }
              }
              that.table.subtableNames[configId] = item.name;
              that.subtable.data.subtableDatas[configId] = [];
              // 显示列
              config.columns
                .filter((f) => f.isShow)
                .forEach((citem) => {
                  var column = {
                    field: citem.name,
                    fieldReplaceTxt: citem.name.replace(/_Txt*$/, ""),
                    title: citem.description,
                    align: citem.align,
                    sort: citem.isSort,
                    width: citem.width,
                    style: citem.style,
                    total: citem.isTotal,
                    format: citem.format,
                  };
                  columns.push(column);
                });
              that.subtable.data.subtableColumns.push(columns);
            });
          });

          that.table.columnLoadComplate = true;
        }
        this.split.percentage =
          this.agile.config.style.subtableLayoutPercentage;
      }
    },
    /**
     *
     * @param {*} event
     */
    resize(event) {
      this.split.percentage = event[0].size;
      this.initHeight();
    },

    /**
     * 左右查询拖拉
     * @param {*} event
     */
    resizeFilterSearch(event) {
      if (this.haveCard) {
        let that = this;
        var width =
          document.getElementsByClassName("table-panel")[0].offsetWidth;
        that.table.filterSearch.width = event[0].size;
        that.agile.config.style.filterWidth = (width * event[0].size) / 100;
        let form = {
          configId: that.agile.configId,
          saveJson: JSON.stringify(that.agile.config),
          publicJson: JSON.stringify(that.agile.config),
        };
        setTimeout(() => {
          listPublic(form).then(function (result) {
            if (result.code === that.eipResultCode.success) {
              var systemAgileData = that.systemAgile.filter(
                (f) => f.configId == that.agile.configId
              );
              if (systemAgileData && systemAgileData.length > 0) {
                systemAgileData[0].publicJson = form.publicJson;
                that.setSystemAgile(that.systemAgile);
              }
            }
          });
        }, 1000);
      }
    },

    /**
     * 初始化左侧筛选
     */
    initFilter() {
      let that = this;
      if (this.agile.config.style.filter) {
        //得到宽度
        var column = this.$utils.find(
          that.agile.config.columns,
          (f) => f.name == this.agile.config.style.filter
        );
        that.table.filterSearch.format = column.format;
        that.table.filterSearch.title = column.description;
        var width =
          document.getElementsByClassName("table-panel")[0].offsetWidth;
        this.table.filterSearch.width =
          (this.agile.config.style.filterWidth / width) * 100;
        if (["Organization"].includes(column.format)) {
          chosenOrganization(0).then((result) => {
            if (result.code === this.eipResultCode.success) {
              that.table.filterSearch.data = result.data;
            }
            that.table.filterSearch.spinning = false;
          });
        } else if (["TreeSelect"].includes(column.format)) {
          businessDataFormSource({
            dataSourceId: column.options.dynamicConfig.dataSourceId,
            inParams: JSON.stringify(
              that.convertInParams(column.options.dynamicConfig.inParams)
            ),
            formValue: JSON.stringify({}),
          }).then((result) => {
            if (result.code === that.eipResultCode.success) {
              let dynamicData = [];
              let tree = [];
              result.data.forEach((res) => {
                tree.push({
                  value: res[column.options.dynamicConfig.key].toString(),
                  key: res[column.options.dynamicConfig.key],
                  title: res[column.options.dynamicConfig.value],
                  parentId: res[column.options.dynamicConfig.parentId],
                });
              });
              dynamicData = that.setChildrenTreeData(
                that.$utils.toArrayTree(tree, {
                  key: "key",
                })
              );
              that.table.filterSearch.data = dynamicData;
              that.table.filterSearch.spinning = false;
            }
          });
        } else {
          queryFilterSearch({
            configId: that.table.page.param.configId,
            rote: that.table.page.param.rote,
            field: this.agile.config.style.filter,
            haveDataPermission: that.table.page.param.haveDataPermission,
          }).then((result) => {
            if (result.code === this.eipResultCode.success) {
              that.table.filterSearch.data = result.data;
            }
            that.table.filterSearch.spinning = false;
          });
        }
      }
    },

    /**
     * 删除children为空
     * @param {*} data
     */
    setChildrenTreeData(data) {
      for (var i = 0; i < data.length; i++) {
        if (data[i].children) {
          if (data[i].children.length < 1) {
            delete data[i].children;
          } else {
            this.setChildrenTreeData(data[i].children);
          }
        }
      }
      return data;
    },
    /**
     * 转换输入参数
     */
    convertInParams(inParams) {
      let that = this;
      var output = [];
      inParams.forEach((element) => {
        if (element.condition.includes("contenteditable")) {
          var condition = element.condition
            .replaceAll("<p>", "")
            .replaceAll("</p>", "")
            .replaceAll(' class="non-editable" contenteditable="false"', "");
          const tempDiv = document.createElement("div");
          // 设置div的内容为HTML字符串
          tempDiv.innerHTML = condition;
          // 查询所有的span标签
          const spans = tempDiv.querySelectorAll("span");
          spans.forEach((span) => {
            var id = decodeURIComponent(span.id);
            condition = condition.replaceAll(span.outerHTML, id);
          });
          element.condition = condition;
        }
        output.push(element);
      });
      return output;
    },
    /**
     * 初始化高度
     */
    initHeight() {
      this.split.minHeight = (250 / this.split.height) * 100;
      //判断布局
      var subtableLayout = this.agile.config.style.subtableLayout;
      if (subtableLayout == "topBottom") {
        //切分上下高度
        this.split.tableHeight =
          this.split.height * (this.split.percentage / 100);
        var height = this.haveCard ? 106 : 106;
        if (this.table.advanced) {
          //判断具有几行
          var number =
            this.table.search.option.config.length /
            this.table.search.option.num;
          if (number % 1 === 0) {
            number += 1;
          } else {
            number = Math.ceil(number);
          }
          height = height + (number - 1) * 40;
        }
        this.table.height = this.split.tableHeight - height;
        this.split.subtableHeight =
          this.split.height * (1 - this.split.percentage / 100) - 70;
      }
      //初始化高度
      else if (subtableLayout == "leftRight") {
        this.split.subtableHeight = this.split.height - 58;
      }
    },

    /**
     *数量改变
     */
    sizeChange(current, pageSize) {
      this.table.page.param.current = 1;
      this.table.page.param.size = pageSize;
      this.reload();
    },

    /**
     *导出
     */
    report() {},

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

    /**
     * 权限按钮加载完毕
     */
    toolbarOnload(buttons) {},

    /**
     * 调整,收起展开高度
     */
    advanced(advanced) {
      if (this.table.search.option.config.length > 0) {
        var height = this.agile.config.style.paging
          ? this.haveCard
            ? 190
            : 140
          : this.haveCard
          ? 150
          : 100;
        this.table.advanced = advanced;
        if (advanced) {
          //判断具有几行
          var number =
            this.table.search.option.config.length /
            this.table.search.option.num;
          if (number % 1 === 0) {
            number += 1;
          } else {
            number = Math.ceil(number);
          }
          height = height + (number - 1) * 40;
        }
        this.table.height = this.eipHeaderHeight() - height;
      } else {
        this.table.height =
          this.eipHeaderHeight() -
          (this.agile.config.style.paging
            ? this.haveCard
              ? 140
              : 90
            : this.haveCard
            ? 100
            : 50);
      }
    },

    /**
     * 悬浮显示
     */
    tooltip({ type, column, row, items, _columnIndex }) {
      const { property } = column;
      if (row) {
        return row[property];
      }
      return null;
    },

    /**
     * 取消
     */
    workflowRevoke() {
      let that = this;
      selectTableRow(
        that.$refs.runTable,
        function (rows) {
          //判断流程是否结束
          var result = true;
          rows.forEach((item, index) => {
            if (item.Status == 100) {
              that.$message.error("第" + (index + 1) + "行流程已结束");
              result = false;
              return false;
            }
            if (item.Status == 10) {
              that.$message.error("第" + (index + 1) + "行流程已撤销");
              result = false;
              return false;
            }
          });
          if (result) {
            that.$confirm({
              title: "操作提示?",
              content: "确定取消流程?",
              okText: "确定",
              okType: "danger",
              cancelText: "取消",
              onOk() {
                that.$message.loading("取消中", 0);
                let ids = that.$utils.map(rows, (item) => item.RelationId);
                workflowEngineRevokeByCreateUser({
                  processInstanceId: ids.join(","),
                }).then((result) => {
                  that.$message.destroy();
                  if (result.code == that.eipResultCode.success) {
                    that.reload();
                    that.$message.success(result.msg);
                  } else {
                    that.$message.error(result.msg);
                  }
                });
              },
              onCancel() {},
            });
          }
        },
        that,
        false
      );
    },

    /**
     * 查看流程
     */
    workflowMonitor() {
      let that = this;
      if (!this.edit.config) {
        this.$message.error("未配置编辑页面表单");
      } else {
        selectTableRow(
          that.$refs.runTable,
          function (row) {
            that.edit.title = "流程查看";
            that.edit.update = row;
            that.workflow.data = {
              processId: null,
              processInstanceId: row.RelationId,
              activityId: null,
              taskId: null,
              type: that.eipWorkflowDoType["流程监控"],
            };
            that.edit.disabled = true;
            that.edit.copy = false;
            that.edit.visible = true;
          },
          that
        );
      }
    },

    /**
     * 子表点击详情
     */
    batchDetail(data) {
      var row = data.row;
      var item = data.item;
      this.batch.visible = true;
      this.batch.title = item.title + "（" + row[item.field] + "）";
      this.batch.configId = this.edit.config.editConfigId;
      this.batch.relationId = row["RelationId"];
      this.batch.model = item.field;
    },

    /**
     * 导出
     * @param {*} e
     */
    exportMenuClick(e) {
      switch (e.key) {
        case "1":
          this.$refs.runTable.exportData({ type: "csv" });
          break;
        case "2":
          this.$refs.runTable.exportData({
            data: this.$refs.runTable.getCheckboxRecords(),
          });
          break;
        case "3":
          this.$refs.runTable.openExport();
          break;
        case "4":
          this.reportBusinessData();
          break;
      }
      console.log("click", e);
    },
  },
};
</script>
<style>
.menu-item {
  height: 40px;
}
</style>
