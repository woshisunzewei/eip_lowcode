import { SystemAgileSaveJson, SystemAgilePublicJson, SystemAgileFindById } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 保存
 */
export async function editSave(form) {
    return request(SystemAgileSaveJson, METHOD.POST, form)
}
/**
 * 发布
 */
export async function editPublic(form) {
    return request(SystemAgilePublicJson, METHOD.POST, form)
}

/**
 * 根据Id获取
 */
export async function findById(id) {
    return request(SystemAgileFindById + "/" + id, METHOD.GET, {})
}
export default {

    editSave,
    editPublic,
    findById,
}