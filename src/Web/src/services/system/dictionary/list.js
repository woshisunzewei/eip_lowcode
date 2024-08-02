import { SystemDictionary, SystemDictionaryQuery, SystemDictionaryDelete, SystemDictionaryIsFreeze } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 树
 */
export function dictionaryQuery() {
    return request(SystemDictionary, METHOD.GET, {})
}

/**
 * 列表
 */
export function query(param) {
    return request(SystemDictionaryQuery, METHOD.POST, param)
}

/**
 * 删除
 */
export async function del(param) {
    return request(SystemDictionaryDelete, METHOD.POST, param)
}
/**
 * 
 */
export function isFreeze(param) {
    return request(SystemDictionaryIsFreeze, METHOD.POST, param)
}
export default {
    dictionaryQuery,
    query,
    del,
    isFreeze
}