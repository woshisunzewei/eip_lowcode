import {
    SystemMonitorApi
} from '@/services/api'
import {
    request,
    METHOD
} from '@/utils/request'
/**
 * 获取
 */
export function query() {
    return request(SystemMonitorApi, METHOD.GET, {})
}

export default {
    query
}