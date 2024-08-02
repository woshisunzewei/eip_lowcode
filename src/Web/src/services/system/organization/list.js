import {
    SystemOrganizationImport,
    SystemOrganizationDownImportTemplate,
    SystemOrganization,
    SystemOrganizationQuery,
    SystemOrganizationDelete,
    SystemOrganizationIsFreeze
} from '@/services/api'
import { request, excelDownload, METHOD } from '@/utils/request'

/**
 * 树
 */
export function organizationQuery() {
    return request(SystemOrganization, METHOD.GET, {})
}

/**
 * 列表
 */
export function query(param) {
    return request(SystemOrganizationQuery, METHOD.POST, param)
}

/**
 * 删除
 */
export async function del(param) {
    return request(SystemOrganizationDelete, METHOD.POST, param)
}
/**
 * 
 */
export function isFreeze(param) {
    return request(SystemOrganizationIsFreeze, METHOD.POST, param)
}

/**
 * 
 */
export function downImportTemplate() {
    return excelDownload(SystemOrganizationDownImportTemplate, {}, "组织架构导入模版")
}

/**
 * 
 */
export function systemOrganizationImport(param) {
    return request(SystemOrganizationImport, METHOD.POST, param, {
        timeout: 9999999999999,
        headers: {
            "Content-Type": "multipart/form-data;boundary=" + new Date().getTime(),
        }
    })
}

export default {
    organizationQuery,
    query,
    del,
    isFreeze,
    downImportTemplate,
    systemOrganizationImport
}