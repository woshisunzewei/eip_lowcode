import { SystemJobQuery, 
    SystemJobDelete,
    SystemJobPause,
    SystemJobPauseAll,
    SystemJobResume,
    SystemJobResumeAll } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 列表
 */
export function query(param) {
    return request(SystemJobQuery, METHOD.get, {})
}

/**
 * 删除
 */
export async function del(param) {
    return request(SystemJobDelete, METHOD.POST, param)
}
/**
 * 
 */
 export async function pause(param) {
    return request(SystemJobPause, METHOD.POST, param)
}
/**
 * 
 */
 export async function pauseall(param) {
    return request(SystemJobPauseAll, METHOD.POST, param)
}
/**
 * 
 */
 export async function resume(param) {
    return request(SystemJobResume, METHOD.POST, param)
}
/**
 * 
 */
 export async function resumeall(param) {
    return request(SystemJobResumeAll, METHOD.POST, param)
}
export default {
    query,
    del,
    pause,
    pauseall,
    resume,
    resumeall
}