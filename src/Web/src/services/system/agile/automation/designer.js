import {
    SystemAgileAutomationSaveJson,
    SystemAgileAutomationPublicJson,
    SystemAgileAutomationFindById,

    SystemAgileAutomationTable
} from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 保存
 */
export async function automationSave(form) {
    return request(SystemAgileAutomationSaveJson, METHOD.POST, form)
}
/**
 * 发布
 */
export async function automationPublic(form) {
    return request(SystemAgileAutomationPublicJson, METHOD.POST, form)
}

/**
 * 根据Id获取
 */
export async function findById(id) {
    return request(SystemAgileAutomationFindById + "/" + id, METHOD.GET, {})
}
/**
 * 获取工作表
 */
export async function table(form) {
    return request(SystemAgileAutomationTable, METHOD.POST, form)
}
export default {
    automationSave,
    automationPublic,
    findById,
    table
}