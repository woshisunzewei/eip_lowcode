import { SystemRateLimitLogQuery } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 获取所有限流日志
 */
export async function query(param) {
    return request(SystemRateLimitLogQuery, METHOD.POST, param)
}

export default {
    query
}