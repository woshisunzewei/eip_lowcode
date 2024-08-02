import { SystemOrganizationSave, SystemOrganizationFindById, SystemOrganizationRemoveChildren } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 保存
 */
export async function save(form) {
    return request(SystemOrganizationSave, METHOD.POST, form)
}

/**
 * 根据Id获取
 */
export function findById(id) {
    return request(SystemOrganizationFindById + "/" + id, METHOD.GET, {})
}

/**
 * 取消下级
 */
export function organizationRemoveChildren(id) {
    return request(SystemOrganizationRemoveChildren + "/" + id, METHOD.GET, {})
}
export default {
    save,
    findById,
    organizationRemoveChildren
}