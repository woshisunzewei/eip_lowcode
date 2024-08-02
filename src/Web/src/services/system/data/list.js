import { SystemMenuPermission, SystemDataQuery, SystemDataDelete, SystemDataIsFreeze } from '@/services/api'
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
    return request(SystemDataQuery, METHOD.POST, param)
}

/**
 * 删除
 */
export async function del(param) {
    return request(SystemDataDelete, METHOD.POST, param)
}
/**
 * 
 */
export function isFreeze(param) {
    return request(SystemDataIsFreeze, METHOD.POST, param)
}
export default {
    menuQuery,
    query,
    del,
    isFreeze
}