import Cookie from "js-cookie";
import {
  setAuthorization,
  setRefreshToken,
  removeAuthorization,
  getAuthorization,
} from "@/utils/request";
import { refreshToken } from "@/services/system/config/index";
import axios from "axios";
/**
 * 获取cookie主域名
 * @returns
 */
function getCookieDomain() {
  var host = location.hostname;
  var ip =
    /^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$/;
  if (ip.test(host) === true || host === "localhost") return host;
  var regex = /([^]*).*/;
  var match = host.match(regex);
  if (typeof match !== "undefined" && null !== match) host = match[1];
  if (typeof host !== "undefined" && null !== host) {
    var strAry = host.split(".");
    if (strAry.length > 1) {
      host = strAry[strAry.length - 2] + "." + strAry[strAry.length - 1];
    }
  }
  return "." + host;
}

function goLogin() {
  localStorage.removeItem(process.env.VUE_APP_ROUTES_KEY);
  localStorage.removeItem(process.env.VUE_APP_PERMISSIONS_KEY);
  localStorage.removeItem(process.env.VUE_APP_ROLES_KEY);
  removeAuthorization();
  window.location.href = "/login";
}

let isRefreshing = false;
let requestsList = [];
// 响应拦截
const resCommon = {
  /**
   * 响应数据之前做点什么
   * @param response 响应对象
   * @param options 应用配置 包含: {router, i18n, store, message}
   * @returns {*}
   */
  onFulfilled(response, options) {
    const { message } = options;
    if (response.code === 401) {
      message.error("登录状态失效,请重新登录");
      // this.$router.push("/login");
    }

    if (response.code === 403) {
      message.error("请求被拒绝");
    }

    return response.data;
  },

  /**
   * 响应出错时执行
   * @param error 错误对象
   * @param options 应用配置 包含: {router, i18n, store, message}
   * @returns {Promise<never>}
   */
  onRejected(error, options) {
    const { message } = options;
    const { response } = error;

    if (!response) {
      message.error("请求超时");
      return { code: -1, msg: "请求错误,请重试!" };
    }
    if (response.status === 401) {
      var refreshTokenValue = Cookie.get("RefreshToken", {
        domain: getCookieDomain(),
      });
      if (
        response.config.url.indexOf("/authorize/account/refresh/token") > -1
      ) {
        message.error("认证失效,请重新登录");
        goLogin();
      } else {
        if (!isRefreshing) {
          isRefreshing = true;
          //调接口，刷新token
          return refreshToken(refreshTokenValue)
            .then((result) => {
              if (result.code == 200) {
                setAuthorization({
                  token: result.data.token,
                  expire: new Date(result.data.expire),
                });

                setRefreshToken({
                  token: result.data.refreshToken,
                  expire: new Date(result.data.expire),
                });

                error.config.headers.Authorization =
                  "Bearer " + result.data.token;
                error.config.headers.withCredentials = true;
                requestsList.length > 0 &&
                  requestsList.map((cb) => {
                    cb();
                  });
                requestsList = []; // 注意要清空
                return axios(error.config);
              } else {
                // 刷新失败 退出登录
                message.error("认证失效,请重新登录");
                goLogin();
              }
            })
            .catch((err) => {
              message.error("认证失效,请重新登录");
              goLogin();
              return err;
            })
            .finally(() => {
              isRefreshing = false;
            });
        } else {
          // 正在刷新token ,把后来的接口缓冲起来
          return new Promise((resolve) => {
            requestsList.push(() => {
              error.config.headers.Authorization = getAuthorization();
              error.config.headers.withCredentials = true;
              resolve(axios(error.config));
            });
          });
        }
      }
    }
    if (response.status === 429) {
      message.error(response.data.msg);
      return { code: -1, msg: "请求频率过快!" };
    }
    if (response.status === 403) {
      message.error("请求被拒绝");
    }
    return Promise.reject(error);
  },
};

// 请求拦截
const reqCommon = {
  /**
   * 发送请求之前做些什么
   * @param config axios config
   * @param options 应用配置 包含: {router, i18n, store, message}
   * @returns {*}
   */
  onFulfilled(config, options) {
    const { message } = options;
    const { url, xsrfCookieName } = config;
    // if (url.indexOf('login') === -1 && url.indexOf('account/captcha') === -1 && xsrfCookieName && !Cookie.get(xsrfCookieName, {
    //         domain: getCookieDomain()
    //     })) {
    //     message.warning('登录认证已过期，请重新登录')
    // }
    return config;
  },
  /**
   * 请求出错时做点什么
   * @param error 错误对象
   * @param options 应用配置 包含: {router, i18n, store, message}
   * @returns {Promise<never>}
   */
  onRejected(error, options) {
    const { message } = options;
    message.error(error.message);
    return Promise.reject(error);
  },
};

export default {
  request: [reqCommon], // 请求拦截
  response: [resCommon], // 响应拦截
};
