<template>
  <a-modal width="770px" :visible="visible" :bodyStyle="{ padding: '1px' }" :title="title" @cancel="cancel" centered
    v-drag>
    <template slot="footer">
      <a-button key="back" @click="cancel" :disabled="loading"><a-icon type="close" />取消</a-button>
      <a-button key="submit" @click="save" type="primary" :loading="loading"><a-icon type="save" />提交</a-button>
    </template>
    <a-spin :spinning="organization.spinning">
      <a-row>
        <a-col :span="8" style="width: 250px">
          <a-card class="eip-admin-card-small" size="small">
            <div slot="title" style="height: 32px; line-height: 32px">
              组织架构
            </div>
            <a-directory-tree v-if="organization.data.length" :expandAction="false"
              :defaultExpandedKeys="[organization.data[0].key]" :style="organization.style" :tree-data="organization.data"
              :icon="organizationIcon" @select="organizationSelect"></a-directory-tree>
          </a-card>
        </a-col>

        <a-col :span="15" style="margin-left: 3px; width: 508px">
          <a-card class="eip-admin-card-small" size="small">
            <div slot="title">{{ user.org }}人员</div>
            <div slot="extra">
              <a-space>
                <a-input-search v-model="user.key" placeholder="帐号/姓名模糊查询..." style="width: 150px" />
                <a-button type="primary" @click="userAll">
                  <a-icon type="user" />查看所有人员
                </a-button>
              </a-space>
            </div>

            <div class="scrollBar">
              <ul id="accessButton" class="sys_spec_text">
                <li v-for="(user, i) in users" :key="i" :class="user.exist ? 'selected' : ''" @click="userCheck(user)">
                  <a-tooltip>
                    <template slot="title">{{
                      user.name + "-" + user.parentIdsName
                    }}</template>
                    <a>
                      <a-icon type="user" />
                      {{ user.name }}
                    </a>
                  </a-tooltip>
                  <i class="check"></i>
                </li>
              </ul>
            </div>
          </a-card>
        </a-col>
      </a-row>
    </a-spin>
  </a-modal>
</template>

<script>
import {
  queryBindUser,
  queryBindOrganization,
  save,
} from "@/services/wechat/user/binduser";
export default {
  name: "binduser",
  data() {
    return {
      bodyStyle: {
        padding: "4px",
        "background-color": "#f0f2f5",
      },
      organization: {
        style: {
          overflow: "auto",
          height: "300px",
        },
        data: [],
        spinning: true,
      },
      user: {
        org: "",
        data: [],
        key: "",
        orgId: "",
        spinning: false,
      },
      loading: false,
      checkUser: null,
    };
  },
  computed: {
    users() {
      var arr = [];
      this.user.data.forEach((item) => arr.push(item));
      if (this.user.key) {
        arr = this.user.data.filter((item) =>
          item.name.includes(this.user.key)
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
    title: {
      type: String,
    },
    weChatUserId: {
      type: String,
    },
    userId: {
      type: String,
    },
  },
  mounted() {
    this.userOrganization();
    this.userInit();
  },
  methods: {
    /**
     * 树图标
     */
    organizationIcon(props) {
      const { expanded } = props;
      if (props.children.length == 0) {
        return <a-icon type="file" />;
      }
      return <a-icon type={expanded ? "folder-open" : "folder"} />;
    },

    /**
     * 树选中
     */
    organizationSelect(electedKeys, e) {
      this.user.orgId = electedKeys[0];
      this.user.org = e.selectedNodes[0].title;
    },

    /**
     * 取消
     */
    cancel() {
      this.$emit("update:visible", false);
    },

    /**
     * 初始化组织架构
     */
    userOrganization() {
      let that = this;
      that.organization.spinning = true;
      queryBindOrganization().then((result) => {
        that.organization.data = result.data;
        that.organization.spinning = false;
      });
    },

    /**
     * 初始化人员
     */
    userInit() {
      let that = this;
      that.user.spinning = true;
      queryBindUser({ userId: this.userId }).then((result) => {
        that.user.data = result.data;
        that.user.spinning = false;
        let exist = that.user.data.filter((f) => f.exist);
        if (exist.length > 0) {
          that.checkUser = exist[0];
        }
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
     * 用户选择
     */
    userCheck(user) {
      if (!(this.checkUser && this.checkUser.userId == user.userId)) {
        this.user.data.forEach((item) => {
          item.exist = false;
        });
      }
      user.exist = !user.exist;
      this.checkUser = user;
    },

    /**
     * 保存
     */
    save() {
      let that = this;
      let exist = this.user.data.filter((f) => f.exist);
      that.loading = true;
      save({
        weChatUserId: this.weChatUserId,
        userId: exist.length > 0 ? exist[0].userId : null,
      }).then((result) => {
        if (result.code === that.eipResultCode.success) {
          that.$message.success(result.msg);
          that.$emit("save", result);
          that.cancel();
        } else {
          that.$message.error(result.msg);
        }
        that.loading = false;
      });
    },
  },
};
</script>

<style lang="less" scoped>
/*自定义复选框*/
.scrollBar {
  overflow: auto;
  height: 300px;
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
