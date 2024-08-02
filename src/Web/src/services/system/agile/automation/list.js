import {
    SystemAgileAutomationIsFreeze,
    SystemAgileAutomationQuery,
    SystemAgileAutomationDelete
} from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 列表
 */
export function query(param) {
    return request(SystemAgileAutomationQuery, METHOD.POST, param)
}

/**
 * 删除
 */
export function del(param) {
    return request(SystemAgileAutomationDelete, METHOD.POST, param)
}
/**
 * 
 */
export function isFreeze(param) {
    return request(SystemAgileAutomationIsFreeze, METHOD.POST, param)
}
export default {
    query,
    del,
    isFreeze
}