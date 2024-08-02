import { WorkflowEngineModelQuery, WorkflowEngineModelDelete } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 列表
 */
export function query(param) {
    return request(WorkflowEngineModelQuery, METHOD.POST, param)
}

/**
 * 删除
 */
export function del(param) {
    return request(WorkflowEngineModelDelete, METHOD.POST, param)
}

export default {
    query,
    del
}