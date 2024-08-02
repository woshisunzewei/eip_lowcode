import {
    SystemJobLogQuery,
} from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 日志
 */
export async function query(param) {
    return request(SystemJobLogQuery, METHOD.POST, param)
}

export default {
    query
}