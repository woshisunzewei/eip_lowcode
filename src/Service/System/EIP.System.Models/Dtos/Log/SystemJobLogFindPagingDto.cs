/**************************************************************
* Copyright (C) 2018 www.sf-info.cn 盛峰版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 3365690)
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
using EIP.Common.Models.Paging;
using System;

namespace EIP.System.Models.Dtos.Log
{
    /// <summary>
    /// 作业日志分页查询
    /// </summary>
    public class SystemJobLogFindPagingInput : QueryParam
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid? Correlation { get; set; }
    }

    /// <summary>
    /// 作业日志分页查询
    /// </summary>
    public class SystemJobLogFindPagingOutput 
    {
        /// <summary>
        /// 
        /// </summary>		
		public string Message { get; set; }

        /// <summary>
        /// 
        /// </summary>		
        public DateTime CreateTime { get; set; }
    }
}