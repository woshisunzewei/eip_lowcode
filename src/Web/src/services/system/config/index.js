import {
    SystemConfigSave,
    SystemConfigQuery,
    SystemFileUpload,
    SystemConfigSystem,
    SystemLoginRefreshToken
} from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 保存
 */
export async function save(form) {
    return request(SystemConfigSave, METHOD.POST, form)
}

/**
 * 获取
 */
export function query() {
    return request(SystemConfigQuery, METHOD.GET, {})
}

/**
 * 
 */
export function uploadLoginBg(param) {
    return request(SystemFileUpload, METHOD.POST, param, {
        timeout: 9999999999999,
        headers: {
            "Content-Type": "multipart/form-data;boundary=" + new Date().getTime(),
        }
    })
}

/**
 * 
 */
export function uploadSystemLogo(param) {
    return request(SystemFileUpload, METHOD.POST, param, {
        timeout: 9999999999999,
        headers: {
            "Content-Type": "multipart/form-data;boundary=" + new Date().getTime(),
        }
    })
}

/**
 * 系统配置
 */
export function header() {
    return request(SystemConfigSystem, METHOD.GET, {})
}

/**
 * 
 */
export function refreshToken(refreshToken) {
    return request(SystemLoginRefreshToken, METHOD.POST, {}, {
        timeout: 9999999999999,
        headers: {
            "Authorization": refreshToken,
        }
    })
}

export default {
    save,
    query,
    uploadLoginBg,
    uploadSystemLogo,
    header,
    refreshToken
}