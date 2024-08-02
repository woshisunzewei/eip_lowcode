import { SystemDataBaseProc } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 存储过程
 */
export async function proc(param) {
    return request(SystemDataBaseProc, METHOD.POST, param)
}
export default {
    proc
}