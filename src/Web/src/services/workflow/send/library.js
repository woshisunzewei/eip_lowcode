import { WorkflowEngineLibrary } from '@/services/api'
import { request, METHOD } from '@/utils/request'
/**
 * 流程库
 */
export function library(param) {
    return request(WorkflowEngineLibrary, METHOD.POST, param)
}

export default {
    library,
}