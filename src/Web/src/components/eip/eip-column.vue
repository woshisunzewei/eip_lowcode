<template>
  <a-drawer width="370px" :visible="visible" :bodyStyle="{ padding: '1px' }" @close="cancel" :title="title"
    :destroyOnClose="true">
    <div class="eip-drawer-body beauty-scroll">
      <a-spin :spinning="tree.spinning">
        <a-row>
          <a-col>
            <a-card class="eip-admin-card-small" size="small">
              <div :style="tree.style">
                <a-directory-tree default-expand-all :expandAction="false" :tree-data="tree.data"
                  @select="onSelect"></a-directory-tree>
                <eip-empty :style="tree.style" v-if="tree.data.length == 0" description="无模块信息" />
              </div>
            </a-card>
          </a-col>
        </a-row>
      </a-spin>
    </div>
    <div class="eip-drawer-toolbar">
      <a-space>
        <a-button key="back" @click="cancel" :disabled="loading"><a-icon type="close" />取消</a-button>
        <a-button key="submit" @click="save" type="primary"><a-icon type="save" />提交</a-button>
      </a-space>
    </div>
  </a-drawer>
</template>

<script>

export default {
  name: "eip-column",
  data() {
    return {
      tree: {
        style: {
          overflow: "auto",
          height: this.eipHeaderHeight() - 32 + "px",
        },
        data: [
          {
            title: "流程字段",
            children: [
              { title: "流水号", key: "流水号", isLeaf: true },
              { title: "流程名称", key: "流程名称", isLeaf: true },
              { title: "活动名称", key: "活动名称", isLeaf: true },
              { title: "任务Id", key: "任务Id", isLeaf: true },
              { title: "流程Id", key: "流程Id", isLeaf: true },
              { title: "活动Id", key: "活动Id", isLeaf: true },
              { title: "自定义数据", key: "自定义数据", isLeaf: true },
              { title: "当前处理人Id", key: "当前处理人Id", isLeaf: true },
              { title: "当前处理人姓名", key: "当前处理人姓名", isLeaf: true },
              { title: "发起人Id", key: "发起人Id", isLeaf: true },
              { title: "发起人姓名", key: "发起人姓名", isLeaf: true },
              { title: "下一步待处理人Id", key: "下一步待处理人Id", isLeaf: true },
              { title: "下一步待处理人姓名", key: "下一步待处理人姓名", isLeaf: true },
              { title: "下一步待处理任务Id", key: "下一步待处理任务Id", isLeaf: true },
            ],
          },
          {
            title: "表单字段",
            children: [
              { title: "记录ID", key: "RelationId", isLeaf: true },
              { title: "创建用户Id", key: "CreateUserId", isLeaf: true },
              { title: "创建用户名", key: "CreateUserName", isLeaf: true },
            ],
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
    chosen: {
      type: Array,
    }, //已选中对象,编辑传入
    data: {
      type: Array,
    }, //字段
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
          that.tree.data[1].children.push({
            key: item.name,
            isLeaf: true,
            title: (item.description != "" ? item.description : item.name),
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