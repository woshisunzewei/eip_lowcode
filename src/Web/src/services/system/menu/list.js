import {
    SystemMenu,
    SystemMenuQuery,
    SystemMenuDelete,
    SystemMenuIsShowMenu,
    SystemMenuHaveMenuPermission,
    SystemMenuHaveDataPermission,
    SystemMenuHaveFieldPermission,
    SystemMenuHaveButtonPermission,
    SystemMenuIsFreeze
} from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 树
 */
export function menuQuery(menuId = null, isAgileMenu = null) {
    var url = SystemMenu;
    if (menuId) {
        url += "?menuId=" + menuId + "&isAgileMenu=" + isAgileMenu
    }
    return request(url, METHOD.GET, {})
}

/**
 * 列表
 */
export function query(param) {
    return request(SystemMenuQuery, METHOD.POST, param)
}

/**
 * 删除
 */
export async function del(param) {
    return request(SystemMenuDelete, METHOD.POST, param)
}
/**
 * 根据Id获取
 */
export function findById(param) {
    return request(SystemMenuFindById, METHOD.GET, param)
}
/**
 * 
 */
export function isShowMenu(param) {
    return request(SystemMenuIsShowMenu, METHOD.POST, param)
}
/**
 * 
 */
export function haveMenuPermission(param) {
    return request(SystemMenuHaveMenuPermission, METHOD.POST, param)
}
/**
 * 
 */
export function haveDataPermission(param) {
    return request(SystemMenuHaveDataPermission, METHOD.POST, param)
}
/**
 * 
 */
export function haveFieldPermission(param) {
    return request(SystemMenuHaveFieldPermission, METHOD.POST, param)
}
/**
 * 
 */
export function haveButtonPermission(param) {
    return request(SystemMenuHaveButtonPermission, METHOD.POST, param)
}
/**
 * 
 */
export function isFreeze(param) {
    return request(SystemMenuIsFreeze, METHOD.POST, param)
}
export default {
    menuQuery,
    query,
    del,
    isShowMenu,
    haveMenuPermission,
    haveDataPermission,
    haveFieldPermission,
    haveButtonPermission,
    isFreeze
}