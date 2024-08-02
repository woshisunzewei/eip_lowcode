<template>
  <a-modal
    width="1070px"
    v-drag
    :destroyOnClose="true"
    :maskClosable="false"
    :visible="visible"
    :bodyStyle="{ padding: '1px', 'background-color': '#f0f2f5' }"
    :title="title"
    @cancel="cancel"
  >
    <a-spin :spinning="spinning">
      <a-row>
        <a-col :span="8" style="width: 240px">
          <a-card class="eip-admin-card-small" size="small">
            <div slot="title">模块</div>
            <div slot="extra">
              <div class="text-red">选中模块勾选权限</div>
            </div>
            <a-directory-tree
              v-if="!spinning"
              checkable
              :defaultExpandedKeys="[
                menu.data.length > 0 ? menu.data[0].key : '',
              ]"
              :style="menu.style"
              :tree-data="menu.data"
              :icon="treeIcon"
              :defaultCheckedKeys="menu.checkedKeys"
              :expandAction="false"
              :checkedKeys="menu.checkedKeys"
              @check="onCheck"
              @select="treeSelect"
            >
            </a-directory-tree>
          </a-card>
        </a-col>

        <a-col :span="16" style="margin-left: 3px; width: 820px">
          <a-card class="eip-admin-card-small" size="small">
            <div slot="title">{{ menu.menuName }}模块按钮</div>
            <template #extra>
              <a-checkbox
                v-model="menu.checkButtonAll"
                @change="checkAll($event, menubuttons)"
                >全选/反选</a-checkbox
              >
            </template>
            <div class="scrollBar" :style="{ height: menu.style.center }">
              <ul
                id="accessButton"
                class="sys_spec_text"
                v-if="menubuttons.length > 0"
              >
                <li
                  v-for="(menubutton, i) in menubuttons"
                  :key="i"
                  :class="menubutton.remark"
                  @click="check(menubutton)"
                >
                  <a-tooltip>
                    <template slot="title">
                      {{ menubutton.name }}
                    </template>
                    <a
                      ><a-icon :type="menubutton.icon" />
                      {{ menubutton.name }}</a
                    >
                  </a-tooltip>
                  <i class="check"></i>
                </li>
              </ul>

              <eip-empty
                v-if="menubuttons.length == 0"
                :description="menubutton.description"
              />
            </div>
          </a-card>

          <a-card class="eip-admin-card-small" size="small">
            <div slot="title">{{ menu.menuName }}数据权限</div>
            <template #extra>
              <a-checkbox
                v-model="menu.checkDataAll"
                @change="checkAll($event, datas)"
                >全选/反选</a-checkbox
              >
            </template>
            <div class="scrollBar" style="height: 300px">
              <ul
                id="accessButton"
                class="sys_spec_text"
                v-if="datas.length > 0"
              >
                <li
                  v-for="(data, i) in datas"
                  :key="i"
                  :class="data.remark"
                  @click="check(data)"
                >
                  <a-tooltip>
                    <template slot="title">
                      {{ data.name + "-" + data.menuNames }}
                    </template>
                    <a><a-icon type="table" /> {{ data.name }}</a>
                  </a-tooltip>
                  <i class="check"></i>
                </li>
              </ul>

              <eip-empty
                v-if="datas.length == 0"
                :description="data.description"
              />
            </div>
          </a-card>
        </a-col>
      </a-row>
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
import {
  permissionSave,
  menuButtonAll,
  dataAll,
  menu,
  privilegeMaster,
} from "@/services/system/permission/menu-menubutton-data";

export default {
  name: "menubutton",
  data() {
    return {
      menu: {
        style: {
          overflow: "auto",
          height: "550px",
          center: "200px",
        },
        data: [],
        checkedKeys: [],
        halfCheckedKeys: [],
        menuName: "",
        menuId: "",
        param: {
          privilegeAccess: this.eipPrivilegeAccess.menu,
          menuPermissions: "",
          privilegeMaster: "",
          privilegeMasterValue: "",
        },
        select: null,
      },
      menubutton: {
        data: [],
        description: "无模块按钮",
        param: {
          privilegeAccess: this.eipPrivilegeAccess.menubutton,
          privilegeMaster: "",
          privilegeMasterValue: "",
          menuPermissions: "",
        },
      },
      data: {
        data: [],
        description: "无数据权限",
        param: {
          privilegeAccess: this.eipPrivilegeAccess.data,
          privilegeMaster: "",
          privilegeMasterValue: "",
          menuPermissions: "",
        },
      },
      spinning: false,
      loading: false,
    };
  },
  computed: {
    menubuttons() {
      var arr = [];
      if (this.menu.menuId) {
        this.menubutton.data.forEach((item) => arr.push(item));
        arr = this.menubutton.data.filter((item) =>
          item.menuId.includes(this.menu.menuId)
        );
      }
      return arr;
    },

    datas() {
      var arr = [];
      if (this.menu.menuId) {
        this.data.data.forEach((item) => arr.push(item));
        arr = this.data.data.filter((item) =>
          item.menuId.includes(this.menu.menuId)
        );
      }
      return arr;
    },
  },
  mounted() {
    this.menuInit();
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

  methods: {
    /**
     * 选择
     */
    onCheck(checkedKeys, event) {
      this.menu.checkedKeys = [];
      this.menu.halfCheckedKeys = [];
      checkedKeys.forEach((item) => {
        this.menu.checkedKeys.push(item);
      });

      event.halfCheckedKeys.forEach((item) => {
        this.menu.halfCheckedKeys.push(item);
      });

      //判断是否未取消
      if (!event.checked) {
        let menuId = event.node.value;
        this.menubutton.data.forEach((item) => {
          if (item.menuId == menuId) {
            item.remark = "";
          }
        });

        this.data.data.forEach((item) => {
          if (item.menuId == menuId) {
            item.remark = "";
          }
        });
      }
    },

    /**
     * 树图标
     */
    treeIcon(props) {
      const { slots } = props;
      return <a-icon type={slots.icon} />;
    },

    /**
     * 树选中
     */
    treeSelect(electedKeys, e) {
      this.menu.menuId = electedKeys[0];
      this.menu.menuName = e.selectedNodes[0].title;
      this.menu.select = e;
      this.checkAllChange();
    },

    /**
     * 检查勾选
     */
    checkAllChange() {
      let checkButtonAll = true;
      this.menubuttons.forEach((f) => {
        if (!f.remark) {
          checkButtonAll = false;
        }
      });
      this.menu.checkButtonAll =
        this.menubuttons.length > 0 ? checkButtonAll : false;

      let checkDataAll = true;
      this.datas.forEach((f) => {
        if (!f.remark) {
          checkDataAll = false;
        }
      });
      this.menu.checkDataAll = this.datas.length > 0 ? checkDataAll : false;
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
      that.menu.param.privilegeMaster = that.privilegeMaster;
      that.menu.param.privilegeMasterValue = that.privilegeMasterValue;
      that.spinning = true;
      menu().then((result) => {
        that.menu.data = result.data;
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
                that.menu.checkedKeys.push(item.privilegeAccessValue);
              } else {
                that.menu.halfCheckedKeys.push(item.privilegeAccessValue);
              }
            }
          });
          that.spinning = false;
        });
      });

      menuButtonAll(that.privilegeMasterValue, that.privilegeMaster).then(
        (result) => {
          that.menubutton.data = result.data;
          that.menubutton.param.privilegeMaster = that.privilegeMaster;
          that.menubutton.param.privilegeMasterValue =
            that.privilegeMasterValue;
        }
      );

      dataAll(that.privilegeMasterValue, that.privilegeMaster).then(
        (result) => {
          that.data.data = result.data;
          that.data.param.privilegeMaster = that.privilegeMaster;
          that.data.param.privilegeMasterValue = that.privilegeMasterValue;
        }
      );
    },

    /**
     * 用户选择
     */
    check(item) {
      if (this.menu.select.node.checked) {
        item.remark = item.remark == "selected" ? "" : "selected";
      } else {
        this.$message.error("请勾选模块:" + this.menu.menuName);
      }
      this.checkAllChange();
    },
    /**
     *
     * @param {*} menuButtons
     */
    checkAll(event, menuButtons) {
      if (this.menu.select.node.checked) {
        menuButtons.forEach((item) => {
          item.remark = event.target.checked ? "selected" : "";
        });
      } else {
        this.menu.checkButtonAll = false;
        this.menu.checkDataAll = false;
        this.$message.error("请勾选模块:" + this.menu.menuName);
      }
    },
    /**
     * 保存
     */
    save() {
      let that = this;
      let menuPermissions = [];
      this.menu.checkedKeys.forEach((item) => {
        menuPermissions.push({ p: item });
      });
      this.menu.halfCheckedKeys.forEach((item) => {
        menuPermissions.push({ p: item });
      });
      that.menu.param.menuPermissions = JSON.stringify(menuPermissions);
      that.loading = true;
      that.$loading.show({ text: that.eipMsg.saveLoading });
      permissionSave(that.menu.param).then((result) => {});

      //模块按钮
      let menuButtonSelecteds = this.menubutton.data.filter(
        (f) => f.remark == "selected"
      );
      let menuButtonPermissions = [];
      menuButtonSelecteds.forEach((item) => {
        menuButtonPermissions.push({ p: item.menuButtonId });
      });
      that.menubutton.param.menuPermissions = JSON.stringify(
        menuButtonPermissions
      );

      permissionSave(that.menubutton.param).then((result) => {});

      //数据权限
      let menuDataSelecteds = this.data.data.filter(
        (f) => f.remark == "selected"
      );
      let menuDataPermissions = [];
      menuDataSelecteds.forEach((item) => {
        menuDataPermissions.push({ p: item.dataId });
      });
      that.data.param.menuPermissions = JSON.stringify(menuDataPermissions);

      permissionSave(that.data.param).then((result) => {
        if (result.code === that.eipResultCode.success) {
          that.$message.success(result.msg);
          that.cancel();
        } else {
          that.$message.error(result.msg);
        }
        that.loading = false;
        that.$loading.hide();
      });
    },
  },
};
</script>

<style lang="less" scoped>
/*自定义复选框*/
.scrollBar {
  overflow: auto;
  height: 180px;
}

.sys_spec_text {
  padding-left: 15px;
}

.sys_spec_text li {
  display: inline;
  float: left;
  height: 43px;
  margin: 10px 15px 0px 0;
  outline: none;
  outline: none;
  position: relative;
  position: relative;
}

.sys_spec_text li a {
  background: #fff;
  border: 1px solid #ccc;
  cursor: pointer;
  display: inline-block;
  line-height: 39px;
  outline: none;
  overflow: hidden;
  padding: 0px 0px;
  text-align: center;
  text-align: center;
  text-overflow: ellipsis;
  vertical-align: middle;
  white-space: nowrap;
  width: 98px;
  word-break: keep-all;
  color: black;
}

.sys_spec_text li a:hover {
  border: 1px solid #4a5b79;
  padding: 0 0px;
  text-decoration: none;
}

.sys_spec_text li .check {
  background: url(images/selected.gif) no-repeat right bottom;
  bottom: 2px;
  display: none;
  font-size: 0;
  height: 10px;
  line-height: 0;
  position: absolute;
  right: 1px;
  width: 10px;
  z-index: 99;
}

.sys_spec_text li.selected a {
  border: 1px solid #4a5b79;
  padding: 0 0px;
}

.sys_spec_text li.selected .check {
  display: block;
}

.sys_spec_text li img {
  border: 0px solid #fff;
  margin-top: -2px;
  padding-right: 5px;
  vertical-align: middle;
}

.sys_spec_text a.disabled {
  color: #a9a6a6;
  cursor: not-allowed;
  display: block;
  overflow: hidden;
}

.selected {
  background-color: #ddd;
}

li {
  list-style: none;
}
</style>
