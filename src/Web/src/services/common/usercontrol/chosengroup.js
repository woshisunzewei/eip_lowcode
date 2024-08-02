import { SystemGroupAll, SystemOrganizationTopOrg } from '@/services/api'
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
export function group() {
    return request(SystemGroupAll, METHOD.GET, {})
}

export default {
    organization,
    group
}