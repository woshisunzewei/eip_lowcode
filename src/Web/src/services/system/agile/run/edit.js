import {
    SystemSnQuery,
    SystemFileCorrelationId,
    SystemFileUploadDel,
    SystemEventApi,
    SystemDataBaseBusinessDataFindFormSource,
    SystemDataBaseBusinessData,
    SystemDataBaseBusinessDataFindById,
    SystemDataBaseBusinessDataQueryBatch,
    SystemDataBaseBusinessDataImportGet,
    SystemUserMenuAgile,
    SystemAgileDataLogRelationId,
    SystemSerialNoCreate,
    SystemDictionaryFindByParentIds,
    SystemDictionaryFindByParentId
} from '@/services/api'
import { request, requestSync, METHOD } from '@/utils/request'
/**
 * 
 */
export function importDataGet(param) {
    return request(SystemDataBaseBusinessDataImportGet, METHOD.POST, param, {
        timeout: 9999999999999,
        headers: {
            "Content-Type": "multipart/form-data;boundary=" + new Date().getTime(),
        }
    })
}
/**
 * 保存业务数据
 */
export function businessData(param) {
    return request(SystemDataBaseBusinessData, METHOD.POST, param, {
        timeout: 9999999999999
    })
}
/**
 * 根据Id获取业务数据
 */
export function businessDataById(param) {
    return request(SystemDataBaseBusinessDataFindById, METHOD.POST, param)
}

/**
 * 保存业务数据
 */
export function businessDataBatch(param) {
    return request(SystemDataBaseBusinessDataQueryBatch, METHOD.POST, param)
}


/**
 * 保存业务数据
 */
export async function businessDataFormSource(param) {
    return await requestSync(SystemDataBaseBusinessDataFindFormSource, METHOD.POST, param)
}


/**
 * 调用api
 */
export function eventApi(param) {
    return requestSync(SystemEventApi, METHOD.POST, param, {
        timeout: 9999999999999
    })
}

/**
 * 删除附件
 */
export function fileUploadDel(param) {
    return requestSync(SystemFileUploadDel, METHOD.POST, param)
}

/**
 * 获取附件
 */
export function fileCorrelationId(id) {
    return requestSync(SystemFileCorrelationId + "/" + id, METHOD.GET, {})
}
/**
 * 获取编码
 */
export function snQuery(param) {
    return requestSync(SystemSnQuery, METHOD.POST, param)
}

/**
 * 根据Id获取
 */
export async function menuAgile(param) {
    return request(SystemUserMenuAgile, METHOD.POST, param)
}

/**
 * 编号生成
 */
export function serialNoCreate(param) {
    return requestSync(SystemSerialNoCreate, METHOD.POST, param)
}


/**
 * 获取敏捷开发日志
 */
export function agileDataLogRelationId(param) {
    return request(SystemAgileDataLogRelationId, METHOD.POST, param)
}
/**
 * 根据父级Id获取字典树信息
 */
export function dictionaryQueryByIds(param) {
    return request(SystemDictionaryFindByParentIds, METHOD.POST, param)
}

/**
 * 根据父级Id获取字典信息
 */
export function dictionaryParentId(parentId) {
    return request(SystemDictionaryFindByParentId + "/" + parentId, METHOD.GET, {})
}

export default {
    eventApi,
    businessData,
    businessDataById,
    businessDataBatch,
    businessDataFormSource,
    fileUploadDel,
    fileCorrelationId,
    snQuery,
    importDataGet,
    menuAgile,
    serialNoCreate,
    agileDataLogRelationId,
    dictionaryQueryByIds,
    dictionaryParentId
}