import {
    SystemMenuApp,
    SystemMenuDelete,
    SystemMenuIsShowMenu,
    SystemUserMenuAgile
} from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 树
 */
export function menuQuery() {
    return request(SystemMenuApp, METHOD.GET, {})
}
/**
 * 删除
 */
export async function menuDel(param) {
    return request(SystemMenuDelete, METHOD.POST, param)
}

/**
 * 
 */
export function menuShow(param) {
    return request(SystemMenuIsShowMenu, METHOD.POST, param)
}

/**
 * 
 */
export function menuAgile() {
    return request(SystemUserMenuAgile, METHOD.POST, {})
}
export default {
    menuQuery,
    menuDel,
    menuShow,
    menuAgile
}