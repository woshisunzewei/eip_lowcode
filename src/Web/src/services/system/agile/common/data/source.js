import {
    SystemDataBaseTable,
    SystemDataBaseProc,
    SystemDataBaseView,
    SystemDataBaseTableColumn
} from '@/services/api'
import { request, METHOD } from '@/utils/request'

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
 * 列
 */
export async function column(param) {
    return request(SystemDataBaseTableColumn, METHOD.POST, param)
}

export default {
    table,
    view,
    proc,
    column
}