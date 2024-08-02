import { SystemOrganizationTreeRange } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 选择组织架构
 */
export function chosenOrganization(range) {
    return request(SystemOrganizationTreeRange + "/" + range, METHOD.GET, {})
}

export default {
    chosenOrganization
}