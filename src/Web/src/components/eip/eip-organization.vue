<template>
  <a-modal
    width="370px"
    v-drag
    centered
    :destroyOnClose="true"
    :maskClosable="false"
    :visible="visible"
    :zIndex="1001"
    :bodyStyle="{ padding: '1px' }"
    @cancel="cancel"
  >
    <div slot="title">
      <a-badge :offset="[8, 0]" :count="getCheck().length">
        {{ title }}
      </a-badge>
    </div>
    <a-input-group compact>
      <a-select
        style="width: 112px"
        allow-clear
        placeholder="范围"
        v-model="nature"
        @change="onSearch"
      >
        <a-select-option
          v-for="(item, index) in eipOrganizationNatures"
          :key="index"
          :value="item.value"
        >
          {{ item.name }}
        </a-select-option>
      </a-select>
      <a-input-search
        style="width: 256px"
        v-model="key"
        :allowClear="true"
        placeholder="请输入组织名称/拼音模糊搜索"
        @change="onSearch"
      />
    </a-input-group>
    <a-spin :spinning="tree.spinning">
      <a-directory-tree
        :checkable="multiple"
        v-if="!tree.spinning && tree.data.length > 0"
        :defaultExpandedKeys="[tree.data.length > 0 ? tree.data[0].key : '']"
        :style="tree.style"
        :tree-data="tree.data"
        :defaultCheckedKeys="tree.checkedKeys"
        :expandAction="false"
        :checkedKeys="tree.checkedKeys"
        :checkStrictly="checkStrictly"
        @check="onCheck"
        @select="onSelect"
      ></a-directory-tree>

      <eip-empty
        :style="tree.style"
        v-if="tree.data.length == 0"
        description="无模块信息"
      />
    </a-spin>
    <template slot="footer">
      <a-space>
        <a-button key="back" @click="cancel" :disabled="loading"
          ><a-icon type="close" />取消</a-button
        >
        <a-button key="submit" @click="save" type="primary" :loading="loading"
          ><a-icon type="save" />提交</a-button
        >
      </a-space>
    </template>
  </a-modal>
</template>

<script>
import { chosenOrganization } from "@/services/common/usercontrol/chosenorganization";
export default {
  name: "eip-organization",
  data() {
    return {
      key: "",
      nature: undefined,
      tree: {
        style: {
          overflow: "auto",
          height: "500px",
        },
        data: [],
        original: [],
        spinning: false,
        checkedKeys: [],
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
    chosen: {
      type: Array,
    },
    //已选中对象,编辑传入
    range: {
      type: Number,
      default: 0,
    },
    multiple: {
      type: Boolean,
      default: true,
    },
    checkStrictly: {
      type: Boolean,
      default: true,
    },
  },
  mounted() {
    this.organizationInit();
  },
  methods: {
    onSearch(e) {
      var that = this;
      this.tree.data = this.$utils.clone(this.tree.original, true);
      this.tree.data = this.$utils.searchTree(
        this.tree.data,
        (item) => {
          if (that.key || that.nature) {
            var titlePinyin = pinyin.convert(item.title).toLowerCase();
            if (
              item.title.toLowerCase().indexOf(that.key.toLowerCase()) > -1 &&
              that.nature == item.slots.icon
            ) {
              return true;
            } else if (
              titlePinyin.indexOf(that.key.toLowerCase()) > -1 &&
              that.nature == item.slots.icon
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
     * 取消
     */
    cancel() {
      this.$emit("update:visible", false);
    },

    /**
     * 初始化菜单
     */
    organizationInit() {
      let that = this;
      that.tree.spinning = true;
      chosenOrganization(this.range).then((result) => {
        that.tree.original = result.data;
        that.tree.data = that.$utils.clone(result.data, true);
        if (that.chosen) {
          that.tree.checkedKeys = that.chosen.map((m) => m.id);
        }
        that.tree.spinning = false;
      });
    },

    /**
     * 选择
     */
    onCheck(checkedKeys, event) {
      let that = this;
      that.tree.checkedKeys = [];
      if (!that.checkStrictly) {
        that.tree.checkedKeys = checkedKeys;
      } else {
        that.tree.checkedKeys = [];
        checkedKeys.checked.forEach((item) => {
          that.tree.checkedKeys.push(item);
        });
      }
    },
    /**
     *
     * @param {*} checkedKeys
     * @param {*} event
     */
    onSelect(checkedKeys, event) {
      let that = this;
      that.tree.checkedKeys = [];
      if (!this.multiple) {
        checkedKeys.forEach((item) => {
          that.tree.checkedKeys.push(item);
        });
      }
    },
    /**
     * 保存
     */
    save() {
      this.$emit("ok", this.getCheck());
      this.cancel();
    },

    /**
     * 获取已选中
     */
    getCheck() {
      let chosenOrganizations = [];
      let treeList = this.$utils.toTreeArray(this.tree.data);
      this.tree.checkedKeys.forEach((item) => {
        var organization = treeList.filter((f) => f.key == item);
        if (organization.length > 0) {
          chosenOrganizations.push(organization[0]);
        }
      });
      return chosenOrganizations;
    },
  },
};
</script>
