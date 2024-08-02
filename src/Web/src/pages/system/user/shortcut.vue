<template>
  <a-modal width="370px" v-drag :destroyOnClose="true" :maskClosable="false" :visible="visible"
    :bodyStyle="{ padding: '1px' }" :title="title" @cancel="cancel">
    <a-spin :spinning="menu.spinning" :style="menu.style">
      <a-row>
        <a-col>
          <a-card class="eip-admin-card-small beauty-scroll" size="small">
            <a-directory-tree checkable v-if="!menu.spinning"
              :defaultExpandedKeys="[menu.data.length > 0 ? menu.data[0].key : '']" :tree-data="menu.data"
              :expandAction="false" :icon="treeIcon" :defaultCheckedKeys="menu.checkedKeys"
              :checkedKeys="menu.checkedKeys" @check="onCheck"></a-directory-tree>
            <eip-empty :style="menu.style" v-if="menu.data.length == 0" description="无模块信息" />
          </a-card>
        </a-col>
      </a-row>
    </a-spin>
    <template slot="footer">
      <a-space>
        <a-button key="back" @click="cancel" :disabled="loading"><a-icon type="close" />取消</a-button>
        <a-button key="submit" @click="save" type="primary" :loading="loading"><a-icon type="save" />提交</a-button>
      </a-space>
    </template>
  </a-modal>
</template>
<script>
import { menu, query, save } from "@/services/system/user/shortcut";

export default {
  name: "shortcut",
  data() {
    return {
      menu: {
        style: {
          overflow: "auto",
          height: "500px",
        },
        data: [],
        spinning: false,
        checkedKeys: [],
        halfCheckedKeys: [],
      },

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
  },
  mounted() {
    this.menuInit();
  },
  methods: {
    /**
     * 树图标
     */
    treeIcon(props) {
      const { slots } = props;
      return <a-icon type={slots.icon} />;
    },

    /**
     * 取消
     */
    cancel() {
      this.$emit("update:visible", false);
    },

    /**
     * 初始化菜单
     */
    menuInit() {
      let that = this;
      that.menu.spinning = true;
      menu().then((result) => {
        if (result.code === that.eipResultCode.success) {
          that.menu.data = result.data;
          let treeList = that.$utils.toTreeArray(result.data);
          query({ type: 0 }).then((queryResult) => {
            queryResult.data.forEach((item) => {
              //判断是否具有子项
              let children = treeList.filter((f) => f.key == item.menuId);
              if (children.length > 0) {
                if (children[0].children.length == 0) {
                  that.menu.checkedKeys.push(item.menuId);
                } else {
                  that.menu.halfCheckedKeys.push(item.menuId);
                }
              }
            });
            that.menu.spinning = false;
          });
        }
      });
    },

    /**
     * 选择
     */
    onCheck(checkedKeys, event) {
      this.menu.checkedKeys = [];
      checkedKeys.forEach((item) => {
        this.menu.checkedKeys.push(item);
      });

      event.halfCheckedKeys.forEach((item) => {
        this.menu.halfCheckedKeys.push(item);
      });
    },

    /**
     * 保存
     */
    save() {
      let that = this;
      let shortCuts = [];
      this.menu.checkedKeys.forEach((item) => {
        shortCuts.push({ menuId: item });
      });
      this.menu.halfCheckedKeys.forEach((item) => {
        shortCuts.push({ menuId: item });
      });
      that.menu.spinning = true;
      that.loading = true;
      save({
        type: 0,
        shortCuts: shortCuts,
      }).then((result) => {
        if (result.code === that.eipResultCode.success) {
          that.$message.success(result.msg);
          that.$emit("ok");
          that.cancel();
        } else {
          that.$message.error(result.msg);
        }
        that.loading = false;
        that.menu.spinning = false;
      });
    },
  },
};
</script>