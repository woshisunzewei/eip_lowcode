import { SystemLogin, SystemLogout, SystemUserMenu, SystemCaptcha, SystemConfigLogin, SystemLoginOauth } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 登录服务
 * @param code 账户名
 * @param password 账户密码
 * @returns {Promise<AxiosResponse<T>>}
 */
export async function login(param) {
    return request(SystemLogin, METHOD.POST, param)
}

/**
 * 菜单
 */
export async function menu(param) {
    return request(SystemUserMenu, METHOD.POST, param)
}
/**
 * 验证码
 */
export async function captcha(param) {
    return request(SystemCaptcha + "/" + param.id, METHOD.GET, {}, { responseType: 'blob' })
}
/**
 * 登录
 */
export async function loginConfig() {
    return request(SystemConfigLogin, METHOD.GET, {})
}
/**
 * 退出登录
 */
export function logout() {
    return request(SystemLogout, METHOD.POST, {});
}

/**
 * 获取收取地址
 */
export async function loginOauth(param) {
    return request(SystemLoginOauth, METHOD.POST, param)
}

export default {
    captcha,
    login,
    logout,
    menu,
    loginConfig,
    loginOauth
}