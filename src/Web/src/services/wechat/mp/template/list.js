import { WeChatMpTemplateQuery, WeChatMpTemplateDelete } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 列表
 */
export function query(param) {
    return request(WeChatMpTemplateQuery, METHOD.POST, param)
}

/**
 * 删除
 */
export function del(param) {
    return request(WeChatMpTemplateDelete, METHOD.POST, param)
}
export default {
    query,
    del
}