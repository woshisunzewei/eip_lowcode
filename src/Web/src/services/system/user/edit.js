import { SystemUserSave, SystemUserFindById, SystemRoleSelect, SystemUserDetail,SystemUserCheckCode } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 保存
 */
export async function save(form) {
    return request(SystemUserSave, METHOD.POST, form)
}

/**
 * 根据Id获取
 */
export function findById(id) {
    return request(SystemUserFindById + "/" + id, METHOD.GET, {})
}

/**
 * 角色下拉
 */
export function roleSelect() {
    return request(SystemRoleSelect, METHOD.GET, {})
}
/**
 * 详情
 */
export function detail(id) {
    return request(SystemUserDetail + "/" + id, METHOD.GET, {})
}

/**
 * 检测代码是否已经具有重复项
 */
 export function checkCode(param) {
    return request(SystemUserCheckCode, METHOD.POST, param)
}
export default {
    detail,
    save,
    findById,
    roleSelect,
    checkCode
}