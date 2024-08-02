import {
    WorkflowProcessFindById,
    SystemAgileFindColumnJsonByDataFromName,
    SystemAgileQuery,
    SystemDataBaseTableColumn,
    SystemAgileSaveJson,
    SystemAgilePublicJson,
    SystemAgileFindById,
} from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 保存
 */
export async function listSave(form) {
    return request(SystemAgileSaveJson, METHOD.POST, form)
}
/**
 * 发布
 */
export async function listPublic(form) {
    return request(SystemAgilePublicJson, METHOD.POST, form)
}
/**
 * 列
 */
export async function column(param) {
    return request(SystemDataBaseTableColumn, METHOD.POST, param)
}
/**
 * 根据Id获取
 */
export async function findById(id) {
    return request(SystemAgileFindById + "/" + id, METHOD.GET, {})
}

/**
 * 列表
 */
export async function query(param) {
    return request(SystemAgileQuery, METHOD.POST, param)
}

/**
 * 根据表名获取配置
 */
export async function findDataFromName(param) {
    return request(SystemAgileFindColumnJsonByDataFromName, METHOD.POST, param)
}
/**
 * 根据Id获取
 */
export function findWorkflowById(id) {
    return request(WorkflowProcessFindById + "/" + id, METHOD.GET, {})
}
export default {
    listSave,
    listPublic,
    findById,
    column,
    query,
    findDataFromName,
    findWorkflowById
}