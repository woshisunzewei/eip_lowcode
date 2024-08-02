<template>
  <div></div>
</template>

<script>
import { loginByUserId, menu } from "@/services/account/sso";
import { setAuthorization } from "@/utils/request";
import { mapMutations, mapGetters } from "vuex";
import { loadRoutes } from "@/utils/routerUtil";
import { newGuid, getBrowser } from "@/utils/util";

export default {
  name: "sso-userid",
  data() {
    return {
      logging: false,
      loginId: newGuid(),
      optenType: "root", //默认框架打开,若非框架打开传递rootblock
    };
  },
  created() {
    this.initConfig();
  },

  methods: {
    ...mapMutations("account", ["setUser"]),

    /**
     * 获取系统配置
     */
    async initConfig() {
      const route = this.$route;
      document.title = "跳转中...";
      var that = this;
      that.$loading.show({ text: "跳转中..." });
      that.logging = true;
      //根据用户Id登录
      loginByUserId({
        loginId: that.loginId,
        userId: route.query.userId,
        userAgent: navigator.userAgent,
        browser: getBrowser(),
      })
        .then(that.afterLogin)
        .catch((error) => {
          that.$message.error("访问异常", 3);
          that.logging = false;
        });
    },
  },

  /**
   * 登录成功
   * @param {} result
   */
  afterLogin(result) {
    const loginRes = result;
    let that = this;
    if (loginRes.code === this.eipResultCode.success) {
      const route = this.$route;
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
      that.$message.error(loginRes.msg);
    }
    this.logging = false;
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
};
</script>
