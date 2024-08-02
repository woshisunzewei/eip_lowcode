import {
    WorkflowProcessSave,
    WorkflowProcessFindById,
    SystemAgileBase
} from '@/services/api'
import {
    request,
    METHOD
} from '@/utils/request'
/**
 * 基础表单
 */
export function findForm(param) {
    return request(SystemAgileBase, METHOD.POST, param)
}
/**
 * 保存
 */
export async function save(form) {
    return request(WorkflowProcessSave, METHOD.POST, form)
}

/**
 * 根据Id获取
 */
export function findById(id) {
    return request(WorkflowProcessFindById + "/" + id, METHOD.GET, {})
}


export default {
    findForm,
    save,
    findById,
}