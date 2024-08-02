import {
    SystemMenuButtonSave,
    SystemAgileSave,
    SystemAgileSaveType,
    SystemAgilePublicJson,
    SystemDataBaseSaveFormTable,
    SystemAgileFindByMenuId,
    SystemPrefixQuery,
    SystemMenuSave,
    SystemMenuFindById,
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
export async function buttonSave(form) {
    return request(SystemMenuButtonSave, METHOD.POST, form)
}
/**
 * 保存
 */
export async function agileSave(form) {
    return request(SystemAgileSave, METHOD.POST, form)
}
/**
 * 保存
 */
export async function agileSaveType(form) {
    return request(SystemAgileSaveType, METHOD.POST, form)
}


/**
 * 创建表
 */
export function table(param) {
    return request(SystemDataBaseSaveFormTable, METHOD.POST, param)
}

/**
 * 发布
 */
export async function listPublic(form) {
    return request(SystemAgilePublicJson, METHOD.POST, form)
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
export async function save(param) {
    return request(SystemMenuSave, METHOD.POST, param)
}

/**
 * 根据Id获取
 */
export function findByMenuId(param) {
    return request(SystemAgileFindByMenuId, METHOD.POST, param)
}

export default {
    save,
    findById,
    agileSave,
    agileSaveType,
    listPublic,
    buttonSave,
    findByMenuId,
    systemPrefix
}