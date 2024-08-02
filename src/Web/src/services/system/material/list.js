import {
    SystemMaterialQuery,
    SystemMaterialDelete,
    SystemMaterialSave,
    SystemMaterialFindById
} from '@/services/api'
import { request, METHOD } from '@/utils/request'
/**
 * 列表
 */
export function query(param) {
    return request(SystemMaterialQuery, METHOD.POST, param)
}

/**
 * 删除
 */
export async function del(param) {
    return request(SystemMaterialDelete, METHOD.POST, param)
}
/**
 * 根据Id获取
 */
export function findById(param) {
    return request(SystemMaterialFindById, METHOD.GET, param)
}

/**
 * 保存
 */
export async function save(param) {
    return request(SystemMaterialSave, METHOD.POST, param, {
        timeout: 9999999999999,
        headers: {
            "Content-Type": "multipart/form-data;boundary=" + new Date().getTime(),
        }
    })
}

export default {
    query,
    del,
    findById,
    save
}