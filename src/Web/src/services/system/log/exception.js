import { SystemExceptionLogQuery } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 获取所有异常日志
 */
export async function query(param) {
    return request(SystemExceptionLogQuery, METHOD.POST, param)
}

export default {
    query
}