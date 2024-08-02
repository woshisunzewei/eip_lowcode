import {
    SystemLoginOauthCalback,
    SystemUserMenu,
} from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 登录
 */
export async function loginByOauthCalback(param) {
    return request(SystemLoginOauthCalback, METHOD.POST, param)
}

/**
 * 菜单
 */
export async function menu() {
    return request(SystemUserMenu, METHOD.POST, {})
}

export default {
    loginByOauthCalback,
    menu,
}