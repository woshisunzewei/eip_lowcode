<template>
  <a-modal width="770px" v-drag :destroyOnClose="true" :maskClosable="false" :visible="visible"
    :bodyStyle="{ padding: '1px' }" :title="title" @cancel="cancel">
    <a-spin :spinning="menubutton.spinning">
      <a-row>
        <a-col :span="7">
          <a-card class="eip-admin-card-small" size="small">
            <div slot="title" style="height: 32px; line-height: 32px">模块</div>
            <a-directory-tree v-if="tree.data.length > 0" :style="{ height: height, overflow: 'auto' }"
              :defaultExpandedKeys="[tree.data.length > 0 ? tree.data[0].key : '']" :tree-data="tree.data"
              :expandAction="false" :icon="treeIcon" @select="treeSelect">
            </a-directory-tree>
            <eip-empty :style="{ height: height, overflow: 'auto' }" v-if="tree.data.length == 0" description="无模块信息" />
          </a-card>
        </a-col>

        <a-col :span="17">
          <a-card class="eip-admin-card-small" size="small">
            <div slot="title">{{ menubutton.menuName }}按钮</div>
            <div slot="extra">
              <a-space>
                <a-input-search v-model="menubutton.key" placeholder="名称模糊查询..." allowClear />
                <a-button type="primary" @click="all"><a-icon type="border" />所有模块按钮</a-button></a-space>
            </div>

            <vxe-table ref="table" border row-id="menuButtonId" :data="menubuttons" @checkbox-change="selectChangeEvent"
              @checkbox-all="selectChangeEvent" :height="height" :checkbox-config="{
                checkRowKeys: defaultSelecteRows,
                trigger: 'row',
                highlight: true,
                range: true,
                reserve: true,
              }">
              <template #loading>
                <a-spin></a-spin>
              </template>
              <template #empty>
                <eip-empty />
              </template>
              <vxe-column type="seq" title="序号" width="60"></vxe-column>
              <vxe-column type="checkbox" width="60" align="center">
                <template #header="{ checked, indeterminate }">
                  <span @click.stop="$refs.table.toggleAllCheckboxRow()">
                    <a-checkbox :indeterminate="indeterminate" :checked="checked">
                    </a-checkbox>
                  </span>
                </template>
                <template #checkbox="{ row, checked, indeterminate }">
                  <span @click.stop="$refs.table.toggleCheckboxRow(row)">
                    <a-checkbox :indeterminate="indeterminate" :checked="checked">
                    </a-checkbox>
                  </span>
                </template>
              </vxe-column>
              <vxe-column field="icon" title="名称" width="120">
                <template v-slot="{ row }">
                  <a-icon :type="row.icon" />
                  {{ row.name }}
                </template>
              </vxe-column>
              <vxe-column field="menuNames" title="归属模块" showOverflow="ellipsis"></vxe-column>
            </vxe-table>
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
import {
  menuHave,
  save,
  menuButton,
} from "@/services/system/permission/menubutton";
export default {
  name: "menubutton-permission",
  data() {
    return {
      height: "500px",
      tree: {
        data: [],
        loading: true,
      },
      menubutton: {
        key: "",
        data: [],
        menuName: "",
        menuId: "",
        spinning: false,
      },
      loading: false,

      defaultSelecteRows: [],
      selectRows: [],
    };
  },
  computed: {
    menubuttons() {
      var arr = [];
      this.menubutton.data.forEach((item) => arr.push(item));
      if (this.menubutton.key) {
        arr = this.menubutton.data.filter((item) =>
          item.name.includes(this.menubutton.key)
        );
      }
      if (this.menubutton.menuId) {
        arr = this.menubutton.data.filter((item) =>
          item.menuId.includes(this.menubutton.menuId)
        );
      }
      if (this.menubutton.key && this.menubutton.menuId) {
        arr = this.menubutton.data.filter(
          (item) =>
            item.name.includes(this.menubutton.key) &&
            item.menuId.includes(this.menubutton.menuId)
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
     *
     * @param {*} param0
     */
    selectChangeEvent(e) {
      this.menubutton.data.forEach((f) => (f.exist = false));
      e.records.forEach((f) => {
        f.exist = true;
      });
      e.reserves.forEach((f) => {
        f.exist = true;
      });
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
      this.menubutton.menuId = electedKeys[0];
      this.menubutton.menuName = e.selectedNodes[0].title;
    },

    /**
     * 取消
     */
    cancel() {
      this.$emit("update:visible", false);
    },

    /**
     * 初始化以后菜单
     */
    menuInit() {
      let that = this;
      that.menubutton.spinning = true;
      menuHave(
        that.privilegeMasterValue,
        that.privilegeMaster,
        that.eipPrivilegeAccess.menubutton
      ).then((result) => {
        that.tree.data = result.data;
        menuButton(that.privilegeMasterValue, that.privilegeMaster).then(
          (result) => {
            that.selectRows = result.data.filter((f) => f.exist);
            that.menubutton.data = result.data;
            that.defaultSelecteRows = result.data
              .filter((f) => f.exist)
              .map((m) => m.menuButtonId);
            that.menubutton.spinning = false;
          }
        );
      });
    },

    /**
     * 所有
     */
    all() {
      this.menubutton.key = "";
      this.menubutton.menuId = "";
      this.menubutton.menuName = "所有";
    },

    /**
     * 保存
     */
    save() {
      let that = this;
      let menuPermissions = [];
      that.selectRows = this.menubutton.data.filter((f) => f.exist);
      that.selectRows.forEach((item) => {
        menuPermissions.push({ p: item.menuButtonId });
      });

      that.$loading.show({ text: that.eipMsg.saveLoading });
      save({
        privilegeAccess: this.eipPrivilegeAccess.menubutton,
        privilegeMaster: this.privilegeMaster,
        privilegeMasterValue: this.privilegeMasterValue,
        menuPermissions: JSON.stringify(menuPermissions),
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