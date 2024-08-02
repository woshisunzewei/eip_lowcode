import {
    SystemMonitorOnlineUser,
    SystemMonitorOnlineUserKickout
} from '@/services/api'
import {
    request,
    METHOD
} from '@/utils/request'
/**
 * 所有在线用户
 */
export function online() {
    return request(SystemMonitorOnlineUser, METHOD.GET, {})
}
/**
 * 强制下线
 */
export function kickout(param) {
    return request(SystemMonitorOnlineUserKickout, METHOD.POST, param)
}
export default {
    online,
    kickout
}