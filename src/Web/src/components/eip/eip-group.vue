<template>
  <a-modal width="1170px" v-drag centered :destroyOnClose="true" :maskClosable="false" :visible="visible" :zIndex="1001"
    :bodyStyle="{ padding: '1px', 'background-color': '#f0f2f5' }" :title="title" @cancel="cancel">
    <a-row :gutter="[1, 0]">
      <a-col :span="5">
        <a-card class="eip-admin-card-small" size="small">
          <div slot="title" style="height: 32px; line-height: 32px">
            <a-input-search v-model="organization.key" :allowClear="true" placeholder="名称/拼音模糊搜索"
              @change="organizationSearch" />
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
        <a-spin :spinning="group.spinning">
          <a-row>
            <a-col :span="12">
              <a-card size="small" :hoverable="true">
                <div slot="title">
                  <a-badge :offset="[8, 0]" :count="groups.filter((f) => !f.exist).length">
                    {{ group.org }} 待选组
                  </a-badge>
                </div>
                <div slot="extra">
                  <a-space>
                    <a-input-search v-model="group.key" placeholder="名称模糊查询..." allowClear style="width: 200px" />
                  </a-space>
                </div>
                <vxe-table row-id="groupId" border id="chosenGroup" ref="chosenGroup"
                  :data="groups.filter((f) => !f.exist)" :height="height" :tooltip-config="tooltipConfig">
                  <template #loading>
                    <a-spin></a-spin>
                  </template>
                  <template #empty>
                    <eip-empty />
                  </template>
                  <vxe-column type="seq" title="序号" align="center" width="50" showOverflow="ellipsis"></vxe-column>
                  <vxe-column align="center" width="60" showOverflow="ellipsis">
                    <template #header>
                      <a-button @click="chosenGroupCheckAddAll()" size="small" type="dashed">
                        <a-icon type="plus" />
                      </a-button>
                    </template>
                    <template v-slot="{ row }">
                      <a-button @click="chosenGroupCheckAdd(row)" size="small" type="dashed">
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
                  <a-badge :offset="[8, 0]" :count="group.data.filter((f) => f.exist).length">
                    已选组
                  </a-badge>
                </div>
                <div slot="extra">
                  <a-space>
                    <a-input-search v-model="group.checkkey" placeholder="名称模糊查询..." allowClear style="width: 200px" />
                  </a-space>
                </div>
                <vxe-table row-id="groupId" border id="chosenGroupCheck" ref="chosenGroupCheck" :data="groupsExist"
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
                      <a-button @click="chosenGroupCheckDelAll()" size="small" type="danger">
                        <a-icon type="delete" />
                      </a-button>
                    </template>
                    <template v-slot="{ row }">
                      <a-button @click="chosenGroupCheckDel(row)" size="small" type="danger">
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
import { organization, group } from "@/services/common/usercontrol/chosengroup";
export default {
  name: "eip-group",
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
      group: {
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
    groups() {
      var arr = [];
      this.group.data.forEach((item) => arr.push(item));
      if (this.group.key) {
        arr = this.group.data.filter(
          (item) =>
            item.name.includes(this.group.key)
        );
      }
      if (this.group.orgId.length > 0) {
        var datas = []
        this.group.orgId.forEach(item => {
          var cdata = arr.filter((itemFilter) =>
            itemFilter.organizationId == item
          );
          datas = datas.concat(cdata);
        })
        arr = datas;
      }
      if (this.group.key && this.group.orgId.length > 0) {
        var datas = []
        this.group.orgId.forEach(item => {
          var cdata = arr.filter((itemFilter) =>
            itemFilter.name.includes(this.group.key) &&
            itemFilter.organizationId == item
          );
          datas = datas.concat(cdata);
        })
        arr = datas;
      }
      return arr;
    },

    /**
    * 已选中组
    */
    groupsExist() {
      var arr = [];
      this.group.data.filter(f => f.exist).forEach((item) => arr.push(item));
      if (this.group.checkkey) {
        arr = arr.filter(
          (item) =>
            item.name.includes(this.group.checkkey)
        );
      }
      if (this.group.orgId.length > 0) {
        var datas = []
        this.group.orgId.forEach(item => {
          var cdata = arr.filter((itemFilter) =>
            itemFilter.organizationId.includes(item)
          );
          datas = datas.concat(cdata);
        })
        arr = datas;
      }
      if (this.group.checkkey && this.group.orgId.length > 0) {
        var datas = []
        this.group.orgId.forEach(item => {
          var cdata = arr.filter((itemFilter) =>
            (itemFilter.name.includes(this.group.checkkey)) &&
            itemFilter.organizationId.includes(item)
          );
          datas = datas.concat(cdata);
        })
        arr = datas;
      }
      return arr;
    },
  },

  props: {
    visible: {
      type: Boolean,
      default: false,
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
    this.groupInit();
  },
  methods: {
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
      this.group.orgId = orgIds;
      this.group.org = e.selectedNodes[0].title;

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
     * 初始化组
     */
    groupInit() {
      let that = this;
      that.group.spinning = true;
      group({ topOrg: this.topOrg }).then((result) => {
        that.selectRows = [];
        that.group.spinning = false;
        result.data.forEach((item) => {
          if (that.chosen) {
            item.exist =
              that.chosen.filter((f) => f.id == item.roleId).length > 0;
          } else {
            item.exist = false;
          }
        });
        that.group.data = result.data;
      });
    },

    /**
     * 保存
     */
    save() {
      this.selectRows = this.group.data.filter((f) => f.exist);
      this.$emit("ok", this.selectRows);
      this.cancel();
    },

    /**
     * 添加对应人
     * @param {} row 
     */
    chosenGroupCheckAdd(row) {
      row.exist = true;
    },

    /**
     * 添加所有
     */
    chosenGroupCheckAddAll() {
      var getTableData = this.$refs.chosenGroup.getTableData();
      getTableData.fullData.filter(f => !f.exist).forEach(item => {
        item.exist = true
      })
    },

    /**
     * 已选清除
     */
    chosenGroupCheckDel(row) {
      row.exist = false;
    },

    /**
     * 清空所有
     */
    chosenGroupCheckDelAll() {
      var getTableData = this.$refs.chosenGroupCheck.getTableData();
      getTableData.fullData.forEach(item => {
        item.exist = false
      })
    }
  },
};
</script>