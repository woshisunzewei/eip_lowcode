import {
    WorkflowProcessSaveDesignJson,
    WorkflowProcessFindById,
    WorkflowEngineFlowChart,
    WorkflowProcessSaveThumbnail
} from '@/services/api'
import {
    request,
    METHOD
} from '@/utils/request'

/**
 * 保存
 */
export async function save(form) {
    return request(WorkflowProcessSaveDesignJson, METHOD.POST, form)
}

/**
 * 根据Id获取
 */
export function findById(id) {
    return request(WorkflowProcessFindById + "/" + id, METHOD.GET, {})
}

/**
 * 获取监控流程图
 */
export async function flowchart(form) {
    return request(WorkflowEngineFlowChart, METHOD.POST, form)
}
/**
 * 保存缩略图
 */
export async function thumbnail(form) {
    return request(WorkflowProcessSaveThumbnail, METHOD.POST, form)
}
export default {
    save,
    findById,
    flowchart,
    thumbnail
}