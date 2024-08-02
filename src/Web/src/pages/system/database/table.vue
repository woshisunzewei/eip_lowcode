<template>
  <splitpanes class="default-theme" style="height: auto">
    <pane :min-size="split.minWidth" :size="split.minWidth">
      <a-card size="small" class="eip-admin-card-small">
        <div slot="title">表结构</div>
        <a-spin :spinning="tree.loading" :delay="0">
          <a-directory-tree :style="tree.style" :tree-data="tree.data" :icon="treeIcon" :expandAction="false"
            @select="treeSelect"></a-directory-tree>
        </a-spin>
      </a-card>
    </pane>
    <pane min-size="50">
      <a-card class="eip-admin-card-small" size="small">
        <div slot="title">{{ table.description }}</div>
        <vxe-table :loading="table.loading" ref="table" id="table" :column-config="{ isCurrent: true, isHover: true }"
          :row-config="{ isCurrent: true, isHover: true }" :height="table.height" :export-config="{}" :print-config="{}"
          :data="table.data">
          <template #loading>
            <a-spin></a-spin>
          </template>
          <template #empty>
            <eip-empty />
          </template>
          <vxe-column type="seq" title="序号" width="60"></vxe-column>
          <vxe-column field="name" title="列名"></vxe-column>
          <vxe-column field="description" title="描述"></vxe-column>

          <vxe-column field="defaultSetting" title="默认值" showOverflow="ellipsis"></vxe-column>
          <vxe-column field="isNullable" title="允许空" showOverflow="ellipsis"></vxe-column>
          <vxe-column field="dataType" title="数据类型" showOverflow="ellipsis"></vxe-column>
          <vxe-column field="maxLength" title="最大长度" showOverflow="ellipsis"></vxe-column>
          <vxe-column field="isIdentity" title="是否自增" showOverflow="ellipsis"></vxe-column>
        </vxe-table>
      </a-card>
    </pane>
  </splitpanes>
</template>

<script>
import { tree, column } from "@/services/system/database/table";
import { Splitpanes, Pane } from 'splitpanes'
import 'splitpanes/dist/splitpanes.css'
export default {
  components: {
    Splitpanes,
    Pane
  },
  data() {
    return {
      split: {
        minWidth: (250 / window.innerWidth) * 100,
      },
      tree: {
        style: {
          overflow: "auto",
          height: window.innerHeight - 152 + "px",
        },
        data: [],
        loading: true,
        right: {
          item: null,
          style: "",
        },
      },
      table: {
        toolbar: {
          export: true,
          print: true,
          zoom: true,
          custom: true,
          slots: {
            buttons: "buttons",
          },
        },
        param: {
          name: null,
        },
        loading: true,
        data: [],
        height: window.innerHeight - 152,
        description: "点击表查询字段信息",
      },
    };
  },
  created() {
    this.init();
  },

  methods: {
    /**
     * 初始化
     */
    init() {
      this.table.param.name = null;
      this.treeInit();
      this.tableInit();
    },
    /**
     * 菜单树
     */
    treeInit() {
      let that = this;
      tree().then((result) => {
        that.tree.loading = false;
        that.tree.data = result.data;
      });
    },

    /**
     * 树图标
     */
    treeIcon(props) {
      return <a-icon type={props.dataRef.slots.icon} />;
    },

    /**
     * 树选中
     */
    treeSelect(selectedKeys, { selected, selectedNodes, node, event }) {
      this.table.param.name = selectedKeys[0];
      this.table.description = node.title;
      this.tableInit();
    },

    /**
     * 列表数据
     */
    tableInit() {
      let that = this;
      that.table.loading = true;
      if (that.table.param.name != null) {
        column(that.table.param).then((result) => {
          if (result.code == that.eipResultCode.success) {
            that.table.data = result.data;
          }
          that.table.loading = false;
        });
      } else {
        that.table.data = [];
        that.table.loading = false;
      }
    },
  },
};
</script>