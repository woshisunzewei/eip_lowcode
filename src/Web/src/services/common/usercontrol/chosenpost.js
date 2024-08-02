import { SystemPostAll, SystemOrganizationTopOrg } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 组织架构
 */
export function organization(param) {
    return request(SystemOrganizationTopOrg, METHOD.POST, param)
}

/**
 * 角色
 */
export function post() {
    return request(SystemPostAll, METHOD.GET, {})
}

export default {
    organization,
    post
}