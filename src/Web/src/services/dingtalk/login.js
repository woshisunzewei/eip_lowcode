import { DingTalkLoginCode, SystemUserMenu, SystemConfigLogin } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 登录服务
 * @param code 账户名
 * @param password 账户密码
 * @returns {Promise<AxiosResponse<T>>}
 */
export async function login(param) {
    return request(DingTalkLoginCode, METHOD.POST, param)
}

/**
 * 菜单
 */
export async function menu(param) {
    return request(SystemUserMenu, METHOD.POST, param)
}

/**
 * 登录
 */
export async function loginConfig() {
    return request(SystemConfigLogin, METHOD.GET, {})
}

export default {
    login,
    menu,
    loginConfig
}