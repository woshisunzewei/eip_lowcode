import {
    SystemFileUploadReurnPaths,
    SystemFileCorrelationId,
    SystemFileUploadDel
} from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 上传
 */
export async function upload(param) {
    return request(SystemFileUploadReurnPaths, METHOD.POST, param, {
        headers: {
            "Content-Type": "multipart/form-data;boundary=" + new Date().getTime(),
        }
    })
}
/**
 * 根据Id获取
 */
export function findCorrelationId(id) {
    return request(SystemFileCorrelationId + "/" + id, METHOD.GET, {})
}
/**
 * 删除
 */
export async function fileDel(param) {
    return request(SystemFileUploadDel, METHOD.POST, param)
}
export default {
    upload,
    findCorrelationId,
    fileDel
}