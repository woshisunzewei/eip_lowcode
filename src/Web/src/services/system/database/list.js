import {
    SystemDataBaseQuery,
    SystemDataBaseDelete,
} from '@/services/api'
import { request, METHOD } from '@/utils/request'
/**
 * 列表
 */
export function query(param) {
    return request(SystemDataBaseQuery, METHOD.POST, param)
}

/**
 * 删除
 */
export async function del(param) {
    return request(SystemDataBaseDelete, METHOD.POST, param)
}

export default {
    query,
    del,
}