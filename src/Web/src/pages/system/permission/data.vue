<template>
  <a-modal width="770px" v-drag :destroyOnClose="true" :maskClosable="false" :visible="visible"
    :bodyStyle="{ padding: '1px' }" :title="title" @cancel="cancel">
    <a-spin :spinning="data.spinning">
      <a-row>
        <a-col :span="7">
          <a-card class="eip-admin-card-small" size="small">
            <div slot="title" style="height: 32px; line-height: 32px">模块</div>
            <a-directory-tree v-if="tree.data.length > 0"
              :defaultExpandedKeys="[tree.data.length > 0 ? tree.data[0].key : '']" :expandAction="false"
              :style="{ height: height, overflow: 'auto' }" :tree-data="tree.data" :icon="treeIcon"
              @select="treeSelect">
            </a-directory-tree>

            <eip-empty :style="{ height: height, overflow: 'auto' }" v-if="tree.data.length == 0" description="无模块信息" />
          </a-card>
        </a-col>

        <a-col :span="17">
          <a-card class="eip-admin-card-small" size="small">
            <div slot="title">{{ data.menuName }}数据权限</div>
            <div slot="extra">
              <a-space>
                <a-input-search v-model="data.key" placeholder="名称模糊查询..." allowClear />
                <a-button type="primary" @click="all"><a-icon type="table" />所有数据权限</a-button></a-space>
            </div>
            <vxe-table ref="table" border row-id="dataId" :data="datas" @checkbox-change="selectChangeEvent"
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
              <vxe-column type="checkbox" width="40" align="center" fixed="left">
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
              <vxe-column field="name" title="归属模块" showOverflow="ellipsis"></vxe-column>
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
import { menuHave, save, data } from "@/services/system/permission/data";
export default {
  name: "data-permission",
  data() {
    return {
      height: "500px",
      defaultSelecteRows: [],
      selectRows: [],
      tree: {
        data: [],
        loading: true,
      },
      data: {
        key: "",
        data: [],
        menuName: "",
        menuId: "",
        spinning: false,
      },
      loading: false,
    };
  },
  computed: {
    datas() {
      var arr = [];
      this.data.data.forEach((item) => arr.push(item));
      if (this.data.key) {
        arr = this.data.data.filter((item) =>
          item.name.includes(this.data.key)
        );
      }
      if (this.data.menuId) {
        arr = this.data.data.filter((item) =>
          item.menuId.includes(this.data.menuId)
        );
      }
      if (this.data.key && this.data.menuId) {
        arr = this.data.data.filter(
          (item) =>
            item.name.includes(this.data.key) &&
            item.menuId.includes(this.data.menuId)
        );
      }
      return arr;
    },
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
     *
     * @param {*} param0
     */
    selectChangeEvent(e) {
      this.data.data.forEach((f) => (f.exist = false));
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
      this.data.menuId = electedKeys[0];
      this.data.menuName = e.selectedNodes[0].title;
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
      that.data.spinning = true;
      menuHave(
        this.privilegeMasterValue,
        this.privilegeMaster,
        this.eipPrivilegeAccess.data
      ).then((result) => {
        that.tree.data = result.data;
        data(that.privilegeMasterValue, that.privilegeMaster).then((result) => {
          that.selectRows = result.data.filter((f) => f.exist);
          that.data.data = result.data;
          that.defaultSelecteRows = result.data
            .filter((f) => f.exist)
            .map((m) => m.dataId);
          that.data.spinning = false;
        });
      });
    },

    /**
     * 所有
     */
    all() {
      this.data.key = "";
      this.data.menuId = "";
      this.data.menuName = "所有";
    },

    /**
     * 用户选择
     */
    check(data) {
      data.remark = data.remark == "selected" ? "" : "selected";
    },

    /**
     * 保存
     */
    save() {
      let that = this;
      let menuPermissions = [];
      that.selectRows = this.data.data.filter((f) => f.exist);
      that.selectRows.forEach((item) => {
        menuPermissions.push({ p: item.dataId });
      });
      that.$loading.show({ text: that.eipMsg.saveLoading });
      save({
        privilegeAccess: this.eipPrivilegeAccess.data,
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