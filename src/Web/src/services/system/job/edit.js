import { SystemJobCalendarQuery, SystemJobSchedule, SystemJobEdit } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 排除日历查询
 */
export async function calendarQuery() {
    return request(SystemJobCalendarQuery, METHOD.GET)
}
/**
 * 保存
 */
export async function findByJobName(param) {
    return request(SystemJobEdit, METHOD.POST, param)
}
/**
 * 保存
 */
export async function save(form) {
    return request(SystemJobSchedule, METHOD.POST, form)
}
export default {
    calendarQuery,
    save,
    findByJobName
}