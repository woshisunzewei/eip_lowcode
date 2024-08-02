import { SystemPermissionDataAll, SystemPermissionSave, SystemPermissionMenuButtonAll, SystemPermissionMenu, SystemPermissionPrivilegeMaster } from '@/services/api'
import { request, METHOD } from '@/utils/request'


/**
 * 保存
 */
export function permissionSave(param) {
    return request(SystemPermissionSave, METHOD.POST, param)
}

/**
 * 所有模块按钮
 */
export async function menuButtonAll(privilegeMasterValue, privilegeMaster) {
    return request(SystemPermissionMenuButtonAll + "/" + privilegeMasterValue + "/" + privilegeMaster, METHOD.GET, {})
}


/**
 * 所有模块数据权限
 */
export async function dataAll(privilegeMasterValue, privilegeMaster) {
    return request(SystemPermissionDataAll + "/" + privilegeMasterValue + "/" + privilegeMaster, METHOD.GET, {})
}


/**
 * 保存菜单
 */
export function menu() {
    return request(SystemPermissionMenu, METHOD.GET, {})
}

/**
 * 获取已有菜单
 */
export function privilegeMaster(privilegeMasterValue, privilegeMaster, privilegeAccess) {
    return request(SystemPermissionPrivilegeMaster + "/" + privilegeMasterValue + "/" + privilegeMaster + "/" + privilegeAccess, METHOD.GET, {})
}

export default {
    permissionSave,
    menuButtonAll,
    dataAll,
    menu,
    privilegeMaster
}