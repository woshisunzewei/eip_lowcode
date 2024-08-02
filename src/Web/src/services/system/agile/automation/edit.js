import {
    SystemAgileAutomationSave,
    SystemAgileAutomationFindById,
    SystemAgileFindColumnJsonByMenuId
} from '@/services/api'
import {
    request,
    METHOD
} from '@/utils/request'

/**
 * 保存
 */
export async function save(form) {
    return request(SystemAgileAutomationSave, METHOD.POST, form)
}

/**
 * 根据Id获取
 */
export function findById(id) {
    return request(SystemAgileAutomationFindById + "/" + id, METHOD.GET, {})
}

/**
 * 获取配置
 */
export function findColumnJson(form) {
    return request(SystemAgileFindColumnJsonByMenuId, METHOD.POST, form)
}

export default {
    save,
    findById,
    findColumnJson
}