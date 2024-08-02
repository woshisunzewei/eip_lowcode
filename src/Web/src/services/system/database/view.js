import { SystemDataBaseView } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 空间
 */
export function view() {
    return request(SystemDataBaseView, METHOD.GET, {})
}

export default {
    view
}