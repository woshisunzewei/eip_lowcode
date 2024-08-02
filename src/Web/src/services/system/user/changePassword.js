import {
    SystemUserChangePassword,
    SystemUserCheckPasswordRule
} from '@/services/api'
import {
    request,
    METHOD
} from '@/utils/request'

/**
 * 修改密码
 */
export function changePassword(param) {
    return request(SystemUserChangePassword, METHOD.POST, param)
}

/**
 * 修改密码
 */
export function checkPasswordRule(param) {
    return request(SystemUserCheckPasswordRule, METHOD.POST, param)
}
export default {
    changePassword,
    checkPasswordRule
}