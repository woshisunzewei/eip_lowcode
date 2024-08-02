import {
    SystemPrefixQuery,
    SystemAgileSave,
    SystemAgileFindById,
    SystemDataBaseSaveFormTable
} from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 获取所有前缀
 */
export function systemPrefix(param) {
    return request(SystemPrefixQuery, METHOD.POST, param)
}

/**
 * 保存
 */
export async function save(form) {
    return request(SystemAgileSave, METHOD.POST, form)
}

/**
 * 根据Id获取
 */
export function findById(id) {
    return request(SystemAgileFindById + "/" + id, METHOD.GET, {})
}

/**
 * 创建表
 */
export function table(param) {
    return request(SystemDataBaseSaveFormTable, METHOD.POST, param)
}

export default {
    save,
    findById,
    table,
    systemPrefix
}