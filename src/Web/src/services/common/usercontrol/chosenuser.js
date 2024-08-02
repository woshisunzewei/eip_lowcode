import { SystemUserTopOrg, SystemOrganizationTopOrg } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 组织架构
 */
export function organization(param) {
    return request(SystemOrganizationTopOrg, METHOD.POST, param)
}

/**
 * 人
 */
export function user(param) {
    return request(SystemUserTopOrg, METHOD.POST, param)
}

export default {
    organization,
    user
}