import { SystemPermissionMenuHave, SystemPermissionSave, SystemPermissionMenuButton } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 树
 */
export function menuHave(privilegeMasterValue, privilegeMaster, privilegeAccess) {
    return request(SystemPermissionMenuHave + "/" + privilegeMasterValue + "/" + privilegeMaster + "/" + privilegeAccess, METHOD.GET, {})
}

/**
 * 保存
 */
export async function save(param) {
    return request(SystemPermissionSave, METHOD.POST, param)
}

/**
 * 所有模块按钮
 */
export async function menuButton(privilegeMasterValue, privilegeMaster) {
    return request(SystemPermissionMenuButton + "/" + privilegeMasterValue + "/" + privilegeMaster, METHOD.GET, {})
}

export default {
    menuHave,
    menuButton,
    save
}