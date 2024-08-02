import { SystemPostSave, SystemPostFindById } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 保存
 */
export async function save(form) {
    return request(SystemPostSave, METHOD.POST, form)
}

/**
 * 根据Id获取
 */
export function findById(id) {
    return request(SystemPostFindById + "/" + id, METHOD.GET, {})
}
export default {
    save,
    findById
}