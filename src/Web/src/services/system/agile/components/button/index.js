import {
    SystemAgileAutomationSave,
    SystemAgileAutomationFindById,
    SystemAgileFindColumnJsonByMenuId,
    SystemFileUpload,
    SystemAgileSave,
    SystemAgileFindById,
} from '@/services/api'
import {
    request,
    METHOD
} from '@/utils/request'

/**
 * 保存
 */
export async function saveAutomation(form) {
    return request(SystemAgileAutomationSave, METHOD.POST, form)
}

/**
 * 根据Id获取
 */
export function findByIdAutomation(id) {
    return request(SystemAgileAutomationFindById + "/" + id, METHOD.GET, {})
}

/**
 * 获取配置
 */
export function findColumnJson(form) {
    return request(SystemAgileFindColumnJsonByMenuId, METHOD.POST, form)
}
/**
 * 保存
 */
export async function saveAgile(form) {
    return request(SystemAgileSave, METHOD.POST, form)
}

/**
 * 根据Id获取
 */
export function findByIdAgile(id) {
    return request(SystemAgileFindById + "/" + id, METHOD.GET, {})
}
/**
 * 
 */
export function uploadFile(param) {
    return request(SystemFileUpload, METHOD.POST, param, {
        timeout: 9999999999999,
        headers: {
            "Content-Type": "multipart/form-data;boundary=" + new Date().getTime(),
        }
    })
}

export default {
    saveAutomation,
    findByIdAutomation,
    findColumnJson,
    uploadFile,
    saveAgile,
    findByIdAgile
}