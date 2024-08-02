import { SystemFileUploadReurnPaths } from '@/services/api'
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
export default {
    upload,
}