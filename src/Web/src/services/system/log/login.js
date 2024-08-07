import {
    SystemLoginLogQuery,
    SystemLoginLogAnalysis
} from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 登录日志
 */
export async function query(param) {
    return request(SystemLoginLogQuery, METHOD.POST, param)
}

/**
 * 登录日志分析
 */
export async function analysis(param) {
    return request(SystemLoginLogAnalysis, METHOD.POST, param)
}


export default {
    query,
    analysis
}