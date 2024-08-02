import {
    SystemAgileQuery,
    SystemAgileSave,
    SystemAgileFindById,
    SystemDataBaseTable,
    SystemDataBaseView,
    SystemDataBaseProc
} from '@/services/api'
import {
    request,
    METHOD
} from '@/utils/request'

/**
 * 保存
 */
export async function save(form) {
    return request(SystemAgileSave, METHOD.POST, form)
}

/**
 * 根据Id获取
 */
export function findById(id) {
    return request(SystemAgileFindById + "/" + id, METHOD.GET, {})
}
/**
 * 表
 */
export async function table(param) {
    return request(SystemDataBaseTable, METHOD.POST, param)
}

/**
 * 视图
 */
export async function view(param) {
    return request(SystemDataBaseView, METHOD.POST, param)
}

/**
 * 存储过程
 */
export async function proc(param) {
    return request(SystemDataBaseProc, METHOD.POST, param)
}

/**
 * 列表
 */
export async function query(param) {
    return request(SystemAgileQuery, METHOD.POST, param)
}
export default {
    save,
    findById,
    table,
    view,
    proc,
    query
}