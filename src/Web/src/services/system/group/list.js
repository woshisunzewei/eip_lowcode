import { SystemGroupOrganization, SystemGroupQuery, SystemGroupDelete, SystemGroupIsFreeze } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 组织架构
 */
export function organizationQuery() {
    return request(SystemGroupOrganization, METHOD.GET, {})
}

/**
 * 列表
 */
export function query(param) {
    return request(SystemGroupQuery, METHOD.POST, param)
}

/**
 * 删除
 */
export async function del(param) {
    return request(SystemGroupDelete, METHOD.POST, param)
}
/**
 * 
 */
export function isFreeze(param) {
    return request(SystemGroupIsFreeze, METHOD.POST, param)
}
export default {
    organizationQuery,
    query,
    del,
    isFreeze
}