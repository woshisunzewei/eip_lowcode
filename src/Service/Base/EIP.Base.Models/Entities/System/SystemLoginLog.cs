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
    /// System_LoginLog表实体类
    /// </summary>
    [Table("System_LoginLog")]
    public class SystemLoginLog
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        [Key, Identity, JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// 主键Id
        /// </summary>
        public Guid LoginLogId { get; set; }

        /// <summary>
        /// 客户端Id
        /// </summary>
        public string RemoteIp { get; set; }

        /// <summary>
        /// 客户端Ip对应地址
        /// </summary>
        public string RemoteIpAddress { get; set; }

        /// <summary>
        /// 浏览器信息
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// 操作系统
        /// </summary>
        public string OsInfo { get; set; }

        /// <summary>
        /// 浏览器信息
        /// </summary>
        public string Browser { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginTime { get; set; }

        /// <summary>
        /// 退出时间
        /// </summary>
        public DateTime? LoginOutTime { get; set; }

        /// <summary>
        /// 停留时间(分钟)
        /// </summary>
        public double? StandingTime { get; set; }

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

        /// <summary>
        /// 来源
        /// </summary>
        public string Source { get; set; }
    }
}
