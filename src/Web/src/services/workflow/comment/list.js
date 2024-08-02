import { WorkflowCommentQuery, WorkflowCommentDelete } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 列表
 */
export function query(param) {
    return request(WorkflowCommentQuery, METHOD.POST, param)
}

/**
 * 删除
 */
export function del(param) {
    return request(WorkflowCommentDelete, METHOD.POST, param)
}

export default {
    query,
    del
}