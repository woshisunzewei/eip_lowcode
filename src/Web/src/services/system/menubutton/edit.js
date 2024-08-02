import { SystemMenuButtonSave, SystemMenuButtonFindById, SystemMenuButtonSaveAll } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 保存
 */
export async function save(form) {
    return request(SystemMenuButtonSave, METHOD.POST, form)
}
/**
 * 保存
 */
export async function saveAll(form) {
    return request(SystemMenuButtonSaveAll, METHOD.POST, form)
}

/**
 * 根据Id获取
 */
export function findById(id) {
    return request(SystemMenuButtonFindById + "/" + id, METHOD.GET, {})
}
export default {
    save,
    saveAll,
    findById
}