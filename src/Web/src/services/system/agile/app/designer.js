import { SystemAgileFindByMenuId } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 根据Id获取
 */
export function findByMenuId(param) {
    return request(SystemAgileFindByMenuId, METHOD.POST, param)
}
export default {
    findByMenuId
}