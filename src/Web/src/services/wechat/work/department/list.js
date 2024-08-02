import { WeChatWorkDepartmentQuery, WeChatWorkDepartmentTree, WeChatWorkDepartmentAsync, WeChatWorkDepartmentAsyncToSystem } from '@/services/api'
import {
    request,
    METHOD
} from '@/utils/request'


/**
 * 列表
 */
export function query() {
    return request(WeChatWorkDepartmentQuery, METHOD.POST)
}

/**
 * 树
 */
export function tree() {
    return request(WeChatWorkDepartmentTree, METHOD.POST)
}

/**
 * 同步
 */
export function async() {
    return request(WeChatWorkDepartmentAsync, METHOD.POST)
}

/**
 * 同步到系统
 */
export function asyncToSystem() {
    return request(WeChatWorkDepartmentAsyncToSystem, METHOD.POST)
}

export default {
    query,
    tree,
    async,
    asyncToSystem
}