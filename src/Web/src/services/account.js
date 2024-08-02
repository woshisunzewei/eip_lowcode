import { Login } from '@/services/api'
import { request, method, removeAuthorization } from '@/utils/request'

/**
 * 登录服务
 * @param code 账户名
 * @param pwd 账户密码
 * @returns {Promise<AxiosResponse<T>>}
 */
export async function login(code, pwd) {
    return request(Login, method.POST, {
        code: code,
        pwd: pwd
    })
}

/**
 * 退出登录
 */
export function logout() {
    localStorage.removeItem(process.env.VUE_APP_ROUTES_KEY)
    localStorage.removeItem(process.env.VUE_APP_PERMISSIONS_KEY)
    localStorage.removeItem(process.env.VUE_APP_ROLES_KEY)
    removeAuthorization()
}
export default {
    login,
    logout,
}