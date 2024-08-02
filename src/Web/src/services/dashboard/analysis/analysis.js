import {
    SystemNoticeQuery,
    SystemNoticeFindById,
    SystemUpdateLogQuery,
    WorkflowEngineNeedDo,
    WorkflowEngineHaveDo,
    WorkflowEngineHaveSend,
    WorkflowEngineOverTime
} from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 更新日志
 */
export function updatelog() {
    return request(SystemUpdateLogQuery, METHOD.POST, {})
}

export function workflowNeedDo(param) {
    return request(WorkflowEngineNeedDo, METHOD.POST, param)
}

export function workflowHaveDo(param) {
    return request(WorkflowEngineHaveDo, METHOD.POST, param)
}

export function workflowHaveSend(param) {
    return request(WorkflowEngineHaveSend, METHOD.POST, param)
}

export function workflowOverTime(param) {
    return request(WorkflowEngineOverTime, METHOD.POST, param)
}
/**
 * 列表
 */
export function noticeQuery(param) {
    return request(SystemNoticeQuery, METHOD.POST, param)
}
/**
 * 根据Id获取
 */
export function noticeFindById(id) {
    return request(SystemNoticeFindById + "/" + id, METHOD.GET, {})
}


export default {
    updatelog,
    workflowNeedDo,
    workflowHaveDo,
    workflowHaveSend,
    workflowOverTime,
    noticeQuery,
    noticeFindById
}