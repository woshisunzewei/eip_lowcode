import { SystemAgileSave, SystemDataBaseIsTableExist, SystemDataBaseSaveFormTable, SystemMenuSave, SystemMenuFindById } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 保存
 */
export async function agileSave(form) {
    return request(SystemAgileSave, METHOD.POST, form)
}


/**
 * 表是否存在
 */
export function tableExist(param) {
    return request(SystemDataBaseIsTableExist, METHOD.POST, param)
}

/**
 * 创建表
 */
export function table(param) {
    return request(SystemDataBaseSaveFormTable, METHOD.POST, param)
}


/**
 * 根据Id获取
 */
export function findById(id) {
    return request(SystemMenuFindById + "/" + id, METHOD.GET, {})
}

/**
 * 保存
 */
export async function save(form) {
    return request(SystemMenuSave, METHOD.POST, form)
}
export default {
    save,
    findById,
    agileSave,
    tableExist,
    findById
}