import { SystemUserDetail } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 详情
 */
export function detail(id) {
    return request(SystemUserDetail + "/" + id, METHOD.GET, {})
}
export default {
    detail,
}