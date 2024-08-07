import { SystemOperationLogQuery } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 获取所有异常日志
 */
export async function query(param) {
    return request(SystemOperationLogQuery, METHOD.POST, param)
}

export default {
    query
}