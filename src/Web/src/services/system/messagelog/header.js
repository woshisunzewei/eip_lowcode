import {
    SystemMessageLogQuery,
    SystemMessageLogFindById,
    SystemMessageLogRead,
    SystemUserMenuAgile
} from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 列表
 */
export function query(param) {
    return request(SystemMessageLogQuery, METHOD.POST, param)
}
/**
 * 根据Id获取
 */
export function findById(id) {
    return request(SystemMessageLogFindById + "/" + id, METHOD.GET, {})
}

/**
 * 阅读
 */
export function read(param) {
    return request(SystemMessageLogRead, METHOD.POST, param)
}

/**
 * 
 */
export function menuAgile() {
    return request(SystemUserMenuAgile, METHOD.POST, {})
}
export default {
    query,
    findById,
    read,
    menuAgile
}