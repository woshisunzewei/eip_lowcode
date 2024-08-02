<template>
  <splitpanes class="default-theme" style="height: auto">
    <pane :min-size="split.minWidth" :size="split.minWidth">
      <a-card @contextDictionary.prevent size="small" class="eip-admin-card-small">
        <div slot="title">
          <a-space>
            <a-input-search v-model="organization.key" :allowClear="true" placeholder="名称/拼音模糊搜索"
              @change="organizationSearch" />
            <a-tooltip title="刷新">
              <a-button type="primary" @click="reload(true)">
                <a-icon type="redo" />
              </a-button>
            </a-tooltip>
          </a-space>
        </div>
        <a-spin :spinning="organization.spinning" :delay="0" :style="organization.style">
          <div>
            <a-directory-tree v-if="organization.data.length" :expandAction="false"
              :defaultExpandedKeys="[organization.data.length > 0 ? organization.data[0].key : '']"
              :tree-data="organization.data" :icon="organizationIcon" @select="organizationSelect"></a-directory-tree>
          </div>
        </a-spin>
      </a-card>
    </pane>
    <pane min-size="50">
      <a-card class="eip-admin-card-small eip-admin-card-small-bottom-border" :bordered="false">
        <eip-search :option="table.search.option" @search="tableSearch"></eip-search>
      </a-card>

      <a-card class="eip-admin-card-small" :bordered="false">
        <vxe-toolbar ref="toolbar" custom print export :refresh="{ query: groupInit }">
          <template v-slot:buttons>
            <eip-toolbar @del="del" @update="update" @add="add" @groupUser="groupUser" @menu="menu"
              @menuButton="menuButton" @data="data" @menu-button-data="menuMenuButtonData"
              @onload="toolbarOnload"></eip-toolbar>
          </template>
        </vxe-toolbar>
        <vxe-table :loading="table.loading" ref="table" id="systemgrouplist" :seq-config="{
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
          <vxe-column field="name" width="150" title="名称"></vxe-column>
          <vxe-column field="parentIdsName" width="150" title="归属组织" showOverflow="ellipsis"></vxe-column>
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
          :page-size="table.page.param.size" :total="table.page.total" @change="groupInit"
          @showSizeChange="tableSizeChange"></a-pagination>
      </a-card>
    </pane>
    <edit ref="edit" v-if="edit.visible" :visible.sync="edit.visible" :data="organization.data" :title="edit.title"
      :groupId="edit.groupId" @save="operateStatus"></edit>

    <eip-privilegemasteruser ref="chosenPrivilegeMasterUser" v-if="toolbar.chosenPrivilegeMasterUser.visible"
      :visible.sync="toolbar.chosenPrivilegeMasterUser.visible" :title="toolbar.chosenPrivilegeMasterUser.title"
      :privilegeMaster="privilege.master" :privilegeMasterValue="privilege.masterValue"
      :data="organization.data"></eip-privilegemasteruser>

    <eip-permission-menu ref="permissionMenu" v-if="permission.menu.visible" :visible.sync="permission.menu.visible"
      :title="permission.menu.title" :privilegeMaster="privilege.master"
      :privilegeMasterValue="privilege.masterValue"></eip-permission-menu>

    <eip-permission-menu-button ref="permissionMenuButton" v-if="permission.menuButton.visible"
      :visible.sync="permission.menuButton.visible" :title="permission.menuButton.title"
      :privilegeMaster="privilege.master" :privilegeMasterValue="privilege.masterValue"></eip-permission-menu-button>

    <eip-permission-data ref="permissionData" v-if="permission.data.visible" :visible.sync="permission.data.visible"
      :title="permission.data.title" :privilegeMaster="privilege.master"
      :privilegeMasterValue="privilege.masterValue"></eip-permission-data>

    <eip-permission-menu-menuButton-data ref="permissionMenuMenuButtonData" v-if="permission.menuMenuButtonData.visible"
      :visible.sync="permission.menuMenuButtonData.visible" :title="permission.menuMenuButtonData.title"
      :privilegeMaster="privilege.master"
      :privilegeMasterValue="privilege.masterValue"></eip-permission-menu-menuButton-data>
  </splitpanes>
</template>

<script>
import {
  organizationQuery,
  query,
  del,
  isFreeze,
} from "@/services/system/group/list";
import edit from "./edit";

import { selectTableRow, deleteConfirm } from "@/utils/util";
import eipPermissionMenu from "@/pages/system/permission/menu";
import eipPermissionMenuButton from "@/pages/system/permission/menubutton";
import eipPermissionData from "@/pages/system/permission/data";
import eipPermissionMenuMenuButtonData from "@/pages/system/permission/menu-menubutton-data";
import { Splitpanes, Pane } from 'splitpanes'
import 'splitpanes/dist/splitpanes.css'
export default {
  components: {
    edit,
    eipPermissionMenu,
    eipPermissionMenuButton,
    eipPermissionData,
    eipPermissionMenuMenuButtonData,
    Splitpanes,
    Pane
  },
  data() {
    return {
      split: {
        minWidth: (250 / window.innerWidth) * 100,
      },
      organization: {
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
        height: this.eipHeaderHeight() - 162 + "px",
        search: {
          option: {
            num: 4,
            config: [
              {
                field: "gr.Name",
                op: "cn",
                placeholder: "请输入名称",
                title: "名称",
                value: "",
                type: "text",
              },
              {
                field: "gr.Remark",
                op: "cn",
                placeholder: "请输入备注",
                title: "备注",
                value: "",
                type: "text",
              },
              {
                field: "gr.IsFreeze",
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
      toolbar: {
        chosenPrivilegeMasterUser: {
          visible: false,
          title: "",
        },
      },
      edit: {
        visible: false,
        groupId: "",
        title: "",
      },

      visible: {
        update: false,
        del: false,
      },

      permission: {
        menu: {
          visible: false,
          title: "",
        },

        menuButton: {
          visible: false,
          title: "",
        },

        data: {
          visible: false,
          title: "",
        },

        menuMenuButtonData: {
          visible: false,
          title: "",
        },
      },

      privilege: {
        master: this.eipPrivilegeMaster.group,
        masterValue: null,
      },
    };
  },
  created() {
    this.organizationInit();
    this.groupInit();
  },
  mounted() {
    this.$refs.table.connect(this.$refs.toolbar);
  },
  methods: {
    organizationSearch(e) {
      var that = this;
      this.organization.data = that.$utils.clone(this.organization.original, true);
      this.organization.data = that.$utils.searchTree(
        this.organization.data,
        (item) => {
          if (that.organization.key) {
            var titlePinyin = pinyin.convert(item.title).toLowerCase();
            if (
              item.title
                .toLowerCase()
                .includes(that.organization.key.toLowerCase())
            ) {
              return true;
            } else if (
              titlePinyin.includes(that.organization.key.toLowerCase())
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
    },
    /**
     *冻结
     */
    isFreezeChange(row) {
      if (this.visible.update) {
        let that = this;
        isFreeze({ id: row.groupId }).then((result) => {
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
    organizationInit() {
      let that = this;
      that.organization.spinning = true;
      organizationQuery().then((result) => {
        that.organization.data = result.data;
        that.organization.original = result.data;
        that.organization.spinning = false;
        that.organizationSearch();
      });
    },

    /**
     * 树图标
     */
    organizationIcon(props) {
      const { expanded } = props;
      if (props.children.length == 0) {
        return <a-icon type="file" />;
      }
      return <a-icon type={expanded ? "folder-open" : "folder"} />;
    },

    /**
     * 树选中
     */
    organizationSelect(electedKeys, e) {
      this.table.page.param.id = electedKeys[0];
      this.table.page.param.current = 1;
      this.groupInit();
    },
    /**
     * 列表数据
     */
    groupInit() {
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
      this.groupInit();
    },
    /**
     *数量改变
     */
    tableSizeChange(current, pageSize) {
      this.table.page.param.size = pageSize;
      this.groupInit();
    },
    /**
     * 列表查询
     */
    tableSearch(filters) {
      this.table.page.param.current = 1;
      this.table.page.param.filters = filters;
      this.groupInit();
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
      this.edit.groupId = row.groupId;
      this.edit.title = "组编辑-" + row.name;
      this.edit.visible = true;
    },
    /**
     * 删除
     */
    tableDelRow(row) {
      let that = this;
      deleteConfirm(
        "组【" + row.name + "】" + that.eipMsg.delete,
        function () {
          that.$loading.show({ text: that.eipMsg.delloading });
          del({ id: row.groupId }).then((result) => {
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
     * 新增
     */
    add() {
      this.edit.groupId = null;
      this.edit.title = "新增组";
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
          that.edit.groupId = row.groupId;
          that.edit.title = "编辑组-" + row.name;
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
              let ids = that.$utils.map(rows, (item) => item.groupId);
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
        this.reload(false);
      }
    },

    /**
     * 重新加载
     */
    reload(reset) {
      if (reset) this.table.page.param.id = this.eipEmptyGuid;
      this.table.page.param.current = 1;
      this.organizationInit();
      this.groupInit();
    },

    /**
     * 选择人员
     */
    groupUser() {
      let that = this;
      selectTableRow(
        that.$refs.table,
        function (row) {
          that.privilege.masterValue = row.groupId;
          that.toolbar.chosenPrivilegeMasterUser.visible = true;
          that.toolbar.chosenPrivilegeMasterUser.title = "组人员-" + row.name;
        },
        that
      );
    },

    /**
     * 菜单权限
     */
    menu() {
      let that = this;
      selectTableRow(
        that.$refs.table,
        function (row) {
          that.privilege.masterValue = row.groupId;
          that.permission.menu.visible = true;
          that.permission.menu.title = "组模块权限-" + row.name;
        },
        that
      );
    },

    /**
     * 组按钮
     */
    menuButton() {
      let that = this;
      selectTableRow(
        that.$refs.table,
        function (row) {
          that.privilege.masterValue = row.groupId;
          that.permission.menuButton.visible = true;
          that.permission.menuButton.title = "组按钮权限-" + row.name;
        },
        that
      );
    },

    menuButtonData() {
      let that = this;
      selectTableRow(
        that.$refs.table,
        function (row) {
          that.privilege.masterValue = row.groupId;
          that.permission.menuButton.visible = true;
          that.permission.menuButton.title = "组按钮权限-" + row.name;
        },
        that
      );
    },

    /**
     * 组菜单,按钮,数据权限
     */
    menuMenuButtonData() {
      let that = this;
      selectTableRow(
        that.$refs.table,
        function (row) {
          that.privilege.masterValue = row.groupId;
          that.permission.menuMenuButtonData.visible = true;
          that.permission.menuMenuButtonData.title = "组权限-" + row.name;
        },
        that
      );
    },

    /**
     * 数据权限
     */
    data() {
      let that = this;
      selectTableRow(
        that.$refs.table,
        function (row) {
          that.privilege.masterValue = row.groupId;
          that.permission.data.visible = true;
          that.permission.data.title = "组数据权限-" + row.name;
        },
        that
      );
    },
  },
};
</script>