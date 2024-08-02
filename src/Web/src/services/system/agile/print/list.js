import {
    SystemAgileIsFreeze,
    SystemAgileQuery,
    SystemAgileDelete,
} from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 列表
 */
export function query(param) {
    return request(SystemAgileQuery, METHOD.POST, param)
}

/**
 * 删除
 */
export function del(param) {
    return request(SystemAgileDelete, METHOD.POST, param)
}
/**
 * 
 */
export function isFreeze(param) {
    return request(SystemAgileIsFreeze, METHOD.POST, param)
}
export default {
    query,
    del,
    isFreeze
}