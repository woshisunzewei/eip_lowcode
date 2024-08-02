import {
    WorkflowCommentMy,
    WorkflowCommentSave,
    SystemFileUploadReurnPaths,
    SystemFileUploadDel
} from '@/services/api'
import { request, METHOD } from '@/utils/request'

/**
 * 
 */
export async function commentMy() {
    return request(WorkflowCommentMy, METHOD.POST, {})
}

/**
 * 
 */
export async function commentSave(param) {
    return request(WorkflowCommentSave, METHOD.POST, param)
}
/**
 * 上传
 */
export async function fileUpload(param) {
    return request(SystemFileUploadReurnPaths, METHOD.POST, param, {
        headers: {
            "Content-Type": "multipart/form-data;boundary=" + new Date().getTime(),
        }
    })
}
/**
 * 删除
 */
export async function fileDel(param) {
    return request(SystemFileUploadDel, METHOD.POST, param)
}
export default {
    commentMy,
    commentSave,
    fileUpload,
    fileDel
}