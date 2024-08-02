import { WorkflowEngineMonitorQuery, } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 列表
 */
export function query(param) {
    return request(WorkflowEngineMonitorQuery, METHOD.POST, param)
}

export default {
    query,
}