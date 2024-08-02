import {
    SystemMenu,
    SystemMenuSave,
    SystemMenuFindById,
    SystemMenuSaveMove
} from '@/services/api'
import { request, METHOD } from '@/utils/request'
/**
 * 树
 */
export function menuQuery(menuId = null, isAgileMenu = null) {
    var url = SystemMenu;
    if (menuId) {
        url += "?menuId=" + menuId + "&isAgileMenu=" + isAgileMenu
    }
    return request(url, METHOD.GET, {})
}

/**
 * 保存
 */
export async function save(form) {
    return request(SystemMenuSave, METHOD.POST, form)
}

/**
 * 根据Id获取
 */
export function findById(id) {
    return request(SystemMenuFindById + "/" + id, METHOD.GET, {})
}

/**
 * 保存
 */
export async function saveMove(form) {
    return request(SystemMenuSaveMove, METHOD.POST, form)
}
export default {
    save,
    findById,
    menuQuery,
    saveMove
}