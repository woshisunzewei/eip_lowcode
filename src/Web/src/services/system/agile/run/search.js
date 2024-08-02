import {
    SystemSearchQuery,
    SystemSearchDelete,
    SystemSearchSave,
    SystemSearchFindById,
} from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 列表
 */
export function query(param) {
    return request(SystemSearchQuery, METHOD.POST, param)
}

/**
 * 删除
 */
export async function del(param) {
    return request(SystemSearchDelete, METHOD.POST, param)
}

/**
 * 
 * @param {*} form 
 * @returns 
 */
export async function save(form) {
    return request(SystemSearchSave, METHOD.POST, form)
}

/**
 * 根据Id获取
 */
export function findById(id) {
    return request(SystemSearchFindById + "/" + id, METHOD.GET, {})
}
export default {
    query,
    del,
    save,
    findById
}