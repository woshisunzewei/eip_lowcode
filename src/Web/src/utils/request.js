import axios from 'axios'
import Cookie from 'js-cookie'
import warterMark from "@/utils/warterMark.js"
// 跨域认证信息 header 名
const xsrfHeaderName = 'Authorization'

axios.defaults.timeout = 50000
axios.defaults.withCredentials = true
axios.defaults.xsrfHeaderName = xsrfHeaderName
axios.defaults.xsrfCookieName = xsrfHeaderName

// 认证类型
const AUTH_TYPE = {
    BEARER: 'Bearer',
    BASIC: 'basic',
    AUTH1: 'auth1',
    AUTH2: 'auth2',
}

// http method
const METHOD = {
    GET: 'get',
    POST: 'post',
    DELETE: 'delete',
    PUT: 'put'
}

/**
 * axios请求
 * @param url 请求地址
 * @param method {METHOD} http method
 * @param params 请求参数
 * @returns {Promise<AxiosResponse<T>>}
 */
function request(url, method, params, config) {
    switch (method) {
        case METHOD.GET:
            return axios.get(url, { params, ...config })
        case METHOD.POST:
            return axios.post(url, params, config)
        case METHOD.DELETE:
            return axios.delete(url, params, config)
        case METHOD.PUT:
            return axios.put(url, params, config)
        default:
            return axios.get(url, { params, ...config })
    }
}
/**
 * axios请求
 * @param url 请求地址
 * @param method {METHOD} http method
 * @param params 请求参数
 * @returns {Promise<AxiosResponse<T>>}
 */
async function requestSync(url, method, params, config) {
    switch (method) {
        case METHOD.GET:
            return await axios.get(url, { params, ...config })
        case METHOD.POST:
            return await axios.post(url, params, config)
        case METHOD.DELETE:
            return await axios.delete(url, params, config)
        case METHOD.PUT:
            return await axios.put(url, params, config)
        default:
            return await axios.get(url, { params, ...config })
    }
}

/**
 * axios请求
 * @param url 请求地址
 * @param params 请求参数
 * @returns {Promise<AxiosResponse<T>>}
 */
async function excelDownload(url, params = {}, filename) {
    return new Promise((resolve, reject) => {
        axios.post(url, params, { responseType: 'arraybuffer' }).then(res => {
            let blob = new Blob([res], { type: params.contentType ? params.contentType : "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
            var a = document.createElement('a');
            var aurl = window.URL.createObjectURL(blob);
            a.href = aurl;
            a.download = filename;
            a.click();
            resolve(a)
        }).catch(error => {
            reject(null, e);
        })
    })
}
/**
 * axios请求
 * @param url 请求地址
 * @param params 请求参数
 * @returns {Promise<AxiosResponse<T>>}
 */
async function wordDownload(url, params = {}, filename) {
    return new Promise((resolve, reject) => {
        axios.post(url, params, { responseType: 'arraybuffer' }).then(res => {
            let blob = new Blob([res], { type: "application/vnd.openxmlformats-officedocument.wordprocessingml.document" });
            var a = document.createElement('a');
            var aurl = window.URL.createObjectURL(blob);
            a.href = aurl;
            a.download = filename;
            a.click();
            resolve(a)
        }).catch(error => {
            reject(null, e);
        })
    })
}
/**
 * 设置认证信息
 * @param auth {Object}
 * @param authType {AUTH_TYPE} 认证类型，默认：{AUTH_TYPE.BEARER}
 */
function setAuthorization(auth, authType = AUTH_TYPE.BEARER) {
    switch (authType) {
        case AUTH_TYPE.BEARER:
            Cookie.set(xsrfHeaderName, 'Bearer ' + auth.token, {
                expires: auth.expire,
                domain: getCookieDomain()
            })
            break
        case AUTH_TYPE.BASIC:
        case AUTH_TYPE.AUTH1:
        case AUTH_TYPE.AUTH2:
        default:
            break
    }
}
/**
 * 设置认证信息
 * @param auth {Object}
 * @param authType {AUTH_TYPE} 认证类型，默认：{AUTH_TYPE.BEARER}
 */
function setRefreshToken(auth) {
    Cookie.set("RefreshToken", 'Bearer ' + auth.token, {
        domain: getCookieDomain()
    })
}

/**
 * 移出认证信息
 * @param authType {AUTH_TYPE} 认证类型
 */
function removeAuthorization(authType = AUTH_TYPE.BEARER) {
    switch (authType) {
        case AUTH_TYPE.BEARER:
            Cookie.remove(xsrfHeaderName, {
                domain: getCookieDomain()
            })
            break
        case AUTH_TYPE.BASIC:
        case AUTH_TYPE.AUTH1:
        case AUTH_TYPE.AUTH2:
        default:
            break
    }
    warterMark.set('')
}
/**
 * 获取认证信息
 * @param authType {AUTH_TYPE} 认证类型
 */
function getAuthorization(authType = AUTH_TYPE.BEARER) {
    switch (authType) {
        case AUTH_TYPE.BEARER:
            return Cookie.get(xsrfHeaderName)
        case AUTH_TYPE.BASIC:
        case AUTH_TYPE.AUTH1:
        case AUTH_TYPE.AUTH2:
        default:
            break
    }
}

/**
 * 获取cookie主域名
 * @returns 
 */
function getCookieDomain() {
    var host = location.hostname;
    var ip = /^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$/;
    if (ip.test(host) === true || host === 'localhost') return host;
    var regex = /([^]*).*/;
    var match = host.match(regex);
    if (typeof match !== "undefined" && null !== match) host = match[1];
    if (typeof host !== "undefined" && null !== host) {
        var strAry = host.split(".");
        if (strAry.length > 1) {
            host = strAry[strAry.length - 2] + "." + strAry[strAry.length - 1];
        }
    }
    return '.' + host;
}

/**
 * 加载 axios 拦截器
 * @param interceptors
 * @param options
 */
function loadInterceptors(interceptors, options) {
    const { request, response } = interceptors
    // 加载请求拦截器
    request.forEach(item => {
            let { onFulfilled, onRejected } = item
            if (!onFulfilled || typeof onFulfilled !== 'function') {
                onFulfilled = config => config
            }
            if (!onRejected || typeof onRejected !== 'function') {
                onRejected = error => Promise.reject(error)
            }
            axios.interceptors.request.use(
                config => onFulfilled(config, options),
                error => onRejected(error, options)
            )
        })
        // 加载响应拦截器
    response.forEach(item => {
        let { onFulfilled, onRejected } = item
        if (!onFulfilled || typeof onFulfilled !== 'function') {
            onFulfilled = response => response
        }
        if (!onRejected || typeof onRejected !== 'function') {
            onRejected = error => Promise.reject(error)
        }
        axios.interceptors.response.use(
            response => onFulfilled(response, options),
            error => onRejected(error, options)
        )
    })
}

/**
 * 解析 url 中的参数
 * @param url
 * @returns {Object}
 */
function parseUrlParams(url) {
    const params = {}
    if (!url || url === '' || typeof url !== 'string') {
        return params
    }
    const paramsStr = url.split('?')[1]
    if (!paramsStr) {
        return params
    }
    const paramsArr = paramsStr.replace(/&|=/g, ' ').split(' ')
    for (let i = 0; i < paramsArr.length / 2; i++) {
        const value = paramsArr[i * 2 + 1]
        params[paramsArr[i * 2]] = value === 'true' ? true : (value === 'false' ? false : value)
    }
    return params
}

export {
    METHOD,
    AUTH_TYPE,
    request,
    requestSync,
    excelDownload,
    getAuthorization,
    setAuthorization,
    setRefreshToken,
    removeAuthorization,
    loadInterceptors,
    parseUrlParams
}