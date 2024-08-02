import { WorkflowPermission, SystemAgileBase, SystemAgileColumns, WorkflowEngineColumn } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 基础表单
 */
export function findForm(param) {
    return request(SystemAgileBase, METHOD.POST, param)
}

/**
 * 
 */
export function findPermissionProcess() {
    return request(WorkflowPermission, METHOD.GET, {})
}


/**
 * 根据表单获取字段
 */
export function findFormColumns(id) {
    return request(SystemAgileColumns + "/" + id, METHOD.GET, {})
}

/**
 * 根据实例Id获取列值
 */
export function findColumns(id) {
    return request(WorkflowEngineColumn + "/" + id, METHOD.GET, {})
}

export default {
    findForm,
    findFormColumns,
    findPermissionProcess,
    findColumns
}