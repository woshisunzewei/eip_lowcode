import {
    SystemUserImport,
    SystemUserExport,
    SystemUserDownImportTemplate,
    SystemUserOrganization,
    SystemUserQuery,
    SystemUserDelete,
    SystemUserResetPassword,
    SystemUserIsFreeze,
    SystemUserImportMobile
} from '@/services/api'
import {
    request,
    excelDownload,
    METHOD
} from '@/utils/request'

/**
 * 树
 */
export function organizationQuery() {
    return request(SystemUserOrganization, METHOD.GET, {})
}

/**
 * 列表
 */
export function query(param) {
    return request(SystemUserQuery, METHOD.POST, param)
}

/**
 * 删除
 */
export async function del(param) {
    return request(SystemUserDelete, METHOD.POST, param)
}

/**
 * 重置某人密码
 */
export async function resetPassword(param) {
    return request(SystemUserResetPassword, METHOD.POST, param)
}

/**
 * 
 */
export function isFreeze(param) {
    return request(SystemUserIsFreeze, METHOD.POST, param)
}

/**
 * 
 */
export function downImportTemplate() {
    return excelDownload(SystemUserDownImportTemplate, {}, "用户导入模版")
}

/**
 * 
 */
export function systemUserImport(param) {
    return request(SystemUserImport, METHOD.POST, param, {
        timeout: 9999999999999,
        headers: {
            "Content-Type": "multipart/form-data;boundary=" + new Date().getTime(),
        }
    })
}
/**
 * 
 */
export function systemUserImportMobile(param) {
    return request(SystemUserImportMobile, METHOD.POST, param, {
        timeout: 9999999999999,
        headers: {
            "Content-Type": "multipart/form-data;boundary=" + new Date().getTime(),
        }
    })
}

/**
 * 导出用户
 */
export function systemUserExport(param) {
    return excelDownload(SystemUserExport, param, "导出用户")
}
export default {
    organizationQuery,
    query,
    del,
    resetPassword,
    isFreeze,
    downImportTemplate,
    systemUserImport,
    systemUserExport,
    systemUserImportMobile
}