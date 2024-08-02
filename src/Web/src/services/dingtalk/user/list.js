import {
    DingTalkDepartmentTree,
    DingTalkUserQuery,
    DingTalkUserAsync,
    DingTalkUserAsyncToSystem,
    DingTalkUserTransfer,
    DingTalkUserBind,
    DingTalkUserBindUserId,
} from '@/services/api'
import {
    request,
    METHOD
} from '@/utils/request'

/**
 * 列表
 */
export function query(param) {
    return request(DingTalkUserQuery, METHOD.POST, param)
}
/**
 * 树
 */
export function tree() {
    return request(DingTalkDepartmentTree, METHOD.POST)
}
/**
 * 同步
 */
export function async(param) {
    return request(DingTalkUserAsync, METHOD.POST, param, {
        timeout: 9999999999999
    })
}

/**
 * 同步到系统
 */
export function asyncToSystem(param) {
    return request(DingTalkUserAsyncToSystem, METHOD.POST, param, {
        timeout: 9999999999999
    })
}
/**
 * 绑定用户
 */
export function bind(param) {
    return request(DingTalkUserBind, METHOD.POST, param, {
        timeout: 9999999999999
    })
}

/**
 * 绑定用户Id
 */
export function bindUserId(param) {
    return request(DingTalkUserBindUserId, METHOD.POST, param)
}

/**
 * 同步
 */
export function transfer(param) {
    return request(DingTalkUserTransfer, METHOD.POST, param)
}
export default {
    query,
    async,
    asyncToSystem,
    transfer,
    tree,
    bind,
    bindUserId
}