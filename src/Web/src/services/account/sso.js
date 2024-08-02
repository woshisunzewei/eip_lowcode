import {
    SystemLoginByUserId,
    SystemLoginByCode,
    SystemUserMenu,
    SystemConfigLogin
} from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 登录服务
 */
export async function loginByUserId(param) {
    return request(SystemLoginByUserId, METHOD.POST, param)
}

/**
 * 登录服务
 */
export async function loginByCode(param) {
    return request(SystemLoginByCode, METHOD.POST, param)
}

/**
 * 菜单
 */
export async function menu() {
    return request(SystemUserMenu, METHOD.POST, {})
}

export default {
    loginByUserId,
    loginByCode,
    menu,
}