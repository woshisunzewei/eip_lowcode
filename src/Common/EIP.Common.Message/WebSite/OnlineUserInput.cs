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

using System;
using System.Collections.Generic;

namespace EIP.Common.Message.WebSite
{
    public class OnlineUsers
    {
        public static readonly IList<OnlineUserInput> Users = new List<OnlineUserInput>();
    }

    /// <summary>
    /// 用户在线
    /// </summary>
    public class OnlineUserInput
    {
        /// <summary>
        /// 连接Id
        /// </summary>
        public string ConnectionId { get; set; }

        /// <summary>
        /// 登录账户Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 登录账户
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 用户真实姓名
        /// </summary>		
        public string Name { get; set; }

        /// <summary>
        /// 组织机构名称
        /// </summary>
        public string OrganizationName { get; set; }

        /// <summary>
        /// Ip
        /// </summary>
        public string RemoteIp { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string RemoteIpAddress { get; set; }

        /// <summary>
        /// 客户端浏览器
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
        /// 上线时间
        /// </summary>
        public DateTime GoOnlineTime { get; set; }

        /// <summary>
        /// 登录Id
        /// </summary>
        public Guid LoginId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Sso { get; set; }
    }
}