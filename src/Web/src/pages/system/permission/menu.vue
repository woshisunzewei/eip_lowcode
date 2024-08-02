<template>
  <a-modal width="370px" v-drag :destroyOnClose="true" :maskClosable="false" :visible="visible"
    :bodyStyle="{ padding: '1px' }" :title="title" @cancel="cancel">

    <a-spin :spinning="tree.spinning" :style="tree.style">
      <a-row>
        <a-col>
          <a-directory-tree v-if="!tree.spinning" checkable
            :defaultExpandedKeys="[tree.data.length > 0 ? tree.data[0].key : '']" :style="tree.style"
            :expandAction="false" :tree-data="tree.data" :icon="treeIcon" :defaultCheckedKeys="tree.checkedKeys"
            :checkedKeys="tree.checkedKeys" @check="onCheck">
          </a-directory-tree>

          <eip-empty :style="tree.style" v-if="tree.data.length == 0" description="无模块信息" />
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
import { menu, save, privilegeMaster } from "@/services/system/permission/menu";

export default {
  name: "menu-permission",
  data() {
    return {
      tree: {
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
    privilegeMaster: {
      type: Number,
    },
    privilegeMasterValue: {
      type: String,
    },
  },
  mounted() {
    this.menuInit();
  },
  methods: {
    /**
     * 搜索
     * @param {*} e
     */
    onChange(e) { },
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
      that.tree.spinning = true;
      menu().then((result) => {
        that.tree.data = result.data;
        let treeList = that.$utils.toTreeArray(result.data);
        privilegeMaster(
          that.privilegeMasterValue,
          that.privilegeMaster,
          that.eipPrivilegeAccess.menu
        ).then((result) => {
          result.data.forEach((item) => {
            //判断是否具有子项
            let children = treeList.filter(
              (f) => f.key == item.privilegeAccessValue
            );
            if (children.length > 0) {
              if (children[0].children.length == 0) {
                that.tree.checkedKeys.push(item.privilegeAccessValue);
              } else {
                that.tree.halfCheckedKeys.push(item.privilegeAccessValue);
              }
            }
          });
          that.tree.spinning = false;
        });
      });
    },

    /**
     * 选择
     */
    onCheck(checkedKeys, event) {
      this.tree.checkedKeys = [];
      checkedKeys.forEach((item) => {
        this.tree.checkedKeys.push(item);
      });

      event.halfCheckedKeys.forEach((item) => {
        this.tree.halfCheckedKeys.push(item);
      });
    },

    /**
     * 保存
     */
    save() {
      let that = this;
      let menuPermissions = [];
      this.tree.checkedKeys.forEach((item) => {
        menuPermissions.push({ p: item });
      });
      this.tree.halfCheckedKeys.forEach((item) => {
        menuPermissions.push({ p: item });
      });
      that.$loading.show({ text: that.eipMsg.saveLoading });
      save({
        privilegeAccess: this.eipPrivilegeAccess.menu,
        menuPermissions: JSON.stringify(menuPermissions),
        privilegeMaster: this.privilegeMaster,
        privilegeMasterValue: this.privilegeMasterValue,
      }).then((result) => {
        if (result.code === that.eipResultCode.success) {
          that.$message.success(result.msg);
          that.cancel();
        } else {
          that.$message.error(result.msg);
        }
        that.$loading.hide();
      });
    },
  },
};
</script>