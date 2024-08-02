import { SystemNoticeQuery, SystemNoticeDelete } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 列表
 */
export function query(param) {
    return request(SystemNoticeQuery, METHOD.POST, param)
}

/**
 * 删除
 */
export function del(param) {
    return request(SystemNoticeDelete, METHOD.POST, param)
}

export default {
    query,
    del
}