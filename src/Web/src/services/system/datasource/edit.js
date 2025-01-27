import { SystemDataSourceSave, SystemDataSourceFindById } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 保存
 */
export async function save(form) {
    return request(SystemDataSourceSave, METHOD.POST, form)
}

/**
 * 根据Id获取
 */
export function findById(id) {
    return request(SystemDataSourceFindById + "/" + id, METHOD.GET, {})
}
export default {
    save,
    findById
}