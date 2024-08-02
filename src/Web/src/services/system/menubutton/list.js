import { SystemMenuButtonIsShowTable, SystemMenuPermission, SystemMenuButtonQuery, SystemMenuButtonDelete, SystemMenuButtonIsFreeze } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 树
 */
export function menuQuery(privilegeaccess) {
    return request(SystemMenuPermission + "/" + privilegeaccess, METHOD.GET, {})
}

/**
 * 列表
 */
export function query(param) {
    return request(SystemMenuButtonQuery, METHOD.POST, param)
}

/**
 * 删除
 */
export async function del(param) {
    return request(SystemMenuButtonDelete, METHOD.POST, param)
}
/**
 * 
 */
export function isFreeze(param) {
    return request(SystemMenuButtonIsFreeze, METHOD.POST, param)
}
/**
 * 
 */
export function isShowTable(param) {
    return request(SystemMenuButtonIsShowTable, METHOD.POST, param)
}
export default {
    menuQuery,
    query,
    del,
    isFreeze,
    isShowTable
}