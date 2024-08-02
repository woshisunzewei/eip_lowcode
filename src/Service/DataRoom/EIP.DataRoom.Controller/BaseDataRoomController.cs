/**************************************************************
* Copyright (C) 2022 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2022/01/12 22:40:15
* 文件名: 
* 描述: 
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Common.Controller;
using EIP.Common.Models.Paging;
using EIP.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace EIP.DataRoom.Controller
{
    /// <summary>
    /// 基础控制器
    /// </summary>
    [Route("system/[controller]/[action]")]
    public class BaseDataRoomController : BaseController
    {
        /// <summary>
        /// 返回分页后信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pagedResults"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected ActionResult JsonForGridPagingDataRoom<T>(OperateStatus<PagedResults<T>> pagedResults, QueryParam queryParam)
        {
            return Ok(new
            {
                code = pagedResults.Code,
                pagedResults.Msg,
                data = new
                {
                    current = queryParam.Current,
                    list = pagedResults.Data.Data,
                    size = queryParam.Size,
                    totalCount = pagedResults.Data.PagerInfo?.RecordCount ?? 0,
                }
            });
        }
    }
}