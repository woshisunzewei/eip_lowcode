import { SystemPermissionDetail } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 详情
 */
export function detail(id, access) {
    return request(SystemPermissionDetail + "/" + id + "/" + access, METHOD.GET, {})
}

export default {
    detail
}