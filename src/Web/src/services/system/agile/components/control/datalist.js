import { SystemDataBaseBusinessDataFindFormSourcePaging } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 查询
 */
export function query(form) {
    return request(SystemDataBaseBusinessDataFindFormSourcePaging, METHOD.POST, form)
}

export default {
    query
}