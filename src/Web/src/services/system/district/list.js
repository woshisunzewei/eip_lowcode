import { SystemDistrict, SystemDistrictQuery } from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 省市区信息树
 */
export async function districtQuery(id) {
    return request(SystemDistrict + '/' + id, METHOD.GET, {})
}

/**
 * 省市区信息列表
 */
export async function query(id) {
    return request(SystemDistrictQuery + '/' + id, METHOD.GET, {})
}

export default {
    districtQuery,
    query
}