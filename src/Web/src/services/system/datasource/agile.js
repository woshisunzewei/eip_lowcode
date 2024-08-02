import { SystemAgileQueryKey } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 保存
 */
export async function queryKey(form) {
    return request(SystemAgileQueryKey, METHOD.POST, form)
}

export default {
    queryKey
}