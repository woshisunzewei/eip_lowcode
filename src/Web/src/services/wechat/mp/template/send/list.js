import { WeChatMpTemplateSendQuery, WeChatMpTemplateSendDelete, WeChatMpTemplateSendSaveStart, WeChatMpTemplateSendPreview } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 列表
 */
export function query(id) {
    return request(WeChatMpTemplateSendQuery + "/" + id, METHOD.GET, {})
}
/**
 * 发送
 */
export function send(id) {
    return request(WeChatMpTemplateSendSaveStart + "/" + id, METHOD.POST, {}, { timeout: 9999999999999 })
}

/**
 * 发送
 */
export function sendpreview(param) {
    return request(WeChatMpTemplateSendPreview, METHOD.POST, param)
}
/**
 * 删除
 */
export function del(param) {
    return request(WeChatMpTemplateSendDelete, METHOD.POST, param)
}
export default {
    query,
    del,
    send
}