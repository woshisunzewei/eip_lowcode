<template>
  <div>
    <a-card
      class="eip-admin-card-small eip-admin-card-small-bottom-border"
      :bordered="false"
    >
      <eip-search
        :option="table.search.option"
        @search="tableSearch"
      ></eip-search>
    </a-card>

    <a-card class="eip-admin-card-small" :bordered="false">
      <vxe-toolbar
        ref="toolbar"
        custom
        print
        export
        :refresh="{ query: tableInit }"
      >
        <template v-slot:buttons>
          <eip-toolbar
            @del="del"
            @update="update"
            @add="add"
            @onload="toolbarOnload"
            @downTemplate="downTemplate"
            @dataImport="dataImport"
            @mapShow="mapShow"
            @treeShow="treeShow"
          ></eip-toolbar>
        </template>
      </vxe-toolbar>
      <vxe-table
        :loading="table.loading"
        ref="table"
        id="systemorganizationlist"
        :seq-config="{
          startIndex: (table.page.param.current - 1) * table.page.param.size,
        }"
        :height="table.height"
        :export-config="{}"
        :print-config="{}"
        :data="table.data"
        :tooltip-config="tooltipConfig"
        :sort-config="{
          trigger: 'cell',
          defaultSort: { field: 'orderNo', order: 'asc' },
          orders: ['desc', 'asc'],
        }"
        @sort-change="tableSort"
        :menu-config="{ body: { options: organization.right } }"
        @menu-click="contextMenuClickEvent"
        :tree-config="{
          transform: true,
          rowField: 'organizationId',
          parentField: 'parentId',
          reserve: true,
          trigger: 'row',
        }"
        :row-config="{ keyField: 'organizationId', isHover: true }"
      >
        <template #loading>
          <a-spin></a-spin>
        </template>
        <template #empty>
          <eip-empty />
        </template>
        <vxe-column type="radio" width="40" align="center" fixed="left">
          <template #radio="{ row, checked }">
            <span @click.stop="$refs.runTable.toggleCheckboxRow(row)">
              <a-radio :checked="checked"> </a-radio>
            </span>
          </template>
        </vxe-column>
        <vxe-column type="seq" title="序号" width="100"></vxe-column>
        <vxe-column
          field="name"
          tree-node
          title="全称"
          width="250"
        ></vxe-column>
        <vxe-column field="shortName" title="简称" width="150"> </vxe-column>
        <vxe-column title="经纬度" width="300">
          <template #default="{ row }">
            <a-tag color="blue" v-if="row.longitude">
              经度：{{ row.longitude }}
            </a-tag>
            <a-tag color="purple" v-if="row.latitude">
              纬度：{{ row.latitude }}
            </a-tag>
          </template>
        </vxe-column>
        <vxe-column
          field="nature"
          title="类型"
          align="center"
          width="80"
          sortable
        >
          <template v-slot="{ row }">
            {{
              eipOrganizationNatures.filter((f) => f.value == row.nature)
                .length > 0
                ? eipOrganizationNatures.filter((f) => f.value == row.nature)[0]
                : "".name
            }}
          </template>
        </vxe-column>

        <vxe-column
          field="mainSupervisor"
          title="主负责人"
          width="100"
        ></vxe-column>

        <vxe-column
          field="mainSupervisorContact"
          title="主负责人联系方式"
          width="150"
        ></vxe-column>

        <vxe-column
          field="isFreeze"
          title="禁用"
          align="center"
          width="80"
          sortable
        >
          <template v-slot="{ row }">
            <a-switch :checked="row.isFreeze" @change="isFreezeChange(row)" />
          </template>
        </vxe-column>
        <vxe-column
          field="orderNo"
          title="排序"
          align="center"
          width="80"
          sortable
        ></vxe-column>
        <vxe-column
          field="remark"
          title="备注"
          min-width="150"
          showOverflow="ellipsis"
        ></vxe-column>
        <vxe-column title="操作" align="center" fixed="right" width="100px">
          <template #default="{ row }">
            <a-tooltip
              @click="tableUpdateRow(row)"
              title="编辑"
              v-if="visible.update"
            >
              <label class="text-eip eip-cursor-pointer">编辑</label>
            </a-tooltip>
            <a-divider type="vertical" v-if="visible.update" />
            <a-tooltip
              title="删除"
              v-if="visible.del"
              @click="tableDelRow(row)"
            >
              <label class="text-red eip-cursor-pointer">删除</label>
            </a-tooltip>
          </template>
        </vxe-column>
      </vxe-table>
    </a-card>

    <edit
      ref="edit"
      v-if="edit.visible"
      :visible.sync="edit.visible"
      :title="edit.title"
      :organizationId="edit.organizationId"
      :parentId="edit.parentId"
      :parentName="edit.parentName"
      @save="operateStatus"
    >
    </edit>

    <maplist
      ref="map"
      v-if="map.visible"
      :visible.sync="map.visible"
      :title="map.title"
    >
    </maplist>

    <tree
      ref="tree"
      v-if="tree.visible"
      :visible.sync="tree.visible"
      :title="tree.title"
    >
    </tree>
  </div>
</template>

<script>
import {
  query,
  del,
  isFreeze,
  downImportTemplate,
  systemOrganizationImport,
} from "@/services/system/organization/list";
import edit from "./edit";
import maplist from "./maplist";
import tree from "./tree";

import { selectTableRowRadio, deleteConfirm } from "@/utils/util";
export default {
  components: { edit, maplist, tree },
  data() {
    return {
      map: {
        visible: false,
        title: "",
      },
      tree: {
        visible: false,
        title: "",
      },
      tooltipConfig: {
        showAll: true,
        enterable: true,
        contentMethod: ({ type, column, row, items, _columnIndex }) => {
          const { property } = column;
          if (row && property === "parentIdsName") {
            return row[property];
          }
          return null;
        },
      },
      organization: {
        data: [],
        right: [],
      },
      table: {
        page: {
          param: {
            current: 1,
            size: 500,
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

        search: {
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
                field: "ShortName",
                op: "cn",
                placeholder: "请输入简称",
                title: "简称",
                value: "",
                type: "text",
              },
              {
                field: "MainSupervisor",
                op: "cn",
                placeholder: "请输入主负责人",
                title: "主负责人",
                value: "",
                type: "text",
              },
              {
                field: "Remark",
                op: "cn",
                placeholder: "请输入备注",
                title: "备注",
                value: "",
                type: "text",
              },
              {
                field: "IsFreeze",
                op: "eq",
                placeholder: "请选择禁用状态",
                title: "禁用",
                type: "bool",
                options: {
                  yes: "是",
                  no: "否",
                },
              },
            ],
          },
        },

        height: this.eipHeaderHeight() - 122 + "px",
      },

      edit: {
        visible: false,
        organizationId: "",
        parentId: "",
        parentName: null,
        title: "",
      },

      visible: {
        add: false,
        update: false,
        del: false,
      },
    };
  },

  created() {
    this.tableInit();
  },
  mounted() {
    this.$refs.table.connect(this.$refs.toolbar);
  },
  methods: {
    /**
     * 组织树
     */
    treeShow() {
      let that = this;
      that.tree.visible = true;
    },
    /**
     * 区域地图
     */
    mapShow() {
      let that = this;
      that.map.visible = true;
    },

    /**
     * 右键点击
     * @param {*} param0
     */
    contextMenuClickEvent({ menu, row, column }) {
      switch (menu.code) {
        case "1":
          this.organizationAdd(row);
          break;
        case "2":
          this.organizationUpdate(row);
          break;
        case "3":
          this.organizationDel(row);
          break;
        default:
          break;
      }
    },

    /**
     *冻结
     */
    isFreezeChange(row) {
      if (this.visible.update) {
        let that = this;
        isFreeze({ id: row.organizationId }).then((result) => {
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
     * 树删除
     */
    organizationDel(item) {
      let that = this;
      deleteConfirm(
        "组织架构【" + item.name + "】" + that.eipMsg.delete,
        function () {
          that.$loading.show({ text: that.eipMsg.delloading });
          del({ id: item.organizationId }).then((result) => {
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
     * 树新增
     */
    organizationAdd(item) {
      this.edit.title = "新增组织架构";
      this.edit.parentId = item.organizationId;
      this.edit.parentName = item.name;
      this.edit.organizationId = null;
      this.edit.visible = true;
    },

    /**
     * 树更新
     */
    organizationUpdate(item) {
      this.edit.organizationId = item.organizationId;
      this.edit.parentId = "";
      this.edit.parentName = "";
      this.edit.title = "编辑组织架构-" + item.name;
      this.edit.visible = true;
    },
    /**
     * 树右键
     */
    right({ event, node }) {
      this.organization.right.visible = true;
      this.organization.right.item = {
        id: node._props.eventKey,
        title: node._props.title,
      };
    },

    /**
     * 树右键菜单隐藏
     */
    hideRight() {
      this.organization.right.visible = false;
      this.organization.right.item = null;
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
     * 权限按钮加载完毕
     */
    toolbarOnload(buttons) {
      let addButton = buttons.filter((f) => f.method == "add");
      this.visible.add = addButton.length > 0;

      let updateButton = buttons.filter((f) => f.method == "update");
      this.visible.update = updateButton.length > 0;

      let delButton = buttons.filter((f) => f.method == "del");
      this.visible.del = delButton.length > 0;

      var organizations = [];
      if (this.visible.add) {
        organizations.push({
          code: "1",
          prefixIcon: "vxe-icon-add",
          name: addButton[0].name,
        });
      }
      if (this.visible.update) {
        organizations.push({
          code: "2",
          prefixIcon: "vxe-icon-" + updateButton[0].icon,
          name: updateButton[0].name,
        });
      }
      if (this.visible.del) {
        organizations.push({
          code: "3",
          prefixIcon: "vxe-icon-" + delButton[0].icon,
          name: delButton[0].name,
        });
      }
      this.organization.right.push(organizations);
    },

    /**
     * 树更新
     */
    tableUpdateRow(row) {
      this.edit.title = "组织架构编辑-" + row.name;
      this.edit.parentId = "";
      this.edit.parentName = "";
      this.edit.organizationId = row.organizationId;
      this.edit.visible = true;
    },

    /**
     * 删除
     */
    tableDelRow(row) {
      let that = this;
      deleteConfirm(
        "组织架构【" + row.name + "】" + that.eipMsg.delete,
        function () {
          that.$loading.show({ text: that.eipMsg.delloading });
          del({ id: row.organizationId }).then((result) => {
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
      this.edit.title = "新增组织架构";
      this.edit.parentId = undefined;
      this.edit.parentName = "";
      this.edit.organizationId = null;
      this.edit.visible = true;
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
          that.edit.organizationId = row.organizationId;
          that.edit.title = "编辑组织架构-" + row.name;
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
      selectTableRowRadio(
        that.$refs.table,
        function (row) {
          //提示是否删除
          deleteConfirm(
            that.eipMsg.delete,
            function () {
              that.$loading.show({ text: that.eipMsg.delloading });
              del({ id: row.organizationId }).then((result) => {
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

    /**
     * 下载导入模版
     */
    downTemplate() {
      downImportTemplate();
    },

    /**
     *
     */
    dataImport(file) {
      let that = this;
      const formData = new FormData();
      formData.append("Files", file.file);
      that.$message.loading({
        content: "导入中...",
        duration: 0,
      });
      systemOrganizationImport(formData)
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
        .catch(() => {
          that.$message.error("上传出错");
        });
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
      if (reset) this.table.page.param.id = null;
      this.table.page.param.current = 1;
      this.tableInit();
    },
  },
};
</script>
