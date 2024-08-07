import {
    SystemTxtLogQuery,
    SystemTxtLogDetail
} from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 文本日志
 */
export async function query(param) {
    return request(SystemTxtLogQuery, METHOD.POST, param)
}

/**
 * 文本日志分析
 */
export async function detail(param) {
    return request(SystemTxtLogDetail, METHOD.POST, param)
}


export default {
    query,
    detail
}