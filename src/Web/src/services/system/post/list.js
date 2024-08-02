import { SystemPostOrganization, SystemPostQuery, SystemPostDelete, SystemPostIsFreeze } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 树
 */
export function organizationQuery() {
    return request(SystemPostOrganization, METHOD.GET, {})
}

/**
 * 列表
 */
export function query(param) {
    return request(SystemPostQuery, METHOD.POST, param)
}

/**
 * 删除
 */
export async function del(param) {
    return request(SystemPostDelete, METHOD.POST, param)
}
/**
 * 
 */
export function isFreeze(param) {
    return request(SystemPostIsFreeze, METHOD.POST, param)
}
export default {
    organizationQuery,
    query,
    del,
    isFreeze
}