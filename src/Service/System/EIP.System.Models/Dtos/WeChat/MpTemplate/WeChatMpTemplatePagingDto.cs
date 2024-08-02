using EIP.Common.Models.Paging;
using System;

namespace EIP.System.Models.Dtos.WeChat.MpTemplate
{
    /// <summary>
    /// 
    /// </summary>
    public class WeChatMpTemplatePagingInput : QueryParam
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public class WeChatMpTemplatePagingOutput
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public Guid TemplateId { get; set; }

        /// <summary>
        /// 模版代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 模版名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 一级行业
        /// </summary>
        public string IndustryOne { get; set; }

        /// <summary>
        /// 二级行业
        /// </summary>
        public string IndustryTwo { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
