import { SystemSmsLogQuery } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 获取所有短信日志
 */
export async function query(param) {
    return request(SystemSmsLogQuery, METHOD.POST, param)
}

export default {
    query
}