import { SystemMessageLogMy } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 
 */
export async function messageLogMy(form) {
    return request(SystemMessageLogMy, METHOD.POST, form)
}

export default {
    messageLogMy
}