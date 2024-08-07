/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2018/10/30 22:40:15
* 文件名: 
* 描述: 
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Common.Core.Extension;
using EIP.Common.Models.Paging;
using System;

namespace EIP.System.Models.Dtos.Log
{
    /// <summary>
    /// 异常日志分页输入查询
    /// </summary>
    public class SystemExceptionLogFindPagingInput : QueryParam
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime BeginCreateTime =>
            CreateTime.IsNullOrEmpty() ? DateTime.Now : Convert.ToDateTime(CreateTime.Trim().Split("至")[0]).ToUniversalTime();

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndCreateTime =>
            CreateTime.IsNullOrEmpty() ? DateTime.Now : Convert.ToDateTime(CreateTime.Trim().Split("至")[1]).ToUniversalTime();
    }
}