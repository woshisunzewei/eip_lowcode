import {
    DingTalkDepartmentQuery,
    DingTalkDepartmentTree,
    DingTalkDepartmentAsync,
    DingTalkDepartmentAsyncToSystem
} from '@/services/api'
import {
    request,
    METHOD
} from '@/utils/request'


/**
 * 列表
 */
export function query(param) {
    return request(DingTalkDepartmentQuery, METHOD.POST, param)
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
export function async() {
    return request(DingTalkDepartmentAsync, METHOD.POST, {
        timeout: 9999999999999
    })
}

/**
 * 同步到系统
 */
export function asyncToSystem() {
    return request(DingTalkDepartmentAsyncToSystem, METHOD.POST, {
        timeout: 9999999999999
    })
}

export default {
    query,
    tree,
    async,
    asyncToSystem
}