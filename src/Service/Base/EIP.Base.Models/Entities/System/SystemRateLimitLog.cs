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
using EIP.Common.Models.Attributes;
using EIP.Common.Models.Attributes.MicroOrm;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EIP.Base.Models.Entities.System
{
    /// <summary>
    /// 
    /// </summary>
    [Table("System_RateLimitLog")]
    public class SystemRateLimitLog
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        [Key, Identity, JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// 限流Id
        /// </summary>
        public Guid RateLimitLogId { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 客户端Id
        /// </summary>
        public string RemoteIp { get; set; }

        /// <summary>
        /// 控制器名称
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// 操作名称
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// 请求内容大小
        /// </summary>
        public long RequestContentLength { get; set; } = 0;

        /// <summary>
        /// 请求类型 get or post
        /// </summary>
        public string RequestType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RequestData { get; set; }

        /// <summary>
        /// 当前请求Url信息
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 创建人员
        /// </summary>
        public Guid? CreateUserId { get; set; }

        /// <summary>
        /// 创建人员登录代码
        /// </summary>
        public string CreateUserCode { get; set; }

        /// <summary>
        /// 创建人员名字
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
