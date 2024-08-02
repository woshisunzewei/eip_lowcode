import { WorkflowProcessQuery, WorkflowProcessDelete } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 列表
 */
export function query(param) {
    return request(WorkflowProcessQuery, METHOD.POST, param)
}

/**
 * 删除
 */
export function del(param) {
    return request(WorkflowProcessDelete, METHOD.POST, param)
}

export default {
    query,
    del
}