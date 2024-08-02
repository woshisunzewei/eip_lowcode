<template>
  <div style="width: 100%">

    <a-card class="eip-admin-card-small eip-admin-card-small-bottom-border" :bordered="false">
      <eip-search :option="table.search.option" @search="tableSearch" @advanced="tableAdvanced"></eip-search>
    </a-card>

    <a-card class="eip-admin-card-small" :bordered="false">
      <vxe-toolbar ref="toolbar" custom print export :refresh="{ query: tableInit }">
        <template v-slot:buttons>
          <eip-toolbar @del="del" @update="update" @add="add" @copy="copy" @detail="detailClick"
            @onload="toolbarOnload"></eip-toolbar>
          <a-space class="margin-left-sm">
            <a-button icon="fullscreen" type="primary" @click="$refs.table.setAllTreeExpand(true)">展开所有</a-button>
            <a-button icon="fullscreen-exit" type="primary" @click="$refs.table.clearTreeExpand()">关闭所有</a-button>
          </a-space>
        </template>
      </vxe-toolbar>
      <vxe-table :loading="table.loading" ref="table" id="systemmenulist" :seq-config="{
        startIndex:
          (table.page.param.current - 1) * table.page.param.size,
      }" :height="table.height" :export-config="{}" :print-config="{}" :data="table.data"
        :tooltip-config="tooltipConfig" :sort-config="{
          trigger: 'cell',
          defaultSort: { field: 'orderNo', order: 'asc' },
          orders: ['desc', 'asc'],
        }" @sort-change="tableSort" :menu-config="{ body: { options: menu.right } }"
        @menu-click="contextMenuClickEvent"
        :tree-config="{ transform: true, rowField: 'menuId', parentField: 'parentId', reserve: true, trigger: 'row' }"
        :row-config="{ keyField: 'menuId', isHover: true }">
        <template #loading>
          <a-spin></a-spin>
        </template>
        <template #empty>
          <eip-empty />
        </template>
        <vxe-column type="radio" width="40" align="center" fixed="left">
          <template #radio="{ row, checked }">
            <span @click.stop="$refs.runTable.toggleCheckboxRow(row)">
              <a-radio :checked="checked">
              </a-radio>
            </span>
          </template>
        </vxe-column>
        <vxe-column type="seq" title="序号" width="60"></vxe-column>
        <vxe-column field="name" tree-node title="名称" min-width="250" showOverflow="ellipsis">
          <template #default="{ row }">
            <a-icon :type="row.icon" :theme="row.theme" /><span class="margin-left-xs" v-html="row.name"></span>
          </template>
        </vxe-column>

        <vxe-column field="isShowMenu" title="页面显示" align="center" width="100" sortable>
          <template v-slot="{ row }">
            <a-switch :checked="row.isShowMenu" @change="isShowMenuChange(row)" />
          </template>
        </vxe-column>
        <vxe-column field="haveMenuPermission" title="菜单权限" align="center" width="100" sortable>
          <template v-slot="{ row }">
            <a-switch :checked="row.haveMenuPermission" @change="haveMenuPermissionChange(row)" />
          </template>
        </vxe-column>
        <vxe-column field="haveButtonPermission" title="按钮权限" align="center" width="100" sortable>
          <template v-slot="{ row }">
            <a-switch :checked="row.haveButtonPermission" @change="haveButtonPermissionChange(row)" />
          </template>
        </vxe-column>
        <vxe-column field="haveDataPermission" title="数据权限" align="center" width="100" sortable>
          <template v-slot="{ row }">
            <a-switch :checked="row.haveDataPermission" @change="haveDataPermissionChange(row)" />
          </template>
        </vxe-column>

        <vxe-column field="isAgileMenu" title="应用菜单" align="center" width="100" sortable>
          <template v-slot="{ row }">
            <a-switch :checked="row.isAgileMenu" />
          </template>
        </vxe-column>

        <vxe-column field="agileMenuType" title="类型" align="center" width="80" sortable>
          <template v-slot="{ row }">
            <a-tag color="#f50" v-if="row.agileMenuType == eipAgileMenuType.group">
              组
            </a-tag>
            <a-tag color="#2db7f5" v-else-if="row.agileMenuType == eipAgileMenuType.worksheet">
              表
            </a-tag>
            <a-tag color="#87d068" v-else-if="row.agileMenuType == eipAgileMenuType.iframe">
              嵌
            </a-tag>
            <a-tag color="#87d068" v-else-if="row.agileMenuType == eipAgileMenuType.custom">
              自
            </a-tag>
            <a-tag color="#87d068" v-else-if="row.agileMenuType == eipAgileMenuType.list">
              列
            </a-tag>
            <a-tag color="#87d068" v-else-if="row.agileMenuType == eipAgileMenuType.form">
              单
            </a-tag>
            <a-tag color="#87d068" v-else-if="row.agileMenuType == eipAgileMenuType.page">
              页
            </a-tag>
            <a-tag color="#108ee9" v-else>
              系
            </a-tag>
          </template>
        </vxe-column>
        <vxe-column field="path" title="地址" showOverflow="ellipsis" width="200"></vxe-column>
        <vxe-column field="isFreeze" title="禁用" align="center" width="100" sortable>
          <template v-slot="{ row }">
            <a-switch :checked="row.isFreeze" @change="isFreezeChange(row)" />
          </template>
        </vxe-column>
        <vxe-column field="icon" title="图片" width="60" align="center">
          <template v-slot="{ row }">
            <a-avatar shape="square" v-if="row.image" :src="row.image" />
          </template>
        </vxe-column>
        <vxe-column field="orderNo" title="排序" align="center" width="60"></vxe-column>
        <vxe-column field="remark" title="备注" min-width="150" showOverflow="ellipsis"></vxe-column>
        <vxe-column field="menuId" title="记录ID" width="350">
          <template v-slot="{ row }">
            <a-tooltip>
              <template slot="title">
                点击复制
              </template>
              <a-icon class="menuIdCopy" :data-clipboard-text="row.menuId" @click="menuIdCopy()" type="copy"></a-icon>
            </a-tooltip>
            <label>{{ row.menuId }}</label>
          </template>
        </vxe-column>
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
    </a-card>

    <edit ref="edit" v-if="edit.visible" :visible.sync="edit.visible" :data="menu.data" :title="edit.title"
      :menuId="edit.menuId" :copy="edit.copy" :parentId="edit.parentId" :parentName="edit.parentName"
      @save="operateStatus">
    </edit>

    <permission-detail ref="detail" v-if="detail.visible" :visible.sync="detail.visible" :title="detail.title"
      :id="detail.id" :access="detail.access"></permission-detail>
  </div>
</template>

<script>
import Clipboard from "clipboard";
import {
  menuQuery,
  query,
  del,
  isShowMenu,
  haveMenuPermission,
  haveDataPermission,
  haveFieldPermission,
  haveButtonPermission,
  isFreeze,
} from "@/services/system/menu/list";
import edit from "./edit";
import permissionDetail from "@/pages/system/permission/detail";

import { selectTableRowRadio, deleteConfirm } from "@/utils/util";
export default {
  components: { edit, permissionDetail },
  data() {
    return {
      tooltipConfig: {
        showAll: true,
        enterable: true,
        contentMethod: ({ type, column, row, items, _columnIndex }) => {
          const { field } = column
          if (field === 'menuId') {
            return ''
          }
          return null
        }
      },
      menu: {
        data: [],
        right: []
      },
      table: {
        page: {
          param: {
            current: 1,
            size: this.eipPage.size,
            sord: "asc",
            sidx: "",
            id: null,
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
            num: 4,
            config: [
              {
                field: "menu.Name",
                op: "cn",
                placeholder: "请输入菜单名",
                title: "名称",
                value: "",
                type: "text",
              },
              {
                field: "menu.Path",
                op: "cn",
                placeholder: "请输入地址",
                title: "地址",
                value: "",
                type: "text",
              },
              {
                field: "menu.Remark",
                op: "cn",
                placeholder: "请输入备注",
                title: "备注",
                value: "",
                type: "text",
              },
              {
                field: "menu.IsShowMenu",
                op: "eq",
                title: "页面显示",
                placeholder: "请选择页面显示",
                type: "bool",
                options: {
                  yes: '是',
                  no: '否'
                }
              },
              {
                field: "HaveMenuPermission",
                op: "eq",
                title: "菜单权限",
                placeholder: "请选择菜单权限",
                type: "bool",
                options: {
                  yes: '是',
                  no: '否'
                }
              },
              {
                field: "HaveButtonPermission",
                op: "eq",
                title: "按钮权限",
                placeholder: "请选择按钮权限",
                type: "bool",
                options: {
                  yes: '是',
                  no: '否'
                }
              },
              {
                field: "HaveDataPermission",
                op: "eq",
                title: "数据权限",
                placeholder: "请选择数据权限",
                type: "bool",
                options: {
                  yes: '是',
                  no: '否'
                }
              },
              {
                field: "HaveFieldPermission",
                op: "eq",
                title: "字段权限",
                placeholder: "请选择字段权限",
                type: "bool",
                options: {
                  yes: '是',
                  no: '否'
                }
              },
              {
                field: "IsFreeze",
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
        menuId: null,
        title: null,
        parentId: undefined,
        parentName: null,
        copy: false,
      },

      detail: {
        visible: false,
        title: null,
        id: null,
        access: 0,
      },

      visible: {
        add: false,
        update: false,
        del: false,
      },
    };
  },
  created() {
    this.menuInit();
    this.tableInit();
  },
  mounted() {
    this.$refs.table.connect(this.$refs.toolbar);
  },
  methods: {
    /**
           * 
           */
    menuIdCopy() {
      // 复制数据
      let clipboard = new Clipboard('.menuIdCopy');
      clipboard.on("success", () => {
        this.$message.success("复制成功");
      });
      clipboard.on("error", () => {
        this.$message.error("复制失败");
      });
      setTimeout(() => {
        // 销毁实例
        clipboard.destroy();
      }, 122);
    },
    /**
     * 菜单树
     */
    menuInit() {
      let that = this;
      menuQuery().then((result) => {
        that.menu.data = result.data;
      });
    },

    /**
     * 列表数据
     */
    tableInit() {
      let that = this;
      that.table.loading = true;
      query(that.table.page.param).then((result) => {
        if (result.code == that.eipResultCode.success) {
          that.table.data = result.data;
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
      this.tableInit();
    },
    /**
     *数量改变
     */
    tableSizeChange(current, pageSize) {
      this.table.page.param.size = pageSize;
      this.tableInit();
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
     * 树更新
     */
    tableUpdateRow(row) {
      this.edit.title = "模块编辑-" + row.name;
      this.edit.parentId = "";
      this.edit.parentName = "";
      this.edit.menuId = row.menuId;
      this.edit.visible = true;
    },
    /**
     * 删除
     */
    tableDelRow(row) {
      let that = this;
      deleteConfirm(
        "模块【" + row.name + "】" + that.eipMsg.delete,
        function () {
          that.$loading.show({ text: that.eipMsg.delloading });
          del({ id: row.menuId }).then((result) => {
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
      this.edit.title = "模块新增";
      this.edit.parentId = undefined;
      this.edit.parentName = "";
      this.edit.menuId = null;
      this.edit.visible = true;
      this.edit.copy = false;
    },

    /**
     * 修改
     */
    update() {
      let that = this;
      selectTableRowRadio(
        that.$refs.table,
        function (row) {
          that.edit.parentId = "";
          that.edit.parentName = "";
          that.edit.menuId = row.menuId;
          that.edit.title = "模块编辑-" + row.name;
          that.edit.visible = true;
          that.edit.copy = false;
        },
        that
      );
    },
    /**
     * 复制
     */
    copy() {
      let that = this;
      selectTableRowRadio(
        that.$refs.table,
        function (row) {
          that.edit.parentId = "";
          that.edit.parentName = "";
          that.edit.menuId = row.menuId;
          that.edit.title = "模块复制-" + row.name;
          that.edit.visible = true;
          that.edit.copy = true;
        },
        that
      );
    },

    /**
     * 删除
     */
    del() {
      let that = this;
      selectTableRowRadio(
        that.$refs.table,
        function (row) {
          //提示是否删除
          deleteConfirm(
            that.eipMsg.delete,
            function () {
              that.$loading.show({ text: that.eipMsg.delloading });
              del({ id: row.menuId }).then((result) => {
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
      this.tableInit();
    },
    /**
     * 右键点击
     * @param {*} param0 
     */
    contextMenuClickEvent({ menu, row, column }) {
      switch (menu.code) {
        case "1":
          this.menuAdd(row);
          break;
        case "2":
          this.menuUpdate(row);
          break;
        case "3":
          this.menuDel(row);
          break;
        default:
          break;
      }
    },

    /**
     * 树新增
     */
    menuAdd(item) {
      this.edit.parentId = item.menuId;
      this.edit.parentName = item.name;
      this.edit.menuId = null;
      this.edit.title = "模块新增";
      this.edit.visible = true;
    },

    /**
     * 树更新
     */
    menuUpdate(item) {
      this.edit.menuId = item.menuId;
      this.edit.parentId = "";
      this.edit.parentName = "";
      this.edit.copy = false;
      this.edit.title = "模块编辑-" + item.name;
      this.edit.visible = true;
    },

    /**
     * 树删除
     */
    menuDel(item) {
      let that = this;
      deleteConfirm(
        "模块【" + item.name + "】" + that.eipMsg.delete,
        function () {
          that.$loading.show({ text: that.eipMsg.delloading });
          del({ id: item.menuId }).then((result) => {
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
     * 权限按钮加载完毕
     */
    toolbarOnload(buttons) {
      let addButton = buttons.filter((f) => f.method == "add");
      this.visible.add = addButton.length > 0;

      let updateButton = buttons.filter((f) => f.method == "update");
      this.visible.update = updateButton.length > 0;

      let delButton = buttons.filter((f) => f.method == "del");
      this.visible.del = delButton.length > 0;
      var menus = [];
      if (this.visible.add) {
        menus.push({
          code: "1",
          prefixIcon: 'vxe-icon-add',
          name: addButton[0].name,
        });
      }
      if (this.visible.update) {
        menus.push({
          code: "2",
          prefixIcon: 'vxe-icon-' + updateButton[0].icon,
          name: updateButton[0].name,
        });
      }
      if (this.visible.del) {
        menus.push({
          code: "3",
          prefixIcon: 'vxe-icon-' + delButton[0].icon,
          name: delButton[0].name,
        });
      }
      this.menu.right.push(menus)
    },

    /**
     * 调整,收起展开高度
     */
    tableAdvanced(advanced) {
      this.table.height = this.eipHeaderHeight() - (advanced ? 202 : 122);
    },

    /**
     *详情点击
     */
    detailClick() {
      let that = this;
      selectTableRowRadio(
        that.$refs.table,
        function (row) {
          that.detail.id = row.menuId;
          that.detail.title = "模块使用详情-" + row.name;
          that.detail.visible = true;
        },
        that
      );
    },

    /**
     *
     */
    isShowMenuChange(row) {
      if (this.visible.update) {
        let that = this;
        isShowMenu({ id: row.menuId }).then((result) => {
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
     *
     */
    haveMenuPermissionChange(row) {
      if (this.visible.update) {
        let that = this;
        haveMenuPermission({ id: row.menuId }).then((result) => {
          if (result.code == that.eipResultCode.success) {
            that.reload(false);
          }
          that.$message.destroy();
          that.$message.success(result.msg);
        });
      }
    },

    /**
     *
     */
    haveDataPermissionChange(row) {
      if (this.visible.update) {
        let that = this;
        haveDataPermission({ id: row.menuId }).then((result) => {
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
     *
     */
    haveFieldPermissionChange(row) {
      if (this.visible.update) {
        let that = this;
        haveFieldPermission({ id: row.menuId }).then((result) => {
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
     *
     */
    haveButtonPermissionChange(row) {
      if (this.visible.update) {
        let that = this;
        haveButtonPermission({ id: row.menuId }).then((result) => {
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
     *
     */
    isFreezeChange(row) {
      if (this.visible.update) {
        let that = this;
        isFreeze({ id: row.menuId }).then((result) => {
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
  },
};
</script>