import { WeChatUserBindUserQuery, WeChatUserBindOrganizationQuery, WeChatUserBindSave } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 获取绑定用户列表
 */
export function queryBindUser(param) {
    return request(WeChatUserBindUserQuery, METHOD.POST, param)
}
/**
 * 获取绑定用户组织机构树
 */
export function queryBindOrganization(param) {
    return request(WeChatUserBindOrganizationQuery, METHOD.GET, param)
}

/**
 * 获取绑定用户组织机构树
 */
export function save(param) {
    return request(WeChatUserBindSave, METHOD.POST, param)
}
export default {
    queryBindUser,
    queryBindOrganization,
    save
}