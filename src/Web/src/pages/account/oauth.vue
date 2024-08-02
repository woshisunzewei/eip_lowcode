<template>
  <div>
    <div v-if="!loginError" id="loading-mask">
      <div class="loading-wrapper">
        <span class="loading-dot loading-dot-spin"
          ><i></i><i></i><i></i><i></i
        ></span>
        <div class="loading-tips">登录跳转中,请稍等...</div>
      </div>
    </div>
    <a-result v-if="loginError" status="error" :title="loginErrorMsg">
      <template #extra>
        <a-button
          v-if="authSource != 'DINGTALK_SCAN'"
          @click="goLogin"
          key="console"
          type="primary"
        >
          重新登录
        </a-button>
      </template>
    </a-result>
  </div>
</template>

<script>
import { loginByOauthCalback, menu } from "@/services/account/oauth";
import { setAuthorization } from "@/utils/request";
import { mapMutations } from "vuex";
import { loadRoutes } from "@/utils/routerUtil";
import { newGuid, getBrowser } from "@/utils/util";

export default {
  name: "oauth",
  data() {
    return {
      loginId: newGuid(),
      loginError: false,
      loginErrorMsg: "",
      authSource: "",
    };
  },

  created() {
    this.initConfig();
  },

  methods: {
    ...mapMutations("account", ["setUser"]),

    /**
     *
     */
    goLogin() {
      this.$router.push("/login");
    },

    /**
     * 获取系统配置
     */
    initConfig() {
      const route = this.$route;
      document.title = "登录跳转中,请稍等...";
      var that = this;
      //根据代码登录
      this.authSource = route.query.authSource;
      loginByOauthCalback({
        appid: route.query.appid,
        authSource: route.query.authSource,
        code: route.query.code,
        state: route.query.state,
        userAgent: navigator.userAgent,
        browser: getBrowser(),
      })
        .then(that.afterLogin)
        .catch((error) => {
          that.$message.error("访问异常", 3);
          that.loginError = true;
        });
    },

    /**
     * 登录成功
     * @param {} result
     */
    afterLogin(result) {
      const loginRes = result;
      let that = this;
      const route = this.$route;
      if (loginRes.code === this.eipResultCode.success) {
        //获取菜单
        var routesConfig = [
          {
            router: route.query.openType ? route.query.openType : "root",
            children: [],
          },
        ];
        this.setUser(loginRes.data);
        setAuthorization({
          token: loginRes.data.token,
          expire: new Date(loginRes.data.expire),
        });

        //注册路由
        menu().then((result) => {
          if (result.length == 0) {
            this.logging = false;
            that.$message.error("权限不足,请管理员赋予权限后重试");
          } else {
            result.forEach((element) => {
              routesConfig[0].children.push(element);
            });
            loadRoutes(routesConfig);
            var returnUrl = route.query.returnUrl;
            if (returnUrl) {
              that.$router.push(returnUrl);
            } else {
              that.routerGo(routesConfig);
            }
            that.$message.success(loginRes.msg, 3);
          }
        });
      } else {
        this.loginError = true;
        that.$message.error(loginRes.msg);
      }
    },

    /**
     *
     *
     */
    routerGo(routesConfig) {
      var routes = [];
      // 递归遍历控件树
      const traverse = (array) => {
        array.forEach((element) => {
          if (element.path) {
            routes.push(element);
          } else {
            // 栅格布局 and 标签页
            element.children.forEach((item) => {
              if (item.path) {
                routes.push(item);
              } else {
                traverse(item.children);
              }
            });
          }
        });
      };
      traverse(routesConfig);
      this.$router.push(routes[0].path);
    },
  },
};
</script>

<style lang="less" scoped>
#loading-mask {
  position: fixed;
  left: 0;
  top: 0;
  height: 100%;
  width: 100%;
  user-select: none;
  z-index: 9999;
  overflow: hidden;
  background: radial-gradient(circle at 1px 1px, #e4e4e4 1px, #fdfdfd 0);
  background-size: 16px 16px;
}

.loading-wrapper {
  position: absolute;
  top: 50%;
  left: 50%;
  text-align: center;
  transform: translate(-50%, -100%);
}

.loading-tips {
  margin-top: 10px;
  color: #595959;
  font-weight: 400;
  font-size: 14px;
}

.loading-dot {
  animation: antRotate 1.2s infinite linear;
  transform: rotate(45deg);
  position: relative;
  display: inline-block;
  font-size: 40px;
  width: 40px;
  height: 40px;
  box-sizing: border-box;
}

.loading-dot i {
  width: 20px;
  height: 20px;
  position: absolute;
  display: block;
  background-color: #1890ff;
  border-radius: 100%;
  transform: scale(0.75);
  transform-origin: 50% 50%;
  opacity: 0.3;
  animation: antSpinMove 1s infinite linear alternate;
}

.loading-dot i:nth-child(1) {
  top: 0;
  left: 0;
}

.loading-dot i:nth-child(2) {
  top: 0;
  right: 0;
  -webkit-animation-delay: 0.4s;
  animation-delay: 0.4s;
}

.loading-dot i:nth-child(3) {
  right: 0;
  bottom: 0;
  -webkit-animation-delay: 0.8s;
  animation-delay: 0.8s;
}

.loading-dot i:nth-child(4) {
  bottom: 0;
  left: 0;
  -webkit-animation-delay: 1.2s;
  animation-delay: 1.2s;
}

@keyframes antRotate {
  to {
    -webkit-transform: rotate(405deg);
    transform: rotate(405deg);
  }
}

@-webkit-keyframes antRotate {
  to {
    -webkit-transform: rotate(405deg);
    transform: rotate(405deg);
  }
}

@keyframes antSpinMove {
  to {
    opacity: 1;
  }
}

@-webkit-keyframes antSpinMove {
  to {
    opacity: 1;
  }
}
</style>
