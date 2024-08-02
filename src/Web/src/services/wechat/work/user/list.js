import {
    WeChatWorkUserQuery,
    WeChatWorkUserAsync,
    WeChatWorkUserAsyncToSystem,
    WeChatWorkUserBindUserId
} from '@/services/api'
import {
    request,
    METHOD
} from '@/utils/request'

/**
 * 列表
 */
export function query(param) {
    return request(WeChatWorkUserQuery, METHOD.POST, param)
}

/**
 * 同步
 */
export function async() {
    return request(WeChatWorkUserAsync, METHOD.POST)
}
/**
 * 绑定用户Id
 */
export function bindUserId(param) {
    return request(WeChatWorkUserBindUserId, METHOD.POST, param)
}
/**
 * 同步到系统
 */
export function asyncToSystem() {
    return request(WeChatWorkUserAsyncToSystem, METHOD.POST)
}
export default {
    query,
    async,
    asyncToSystem,
    bindUserId
}