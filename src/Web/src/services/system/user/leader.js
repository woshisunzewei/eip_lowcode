import { SystemLeaderQuery, SystemLeaderSave, SystemLeaderSubordinateQuery, SystemLeaderSubordinateSave } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 保存
 */
export async function save(form) {
    return request(SystemLeaderSave, METHOD.POST, form)
}

/**
 *获取
 */
export function query(id) {
    return request(SystemLeaderQuery + "/" + id, METHOD.GET, {})
}

/**
 * 保存
 */
export async function saveSubordinate(form) {
    return request(SystemLeaderSubordinateSave, METHOD.POST, form)
}

/**
 * 
 */
export function querySubordinate(id) {
    return request(SystemLeaderSubordinateQuery + "/" + id, METHOD.GET, {})
}

export default {
    query,
    save,
    saveSubordinate,
    querySubordinate
}