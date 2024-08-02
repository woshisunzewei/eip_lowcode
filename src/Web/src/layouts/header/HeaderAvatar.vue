<template>
  <div>
    <a-dropdown>
      <div class="header-avatar" style="cursor: pointer">
        <img
          style="
            width: 32px;
            height: 32px;
            border-radius: 50%;
            margin-right: 10px;
            align-self: center;
          "
          v-real-img="user.headImage"
        />
        <span class="name">{{ user.name }}</span>
      </div>
      <a-menu :class="['avatar-menu']" slot="overlay">
        <a-menu-item @click="editSecret">
          <a-icon type="key" />
          <span>修改密码</span>
        </a-menu-item>
        <a-menu-item @click="avatarEdit">
          <a-icon type="user" />
          <span>修改头像</span>
        </a-menu-item>
        <a-menu-divider />
        <a-menu-item @click="logout">
          <a-icon style="margin-right: 8px" type="poweroff" />
          <span>退出登录</span>
        </a-menu-item>
      </a-menu>
    </a-dropdown>
    <changePassword
      ref="changePassword"
      :passwordRule="changePassword.rule"
      v-if="changePassword.visible"
      :visible.sync="changePassword.visible"
      :title="changePassword.title"
      @ok="editSecretOk"
    >
    </changePassword>
    <avatar
      ref="avatar"
      v-if="avatar.visible"
      :visible.sync="avatar.visible"
      :headImage="headImage"
      :title="avatar.title"
      @ok="avatarOk"
    ></avatar>
  </div>
</template>

<script>
import { mapMutations, mapGetters } from "vuex";
import changePassword from "@/pages/system/user/changePassword";
import avatar from "@/pages/system/user/avatar";
import { logout } from "@/services/account/login";
import { removeAuthorization } from "@/utils/request";
import { header } from "@/services/system/config/index";
export default {
  components: {
    changePassword,
    avatar,
  },
  name: "HeaderAvatar",
  computed: {
    ...mapGetters("account", ["user"]),
  },
  data() {
    return {
      changePassword: {
        visible: false,
        title: "修改密码",
        rule: [],
      },
      avatar: {
        visible: false,
        title: "",
      },
      headImage: null,

      setInterval: null, //定时器
      timeOut: null, // 设置超时时间: 10秒钟
    };
  },
  created() {
    this.userName = this.user.name;
    this.checkChangePassword();
    this.initConfig();
  },
  beforeDestroy() {
    if (this.setInterval) {
      clearInterval(this.setInterval);
    }
  },
  methods: {
    ...mapMutations("account", ["setUser"]),

    /**
     * 检查是否需要修改密码
     */
    checkChangePassword() {
      if (this.user.changePassword) {
        if (this.user.changePasswordType == 1) {
          this.changePassword.title = "首次登录-修改密码";
        } else if (this.user.changePasswordType == 2) {
          this.changePassword.title =
            "长时间未修改密码-为保证安全请重新修改密码";
        }
        this.changePassword.visible = true;
      }
    },

    /**
     * 退出
     */
    logout() {
      let that = this;
      that.$confirm({
        title: "提出提示?",
        content: "确定退出系统",
        okText: "确定",
        okType: "danger",
        cancelText: "取消",
        onOk() {
          that.$message.loading("退出中,请稍等...", 0);
          that.$emit("connectionClose");
          logout().then((result) => {
            that.$message.destroy();
            if (result.code == that.eipResultCode.success) {
              localStorage.removeItem(process.env.VUE_APP_ROUTES_KEY);
              localStorage.removeItem(process.env.VUE_APP_PERMISSIONS_KEY);
              localStorage.removeItem(process.env.VUE_APP_ROLES_KEY);
              removeAuthorization();
              that.$router.push("/login");
            } else {
              that.$message.error(result.msg);
            }
          });
        },
        onCancel() {},
      });
    },

    /**
     * 检测退出时间
     * @param {*} time
     */
    initCheckTimeOut(time) {
      this.timeOut = time * 60 * 1000;
      window.sessionStorage.setItem("lastTime", new Date().getTime());
    },

    /**
     * 定时检查
     */
    checkTimeout() {
      let that = this;
      let currentTime = new Date().getTime(); // 当前时间
      let lastTime = window.sessionStorage.getItem("lastTime"); //上次操作的时间
      if (currentTime - lastTime >= that.timeOut) {
        // 清除定时器
        window.clearInterval(this.setInterval);
        // 清除sessionStorage
        window.sessionStorage.clear("lastTime");
        that.lastTime = null;
        localStorage.removeItem(process.env.VUE_APP_ROUTES_KEY);
        localStorage.removeItem(process.env.VUE_APP_PERMISSIONS_KEY);
        localStorage.removeItem(process.env.VUE_APP_ROLES_KEY);
        removeAuthorization();
        // 跳到登陆页
        this.$error({
          keyboard: false,
          title: "下线提醒",
          content: "当前您长时间未操作,为安全考虑，请重新登录",
          okText: "确定",
          okType: "danger",
          onOk() {
            that.$router.push("/login");
          },
        });
      }
    },

    /**
     * 获取系统配置
     */
    initConfig() {
      let that = this;
      header().then((result) => {
        if (result.code === this.eipResultCode.success) {
          let data = result.data;
          //是否有时间
          if (data.systemMaxDoCheckTime) {
            that.initCheckTimeOut(data.systemMaxDoCheckTime);
            that.setInterval = window.setInterval(this.checkTimeout, 2000);
            let rule = [];
            if (data.systemPasswordLength) {
              rule.push(
                "密码长度需大于等于" + data.systemPasswordLength + "个字符串"
              );
            }

            if (
              data.systemPasswordHaveNumber &&
              data.systemPasswordHaveNumber == "true"
            ) {
              rule.push(
                "必须包含至少" + data.systemPasswordHaveNumberLength + "个数字"
              );
            }

            if (
              data.systemPasswordHaveLetters &&
              data.systemPasswordHaveLetters == "true"
            ) {
              rule.push(
                "必须包含至少" + data.systemPasswordHaveLettersLength + "个字母"
              );
            }

            if (
              data.systemPasswordHaveSpecialCharacters &&
              data.systemPasswordHaveSpecialCharacters == "true"
            ) {
              rule.push(
                "必须包含至少" +
                  data.systemPasswordHaveSpecialCharactersLength +
                  "个特殊字符"
              );
            }
            that.changePassword.rule = rule;
          }
        } else {
          that.$message.error(result.msg);
        }
      });
    },

    /**
     * 重置密码
     */
    editSecret() {
      this.changePassword.visible = true;
      this.changePassword.title = "修改密码-" + this.userName;
    },
    /**
     * 重置密码
     */
    editSecretOk() {
      this.user.changePassword = false;
      this.setUser(this.user);
    },

    /**
     * 编辑头像
     */
    avatarEdit() {
      this.headImage = !this.user.headImage
        ? require("@/assets/avatar.jpg")
        : this.user.headImage;
      this.avatar.visible = true;
      this.avatar.title = "修改头像-" + this.userName;
    },

    /**
     * 修改头像
     * @param {*} data
     */
    avatarOk(data) {
      this.user.headImage = data;
      this.setUser(this.user);
    },
  },
};
</script>

<style lang="less">
.header-avatar {
  display: inline-flex;

  .avatar,
  .name {
    align-self: center;
  }

  .avatar {
    margin-right: 8px;
  }

  .name {
    font-weight: 500;
  }
}

.avatar-menu {
  width: 150px;
}
</style>
