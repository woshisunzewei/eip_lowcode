import { SystemPermissionMobileMenu, SystemPermissionPrivilegeMaster, SystemPermissionSave } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 树
 */
export function menu() {
    return request(SystemPermissionMobileMenu, METHOD.GET, {})
}

/**
 * 树
 */
export function privilegeMaster(privilegeMasterValue, privilegeMaster, privilegeAccess) {
    return request(SystemPermissionPrivilegeMaster + "/" + privilegeMasterValue + "/" + privilegeMaster + "/" + privilegeAccess, METHOD.GET, {})
}

/**
 * 保存
 */
export async function save(param) {
    return request(SystemPermissionSave, METHOD.POST, param)
}

export default {
    menu,
    privilegeMaster,
    save
}