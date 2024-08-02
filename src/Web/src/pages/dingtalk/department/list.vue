<template>
  <div style="width: 100%">
    <div style="width: 250px; float: left">
      <a-card @contextDictionary.prevent size="small" class="eip-admin-card-small">
        <div slot="title">
          <a-space>
            <a-input-search v-model="department.key" :allowClear="true" placeholder="名称/拼音模糊搜索"
              @change="departmentSearch" />
            <a-tooltip title="刷新">
              <a-button type="primary" @click="init">
                <a-icon type="redo" />
              </a-button>
            </a-tooltip>
          </a-space>
        </div>
        <a-spin :spinning="department.spinning" :delay="0" :style="department.style">
          <div>
            <a-directory-tree v-if="!department.spinning" :expandAction="false"
              :defaultExpandedKeys="[department.data.length > 0 ? department.data[0].key : '']"
              :tree-data="department.data" :icon="icon"></a-directory-tree>
          </div>
        </a-spin>
      </a-card>
    </div>
    <div class="eip-admin-card-tree-left">
      <a-card class="eip-admin-card-small eip-admin-card-small-bottom-border" :bordered="false">
        <eip-search :option="table.search.option" @search="tableSearch"></eip-search>
      </a-card>
      <a-card class="eip-admin-card-small" :bordered="false">
        <vxe-toolbar ref="toolbar" custom print export :refresh="{ query: initTable }">
          <template v-slot:buttons>
            <eip-toolbar @asyncDeparment="asyncDeparment"
              @asyncDeparmentToSystem="asyncDeparmentToSystem"></eip-toolbar>
          </template>
        </vxe-toolbar>
        <vxe-table :loading="table.loading" ref="table" id="systemorganizationlist" :height="table.height"
          :export-config="{}" :print-config="{}" :data="table.data" :tooltip-config="tooltipConfig">
          <template #loading>
            <a-spin></a-spin>
          </template>
          <template #empty>
            <eip-empty />
          </template>
          <vxe-column type="seq" title="序号" width="60"></vxe-column>
          <vxe-column field="platformOrganizationId" title="钉钉编码" showOverflow="ellipsis" width="100"></vxe-column>
          <vxe-column field="name" title="全称" width="250"></vxe-column>
          <vxe-column field="platformOrganizationParentId" title="上级编码" showOverflow="ellipsis"
            width="100"></vxe-column>
          <vxe-column field="createUserName" title="创建人" width="100"></vxe-column>
          <vxe-column field="createTime" title="创建时间" width="160"></vxe-column>
          <vxe-column field="updateUserName" title="修改人" width="100"></vxe-column>
          <vxe-column field="updateTime" title="修改时间" width="160"></vxe-column>
        </vxe-table>
        <a-pagination class="padding-top-sm float-right" v-model="table.page.param.current" show-size-changer
          show-quick-jumper :page-size-options="table.page.sizeOptions" :show-total="(total) => `共 ${total} 条`"
          :page-size="table.page.param.size" :total="table.page.total" @change="initTable"
          @showSizeChange="tableSizeChange"></a-pagination>
      </a-card>
    </div>
  </div>
</template>

<script>
import {
  query,
  tree,
  async,
  asyncToSystem,
} from "@/services/dingtalk/department/list";


import { operationConfirm } from "@/utils/util";

export default {

  data() {
    return {
      tooltipConfig: {
        showAll: true,
        enterable: true,
        contentMethod: ({ type, column, row, items, _columnIndex }) => {
          const { property } = column;
          if (row && property === "parentIdsName") {
            return row[property];
          }
          return null;
        }
      },
      department: {
        key: "",
        style: {
          overflow: "auto",
          height: this.eipHeaderHeight() - 72 + "px",
        },
        data: [],
        original: [],
        spinning: true,
      },
      table: {
        page: {
          param: {
            current: 1,
            size: this.eipPage.size,
            sord: "asc",
            sidx: "Id",
            filters: "",
          },
          total: 0,
          sizeOptions: this.eipPage.sizeOptions,
        },
        loading: true,
        data: [],
        height: this.eipHeaderHeight() - 154 + "px",
        search: {
          option: {
            num: 6,
            config: [
              {
                field: "Name",
                op: "cn",
                placeholder: "请输入全称",
                title: "全称",
                value: "",
                type: "text",
              }
            ],
          },
        },
      },
    };
  },

  created() {
    this.init();
  },
  mounted() {
    this.$refs.table.connect(this.$refs.toolbar);
  },
  methods: {
    /**
     * 
     */
    init() {
      this.initTable();
      this.initTree()
    },

    /**
     * 查询
     * @param {*} e 
     */
    departmentSearch(e) {
      var that = this;
      this.department.data = this.$utils.clone(this.department.original, true);
      this.department.data = this.$utils.searchTree(
        this.department.data,
        (item) => {
          if (that.department.key) {
            var titlePinyin = pinyin.convert(item.title).toLowerCase();
            if (
              item.title
                .toLowerCase()
                .indexOf(that.department.key.toLowerCase()) > -1
            ) {
              return true;
            } else if (
              titlePinyin.indexOf(that.department.key.toLowerCase()) > -1
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
     * 
     */
    initTree() {
      let that = this;
      that.department.spinning = true;
      tree().then((result) => {
        that.department.original = result.data;
        that.department.data = result.data;
        that.department.spinning = false;
        that.departmentSearch();
      });
    },
    /**
     * 菜单树
     */
    initTable() {
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
    *数量改变
    */
    tableSizeChange(current, pageSize) {
      this.table.page.param.size = pageSize;
      this.initTable();
    },

    /**
   * 列表查询
   */
    tableSearch(filters) {
      this.table.page.param.current = 1;
      this.table.page.param.filters = filters;
      this.initTable();
    },

    /**
     * 同步
     */
    asyncDeparment() {
      let that = this;
      that.$loading.show({ text: "获取中" });
      async()
        .then((result) => {
          that.$loading.hide();
          that.$message.destroy();
          if (result.code === that.eipResultCode.success) {
            that.$message.success(result.msg);
            that.init();
          } else {
            that.$message.error(result.msg);
          }
        })
        .catch((err) => {
          that.$loading.hide();
          that.$message.destroy();
        });
    },

    /**
     * 同步
     */
    asyncDeparmentToSystem() {
      let that = this;
      operationConfirm(
        "确定同步到系统组织,同步后组织关系发生变化",
        function () {
          that.$loading.show({ text: "同步中" });
          asyncToSystem()
            .then((result) => {
              that.$loading.hide();
              that.$message.destroy();
              if (result.code === that.eipResultCode.success) {
                that.$message.success(result.msg);
              } else {
                that.$message.error(result.msg);
              }
            })
            .catch((err) => {
              that.$loading.hide();
              that.$message.destroy();
            });
        },
        that
      );
    },

    /**
     * 树图标
     */
    icon(props) {
      const { expanded } = props;
      if (props.children.length == 0) {
        return <a-icon type="file" />;
      }
      return <a-icon type={expanded ? "folder-open" : "folder"} />;
    },
  },
};
</script>