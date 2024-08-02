import { SystemCommonMenuButton } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 获取按钮
 */
export function menubutton(param) {
    return request(SystemCommonMenuButton, METHOD.POST, param)
}

export default {
    menubutton
}