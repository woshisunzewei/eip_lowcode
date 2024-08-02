import { WorkflowButtonQuery, SystemAgileBase, SystemAgileColumns } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 基础表单
 */
export function findForm(param) {
    return request(SystemAgileBase, METHOD.POST, param)
}
/**
 * 基础表单
 */
export function findButton(param) {
    return request(WorkflowButtonQuery, METHOD.POST, param)
}

/**
 * 根据表单获取字段
 */
export function findFormColumns(id) {
    return request(SystemAgileColumns + "/" + id, METHOD.GET, {})
}

export default {
    findForm,
    findFormColumns,
    findButton
}