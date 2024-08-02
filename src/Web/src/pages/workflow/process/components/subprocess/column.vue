<template>
  <a-modal v-drag centered width="370px" :visible="visible" :bodyStyle="{ padding: '1px' }" :title="title"
    @cancel="cancel">
    <template slot="footer">
      <a-button key="back" @click="cancel" :disabled="loading"><a-icon type="close" />取消</a-button>
      <a-button key="submit" @click="save" type="primary" :loading="loading"><a-icon type="save" />提交</a-button>
    </template>
    <a-spin :spinning="tree.spinning">
      <a-row>
        <a-col>
          <a-card class="eip-admin-card-small" size="small">
            <a-directory-tree default-expand-all :expandAction="false" :style="tree.style" :tree-data="tree.data"
              @select="onSelect"></a-directory-tree>
            <eip-empty :style="tree.style" v-if="tree.data.length == 0" description="无模块信息" />
          </a-card>
        </a-col>
      </a-row>
    </a-spin>
  </a-modal>
</template>

<script>

export default {
  name: "eip-workflow-fill-data",
  data() {
    return {
      bodyStyle: {
        padding: "4px",
        "background-color": "#f0f2f5",
      },
      tree: {
        style: {
          overflow: "auto",
          height: "400px",
        },
        data: [
          {
            title: "当前步骤处理人",
            children: [
              {
                title: "当前步骤处理人-账号",
                key: "当前步骤处理人-账号",
                isLeaf: true,
              },
              {
                title: "当前步骤处理人-名称",
                key: "当前步骤处理人-名称",
                isLeaf: true,
              },
              {
                title: "当前步骤处理人-Id",
                key: "当前步骤处理人-Id",
                isLeaf: true,
              },
              {
                title: "当前步骤处理人-组织机构名称",
                key: "当前步骤处理人-组织机构名称",
                isLeaf: true,
              },
              {
                title: "当前步骤处理人-组织机构Id",
                key: "当前步骤处理人-组织机构Id",
                isLeaf: true,
              },
            ],
          },
          {
            title: "流程发起人",
            children: [
              {
                title: "流程发起人-账号",
                key: "流程发起人-账号",
                isLeaf: true,
              },
              {
                title: "流程发起人-名称",
                key: "流程发起人-名称",
                isLeaf: true,
              },
              { title: "流程发起人-Id", key: "流程发起人-Id", isLeaf: true },
              {
                title: "流程发起人-组织机构名称",
                key: "流程发起人-组织机构名称",
                isLeaf: true,
              },
              {
                title: "流程发起人-组织机构Id",
                key: "流程发起人-组织机构Id",
                isLeaf: true,
              },
            ],
          },
          {
            title: "表单字段",
            children: [],
          },
        ],
        spinning: false,
      },
      keys: [],
      loading: false,
    };
  },

  props: {
    visible: {
      type: Boolean,
      default: false,
    },
    title: {
      type: String,
    },
    data: {
      type: Array,
    },
  },
  mounted() {
    this.init();
  },
  methods: {
    /**
     * 取消
     */
    cancel() {
      this.$emit("update:visible", false);
    },

    /**
     * 初始化菜单
     */
    init() {
      let that = this;
      that.tree.spinning = true;
      if (this.data) {
        this.data.forEach((item) => {
          that.tree.data[2].children.push({
            key: item.name,
            isLeaf: true,
            title:
              item.name +
              (item.description != "" ? "(" + item.description + ")" : ""),
          });
        });
      }
      that.tree.spinning = false;
    },

    /**
     * 选择
     */
    onSelect(keys, event) {
      this.keys = keys;
    },

    /**
     * 保存
     */
    save() {
      if (this.keys.length == 0) {
        this.$message.error("请选择字段");
        return false;
      }
      if (this.keys[0] == "0-0") {
        this.$message.error("请选择字段");
        return false;
      }
      let treeList = this.$utils.toTreeArray(this.tree.data);
      var chosenColumns = treeList.filter((f) => f.key == this.keys[0]);

      this.$emit("ok", chosenColumns);
      this.cancel();
    },
  },
};
</script>