import {
    SystemEventApi,
    SystemAgileAutomationRun,
    SystemAgileFindById,
    SystemAgileQuery,
    SystemDataBaseBusinessDataQuery,
    SystemDataBaseBusinessDataQueryPage,
    SystemDataBaseBusinessDataFooter,
    SystemDataBaseBusinessDataDel,

    SystemDataBaseBusinessDataDelPhysics,
    SystemDataBaseBusinessDataDelPhysicsAll,
    SystemDataBaseBusinessDataDelRecovery,
    SystemDataBaseBusinessDataQueryFilterSearch,

    SystemDataBaseBusinessDataReport,
    SystemDataBaseBusinessDataReportTemplate,
    SystemDataBaseBusinessDataImport,
    SystemDataBaseBusinessDataImportTable,
    SystemDataBaseBusinessDataImportTemplate,
    WorkflowEngineRevokeByCreateUser,
    SystemDataBaseBusinessDataFindFormSourcePagingTable,
    SystemDataBaseBusinessDataQueryBatch,
    SystemUserMenuAgile,
    SystemDataBaseBusinessDataFindById
} from '@/services/api'
import {
    request,
    requestSync,
    excelDownload,
    METHOD
} from '@/utils/request'
/**
 * 根据Id获取业务数据
 */
export function businessDataById(param) {
    return request(SystemDataBaseBusinessDataFindById, METHOD.POST, param)
}

/**
 * 查询业务数据
 */
export async function queryConfig(param) {
    return request(SystemAgileQuery, METHOD.POST, param)
}

/**
 * 查询业务数据
 */
export async function query(param) {
    return request(SystemDataBaseBusinessDataQuery, METHOD.POST, param)
}
/**
 * 查询业务数据
 */
export async function queryFilterSearch(param) {
    return request(SystemDataBaseBusinessDataQueryFilterSearch, METHOD.POST, param)
}
/**
 * 查询业务数据
 */
export async function queryPage(param) {
    return request(SystemDataBaseBusinessDataQueryPage, METHOD.POST, param)
}
/**
 * 获取表尾数据
 */
export function queryFooter(param) {
    return request(SystemDataBaseBusinessDataFooter, METHOD.POST, param)
}
/**
 * 删除业务数据
 */
export async function del(param) {
    return request(SystemDataBaseBusinessDataDel, METHOD.POST, param)
}
/**
 * 删除业务数据
 */
export async function delPhysics(param) {
    return request(SystemDataBaseBusinessDataDelPhysics, METHOD.POST, param)
}
/**
 * 删除业务数据
 */
export async function delPhysicsAll(param) {
    return request(SystemDataBaseBusinessDataDelPhysicsAll, METHOD.POST, param)
}
/**
 * 删除业务数据
 */
export async function delRecovery(param) {
    return request(SystemDataBaseBusinessDataDelRecovery, METHOD.POST, param)
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
 * 调用自动化运行
 */
export function automationRun(param) {
    return requestSync(SystemAgileAutomationRun, METHOD.POST, param, {
        timeout: 9999999999999
    })
}

/**
 * 
 */
export function workflowEngineRevokeByCreateUser(param) {
    return request(WorkflowEngineRevokeByCreateUser, METHOD.POST, param)
}

/**
 * 
 */
export function reportData(param) {
    return excelDownload(SystemDataBaseBusinessDataReport, param, param.reportName)
}

/**
 * 
 */
export function reportDataTemplate(param) {
    return excelDownload(SystemDataBaseBusinessDataReportTemplate, param, param.reportName)
}

/**
 * 
 */
export function importData(param) {
    return request(SystemDataBaseBusinessDataImport, METHOD.POST, param, {
        timeout: 9999999999999,
        headers: {
            "Content-Type": "multipart/form-data;boundary=" + new Date().getTime(),
        }
    })
}
/**
 * 
 */
export function importDataTable(param) {
    return request(SystemDataBaseBusinessDataImportTable, METHOD.POST, param, {
        timeout: 9999999999999,
        headers: {
            "Content-Type": "multipart/form-data;boundary=" + new Date().getTime(),
        }
    })
}
/**
 * 
 */
export function importTemplate(param) {
    return excelDownload(SystemDataBaseBusinessDataImportTemplate, param, param.reportName)
}

/**
 * 根据Id获取
 */
export async function menuAgile(param) {
    return request(SystemUserMenuAgile, METHOD.POST, param)
}

/**
 * 根据Id获取
 */
export function findAgileById(id) {
    return request(SystemAgileFindById + "/" + id, METHOD.GET, {})
}

/**
 * 查询
 */
export function queryTable(form) {
    return request(SystemDataBaseBusinessDataFindFormSourcePagingTable, METHOD.POST, form)
}

/**
 * 查询
 */
export function businessDataBatch(form) {
    return request(SystemDataBaseBusinessDataQueryBatch, METHOD.POST, form)
}
export default {
    query,
    queryPage,
    queryTable,
    queryFooter,
    queryConfig,
    del,
    delPhysics,
    delPhysicsAll,
    delRecovery,
    eventApi,
    reportData,
    reportDataTemplate,
    importData,
    importDataTable,
    importTemplate,
    workflowEngineRevokeByCreateUser,
    menuAgile,
    findAgileById,
    automationRun,
    businessDataBatch,
    businessDataById,
    queryFilterSearch
}