<template>
  <a-modal width="1170px" v-drag centered :destroyOnClose="true" :maskClosable="false" :visible="visible" :zIndex="1001"
    :bodyStyle="{ padding: '1px', 'background-color': '#f0f2f5' }" :title="title" @cancel="cancel">
    <a-row :gutter="[1, 0]">
      <a-col :span="5">
        <a-card class="eip-admin-card-small" size="small">
          <div slot="title" style="height: 32px; line-height: 32px">
            <a-space>
              <a-input-search v-model="organization.key" :allowClear="true" placeholder="名称/拼音模糊搜索"
                @change="organizationSearch" />
              <a-button title="显示所有" @click="orgReset" type="primary">
                <a-icon type="retweet" />
              </a-button>
            </a-space>
          </div>
          <a-spin :spinning="organization.spinning">
            <div :style="{ height: '514px', overflow: 'auto' }">
              <a-directory-tree :expandAction="false" v-if="!organization.spinning"
                :defaultExpandedKeys="[organization.data.length > 0 ? organization.data[0].key : '']"
                :tree-data="organization.data" @select="treeSelect"></a-directory-tree>
            </div>
          </a-spin>
        </a-card>
      </a-col>

      <a-col :span="19">
        <a-spin :spinning="role.spinning">
          <a-row>
            <a-col :span="12">
              <a-card size="small" :hoverable="true">
                <div slot="title">
                  <a-badge :offset="[8, 0]" :count="roles.filter((f) => !f.exist).length">
                    {{ role.org }} 待选角色
                  </a-badge>
                </div>
                <div slot="extra">
                  <a-space>
                    <a-input-search v-model="role.key" placeholder="名称模糊查询..." allowClear style="width: 200px" />
                  </a-space>
                </div>
                <vxe-table row-id="roleId" border id="chosenRole" ref="chosenRole" :data="roles.filter((f) => !f.exist)"
                  :height="height" :tooltip-config="tooltipConfig">
                  <template #loading>
                    <a-spin></a-spin>
                  </template>
                  <template #empty>
                    <eip-empty />
                  </template>
                  <vxe-column type="seq" title="序号" align="center" width="50" showOverflow="ellipsis"></vxe-column>
                  <vxe-column align="center" width="60" showOverflow="ellipsis">
                    <template #header>
                      <a-button v-if="multiple" @click="chosenRoleCheckAddAll()" size="small" type="dashed">
                        <a-icon type="plus" />
                      </a-button>
                    </template>
                    <template v-slot="{ row }">
                      <a-button @click="chosenRoleCheckAdd(row)" size="small" type="dashed">
                        <a-icon type="plus" />
                      </a-button>
                    </template>
                  </vxe-column>

                  <vxe-column field="name" title="名称" width="150" showOverflow="ellipsis"></vxe-column>
                  <vxe-column field="organizationName" title="组织" width="200" showOverflow="ellipsis"></vxe-column>
                </vxe-table>
              </a-card>
            </a-col>
            <a-col :span="12"><a-card size="small" :hoverable="true">
                <div slot="title">
                  <a-badge :offset="[8, 0]" :count="role.data.filter((f) => f.exist).length">
                    已选角色
                  </a-badge>
                </div>
                <div slot="extra">
                  <a-space>
                    <a-input-search v-model="role.checkkey" placeholder="名称模糊查询..." allowClear style="width: 200px" />
                  </a-space>
                </div>
                <vxe-table row-id="roleId" border id="chosenRoleCheck" ref="chosenRoleCheck" :data="rolesExist"
                  :height="height" :tooltip-config="tooltipConfig">
                  <template #loading>
                    <a-spin></a-spin>
                  </template>
                  <template #empty>
                    <eip-empty />
                  </template>
                  <vxe-column type="seq" title="序号" align="center" width="50" showOverflow="ellipsis"></vxe-column>
                  <vxe-column align="center" width="60" showOverflow="ellipsis">
                    <template #header>
                      <a-button @click="chosenRoleCheckDelAll()" size="small" type="danger">
                        <a-icon type="delete" />
                      </a-button>
                    </template>
                    <template v-slot="{ row }">
                      <a-button @click="chosenRoleCheckDel(row)" size="small" type="danger">
                        <a-icon type="delete" />
                      </a-button>
                    </template>
                  </vxe-column>
                  <vxe-column field="name" title="名称" width="150" showOverflow="ellipsis"></vxe-column>
                  <vxe-column field="organizationName" title="组织" width="200" showOverflow="ellipsis"></vxe-column>
                </vxe-table>
              </a-card>
            </a-col>
          </a-row>
        </a-spin>
      </a-col>
    </a-row>
    <template slot="footer">
      <a-space>
        <a-button key="back" @click="cancel" :disabled="loading"><a-icon type="close" />取消</a-button>
        <a-button key="submit" @click="save" type="primary" :loading="loading"><a-icon type="save" />提交</a-button>
      </a-space>
    </template>
  </a-modal>
</template>

<script>
import { organization, role } from "@/services/common/usercontrol/chosenrole";

export default {
  name: "eip-role",
  data() {
    return {
      tooltipConfig: {
        showAll: true,
        enterable: true,
        contentMethod: ({ type, column, row, items, _columnIndex }) => {
          const { property } = column;
          if (row && property === "organizationName") {
            return row['parentIdsName'];
          }
          return null;
        }
      },
      height: "500px",
      organization: {
        data: [],
        key: null,
        original: [],
        spinning: true,
      },
      selectRows: [],
      role: {
        org: "",
        data: [],
        key: "",
        checkkey: '',
        orgId: [],
        spinning: false,
      },
      loading: false,
    };
  },
  computed: {
    roles() {
      var arr = [];
      this.role.data.filter(f => !f.exist).forEach((item) => arr.push(item));
      if (this.role.key) {
        arr = arr.filter(
          (item) =>
            item.name.includes(this.role.key) ||
            item.organizationName.includes(this.role.key)
        );
      }
      if (this.role.orgId.length > 0) {
        arr = arr.filter((itemFilter) =>
          itemFilter.parentIds.includes(this.role.orgId[0])
        );
      }

      if (this.role.key && this.role.orgId.length > 0) {
        arr = arr.filter((itemFilter) =>
          (itemFilter.name.includes(this.role.checkkey) ||
            itemFilter.organizationName.includes(this.role.checkkey)) &&
          itemFilter.parentIds.includes(this.role.orgId[0])
        );
      }
      return arr;
    },

    /**
    * 已选中角色
    */
    rolesExist() {
      var arr = [];
      this.role.data.filter(f => f.exist).forEach((item) => arr.push(item));
      if (this.role.checkkey) {
        arr = arr.filter(
          (item) =>
            item.name.includes(this.role.checkkey) ||
            item.organizationName.includes(this.role.checkkey)
        );
      }

      if (this.role.orgId.length > 0) {
        arr = arr.filter((itemFilter) =>
          itemFilter.parentIds.includes(this.role.orgId[0])
        );
      }

      if (this.role.checkkey && this.role.orgId.length > 0) {
        arr = arr.filter((itemFilter) =>
          (itemFilter.name.includes(this.role.checkkey) ||
            itemFilter.organizationName.includes(this.role.checkkey)) &&
          itemFilter.parentIds.includes(this.role.orgId[0])
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
    multiple: {
      type: Boolean,
      default: true,
    },
    topOrg: {
      type: String,
    },
    title: {
      type: String,
    },
    chosen: {
      type: Array,
    }, //已选中对象,编辑传入
  },
  mounted() {
    this.organizationInit();
    this.roleInit();
  },
  methods: {
    /**
     * 
     */
    orgReset() {
      this.role.orgId = [];
    },

    /**
     *
     * @param {*} e
     */
    organizationSearch(e) {
      var that = this;
      this.organization.data = this.$utils.clone(this.organization.original, true);
      this.organization.data = this.$utils.searchTree(
        this.organization.data,
        (item) => {
          if (that.organization.key) {
            var titlePinyin = pinyin.convert(item.title).toLowerCase();
            if (
              item.title
                .toLowerCase()
                .indexOf(that.organization.key.toLowerCase()) > -1
            ) {
              return true;
            } else if (
              titlePinyin.indexOf(that.organization.key.toLowerCase()) > -1
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
     * 树选中
     */
    treeSelect(electedKeys, e) {
      var orgId = electedKeys[0];
      var orgIds = [orgId]
      // 递归遍历控件树
      var orgFilters = [];
      const traverseC = (array) => {
        array.forEach((element) => {
          if (element.value != orgId) {
            traverseC(element.children);
          } else {
            orgFilters = element.children;
          }
        });
      };
      traverseC(this.organization.data);

      const traverse = (array) => {
        array.forEach((element) => {
          orgIds.push(element.value)
          if (element.children.length > 0) {
            traverse(element.children);
          }
        });
      };
      traverse(orgFilters);
      this.role.orgId = orgIds;
      this.role.org = e.selectedNodes[0].title;

    },

    /**
     * 取消
     */
    cancel() {
      this.$emit("update:visible", false);
    },

    /**
     * 组织架构树
     */
    organizationInit() {
      let that = this;
      that.organization.spinning = true;
      organization({ topOrg: this.topOrg }).then((result) => {
        that.organization.data = result.data;
        that.organization.original = result.data;
        that.organization.spinning = false;
      });
    },

    /**
     * 初始化角色
     */
    roleInit() {
      let that = this;
      that.role.spinning = true;
      role({ topOrg: this.topOrg }).then((result) => {
        that.selectRows = [];
        that.role.spinning = false;
        result.data.forEach((item) => {
          if (that.chosen) {
            item.exist =
              that.chosen.filter((f) => f.id == item.roleId).length > 0;
          } else {
            item.exist = false;
          }
        });
        that.role.data = result.data;
      });
    },

    /**
     * 保存
     */
    save() {
      this.selectRows = this.role.data.filter((f) => f.exist);
      this.$emit("ok", this.selectRows);
      this.cancel();
    },

    /**
     * 添加对应人
     * @param {} row 
     */
    chosenRoleCheckAdd(row) {
      if (!this.multiple) {
        this.role.data.forEach(f => [
          f.exist = false
        ])
      }
      row.exist = true;
    },

    /**
     * 添加所有
     */
    chosenRoleCheckAddAll() {
      var getTableData = this.$refs.chosenRole.getTableData();
      getTableData.fullData.filter(f => !f.exist).forEach(item => {
        item.exist = true
      })
    },

    /**
     * 已选清除
     */
    chosenRoleCheckDel(row) {
      row.exist = false;
    },

    /**
     * 清空所有
     */
    chosenRoleCheckDelAll() {
      var getTableData = this.$refs.chosenRoleCheck.getTableData();
      getTableData.fullData.forEach(item => {
        item.exist = false
      })
    }
  },
};
</script>