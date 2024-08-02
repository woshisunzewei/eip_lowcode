<template>
  <div>
    <div class="login-tip">
      <div class="eip-text-left" style="padding: 10px">
        <div v-html="loginTip"></div>
      </div>
    </div>

    <div
      class="login-wrapper"
      :style="{ 'background-image': loginBg ? 'url(' + loginBg + ')' : '' }"
    >
      <form class="ant-form login-form eip-bg-white ant-form-vertical">
        <a-spin :spinning="loginSpinning">
          <h4>{{ loginSystemName }}</h4>

          <a-form
            v-if="loginTypeNow == 'password'"
            @submit="onSubmit"
            :form="form"
          >
            <a-form-item v-if="error">
              <div class="alert alert-error">
                <a-alert
                  type="error"
                  :closable="true"
                  :message="error"
                  showIcon
                />
              </div>
            </a-form-item>
            <a-form-item>
              <a-input
                autocomplete="autocomplete"
                size="large"
                allowClear
                placeholder="请输入账户名"
                v-decorator="[
                  'code',
                  {
                    rules: [
                      {
                        required: true,
                        message: '请输入账户名',
                        whitespace: true,
                      },
                    ],
                  },
                ]"
              >
                <a-icon slot="prefix" type="user" />
              </a-input>
            </a-form-item>
            <a-form-item>
              <a-input-password
                size="large"
                placeholder="请输入密码"
                autocomplete="autocomplete"
                v-decorator="[
                  'password',
                  {
                    rules: [
                      {
                        required: true,
                        message: '请输入密码',
                        whitespace: true,
                      },
                    ],
                  },
                ]"
              >
                <a-icon slot="prefix" type="lock" />
              </a-input-password>
            </a-form-item>
            <a-form-item v-if="loginCaptcha">
              <a-input
                autocomplete="autocomplete"
                size="large"
                allowClear
                placeholder="请输入验证码"
                v-decorator="[
                  'captcha',
                  {
                    rules: [
                      {
                        required: true,
                        message: '请输入验证码',
                        whitespace: true,
                      },
                    ],
                  },
                ]"
                ><a-icon slot="prefix" type="safety-certificate" />
                <div @click="captchaInterval()" slot="addonAfter">
                  <a-tooltip style="cursor: pointer">
                    <template slot="title">点击重新生成验证码</template>
                    <label v-if="!src">加载中...</label>
                    <img
                      v-else
                      :src="src"
                      alt="验证码"
                      width="110"
                      height="38"
                    />
                  </a-tooltip>
                </div>
              </a-input>
            </a-form-item>
            <a-form-item>
              <div>
                <a-checkbox v-model="loginSelf">自动登录</a-checkbox>
                <a style="float: right">忘记密码</a>
              </div>
            </a-form-item>

            <div class="enter">
              <a-form-item>
                <a-button
                  :loading="logging"
                  style="width: 100%"
                  size="large"
                  htmlType="submit"
                  type="primary"
                  >登录</a-button
                >
              </a-form-item>
            </div>
          </a-form>

          <a-form
            v-if="loginTypeNow == 'verificationCode'"
            @submit="onSubmit"
            :form="form"
          >
            <a-form-item>
              <a-input
                autocomplete="autocomplete"
                size="large"
                :maxLength="11"
                allowClear
                placeholder="请输入手机号"
                v-decorator="[
                  'code',
                  {
                    rules: [
                      {
                        required: true,
                        message: '请输入手机号',
                        whitespace: true,
                      },
                    ],
                  },
                ]"
              >
                <a-icon slot="prefix" type="phone" />
              </a-input>
            </a-form-item>
            <a-form-item>
              <a-input-search
                size="large"
                allowClear
                placeholder="请输入短信验证码"
                :maxLength="6"
                v-decorator="[
                  'smscode',
                  {
                    rules: [
                      {
                        required: true,
                        message: '请输入短信验证码',
                        whitespace: true,
                      },
                    ],
                  },
                ]"
              >
                <a-icon slot="prefix" type="mail" />
                <a-button slot="enterButton"> 发送验证码 </a-button>
              </a-input-search>
            </a-form-item>
            <a-form-item v-if="loginCaptcha">
              <a-input
                autocomplete="autocomplete"
                size="large"
                allowClear
                placeholder="请输入图片验证码"
                v-decorator="[
                  'captcha',
                  {
                    rules: [
                      {
                        required: true,
                        message: '请输入图片验证码',
                        whitespace: true,
                      },
                    ],
                  },
                ]"
                ><a-icon slot="prefix" type="safety-certificate" />
                <div @click="captchaInterval()" slot="addonAfter">
                  <a-tooltip style="cursor: pointer">
                    <template slot="title">点击重新生成验证码</template>
                    <label v-if="!src">加载中...</label>
                    <img
                      v-else
                      :src="src"
                      alt="验证码"
                      width="110"
                      height="38"
                    />
                  </a-tooltip>
                </div>
              </a-input>
            </a-form-item>
            <a-form-item>
              <div>
                <a-checkbox v-model="loginSelf">自动登录</a-checkbox>
                <a style="float: right">忘记密码</a>
              </div>
            </a-form-item>
            <div class="enter">
              <a-form-item>
                <a-button
                  :loading="logging"
                  style="width: 100%"
                  size="large"
                  htmlType="submit"
                  type="primary"
                  >登录</a-button
                >
              </a-form-item>
            </div>
          </a-form>

          <div v-if="loginTypeNow == 'wxWorkScan'" style="text-align: center">
            <iframe
              width="300"
              height="316"
              frameborder="no"
              border="0"
              scrolling="no"
              :src="wxWorkScan.src"
            ></iframe>
          </div>
          <div v-if="loginType.length > 1">
            <a-divider>其他登录方式</a-divider>
            <div class="flex align-center justify-around padding-sm">
              <a-icon
                title="帐号密码"
                @click="loginTypeChange('password')"
                v-if="loginType.includes('password')"
                type="lock"
                class="login-icon"
                :class="loginTypeNow == 'password' ? 'login-click' : ''"
              />

              <a-icon
                title="手机验证码"
                @click="loginTypeChange('verificationCode')"
                v-if="loginType.includes('verificationCode')"
                type="mobile"
                class="login-icon"
                :class="loginTypeNow == 'verificationCode' ? 'login-click' : ''"
              />

              <a-icon
                title="钉钉"
                @click="dingtalkScanClick"
                v-if="loginType.includes('dingtalkScan')"
                type="dingding"
                class="login-icon"
                :class="loginTypeNow == 'dingtalkScan' ? 'login-click' : ''"
              />

              <a-icon
                title="企业微信"
                @click="loginTypeChange('wxWorkScan')"
                v-if="loginType.includes('wxWorkScan')"
                class="login-icon"
                type="wechat"
                :class="loginTypeNow == 'wxWorkScan' ? 'login-click' : ''"
              />

              <a-icon
                title="支付宝"
                @click="loginTypeChange('alipayScan')"
                v-if="loginType.includes('alipayScan')"
                class="login-icon"
                type="alipay-circle"
                :class="loginTypeNow == 'alipayScan' ? 'login-click' : ''"
              />
            </div>
          </div>
        </a-spin>
      </form>
      <a-modal
        v-drag
        :zIndex="10000"
        :destroyOnClose="true"
        :maskClosable="false"
        :visible="dingtalkScan.visible"
        :footer="null"
        @cancel="dingtalkScan.visible = false"
      >
        <div style="text-align: center">
          <iframe
            width="400"
            height="400"
            frameborder="no"
            border="0"
            scrolling="no"
            :src="dingtalkScan.src"
          ></iframe>
        </div>
      </a-modal>
      <vue-particles
        v-if="loginParticles"
        color="#fff"
        :particleOpacity="1"
        :particlesNumber="80"
        shapeType="circle"
        :particleSize="2"
        linesColor="#fff"
        :linesWidth="1"
        :lineLinked="true"
        :lineOpacity="0.8"
        :linesDistance="150"
        :moveSpeed="3"
        :hoverEffect="true"
        hoverMode="grab"
        :clickEffect="true"
        clickMode="remove"
        class="lizi"
      ></vue-particles>
      <div class="login-copyright">{{ loginCopyright }}</div>
    </div>
  </div>
</template>

<script>
import Vue from "vue";
import VueParticles from "vue-particles";
Vue.use(VueParticles);
import {
  login,
  menu,
  captcha,
  loginConfig,
  loginOauth,
} from "@/services/account/login";
import { setAuthorization, setRefreshToken } from "@/utils/request";
import { mapMutations, mapGetters } from "vuex";
import { loadRoutes } from "@/utils/routerUtil";
import { encryptedData, newGuid, getBrowser } from "@/utils/util";
export default {
  name: "Login",

  data() {
    return {
      src: "",
      logging: false,
      error: "",
      form: this.$form.createForm(this),
      interval: null,
      loginId: newGuid(),
      loginSelf: false,

      loginSpinning: true,
      loginTitle: "",
      loginSystemName: "",
      loginType: [],
      loginTypeNow: "password", //当前登录方式
      loginCaptcha: false,
      loginParticles: false,
      loginSso: false,
      loginBg: "",
      loginTip: "",
      loginCopyright: "",

      dingtalkScan: {
        visible: false,
        src: "",
      },

      wxWorkScan: {
        src: "",
      },
    };
  },
  beforeDestroy() {
    if (this.interval) {
      clearInterval(this.interval);
    }
  },
  created() {
    this.initConfig();
  },
  computed: {
    systemName() {
      return this.$store.state.setting.systemName;
    },
    ...mapGetters("account", ["loginConfig"]),
  },
  methods: {
    ...mapMutations("account", ["setUser", "setLoginConfig"]),
    /**
     * 登录类型改变
     */
    loginTypeChange(type) {
      let that = this;
      if (this.loginTypeNow != type) {
        if (type == "wxWorkScan") {
          that.loginSpinning = true;
          loginOauth({ Source: "WECHAT_ENTERPRISE_SCAN" }).then((result) => {
            that.loginSpinning = false;
            that.wxWorkScan.src = result.data.url;
          });
        }
        this.loginTypeNow = type;
      }
    },

    /**
     * 钉钉扫码
     */
    dingtalkScanClick() {
      let that = this;
      this.dingtalkScan.visible = true;
      //重新加载
      that.loginSpinning = true;
      loginOauth({ Source: "DINGTALK_SCAN" }).then((result) => {
        that.loginSpinning = false;
        that.dingtalkScan.src = result.data.url;
      });
    },

    /**
     * 获取系统配置
     */
    async initConfig() {
      let that = this;
      that.loginSpinning = true;
      loginConfig().then((result) => {
        that.loginSpinning = false;
        if (result.code === that.eipResultCode.success) {
          let data = result.data;
          that.setLoginConfig(data);
          that.loginCaptcha = data.loginCaptcha == "true";
          that.loginParticles = data.loginParticles == "true";
          that.loginSystemName = data.loginSystemName;
          if (data.loginType) {
            var loginType = JSON.parse(data.loginType);
            if (loginType.length > 0) {
              that.loginTypeNow = loginType[0];
            }
            that.loginType = loginType;
          }
          that.loginTitle = data.loginTitle;
          that.loginSso = data.loginSso;
          that.loginBg = data.loginBg;
          that.loginTip = data.loginTip;
          that.loginCopyright = data.loginCopyright;
          document.title = data.loginTitle;

          if (that.loginCaptcha) {
            that.initCaptcha();
            if (that.interval) {
              clearInterval(this.interval);
            }
            that.interval = setInterval(function () {
              that.initCaptcha();
            }, 58000);
          }
        } else {
          that.$message.error(result.msg);
        }
      });
    },

    /**
     * 初始化验证码
     */
    initCaptcha() {
      let that = this;
      captcha({ id: this.loginId }).then((result) => {
        const myBlob = new window.Blob([result], { type: "image/gif" });
        const qrUrl = window.URL.createObjectURL(myBlob);
        that.src = qrUrl;
      });
    },

    /**
     * 提交
     * @param {} e
     */
    onSubmit(e) {
      var that = this;
      e.preventDefault();
      this.form.validateFields((err) => {
        if (!err) {
          that.logging = true;
          that.error = "";
          const captcha = this.loginCaptcha
            ? encryptedData(that.form.getFieldValue("captcha"))
            : "";
          const code = encryptedData(that.form.getFieldValue("code"));
          const password = encryptedData(that.form.getFieldValue("password"));
          login({
            loginId: that.loginId,
            code,
            password,
            userAgent: navigator.userAgent,
            browser: getBrowser(),
            captcha,
          })
            .then(that.afterLogin)
            .catch((error) => {
              that.$message.error("访问异常", 3);
              that.logging = false;
            });
        }
      });
    },

    /**
     * 登录成功
     * @param {} result
     */
    afterLogin(result) {
      const loginRes = result;
      let that = this;
      if (loginRes.code === this.eipResultCode.success) {
        //获取菜单
        var routesConfig = [
          {
            router: "root",
            children: [],
          },
        ];

        this.setUser(loginRes.data);

        //赋予token
        setAuthorization({
          token: loginRes.data.token,
          expire: new Date(loginRes.data.expire),
        });

        setRefreshToken({
          token: loginRes.data.refreshToken,
          expire: new Date(loginRes.data.expire),
        });

        //注册路由
        menu({}).then((result) => {
          if (result.length == 0) {
            this.logging = false;
            that.$message.error("权限不足,请管理员赋予权限后重试");
          } else {
            result.forEach((element) => {
              routesConfig[0].children.push(element);
            });
            loadRoutes(routesConfig);
            that.routerGo(routesConfig);
          }
        });
      } else {
        this.logging = false;
        this.error = loginRes.msg;
        this.captchaInterval();
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

    /**
     * 定时器
     */
    captchaInterval() {
      let that = this;
      that.initCaptcha();
      if (that.interval) {
        clearInterval(this.interval);
      }
      that.interval = setInterval(function () {
        that.initCaptcha();
      }, 58000);
    },
  },
};
</script>

<style lang="less" scoped>
.lizi {
  width: calc(100% - 100px);
  height: calc(100% - 100px);
  position: absolute;
}

.login-wrapper .anticon {
  cursor: pointer;
}

.login-wrapper .anticon:hover {
  color: @primary-color;
}

.login-wrapper:before {
  content: "";
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.2);
}

.login-wrapper {
  padding: 48px 16px 0 16px;
  position: relative;
  box-sizing: border-box;
  background-repeat: no-repeat;
  background-size: cover;
  min-height: 100vh;
}

.login-form {
  width: 360px;
  margin: 0 auto;
  max-width: 100%;
  padding: 0 28px;
  box-sizing: border-box;
  box-shadow: 0 3px 6px rgb(0 0 0 / 15%);
  border-radius: 2px;
  position: relative;
  z-index: 9999;
}

@media screen and (min-height: 640px) {
  .login-form {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translateX(-50%);
    margin-top: -230px;
  }
}

.eip-bg-white {
  background-color: #fff !important;
}

.login-form h4 {
  padding: 22px 0;
  text-align: center;
}

.login-copyright {
  color: #eee;
  text-align: center;
  padding: 48px 0 22px 0;
  position: relative;
  z-index: 1;
}

@media screen and (min-height: 640px) {
  .login-copyright {
    position: absolute;
    left: 0;
    right: 0;
    bottom: 0;
  }
}

h4 {
  font-size: 20px;
}

.eip-pull-right {
  float: right;
}

.eip-text-center {
  text-align: center;
}

/deep/ .ant-input-group-addon {
  padding: 0 !important;
}

.login-tip {
  position: absolute;
  top: 10px;
  left: 0;
  right: 0;
  z-index: 999;
}

.login-click {
  color: @primary-color;
}

.login-icon {
  font-size: 30px;
}
</style>
