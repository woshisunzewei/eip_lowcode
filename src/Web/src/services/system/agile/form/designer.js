import {
    SystemAgileSaveJson,
    SystemAgileAutomationTable,
    SystemAgilePublicJson,
    SystemAgileFindById,
    SystemDataBaseSaveFormTableField
} from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 保存
 */
export async function editSave(form) {
    return request(SystemAgileSaveJson, METHOD.POST, form)
}
/**
 * 发布
 */
export async function editPublic(form) {
    return request(SystemAgilePublicJson, METHOD.POST, form)
}

/**
 * 根据Id获取
 */
export async function findById(id) {
    return request(SystemAgileFindById + "/" + id, METHOD.GET, {})
}

/**
 * 修改表字段
 */
export async function tableField(param) {
    return request(SystemDataBaseSaveFormTableField, METHOD.POST, param)
}

/**
 * 获取工作表
 */
export async function table(param) {
    return request(SystemAgileAutomationTable, METHOD.POST, param)
}
export default {
    editSave,
    editPublic,
    findById,
    tableField,
    table
}