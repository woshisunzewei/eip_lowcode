import {
    SystemUserHeadImage
} from '@/services/api'
import {
    request,
    METHOD
} from '@/utils/request'

/**
 * 
 */
export function uploadHeadImage(param) {
    return request(SystemUserHeadImage, METHOD.POST, param)
}

export default {
    uploadHeadImage
}