<template>
  <splitpanes class="default-theme" style="height: auto">
    <pane :min-size="split.minWidth" :size="split.minWidth">
      <a-card @contextDictionary.prevent size="small" class="eip-admin-card-small">
        <div slot="title">
          <a-space>
            <a-input-search v-model="menu.key" :allowClear="true" placeholder="名称/拼音模糊搜索" @change="menuSearch" />
            <a-tooltip title="刷新">
              <a-button type="primary" @click="reload(true)">
                <a-icon type="redo" />
              </a-button>
            </a-tooltip>
          </a-space>
        </div>
        <a-spin :spinning="menu.spinning" :delay="0" :style="menu.style">
          <div>
            <a-directory-tree v-if="!menu.spinning"
              :defaultExpandedKeys="[menu.data.length > 0 ? menu.data[0].key : '']" :expandAction="false"
              :tree-data="menu.data" :icon="menuIcon" @select="menuSelect"></a-directory-tree>
          </div>
        </a-spin>
      </a-card>
    </pane>
    <pane min-size="50">
      <a-card class="eip-admin-card-small eip-admin-card-small-bottom-border" :bordered="false">
        <eip-search :option="table.search.option" @search="tableSearch" @advanced="tableAdvanced"></eip-search>
      </a-card>

      <a-card class="eip-admin-card-small" :bordered="false">
        <vxe-toolbar ref="toolbar" custom print export :refresh="{ query: menuButtonInit }">
          <template v-slot:buttons>
            <eip-toolbar @del="del" @update="update" @add="add" @copy="copy" @detail="detailClick"
              @onload="toolbarOnload"></eip-toolbar>
          </template>
        </vxe-toolbar>
        <vxe-table :loading="table.loading" ref="table" id="systemmenubuttonlist" :seq-config="{
          startIndex:
            (table.page.param.current - 1) * table.page.param.size,
        }" :height="table.height" :export-config="{}" :print-config="{}" :data="table.data" :sort-config="{
          trigger: 'cell',
          defaultSort: { field: 'orderNo', order: 'asc' },
          orders: ['desc', 'asc'],
        }" @sort-change="tableSort">
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
          <vxe-column field="name" title="名称" width="100" showOverflow="ellipsis"></vxe-column>
          <vxe-column field="menuNames" title="归属菜单" min-width="240" showOverflow="ellipsis"></vxe-column>
          <vxe-column field="icon" title="图标" width="60" align="center">
            <template v-slot="{ row }">
              <a-icon :type="row.icon" :theme="row.theme" />
            </template>
          </vxe-column>
          <vxe-column field="method" title="事件" width="140" showOverflow="ellipsis"></vxe-column>
          <vxe-column field="isFreeze" title="禁用" align="center" width="80" sortable>
            <template v-slot="{ row }">
              <a-switch :checked="row.isFreeze" @change="isFreezeChange(row)" />
            </template>
          </vxe-column>
          <vxe-column field="isShowTable" title="列表显示" align="center" width="100" sortable>
            <template v-slot="{ row }">
              <a-switch :checked="row.isShowTable" @change="isShowTableChange(row)" />
            </template>
          </vxe-column>
          <vxe-column field="orderNo" title="排序" align="center" width="80" sortable></vxe-column>
          <vxe-column field="remark" title="备注" min-width="80" showOverflow="ellipsis"></vxe-column>
          <vxe-column field="createUserName" title="创建人" width="100"></vxe-column>
          <vxe-column field="createTime" title="创建时间" width="160"></vxe-column>
          <vxe-column field="updateUserName" title="修改人" width="100"></vxe-column>
          <vxe-column field="updateTime" title="修改时间" width="160"></vxe-column>
          <vxe-column title="操作" align="center" fixed="right" width="100px">
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
          :page-size="table.page.param.size" :total="table.page.total" @change="menuButtonInit"
          @showSizeChange="tableSizeChange"></a-pagination>
      </a-card>
    </pane>
    <edit ref="edit" v-if="edit.visible" :visible.sync="edit.visible" :data="menu.data" :title="edit.title"
      :copy="edit.copy" :menuButtonId="edit.menuButtonId" @save="operateStatus"></edit>

    <permission-detail ref="detail" v-if="detail.visible" :visible.sync="detail.visible" :title="detail.title"
      :id="detail.id" :access="detail.access"></permission-detail>
  </splitpanes>
</template>

<script>
import {
  menuQuery,
  query,
  del,
  isFreeze,
  isShowTable
} from "@/services/system/menubutton/list";
import edit from "./edit";
import permissionDetail from "@/pages/system/permission/detail";

import { selectTableRow, deleteConfirm } from "@/utils/util";
import { Splitpanes, Pane } from 'splitpanes'
import 'splitpanes/dist/splitpanes.css'
export default {
  components: {
    edit,
    permissionDetail,
    Splitpanes,
    Pane
  },
  data() {
    return {
      split: {
        minWidth: (250 / window.innerWidth) * 100,
      },
      menu: {
        style: {
          overflow: "auto",
          height: this.eipHeaderHeight() - 72 + "px",
        },
        key: null,
        data: [],
        original: [],
        spinning: true,
        right: {
          item: null,
          style: "",
        },
      },
      table: {
        page: {
          param: {
            current: 1,
            size: this.eipPage.size,
            sord: "asc",
            sidx: "",
            id: "",
            filters: "",
          },
          total: 0,
          sizeOptions: this.eipPage.sizeOptions,
        },
        loading: true,
        data: [],
        height: window.innerHeight - 342,
        search: {
          option: {
            num: 3,
            config: [
              {
                field: "menuButton.Name",
                op: "cn",
                placeholder: "请输入名称",
                title: "名称",
                value: "",
                type: "text",
              },
              {
                field: "menuButton.Method",
                op: "cn",
                placeholder: "请输入事件",
                title: "事件",
                value: "",
                type: "text",
              },

              {
                field: "menuButton.Remark",
                op: "cn",
                placeholder: "请输入备注",
                title: "备注",
                value: "",
                type: "text",
              },
              {
                field: "menuButton.ShowGrid",
                op: "eq",
                placeholder: "请选择列表显示",
                title: "列表显示",
                type: "bool",
                options: {
                  yes: '是',
                  no: '否'
                }
              },
              {
                field: "menuButton.IsFreeze",
                op: "eq",
                placeholder: "请选择禁用状态",
                title: "禁用",
                type: "bool",
                options: {
                  yes: '是',
                  no: '否'
                }
              },
            ],
          },
        },
      },

      edit: {
        visible: false,
        menuButtonId: null,
        title: null,
        copy: false,
      },

      detail: {
        visible: false,
        title: null,
        id: null,
        access: 1,
      },

      visible: {
        update: false,
        del: false,
      },
    };
  },
  created() {
    this.menuInit();
    this.menuButtonInit();
  },
  mounted() {
    this.$refs.table.connect(this.$refs.toolbar);
  },
  methods: {
    menuSearch(e) {
      var that = this;
      this.menu.data = this.$utils.clone(this.menu.original, true);
      this.menu.data = this.$utils.searchTree(
        this.menu.data,
        (item) => {
          if (that.menu.key) {
            var titlePinyin = pinyin.convert(item.title).toLowerCase();
            if (
              item.title.toLowerCase().indexOf(that.menu.key.toLowerCase()) > -1
            ) {
              return true;
            } else if (titlePinyin.indexOf(that.menu.key.toLowerCase()) > -1) {
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
    },
    /**
     *冻结
     */
    isFreezeChange(row) {
      if (this.visible.update) {
        let that = this;
        this.$message.loading("保存中...", 0);
        isFreeze({ id: row.menuButtonId }).then((result) => {
          that.$message.destroy();
          if (result.code == that.eipResultCode.success) {
            that.reload(false);
            that.$message.success(result.msg);
          } else {
            that.$message.error(result.msg);
          }
        });
      }
    },

    /**
     * 是否显示到列表
     * @param {*} row 
     */
    isShowTableChange(row) {
      if (this.visible.update) {
        let that = this;
        this.$message.loading("保存中...", 0);
        isShowTable({ id: row.menuButtonId }).then((result) => {
          that.$message.destroy();
          if (result.code == that.eipResultCode.success) {
            that.reload(false);
            that.$message.success(result.msg);
          } else {
            that.$message.error(result.msg);
          }
        });
      }
    },

    /**
     * 菜单树
     */
    menuInit() {
      let that = this;
      that.menu.spinning = true;
      menuQuery(that.eipPrivilegeAccess.menubutton).then((result) => {
        that.menu.data = result.data;
        that.menu.original = result.data;
        that.menu.spinning = false;

        that.menuSearch();
      });
    },

    /**
     * 树图标
     */
    menuIcon(props) {
      const { slots } = props;
      return <a-icon type={slots.icon} theme={slots.theme} />;
    },

    /**
     * 树选中
     */
    menuSelect(electedKeys, e) {
      this.table.page.param.id = electedKeys[0];
      this.table.page.param.current = 1;
      this.menuButtonInit();
    },

    /**
     * 列表数据
     */
    menuButtonInit() {
      let that = this;
      that.table.loading = true;
      query(that.table.page.param).then((result) => {
        if (result.code == that.eipResultCode.success) {
          that.table.data = result.data;
          that.table.page.total = result.total;
        }
        that.table.loading = false;
      });
    },
    /**
     * 列表排序改变
     */
    tableSort({ sortList }) {
      this.table.page.param.current = 1;
      this.table.page.param.sidx = sortList[0].property;
      this.table.page.param.sord = sortList[0].order;
      this.menuButtonInit();
    },
    /**
     *数量改变
     */
    tableSizeChange(current, pageSize) {
      this.table.page.param.size = pageSize;
      this.menuButtonInit();
    },
    /**
     * 列表查询
     */
    tableSearch(filters) {
      this.table.page.param.current = 1;
      this.table.page.param.filters = filters;
      this.menuButtonInit();
    },

    /**
     * 新增
     */
    add() {
      this.edit.menuButtonId = null;
      this.edit.title = "新增模块按钮";
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
          that.edit.menuButtonId = row.menuButtonId;
          that.edit.copy = false;
          that.edit.title = "编辑模块按钮-" + row.name;
          that.edit.visible = true;
        },
        that
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
          that.edit.menuButtonId = row.menuButtonId;
          that.edit.copy = true;
          that.edit.title = "复制模块按钮-" + row.name;
          that.edit.visible = true;
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
              let ids = that.$utils.map(rows, (item) => item.menuButtonId);
              del({ id: ids.join(",") }).then((result) => {
                that.$loading.hide();
                if (result.code == that.eipResultCode.success) {
                  that.reload(false);
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

    //提示状态信息
    operateStatus(result) {
      if (result.code === this.eipResultCode.success) {
        this.menuButtonInit();
      }
    },

    /**
     * 重新加载
     */
    reload(reset) {
      if (reset) this.table.page.param.id = this.eipEmptyGuid;
      this.table.page.param.current = 1;
      this.menuInit();
      this.menuButtonInit();
    },
    /**
     * 权限按钮加载完毕
     */
    toolbarOnload(buttons) {
      this.visible.update =
        buttons.filter((f) => f.method == "update").length > 0;
      this.visible.del = buttons.filter((f) => f.method == "del").length > 0;
    },

    /**
     * 调整,收起展开高度
     */
    tableAdvanced(advanced) {
      this.table.height = this.eipHeaderHeight() - (advanced ? 214 : 163);
    },

    /**
     * 树更新
     */
    tableUpdateRow(row) {
      this.edit.menuButtonId = row.menuButtonId;
      this.edit.title = "模块按钮编辑-" + row.name;
      this.edit.visible = true;
    },
    /**
     * 删除
     */
    tableDelRow(row) {
      let that = this;
      deleteConfirm(
        "模块按钮【" + row.name + "】" + that.eipMsg.delete,
        function () {
          that.$loading.show({ text: that.eipMsg.delloading });
          del({ id: row.menuButtonId }).then((result) => {
            that.$loading.hide();
            if (result.code == that.eipResultCode.success) {
              that.reload(false);
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
     *详情点击
     */
    detailClick() {
      let that = this;
      selectTableRow(
        that.$refs.table,
        function (row) {
          that.detail.id = row.menuButtonId;
          that.detail.title = "模块按钮使用详情-" + row.name;
          that.detail.visible = true;
        },
        that
      );
    },
  },
};
</script>