import { SystemDictionaryFindByParentId } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 根据父级Id获取字典信息
 */
export function dictionaryParentId(parentId) {
    return request(SystemDictionaryFindByParentId + "/" + parentId, METHOD.GET, {})
}

export default {
    dictionaryParentId
}