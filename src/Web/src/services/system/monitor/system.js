import {
    SystemMonitorServerBase,
    SystemMonitorServerDisk,
    SystemMonitorServerUsed,
} from '@/services/api'
import {
    request,
    METHOD
} from '@/utils/request'
/**
 * 
 */
export function serverBase() {
    return request(SystemMonitorServerBase, METHOD.GET, {})
}
/**
 * 
 */
export function serverDisk() {
    return request(SystemMonitorServerDisk, METHOD.GET, {})
}
/**
 * 
 */
export function serverUsed() {
    return request(SystemMonitorServerUsed, METHOD.GET, {})
}
export default {
    serverBase,
    serverDisk,
    serverUsed
}