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
      <a-card class="eip-admin-card-small" :bordered="false">
        <vxe-toolbar ref="toolbar" custom print export :refresh="{ query: init }">
          <template v-slot:buttons>
            <eip-toolbar @asyncDeparment="asyncDeparment"
              @asyncDeparmentToSystem="asyncDeparmentToSystem"></eip-toolbar>
          </template>
        </vxe-toolbar>
        <vxe-table :loading="table.loading" ref="table" id="systemorganizationlist" :height="table.height"
          :export-config="{}" :print-config="{}" :data="table.data">
          <template #loading>
            <a-spin></a-spin>
          </template>
          <template #empty>
            <eip-empty />
          </template>
          <vxe-column type="seq" title="序号" width="60"></vxe-column>
          <vxe-column field="platformOrganizationId" title="企业微信编码" showOverflow="ellipsis" width="150"></vxe-column>
          <vxe-column field="name" title="全称" width="250"></vxe-column>
          <vxe-column field="platformOrganizationParentId" title="上级编码" showOverflow="ellipsis"
            width="100"></vxe-column>
          <vxe-column field="createUserName" title="创建人" width="100"></vxe-column>
          <vxe-column field="createTime" title="创建时间" width="160"></vxe-column>
          <vxe-column field="updateUserName" title="修改人" width="100"></vxe-column>
          <vxe-column field="updateTime" title="修改时间" width="160"></vxe-column>
        </vxe-table>
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
} from "@/services/wechat/work/department/list";

import { operationConfirm } from "@/utils/util";
export default {
  data() {
    return {
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
        loading: true,
        data: [],
        height: window.innerHeight - 163,
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
    departmentSearch(e) {
      var that = this;

      this.department.data = that.$utils.clone(this.department.original, true);
      this.department.data = that.$utils.searchTree(
        this.department.data,
        (item) => {
          if (that.department.key) {
            var titlePinyin = pinyin.convert(item.title).toLowerCase();
            if (
              item.title
                .toLowerCase()
                .includes(that.department.key.toLowerCase())
            ) {
              return true;
            } else if (
              titlePinyin.includes(that.department.key.toLowerCase())
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
     * 菜单树
     */
    init() {
      let that = this;
      that.department.spinning = true;
      tree().then((result) => {
        that.department.original = result.data;
        that.department.data = result.data;
        that.department.spinning = false;
        that.departmentSearch();
      });

      query().then((result) => {
        that.table.loading = false;
        that.table.data = result.data;
      });
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