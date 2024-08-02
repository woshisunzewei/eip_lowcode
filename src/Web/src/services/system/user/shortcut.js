import {
    SystemUserMenuTree,
    SystemShortCutQuery,
    SystemShortCutSave,
    SystemShortCutDel,
    SystemShortCutDelAll,
    SystemShortCutSaveOrderNo
} from '@/services/api'
import {
    request,
    METHOD
} from '@/utils/request'

/**
 * 
 */
export function query(param) {
    return request(SystemShortCutQuery, METHOD.POST, param)
}
/**
 * 
 */
export function del(param) {
    return request(SystemShortCutDel, METHOD.POST, param)
}
/**
 * 
 */
export function delall() {
    return request(SystemShortCutDelAll, METHOD.POST, {})
}
/**
 * 
 */
export function save(param) {
    return request(SystemShortCutSave, METHOD.POST, param)
}
/**
 * 保存排序
 */
export function saveOrderNo(param) {
    return request(SystemShortCutSaveOrderNo, METHOD.POST, param)
}
/**
 * 用户菜单
 */
export function menu() {
    return request(SystemUserMenuTree, METHOD.GET, {})
}

export default {
    save,
    menu,
    query,
    del,
    delall,
    saveOrderNo
}