import { WorkflowPermission, WorkflowPermissionSave, WorkflowPermissionFind } from '@/services/api'
import { request, METHOD } from '@/utils/request'
/**
 * 获取所有流程
 */
export function findAll() {
    return request(WorkflowPermission, METHOD.GET, {})
}

/**
 * 保存
 */
export function save(param) {
    return request(WorkflowPermissionSave, METHOD.POST, param)
}

/**
 * 已有权限
 */
export function find(param) {
    return request(WorkflowPermissionFind, METHOD.POST, param)
}
export default {
    findAll,
    save,
    find
}