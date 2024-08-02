<template>
  <div style="width: 100%">

    <a-card class="eip-admin-card-small eip-admin-card-small-bottom-border" :bordered="false">
      <eip-search :option="table.search.option" @search="tableSearch"></eip-search>
    </a-card>

    <a-card class="eip-admin-card-small" :bordered="false">
      <vxe-toolbar ref="toolbar" custom print export :refresh="{ query: tableInit }">
        <template v-slot:buttons>
          <eip-toolbar @del="del" @update="update" @add="add" @copy="copy" @onload="toolbarOnload"></eip-toolbar>
          <a-space class="margin-left-sm">
            <a-button icon="fullscreen" type="primary" @click="$refs.table.setAllTreeExpand(true)">展开所有</a-button>
            <a-button icon="fullscreen-exit" type="primary" @click="$refs.table.clearTreeExpand()">关闭所有</a-button>
          </a-space>
        </template>
      </vxe-toolbar>
      <vxe-table :loading="table.loading" ref="table" id="systemDictionaryList"
        :row-config="{ keyField: 'dictionaryId', isHover: true }"
        :tree-config="{ transform: true, rowField: 'dictionaryId', parentField: 'parentId', reserve: true, trigger: 'row' }"
        :seq-config="{
          startIndex:
            (table.page.param.current - 1) * table.page.param.size,
        }" :height="table.height" :export-config="{}" :print-config="{}" :data="table.data"
        :tooltip-config="tooltipConfig" :sort-config="{
          trigger: 'cell',
          defaultSort: { field: 'orderNo', order: 'asc' },
          orders: ['desc', 'asc'],
        }" @sort-change="tableSort" :menu-config="{ body: { options: dictionary.right } }"
        @menu-click="contextMenuClickEvent">
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
        <vxe-column field="name" tree-node title="名称" width="220" showOverflow="ellipsis"></vxe-column>

        <vxe-column field="value" title="值" width="120"></vxe-column>
        <vxe-column field="isFreeze" title="禁用" align="center" width="80">
          <template v-slot="{ row }">
            <a-switch :checked="row.isFreeze" @change="isFreezeChange(row)" />
          </template>
        </vxe-column>
        <vxe-column field="orderNo" title="排序" align="center" width="80" sortable></vxe-column>
        <vxe-column field="remark" title="备注" min-width="150" showOverflow="ellipsis"></vxe-column>
        <vxe-column field="createUserName" title="创建人" width="100"></vxe-column>
        <vxe-column field="createTime" title="创建时间" width="160"></vxe-column>
        <vxe-column field="updateUserName" title="修改人" width="100"></vxe-column>
        <vxe-column field="updateTime" title="修改时间" width="160"></vxe-column>
        <vxe-column field="dictionaryId" title="记录ID" width="320">
          <template v-slot="{ row }">
            <a-tooltip>
              <template slot="title">
                点击复制
              </template>
              <a-icon class="dictionaryIdCopy" :data-clipboard-text="row.dictionaryId" @click="dictionaryIdCopy()"
                type="copy"></a-icon>
            </a-tooltip>
            <label>{{ row.dictionaryId }}</label>
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

    <edit ref="edit" v-if="edit.visible" :visible.sync="edit.visible" :data="dictionary.data" :title="edit.title"
      :dictionaryId="edit.dictionaryId" :parentId="edit.parentId" :parentName="edit.parentName" :copy="edit.copy"
      @save="operateStatus"></edit>
  </div>
</template>

<script>
import Clipboard from "clipboard";
import {
  dictionaryQuery,
  query,
  del,
  isFreeze,
} from "@/services/system/dictionary/list";
import edit from "./edit";

import { selectTableRowRadio, deleteConfirm } from "@/utils/util";
export default {
  components: { edit },
  data() {
    return {
      tooltipConfig: {
        showAll: true,
        enterable: true,
        contentMethod: ({ type, column, row, items, _columnIndex }) => {
          const { field, title } = column
          if (title == '操作' || field === 'dictionaryId') {
            return ''
          }
          return null
        }
      },
      dictionary: {
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
            id: "",
            filters: "",
            haveSelf: false,
          },
          total: 0,
          sizeOptions: this.eipPage.sizeOptions,
        },
        loading: true,
        data: [],
        height: this.eipHeaderHeight() - 122 + "px",
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
                field: "ParentIdsName",
                op: "cn",
                placeholder: "请输入层级",
                title: "层级",
                value: "",
                type: "text",
              },
              {
                field: "Value",
                op: "cn",
                placeholder: "请输入值",
                title: "值",
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
        dictionaryId: "",
        title: "",
        parentId: "",
        parentName: null,
        copy: false,
      },

      visible: {
        add: false,
        update: false,
        del: false,
        copy: false,
      },
    };
  },
  created() {
    this.dictionaryInit();
    this.tableInit();
  },
  mounted() {
    this.$refs.table.connect(this.$refs.toolbar);
  },
  methods: {
    /**
      * 
      */
    dictionaryIdCopy() {
      // 复制数据
      let clipboard = new Clipboard('.dictionaryIdCopy');
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
    dictionaryInit() {
      let that = this;
      dictionaryQuery().then((result) => {
        that.dictionary.data = result.data;
      });
    },

    /**
     *冻结
     */
    isFreezeChange(row) {
      if (this.visible.update) {
        let that = this;
        isFreeze({ id: row.dictionaryId }).then((result) => {
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

      let copyButton = buttons.filter((f) => f.method == "copy");
      this.visible.copy = copyButton.length > 0;

      var dics = [];
      if (this.visible.add) {
        dics.push({
          code: "1",
          prefixIcon: 'vxe-icon-add',
          name: addButton[0].name,
        });
      }
      if (this.visible.update) {
        dics.push({
          code: "2",
          prefixIcon: 'vxe-icon-' + updateButton[0].icon,
          name: updateButton[0].name,
        });
      }
      if (this.visible.del) {
        dics.push({
          code: "3",
          prefixIcon: 'vxe-icon-' + delButton[0].icon,
          name: delButton[0].name,
        });
      }
      this.dictionary.right.push(dics)
    },
    /**
         * 右键点击
         * @param {*} param0 
         */
    contextMenuClickEvent({ menu, row, column }) {
      switch (menu.code) {
        case "1":
          this.dictionaryAdd(row);
          break;
        case "2":
          this.dictionaryUpdate(row);
          break;
        case "3":
          this.dictionaryDel(row);
          break;
        default:
          break;
      }
    },


    /**
     * 树新增
     */
    dictionaryAdd(item) {
      this.edit.title = "新增字典";
      this.edit.copy = false;
      this.edit.dictionaryId = undefined;
      this.edit.parentId = item.dictionaryId;
      this.edit.parentName = item.name;
      this.edit.visible = true;
    },

    /**
     * 树更新
     */
    dictionaryUpdate(item) {
      this.edit.copy = false;
      this.edit.dictionaryId = item.dictionaryId;
      this.edit.title = "编辑字典-" + item.name;
      this.edit.visible = true;
    },
    /**
         * 树删除
         */
    dictionaryDel(item) {
      let that = this;
      deleteConfirm(
        "字典【" + item.name + "】" + that.eipMsg.delete,
        function () {
          that.$loading.show({ text: that.eipMsg.delloading });
          del({ id: item.dictionaryId }).then((result) => {
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
     * 树更新
     */
    tableUpdateRow(row) {
      this.edit.title = "字典编辑-" + row.name;
      this.edit.visible = true;
      this.edit.dictionaryId = row.dictionaryId;
    },
    /**
     * 删除
     */
    tableDelRow(row) {
      let that = this;
      deleteConfirm(
        "字典【" + row.name + "】" + that.eipMsg.delete,
        function () {
          that.$loading.show({ text: that.eipMsg.delloading });
          del({ id: row.dictionaryId }).then((result) => {
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
      this.edit.copy = false;
      this.edit.title = "新增字典";
      this.edit.parentId = undefined;
      this.edit.parentName = "";
      this.edit.dictionaryId = null;
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
          that.edit.copy = false;
          that.edit.parentId = undefined;
          that.edit.parentName = "";
          that.edit.dictionaryId = row.dictionaryId;
          that.edit.title = "编辑字典-" + row.name;
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
      selectTableRowRadio(
        that.$refs.table,
        function (row) {
          that.edit.copy = true;
          that.edit.parentId = undefined;
          that.edit.parentName = "";
          that.edit.dictionaryId = row.dictionaryId;
          that.edit.title = "复制字典-" + row.name;
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
              del({ id: row.dictionaryId }).then((result) => {
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
      this.dictionaryInit();
      this.tableInit();
    },
  },
};
</script>