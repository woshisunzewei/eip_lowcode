import {
    WeChatMpTemplateSendSave,
    WeChatMpTemplateSendFindById,
} from '@/services/api'
import {
    request,
    METHOD
} from '@/utils/request'

/**
 * 保存
 */
export async function save(form) {
    return request(WeChatMpTemplateSendSave, METHOD.POST, form)
}

/**
 * 根据Id获取
 */
export function findByTemplateSendId(id) {
    return request(WeChatMpTemplateSendFindById + "/" + id, METHOD.GET, {})
}

export default {
    save,
    findByTemplateSendId
}