import { SystemDataBaseTableSpaceused } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 空间
 */
export function spaceused() {
    return request(SystemDataBaseTableSpaceused, METHOD.GET, {})
}

export default {
    spaceused
}