<template>

  <a-modal width="1170px" v-drag :destroyOnClose="true" :maskClosable="false" :visible="visible"
    :bodyStyle="{ padding: '1px', 'background-color': '#f0f2f5' }" :title="title" @cancel="cancel">
    <a-row :gutter="[1, 0]">
      <a-col :span="5">
        <a-card class="eip-admin-card-small" size="small">
          <div slot="title" style="height: 32px; line-height: 32px">
            <a-input-search v-model="organization.key" :allowClear="true" placeholder="名称/拼音模糊搜索"
              @change="organizationSearch" />
          </div>
          <a-directory-tree :expandAction="false"
            :defaultExpandedKeys="[organization.data.length > 0 ? organization.data[0].key : '']"
            :style="{ height: height, overflow: 'auto' }" :tree-data="organization.data"
            @select="treeSelect"></a-directory-tree>
        </a-card>
      </a-col>

      <a-col :span="19">
        <a-spin :spinning="user.spinning">
          <a-row>
            <a-col :span="12">
              <a-card class="eip-admin-card-small" size="small" :hoverable="true">
                <div slot="title">
                  <a-badge :offset="[8, 0]" :count="users.filter((f) => !f.exist).length">
                    {{ user.org }} 待选人员
                  </a-badge>
                </div>
                <div slot="extra">
                  <a-space>
                    <a-input-search v-model="user.key" placeholder="姓名/组织模糊查询..." allowClear style="width: 200px" />
                  </a-space>
                </div>
                <vxe-table row-id="userId" border id="chosenUser" ref="chosenUser" :data="users.filter((f) => !f.exist)"
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
                      <a-button @click="chosenUserCheckAddAll()" size="small" type="dashed">
                        <a-icon type="plus" />
                      </a-button>
                    </template>
                    <template v-slot="{ row }">
                      <a-button @click="chosenUserCheckAdd(row)" size="small" type="dashed">
                        <a-icon type="plus" />
                      </a-button>
                    </template>
                  </vxe-column>
                  <vxe-column field="headImage" title="头像" align="center" width="60" showOverflow="ellipsis">
                    <template v-slot="{ row }">
                      <img style="width: 32px; height: 32px; border-radius: 50%" v-real-img="row.headImage" />
                    </template>
                  </vxe-column>
                  <!-- <vxe-column field="code" title="账号" width="120"></vxe-column> -->
                  <vxe-column field="name" title="姓名" width="100" showOverflow="ellipsis"></vxe-column>
                  <!-- <vxe-column field="sex" title="性别" width="60" align="center">
                  <template v-slot="{ row }">
                    <a-tag color="red" v-if="row.sex == 1">女</a-tag>
                    <a-tag color="green" v-else>男</a-tag>
                  </template>
                </vxe-column> -->
                  <vxe-column field="organizationName" title="组织" width="180" showOverflow="ellipsis"></vxe-column>
                </vxe-table>
              </a-card></a-col>
            <a-col :span="12"><a-card class="eip-admin-card-small" size="small" :hoverable="true">
                <div slot="title">
                  <a-badge :offset="[8, 0]" :count="user.data.filter((f) => f.exist).length">
                    已选人员
                  </a-badge>
                </div>
                <div slot="extra">
                  <a-space>
                    <a-input-search v-model="user.checkkey" placeholder="姓名/组织模糊查询..." allowClear
                      style="width: 200px" />
                  </a-space>
                </div>
                <vxe-table border id="chosenUserCheck" ref="chosenUserCheck" :data="usersexist" :height="height"
                  :tooltip-config="tooltipConfig">
                  <template #loading>
                    <a-spin></a-spin>
                  </template>
                  <template #empty>
                    <eip-empty />
                  </template>
                  <vxe-column type="seq" title="序号" align="center" width="50" showOverflow="ellipsis"></vxe-column>
                  <vxe-column align="center" width="60" showOverflow="ellipsis">
                    <template #header>
                      <a-button @click="chosenUserCheckDelAll()" size="small" type="danger">
                        <a-icon type="delete" />
                      </a-button>
                    </template>
                    <template v-slot="{ row }">
                      <a-button @click="chosenUserCheckDel(row)" size="small" type="danger">
                        <a-icon type="delete" />
                      </a-button>
                    </template>
                  </vxe-column>
                  <vxe-column field="headImage" title="头像" align="center" width="60" showOverflow="ellipsis">
                    <template v-slot="{ row }">
                      <img style="width: 32px; height: 32px; border-radius: 50%" v-real-img="row.headImage" />
                    </template>
                  </vxe-column>
                  <!-- <vxe-column field="code" title="账号" width="120"></vxe-column> -->
                  <vxe-column field="name" title="姓名" width="100" showOverflow="ellipsis"></vxe-column>
                  <!-- <vxe-column field="sex" title="性别" width="60" align="center">
                  <template v-slot="{ row }">
                    <a-tag color="red" v-if="row.sex == 1">女</a-tag>
                    <a-tag color="green" v-else>男</a-tag>
                  </template>
                </vxe-column> -->
                  <vxe-column field="organizationName" title="组织" showOverflow="ellipsis" width="180"></vxe-column>
                </vxe-table>
              </a-card></a-col>
          </a-row>
        </a-spin>
      </a-col>
    </a-row>
    <template slot="footer">
      <div class="flex justify-between align-center">
        <div>
        </div>
        <a-space>
          <a-button key="back" @click="cancel" :disabled="loading"><a-icon type="close" />取消</a-button>
          <a-button key="submit" @click="save" type="primary" :loading="loading"><a-icon type="save" />提交</a-button>
        </a-space>
      </div>
    </template>
  </a-modal>
</template>

<script>
import {
  chosenprivilegemasteruser,
  save,
} from "@/services/common/usercontrol/chosenprivilegemasteruser";

export default {
  name: "eip-privilegemasteruser",
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
      selectRows: [],
      user: {
        org: "",
        data: [],
        key: "",
        checkkey: '',
        orgId: [],
        spinning: false,
      },
      organization: {
        key: null,
        original: [],
        data: []
      },
      loading: false,
    };
  },
  computed: {
    /**
      * 未所有人员
      */
    users() {
      var arr = [];
      this.user.data.filter(f => !f.exist).forEach((item) => arr.push(item));
      if (this.user.key) {
        arr = arr.filter(
          (item) =>
            item.name.includes(this.user.key) ||
            item.organizationName.includes(this.user.key)
        );
      }
      if (this.user.orgId.length > 0) {
        var datas = []
        this.user.orgId.forEach(item => {
          var cdata = arr.filter((itemFilter) =>
            itemFilter.organizationId.includes(item)
          );
          datas = datas.concat(cdata);
        })
        arr = datas;
      }
      if (this.user.key && this.user.orgId.length > 0) {
        var datas = []
        this.user.orgId.forEach(item => {
          var cdata = arr.filter((itemFilter) =>
            (itemFilter.name.includes(this.user.key) ||
              itemFilter.organizationName.includes(this.user.key)) &&
            itemFilter.organizationId.includes(item)
          );
          datas = datas.concat(cdata);
        })
        arr = datas;
      }
      return arr;
    },
    /**
     * 已选中人员
     */
    usersexist() {
      var arr = [];
      this.user.data.filter(f => f.exist).forEach((item) => arr.push(item));
      if (this.user.checkkey) {
        arr = arr.filter(
          (item) =>
            item.name.includes(this.user.checkkey) ||
            item.organizationName.includes(this.user.checkkey)
        );
      }
      if (this.user.orgId.length > 0) {
        var datas = []
        this.user.orgId.forEach(item => {
          var cdata = arr.filter((itemFilter) =>
            itemFilter.organizationId.includes(item)
          );
          datas = datas.concat(cdata);
        })
        arr = datas;
      }
      if (this.user.checkkey && this.user.orgId.length > 0) {
        var datas = []
        this.user.orgId.forEach(item => {
          var cdata = arr.filter((itemFilter) =>
            (itemFilter.name.includes(this.user.checkkey) ||
              itemFilter.organizationName.includes(this.user.checkkey)) &&
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
    data: {
      type: Array,
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
    this.userInit();
    this.organization.original = this.data;
    this.organization.data = this.data;
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
      this.user.orgId = orgIds;
      this.user.org = e.selectedNodes[0].title;
    },
    /**
     * 取消
     */
    cancel() {
      this.$emit("update:visible", false);
    },

    /**
     * 初始化人员
     */
    userInit() {
      let that = this;
      that.user.spinning = true;
      chosenprivilegemasteruser(
        this.privilegeMaster,
        this.privilegeMasterValue
      ).then((result) => {
        that.selectRows = result.data.filter((f) => f.exist);
        that.user.data = result.data;
        that.user.spinning = false;
      });
    },

    /**
     * 保存
     */
    save() {
      let that = this;
      let privilegeMasterUser = [];
      that.selectRows = this.user.data.filter((f) => f.exist);
      that.selectRows.forEach((item) => {
        privilegeMasterUser.push({ u: item.userId });
      });
      that.$loading.show({ text: that.eipMsg.saveLoading });
      that.loading = true;
      save({
        privilegeMaster: this.privilegeMaster,
        privilegeMasterValue: this.privilegeMasterValue,
        privilegeMasterUser: JSON.stringify(privilegeMasterUser),
      }).then((result) => {
        that.$loading.hide();
        if (result.code === that.eipResultCode.success) {
          that.$message.success(result.msg);
          that.cancel();
        } else {
          that.$message.error(result.msg);
        }
        that.loading = false;
      });
    },


    /**
     * 添加对应人
     * @param {} row 
     */
    chosenUserCheckAdd(row) {
      row.exist = true;
    },

    /**
     * 添加所有
     */
    chosenUserCheckAddAll() {
      var getTableData = this.$refs.chosenUser.getTableData();
      getTableData.fullData.filter(f => !f.exist).forEach(item => {
        item.exist = true
      })
    },

    /**
     * 已选清除
     */
    chosenUserCheckDel(row) {
      row.exist = false;
    },

    /**
     * 清空所有
     */
    chosenUserCheckDelAll() {
      var getTableData = this.$refs.chosenUserCheck.getTableData();
      getTableData.fullData.forEach(item => {
        item.exist = false
      })
    }
  },
};
</script>