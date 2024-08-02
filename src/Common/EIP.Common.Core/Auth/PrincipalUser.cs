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

namespace EIP.Common.Core.Auth
{
    /// <summary>
    /// 页面中需获取的用户信息类
    /// </summary>
    public class PrincipalUser
    {
        #region 基础实体

        /// <summary>
        /// 主键Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string ContactNumber { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 组织机构Id
        /// </summary>
        public Guid? OrganizationId { get; set; }

        /// <summary>
        /// 组织机构名称
        /// </summary>
        public string OrganizationName { get; set; }

        /// <summary>
        /// 登录后的Id值,退出时更新退出时间
        /// </summary>
        public Guid LoginId { get; set; }

        /// <summary>
        /// Ip
        /// </summary>
        public string RemoteIp { get; set; }

        /// <summary>
        /// Ip地址
        /// </summary>
        public string RemoteIpAddress { get; set; }

        /// <summary>
        /// 浏览器代理
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
        /// 来源
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 是否为超级管理员
        /// </summary>
        public bool IsAdmin { get; set; }
        #endregion
    }
}