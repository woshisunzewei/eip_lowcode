import { SystemExceptionLogById } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 根据Id获取
 */
export function findById(id) {
    return request(SystemExceptionLogById + "/" + id, METHOD.GET, {})
}
export default {
    findById
}