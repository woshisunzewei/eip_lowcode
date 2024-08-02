import {
    SystemDataBaseTableColumn
} from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 列
 */
export function column(param) {
    return request(SystemDataBaseTableColumn, METHOD.POST, param)
}

export default {
    column
}