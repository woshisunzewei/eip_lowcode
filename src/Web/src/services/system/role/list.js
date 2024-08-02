import { SystemRoleOrganization, SystemRoleQuery, SystemRoleDelete, SystemRoleCopy, SystemRoleIsFreeze } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 组织架构
 */
export function organizationQuery() {
    return request(SystemRoleOrganization, METHOD.GET, {})
}

/**
 * 列表
 */
export function query(param) {
    return request(SystemRoleQuery, METHOD.POST, param)
}

/**
 * 删除
 */
export async function del(param) {
    return request(SystemRoleDelete, METHOD.POST, param)
}

/**
 * 复制
 */
export async function copy(param) {
    return request(SystemRoleCopy, METHOD.POST, param)
}
/**
 * 
 */
export function isFreeze(param) {
    return request(SystemRoleIsFreeze, METHOD.POST, param)
}
export default {
    organizationQuery,
    query,
    del,
    copy,
    isFreeze
}