import { SystemAgileSaveThumbnail } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 保存缩略图
 */
export async function thumbnail(form) {
    return request(SystemAgileSaveThumbnail, METHOD.POST, form)
}

export default {
    thumbnail
}