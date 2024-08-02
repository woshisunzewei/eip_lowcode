<template>
  <div></div>
</template>

<script>
import * as dd from "dingtalk-jsapi";
import { menu, loginConfig, login } from "@/services/dingtalk/login";
import { setAuthorization } from "@/utils/request";
import { mapMutations } from "vuex";
import { loadRoutes } from "@/utils/routerUtil";
import { encryptedData, newGuid, getBrowser } from "@/utils/util";

export default {
  name: "dingTalk-login",
  data() {
    return {
      loginId: newGuid(),
      code: "",
    };
  },
  created() {
    const route = this.$route;
    this.code = route.query.code;
    this.initConfig();
  },

  methods: {
    ...mapMutations("account", ["setUser", "setLoginConfig"]),

    /**
     * 获取系统配置
     */
    async initConfig() {
      var that = this;
      document.title = "跳转中...";

      if (dd.env.platform == "notInDingTalk") {
        this.$router.push("/");
      } else {
        that.$loading.show({ text: "跳转中..." });
        loginConfig().then((result) => {
          if (result.code === that.eipResultCode.success) {
            let data = result.data;
            that.setLoginConfig(data);
            dd.ready(function () {
              const corpId = result.data.dingTalkCorpId;
              dd.runtime.permission.requestAuthCode({
                corpId, // 企业id
                onSuccess: async (info) => {
                  const code = encryptedData(info.code);
                  login({
                    loginId: that.loginId,
                    code,
                    userAgent: navigator.userAgent,
                    browser: getBrowser(),
                  })
                    .then(that.afterLogin)
                    .catch((error) => {
                      that.$loading.hide();
                      that.$message.error("访问异常", 3);
                    });
                },
                onFail: () => {},
              });
            });
          } else {
            that.$loading.hide();
            that.$message.error(result.msg);
          }
        });
      }
    },

    /**
     * 登录成功
     * @param {} result
     */
    async afterLogin(result) {
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
        menu({
          userId: "",
          configId: "",
          isShowMobile: false,
        }).then((result) => {
          that.$loading.hide();
          if (result.length == 0) {
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
          }
        });
      } else {
        that.$loading.hide();
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
