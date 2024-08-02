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
        <eip-search :option="table.search.option" @search="tableSearch"></eip-search>
      </a-card>

      <a-card class="eip-admin-card-small" :bordered="false">
        <vxe-toolbar ref="toolbar" custom print export :refresh="{ query: dataInit }">
          <template v-slot:buttons>
            <eip-toolbar @del="del" @update="update" @add="add" @copy="copy" @detail="detailClick"
              @onload="toolbarOnload"></eip-toolbar>
          </template>
        </vxe-toolbar>
        <vxe-table :loading="table.loading" ref="table" id="systemdatalist" :seq-config="{
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
          <vxe-column field="name" title="名称" min-width="120"></vxe-column>
          <vxe-column field="menuNames" title="归属菜单" width="240" showOverflow="ellipsis"></vxe-column>
          <vxe-column field="isFreeze" title="禁用" align="center" width="80">
            <template v-slot="{ row }">
              <a-switch :checked="row.isFreeze" @change="isFreezeChange(row)" />
            </template>
          </vxe-column>

          <vxe-column field="orderNo" title="排序" align="center" width="80" sortable></vxe-column>
          <vxe-column field="remark" title="备注" width="150" showOverflow="ellipsis"></vxe-column>
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
          :page-size="table.page.param.size" :total="table.page.total" @change="dataInit"
          @showSizeChange="tableSizeChange"></a-pagination>
      </a-card>
    </pane>
    <edit ref="edit" v-if="edit.visible" :visible.sync="edit.visible" :data="menu.data" :title="edit.title"
      :dataId="edit.dataId" :copy="edit.copy" @save="operateStatus"></edit>

    <permission-detail ref="detail" v-if="detail.visible" :visible.sync="detail.visible" :title="detail.title"
      :id="detail.id" :access="detail.access"></permission-detail>
  </splitpanes>
</template>

<script>
import { menuQuery, query, del, isFreeze } from "@/services/system/data/list";
import edit from "./edit";
import permissionDetail from "@/pages/system/permission/detail";

import { selectTableRow, deleteConfirm } from "@/utils/util";
import { Splitpanes, Pane } from 'splitpanes'
import 'splitpanes/dist/splitpanes.css'
export default {
  components: {
    edit, permissionDetail,
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
        height: this.eipHeaderHeight() - 174 + "px",
        search: {
          option: {
            num: 4,
            config: [
              {
                field: "data.Name",
                op: "cn",
                placeholder: "请输入名称",
                title: "名称",
                value: "",
                type: "text",
              },
              {
                field: "data.Remark",
                op: "cn",
                placeholder: "请输入备注",
                title: "备注",
                value: "",
                type: "text",
              },
              {
                field: "data.IsFreeze",
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
        dataId: null,
        title: null,
        copy: false,
      },

      detail: {
        visible: false,
        title: null,
        id: null,
        access: 3,
      },

      visible: {
        update: false,
        del: false,
      },
    };
  },
  created() {
    this.menuInit();
    this.dataInit();
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
          }
          else {
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
        isFreeze({ id: row.dataId }).then((result) => {
          if (result.code == that.eipResultCode.success) {
            that.reload(false);
          }
          that.$message.destroy();
          that.$message.success(result.msg);
        });
      }
    },
    /**
     * 菜单树
     */
    menuInit() {
      let that = this;
      that.menu.spinning = true;
      menuQuery(that.eipPrivilegeAccess.data).then((result) => {
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
      return <a-icon type={slots.icon} />;
    },

    /**
     * 树选中
     */
    menuSelect(electedKeys, e) {
      this.table.page.param.id = electedKeys[0];
      this.table.page.param.current = 1;
      this.dataInit();
    },

    /**
     * 列表数据
     */
    dataInit() {
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
      this.dataInit();
    },
    /**
     *数量改变
     */
    tableSizeChange(current, pageSize) {
      this.table.page.param.size = pageSize;
      this.dataInit();
    },
    /**
     * 列表查询
     */
    tableSearch(filters) {
      this.table.page.param.current = 1;
      this.table.page.param.filters = filters;
      this.dataInit();
    },
    /**
     * 权限按钮加载完毕
     */
    toolbarOnload(buttons) {
      this.visible.add = buttons.filter((f) => f.method == "add").length > 0;
      this.visible.update =
        buttons.filter((f) => f.method == "update").length > 0;
      this.visible.del = buttons.filter((f) => f.method == "del").length > 0;
    },

    /**
     * 树更新
     */
    tableUpdateRow(row) {
      this.edit.dataId = row.dataId;
      this.edit.copy = false;
      this.edit.title = "数据权限编辑-" + row.name;
      this.edit.visible = true;
    },
    /**
     * 删除
     */
    tableDelRow(row) {
      let that = this;
      deleteConfirm(
        "数据权限【" + row.name + "】" + that.eipMsg.delete,
        function () {
          that.$loading.show({ text: that.eipMsg.delloading });
          del({ id: row.dataId }).then((result) => {
            if (result.code == that.eipResultCode.success) {
              that.reload(false);
            }
            that.$loading.hide();
            that.$message.success(result.msg);
          });
        },
        that
      );
    },

    /**
     * 新增
     */
    add() {
      this.edit.dataId = null;
      this.edit.copy = false;
      this.edit.title = "新增数据权限";
      this.edit.visible = true;
    },

    /**
     * 修改
     */
    update() {
      let that = this;
      selectTableRow(
        that.$refs.table,
        function (row) {
          that.edit.dataId = row.dataId;
          that.edit.copy = false;
          that.edit.title = "编辑数据权限-" + row.name;
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
          that.edit.dataId = row.dataId;
          that.edit.copy = true;
          that.edit.title = "复制数据权限-" + row.name;
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
              let ids = that.$utils.map(rows, (item) => item.dataId);
              del({ id: ids.join(",") }).then((result) => {
                if (result.code == that.eipResultCode.success) {
                  that.reload(false);
                }
                that.$loading.hide();
                that.$message.success(result.msg);
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
        this.reload(false);
      }
    },

    /**
     * 重新加载
     */
    reload(reset) {
      if (reset) this.table.page.param.id = this.eipEmptyGuid;
      this.table.page.param.current = 1;
      this.menuInit();
      this.dataInit();
    },

    /**
     *详情点击
     */
    detailClick() {
      let that = this;
      selectTableRow(
        that.$refs.table,
        function (row) {
          that.detail.id = row.dataId;
          that.detail.title = "数据权限使用详情-" + row.name;
          that.detail.visible = true;
        },
        that
      );
    },
  },
};
</script>