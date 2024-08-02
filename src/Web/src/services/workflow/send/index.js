import {
    WorkflowEngineInvitationRead,
    WorkflowEngineInvitationReadSure,
    WorkflowEngineInvitationReadApprove,
    WorkflowEngineUnderStanding,
    WorkflowEngineUnderStandingRead,
    WorkflowEngineDraftSave,
    WorkflowEngineModelSave,
    WorkflowEngineActivityStart,
    WorkflowEngineStart,
    WorkflowEngineStartRun,
    WorkflowEngineTaskProcess,
    WorkflowEngineTaskProcessRun,
    WorkflowEngineInstanceProcess,
    WorkflowEngineActivityByTaskId,
    WorkflowEngineTaskById,
    WorkflowEngineRefuse,
    WorkflowEngineReturnAndWrite,
    WorkflowEngineReturnActivity,
    WorkflowEngineRevoke,
    WorkflowEngineRevokeByCreateUser,
    WorkflowEngineEnd,
    WorkflowEngineAdd,
    WorkflowEngineAddApprove,
    WorkflowEngineDeleteTask,
    WorkflowEngineInvitationReadApprovePass,
    WorkflowEngineInvitationReadApproveRefuse,
    WorkflowEngineMonitorDetail,
    WorkflowEngineEvent,

    SystemFileCorrelationId
} from '@/services/api'
import { request, METHOD, requestSync } from '@/utils/request'

/**
 * 获取开始流程节点
 */
export function workflowEngineActivityStart(id) {
    return request(WorkflowEngineActivityStart + "/" + id, METHOD.GET, {})
}

/**
 * 
 */
export function workflowStart(param) {
    return request(WorkflowEngineStart, METHOD.POST, param)
}

/**
 * 
 */
export function workflowStartRun(param) {
    return request(WorkflowEngineStartRun, METHOD.POST, param)
}

/**
 * 
 */
export function workflowTaskProcess(param) {
    return request(WorkflowEngineTaskProcess, METHOD.POST, param)
}

/**
 * 
 */
export function workflowTaskProcessRun(param) {
    return request(WorkflowEngineTaskProcessRun, METHOD.POST, param)
}

/**
 * 
 * @param {*} param 
 * @returns 
 */
export function workflowInstanceProcess(param) {
    return request(WorkflowEngineInstanceProcess, METHOD.POST, param)
}

/**
 * 
 */
export function workflowEngineActivityByTaskId(id) {
    return request(WorkflowEngineActivityByTaskId + "/" + id, METHOD.GET, {})
}

/**
 * 
 */
export function workflowEngineRefuse(param) {
    return request(WorkflowEngineRefuse, METHOD.POST, param)
}

/**
 * 
 */
export function workflowEngineReturnAndWrite(param) {
    return request(WorkflowEngineReturnAndWrite, METHOD.POST, param)
}

/**
 * 
 */
export function workflowEngineReturnActivity(param) {
    return request(WorkflowEngineReturnActivity, METHOD.POST, param)
}

/**
 * 
 */
export function workflowEngineDraftSave(param) {
    return request(WorkflowEngineDraftSave, METHOD.POST, param)
}

/**
 * 
 */
export function workflowEngineModelSave(param) {
    return request(WorkflowEngineModelSave, METHOD.POST, param)
}
/**
 * 
 */
export function workflowEngineUnderStanding(param) {
    return request(WorkflowEngineUnderStanding, METHOD.POST, param)
}
/**
 * 
 */
export function workflowEngineUnderStandingRead(param) {
    return request(WorkflowEngineUnderStandingRead, METHOD.POST, param)
}

/**
 * 
 */
export function workflowEngineInvitationRead(param) {
    return request(WorkflowEngineInvitationRead, METHOD.POST, param)
}
/**
 * 
 */
export function workflowEngineInvitationReadSure(param) {
    return request(WorkflowEngineInvitationReadSure, METHOD.POST, param)
}
/**
 * 
 */
export function workflowEngineInvitationReadApprove(param) {
    return request(WorkflowEngineInvitationReadApprove, METHOD.POST, param)
}

/**
 * 
 */
export function workflowEngineRevoke(param) {
    return request(WorkflowEngineRevoke, METHOD.POST, param)
}

/**
 * 
 */
export function workflowEngineRevokeByCreateUser(param) {
    return request(WorkflowEngineRevokeByCreateUser, METHOD.POST, param)
}

/**
 * 
 */
export function workflowEngineEnd(param) {
    return request(WorkflowEngineEnd, METHOD.POST, param)
}

/**
 * 
 */
export function workflowEngineAdd(param) {
    return request(WorkflowEngineAdd, METHOD.POST, param)
}

/**
 * 
 */
export function workflowEngineAddApprove(param) {
    return request(WorkflowEngineAddApprove, METHOD.POST, param)
}

/**
 * 
 */
export function workflowEngineDeleteTask(param) {
    return request(WorkflowEngineDeleteTask, METHOD.POST, param)
}

/**
 * 
 */
export function workflowEngineDetail(param) {
    return request(WorkflowEngineMonitorDetail, METHOD.POST, param)
}
/**
 * 
 */
export function workflowEngineInvitationReadApprovePass(param) {
    return request(WorkflowEngineInvitationReadApprovePass, METHOD.POST, param)
}
/**
 * 
 */
export function workflowEngineInvitationReadApproveRefuse(param) {
    return request(WorkflowEngineInvitationReadApproveRefuse, METHOD.POST, param)
}
/**
 * 
 */
export function workflowEngineTaskById(id) {
    return request(WorkflowEngineTaskById + "/" + id, METHOD.GET, {})
}

/**
 * 
 */
export function workflowEngineEvent(param) {
    return request(WorkflowEngineEvent, METHOD.POST, param)
}

/**
 * 获取附件
 */
export function fileCorrelationId(id) {
    return requestSync(SystemFileCorrelationId + "/" + id, METHOD.GET, {})
}
export default {
    workflowEngineActivityStart,
    workflowStart,
    workflowStartRun,
    workflowTaskProcess,
    workflowTaskProcessRun,
    workflowInstanceProcess,
    workflowEngineActivityByTaskId,
    workflowEngineTaskById,
    workflowEngineRefuse,
    workflowEngineReturnAndWrite,
    workflowEngineUnderStanding,
    workflowEngineUnderStandingRead,
    workflowEngineReturnActivity,
    workflowEngineDraftSave,
    workflowEngineModelSave,
    workflowEngineInvitationRead,
    workflowEngineInvitationReadSure,
    workflowEngineInvitationReadApprove,
    workflowEngineInvitationReadApprovePass,
    workflowEngineInvitationReadApproveRefuse,

    workflowEngineRevoke,
    workflowEngineRevokeByCreateUser,
    workflowEngineEnd,
    workflowEngineAdd,
    workflowEngineAddApprove,
    workflowEngineDeleteTask,
    workflowEngineDetail,
    workflowEngineEvent,
    fileCorrelationId
}