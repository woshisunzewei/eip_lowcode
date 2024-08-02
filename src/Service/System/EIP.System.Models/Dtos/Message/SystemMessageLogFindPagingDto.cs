/**************************************************************
* Copyright (C) www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2019/7/25 8:06:40
* 文件名: SystemMessageFindPagingDto
* 描述: 
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Common.Models.Paging;
using EIP.Common.Core.Util;
using Newtonsoft.Json;
using System;

namespace EIP.System.Models.Dtos.Message
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemMessageLogFindPagingInput : QueryParam
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 已经处理
        /// </summary>
        public bool HaveDo { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class SystemMessageLogFindPagingOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid MessageLogId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 打开地址
        /// </summary>
        public string OpenUrl { get; set; }
        /// <summary>
        /// 阅读数
        /// </summary>
        [JsonIgnore]
        public int ReadCount { get; set; }

        /// <summary>
        /// 是否阅读
        /// </summary>
        public bool IsRead => ReadCount > 0;

        /// <summary>
        /// 时间格式化
        /// </summary>
        public string CreateTimeFormat => DateTimeUtil.GetTime(CreateTime);
    }
}
