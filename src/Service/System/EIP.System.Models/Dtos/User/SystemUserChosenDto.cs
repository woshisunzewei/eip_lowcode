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

namespace EIP.System.Models.Dtos.User
{
    /// <summary>
    /// 获取所有未冻结用户
    /// </summary>
    public class SystemUserChosenOutput
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 用户帐号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 归属组织机构
        /// </summary>
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// 归属组织机构
        /// </summary>
        public string OrganizationName { get; set; }

        /// <summary>
        /// 是否已存在
        /// </summary>
        public bool Exist { get; set; }

    }
}