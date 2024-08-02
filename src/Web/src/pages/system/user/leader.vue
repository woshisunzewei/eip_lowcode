<template>
  <a-modal width="1070px" v-drag :destroyOnClose="true" :maskClosable="false" :visible="visible"
    :bodyStyle="{ padding: '1px' }" :title="title" @cancel="cancel">

    <a-tabs default-active-key="1" v-model="tabKey">
      <a-tab-pane key="1">
        <span slot="tab">
          <a-icon type="branches" />
          上级领导
        </span>
        <div style="background-color:#f0f2f5">
          <a-row :gutter="[1, 0]">
            <a-col :span="6">
              <a-card class="eip-admin-card-small" size="small">
                <div slot="title" style="height: 32px; line-height: 32px">
                  <a-input-search v-model="organization.key" :allowClear="true" placeholder="名称/拼音模糊搜索"
                    @change="organizationSearch" />
                </div>
                <a-spin :spinning="organization.spinning">
                  <div :style="{ height: height, overflow: 'auto' }">
                    <a-directory-tree v-if="!organization.spinning" :expandAction="false"
                      :defaultExpandedKeys="[organization.data.length > 0 ? organization.data[0].key : '']"
                      :tree-data="organization.data" @select="treeSelect"></a-directory-tree>
                  </div>
                </a-spin>
              </a-card>
            </a-col>

            <a-col :span="18">
              <a-spin :spinning="user.spinning">
                <a-card class="eip-admin-card-small" size="small">
                  <div slot="title">{{ user.org }}人员</div>
                  <div slot="extra">
                    <a-space>
                      <a-input-search v-model="user.key" placeholder="账号/姓名模糊查询..." allowClear style="width: 240px" />
                      <a-button type="primary" @click="userAll">
                        <a-icon type="user" />查看所有人员
                      </a-button>
                    </a-space>
                  </div>

                  <div>
                    <vxe-table ref="chosenUserTable" row-id="userId" border id="chosenUser" :data="users"
                      @checkbox-change="userChange" @checkbox-all="userChange" :height="height" :checkbox-config="{
                        checkRowKeys: leaderUsers,
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
                      <vxe-column type="checkbox" width="40" align="center" fixed="left">
                        <template #header="{ checked, indeterminate }">
                          <span @click.stop="$refs.chosenUserTable.toggleAllCheckboxRow()">
                            <a-checkbox :indeterminate="indeterminate" :checked="checked">
                            </a-checkbox>
                          </span>
                        </template>
                        <template #checkbox="{ row, checked, indeterminate }">
                          <span @click.stop="$refs.chosenUserTable.toggleCheckboxRow(row)">
                            <a-checkbox :indeterminate="indeterminate" :checked="checked">
                            </a-checkbox>
                          </span>
                        </template>
                      </vxe-column>
                      <vxe-column type="seq" title="序号" width="60"></vxe-column>
                      <vxe-column field="headImage" title="头像" align="center" width="80">
                        <template v-slot="{ row }">
                          <img style="width: 32px; height: 32px; border-radius: 50%" v-real-img="row.headImage" />
                        </template>
                      </vxe-column>
                      <vxe-column field="code" title="账号" width="120"></vxe-column>
                      <vxe-column field="name" title="姓名" width="120"></vxe-column>
                      <!-- <vxe-column field="sex" title="性别" width="60" align="center">
                  <template v-slot="{ row }">
                    <a-tag color="red" v-if="row.sex == 1">女</a-tag>
                    <a-tag color="green" v-else>男</a-tag>
                  </template>
                </vxe-column> -->
                      <vxe-column field="parentIdsName" title="组织" showOverflow="ellipsis"></vxe-column>
                    </vxe-table>
                  </div>
                </a-card>
              </a-spin>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane key="2">
        <span slot="tab">
          <a-icon type="apartment" />
          下级人员
        </span>
        <div style="background-color:#f0f2f5">
          <a-row :gutter="[1, 0]">
            <a-col :span="6">
              <a-card class="eip-admin-card-small" size="small">
                <div slot="title" style="height: 32px; line-height: 32px">
                  <a-input-search v-model="organization.key" :allowClear="true" placeholder="名称/拼音模糊搜索"
                    @change="organizationSearch" />
                </div>
                <a-spin :spinning="organization.spinning">
                  <div :style="{ height: height, overflow: 'auto' }">
                    <a-directory-tree v-if="!organization.spinning" :expandAction="false"
                      :defaultExpandedKeys="[organization.data.length > 0 ? organization.data[0].key : '']"
                      :tree-data="organization.data" @select="subordinateSelect"></a-directory-tree>
                  </div>
                </a-spin>
              </a-card>
            </a-col>

            <a-col :span="18">
              <a-spin :spinning="subordinate.spinning">
                <a-card class="eip-admin-card-small" size="small">
                  <div slot="title">{{ subordinate.org }}人员</div>
                  <div slot="extra">
                    <a-space>
                      <a-input-search v-model="subordinate.key" placeholder="账号/姓名模糊查询..." allowClear
                        style="width: 240px" />
                      <a-button type="primary" @click="subordinateAll">
                        <a-icon type="user" />查看所有人员
                      </a-button>
                    </a-space>
                  </div>

                  <div>
                    <vxe-table ref="subordinateUsersTable" row-id="userId" border :data="subordinateUsers"
                      @checkbox-change="userSubordinateChange" @checkbox-all="userSubordinateChange" :height="height"
                      :checkbox-config="{
                        checkRowKeys: subordinate.users,
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
                      <vxe-column type="checkbox" width="40" align="center" fixed="left">
                        <template #header="{ checked, indeterminate }">
                          <span @click.stop="$refs.subordinateUsersTable.toggleAllCheckboxRow()">
                            <a-checkbox :indeterminate="indeterminate" :checked="checked">
                            </a-checkbox>
                          </span>
                        </template>
                        <template #checkbox="{ row, checked, indeterminate }">
                          <span @click.stop="$refs.subordinateUsersTable.toggleCheckboxRow(row)">
                            <a-checkbox :indeterminate="indeterminate" :checked="checked">
                            </a-checkbox>
                          </span>
                        </template>
                      </vxe-column>
                      <vxe-column type="seq" title="序号" width="60"></vxe-column>
                      <vxe-column field="headImage" title="头像" align="center" width="80">
                        <template v-slot="{ row }">
                          <img style="width: 32px; height: 32px; border-radius: 50%" v-real-img="row.headImage" />
                        </template>
                      </vxe-column>
                      <vxe-column field="code" title="账号" width="120"></vxe-column>
                      <vxe-column field="name" title="姓名" width="120"></vxe-column>
                      <!-- <vxe-column field="sex" title="性别" width="60" align="center">
                  <template v-slot="{ row }">
                    <a-tag color="red" v-if="row.sex == 1">女</a-tag>
                    <a-tag color="green" v-else>男</a-tag>
                  </template>
                </vxe-column> -->
                      <vxe-column field="parentIdsName" title="组织" showOverflow="ellipsis"></vxe-column>
                    </vxe-table>
                  </div>
                </a-card>
              </a-spin>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
    </a-tabs>
    <template slot="footer">
      <a-space>
        <a-button key="back" @click="cancel" :disabled="loading"><a-icon type="close" />取消</a-button>
        <a-button key="submit" @click="save" type="primary" :loading="loading"><a-icon type="save" />提交</a-button>
      </a-space>
    </template>

  </a-modal>
</template>

<script>
import { organization, user } from "@/services/common/usercontrol/chosenuser";
import {
  query,
  save,
  saveSubordinate,
  querySubordinate
} from "@/services/system/user/leader";

export default {
  name: "eip-user",
  data() {
    return {
      height: "500px",
      tabKey: "1",
      organization: {
        data: [],
        key: null,
        original: [],
        spinning: true,
      },
      leaderUsers: [],
      user: {
        org: "",
        data: [],
        key: "",
        orgId: "",
        spinning: false,
      },
      loading: false,

      subordinate: {
        org: "",
        data: [],
        key: "",
        orgId: "",
        spinning: false,
      }
    };
  },
  computed: {
    subordinateUsers() {
      var arr = [];
      this.subordinate.data.forEach((item) => arr.push(item));
      if (this.subordinate.key) {
        arr = this.subordinate.data.filter(
          (item) =>
            item.name.includes(this.subordinate.key) ||
            item.code.includes(this.subordinate.key)
        );
      }
      if (this.subordinate.orgId) {
        arr = this.subordinate.data.filter((item) =>
          item.organizationId.includes(this.subordinate.orgId)
        );
      }
      if (this.subordinate.key && this.subordinate.orgId) {
        arr = this.subordinate.data.filter(
          (item) =>
            (item.name.includes(this.subordinate.key) ||
              item.code.includes(this.subordinate.key)) &&
            item.organizationId.includes(this.subordinate.orgId)
        );
      }
      return arr;
    },
    users() {
      var arr = [];
      this.user.data.forEach((item) => arr.push(item));
      if (this.user.key) {
        arr = this.user.data.filter(
          (item) =>
            item.name.includes(this.user.key) ||
            item.code.includes(this.user.key)
        );
      }
      if (this.user.orgId) {
        arr = this.user.data.filter((item) =>
          item.organizationId.includes(this.user.orgId)
        );
      }
      if (this.user.key && this.user.orgId) {
        arr = this.user.data.filter(
          (item) =>
            item.name.includes(this.user.key) &&
            item.organizationId.includes(this.user.orgId)
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
    topOrg: {
      type: String,
    },
    title: {
      type: String,
    },
    userId: {
      type: String,
    }
  },
  mounted() {
    this.organizationInit();
    this.userInit();
  },
  methods: {
    /**
     * 
     * @param {*} e 
     */
    organizationSearch(e) {
      var that = this;
      this.organization.data = that.$utils.clone(this.organization.original, true);
      this.organization.data = that.$utils.searchTree(
        this.organization.data,
        (item) => {
          if (that.organization.key) {
            var titlePinyin = pinyin.convert(item.title).toLowerCase();
            if (
              item.title
                .toLowerCase()
                .includes(that.organization.key.toLowerCase())
            ) {
              return true;
            } else if (
              titlePinyin.includes(that.organization.key.toLowerCase())
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
     *
     * @param {*} param0
     */
    userChange(e) {
      this.user.data.forEach((f) => (f.exist = false));
      e.records.forEach((f) => {
        f.exist = true;
      });
      e.reserves.forEach((f) => {
        f.exist = true;
      });

    },

    /**
  *
  * @param {*} param0
  */
    userSubordinateChange(e) {
      this.subordinate.data.forEach((f) => (f.exist = false));
      e.records.forEach((f) => {
        f.exist = true;
      });
      e.reserves.forEach((f) => {
        f.exist = true;
      });
    },

    /**
     * 树选中
     */
    treeSelect(electedKeys, e) {
      this.user.orgId = electedKeys[0];
      this.user.org = e.selectedNodes[0].title;
    },

    /**
   * 树选中
   */
    subordinateSelect(electedKeys, e) {
      this.subordinate.orgId = electedKeys[0];
      this.subordinate.org = e.selectedNodes[0].title;
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
      organization({}).then((result) => {
        that.organization.data = result.data;
        that.organization.original = result.data;
        that.organization.spinning = false;
      });
    },

    /**
     * 初始化人员
     */
    userInit() {
      let that = this;
      that.user.spinning = true;
      user({ topOrg: this.topOrg }).then((result) => {
        that.user.spinning = false;
        query(that.userId).then(queryResult => {
          var queryUsers = that.$utils.clone(result.data, true);
          if (queryResult.data.length > 0) {
            that.leaderUsers = queryResult.data.map((m) => m.leaderUserId);
            queryUsers.forEach((item) => {
              item.exist =
                queryResult.data.filter((f) => f.leaderUserId == item.userId).length > 0;
            });
          }
          that.user.data = that.$utils.orderBy(queryUsers, [['exist', 'desc']]);
        })
        querySubordinate(that.userId).then(queryResult => {
          var subordinateUsers = that.$utils.clone(result.data, true);
          if (queryResult.data.length > 0) {
            that.subordinate.users = queryResult.data.map((m) => m.userId);
            subordinateUsers.forEach((item) => {
              item.exist =
                queryResult.data.filter((f) => f.userId == item.userId).length > 0;
            });
          }

          that.subordinate.data = that.$utils.orderBy(subordinateUsers, [['exist', 'desc']]);
        })
      });
    },

    /**
     * 所有用户
     */
    userAll() {
      this.user.key = "";
      this.user.orgId = "";
      this.user.org = "所有";
    },

    /**
     * 所有下级
     */
    subordinateAll() {
      this.subordinate.key = "";
      this.subordinate.orgId = "";
      this.subordinate.org = "所有";
    },
    /**
     * 用户选择
     */
    userCheck(user) {
      user.exist = !user.exist;
    },


    /**
     * 保存
     */
    save() {
      let that = this;
      let privilegeMasterUser = [];

      if (this.tabKey == "1") {
        this.user.data.filter((f) => f.exist).forEach((item) => {
          privilegeMasterUser.push(item.userId);
        });
        that.loading = true;
        that.$loading.show({ text: that.eipMsg.saveLoading });
        save({
          userId: this.userId,
          u: privilegeMasterUser,
        }).then((result) => {
          that.$loading.hide();
          if (result.code === that.eipResultCode.success) {
            that.$message.success(result.msg);
          } else {
            that.$message.error(result.msg);
          }
          that.loading = false;
        });
      }
      else {
        this.subordinate.data.filter((f) => f.exist).forEach((item) => {
          privilegeMasterUser.push(item.userId);
        });
        that.loading = true;
        that.$loading.show({ text: that.eipMsg.saveLoading });
        saveSubordinate({
          userId: this.userId,
          u: privilegeMasterUser,
        }).then((result) => {
          that.$loading.hide();
          if (result.code === that.eipResultCode.success) {
            that.$message.success(result.msg);
          } else {
            that.$message.error(result.msg);
          }
          that.loading = false;
        });
      }

    },
  },
};
</script>

<style lang="less" scoped>
/deep/ .ant-tabs-bar {
  margin: 0 0 2px 0 !important;
}
</style>
