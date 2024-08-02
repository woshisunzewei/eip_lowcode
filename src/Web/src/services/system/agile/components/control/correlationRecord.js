import { SystemDataBaseBusinessDataFindFormSourcePagingTable } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 查询
 */
export function query(form) {
    return request(SystemDataBaseBusinessDataFindFormSourcePagingTable, METHOD.POST, form)
}

export default {
    query
}