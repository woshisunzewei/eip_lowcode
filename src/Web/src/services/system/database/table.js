import { SystemDataBaseTableTree, SystemDataBaseTableColumn } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 树
 */
export function tree() {
    return request(SystemDataBaseTableTree, METHOD.GET, {})
}
/**
 * 列
 */
export function column(param) {
    return request(SystemDataBaseTableColumn, METHOD.POST, param)
}

export default {
    tree,
    column
}