import { WeChatUserQuery, WeChatUserUnBind } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 列表
 */
export function query(param) {
    return request(WeChatUserQuery, METHOD.POST, param)
}

/**
 * 解绑
 */
export async function unbind(param) {
    return request(WeChatUserUnBind, METHOD.POST, param)
}

export default {
    query,
    unbind
}