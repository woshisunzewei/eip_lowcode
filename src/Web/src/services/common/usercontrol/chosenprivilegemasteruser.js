import { SystemCommonChosenUser, SystemCommonChosenUserSave } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 选择人
 */
export function chosenprivilegemasteruser(privilegeMaster, privilegeaccess) {
    return request(SystemCommonChosenUser + "/" + privilegeMaster + "/" + privilegeaccess, METHOD.GET, {})
}
/**
 * 保存人
 */
export function save(param) {
    return request(SystemCommonChosenUserSave, METHOD.POST, param)
}

export default {
    chosenprivilegemasteruser,
    save
}