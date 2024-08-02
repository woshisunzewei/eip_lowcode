import {
    SystemDataBaseQuery,
    SystemDataSourceQuery,
    SystemDataSourceDelete,
} from '@/services/api'
import { request, METHOD } from '@/utils/request'
/**
 * 列表
 */
export function query(param) {
    return request(SystemDataSourceQuery, METHOD.POST, param)
}

/**
 * 删除
 */
export async function del(param) {
    return request(SystemDataSourceDelete, METHOD.POST, param)
}
/**
 * 列表
 */
export function dataBaseQuery(param) {
    return request(SystemDataBaseQuery, METHOD.POST, param)
}
export default {
    dataBaseQuery,
    query,
    del,
}