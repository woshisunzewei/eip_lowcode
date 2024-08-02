import {
    SystemMonitorAssemblies
} from '@/services/api'
import {
    request,
    METHOD
} from '@/utils/request'
/**
 * 获取
 */
export function query() {
    return request(SystemMonitorAssemblies, METHOD.GET, {})
}

export default {
    query
}