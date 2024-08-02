import {
    SystemCodeGenerationCode,
    SystemDataBaseTableColumn,
    SystemCodeGenerationFile,
    SystemDataBaseTable
} from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 生成代码
 */
export function code(param) {
    return request(SystemCodeGenerationCode, METHOD.POST, param)
}
/**
 * 表
 */
export async function table(param) {
    return request(SystemDataBaseTable, METHOD.POST, param)
}
/**
 * 列
 */
export function column(param) {
    return request(SystemDataBaseTableColumn, METHOD.POST, param)
}
/**
 * 列
 */
export function file(param) {
    return request(SystemCodeGenerationFile, METHOD.POST, param)
}
export default {
    table,
    column,
    code,
    file
}