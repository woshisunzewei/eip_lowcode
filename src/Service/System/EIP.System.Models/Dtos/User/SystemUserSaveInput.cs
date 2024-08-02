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
using Microsoft.AspNetCore.Http;
using System;

namespace EIP.System.Models.Dtos.User
{
    /// <summary>
    /// 保存用户
    /// </summary>
    public class SystemUserSaveInput
    {
        /// <summary>
        /// 人员Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 组织机构Id
        /// </summary>
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// 组织机构名称
        /// </summary>
        public string OrganizationName { get; set; }

        /// <summary>
        /// 用户登录名
        /// </summary>		
        public string Code { get; set; }

        /// <summary>
        /// 用户真实姓名
        /// </summary>		
        public string Name { get; set; }

        /// <summary>
        /// 密码
        /// </summary>		
        public string Password { get; set; } = "";

        /// <summary>
        /// 电话
        /// </summary>		
        public string Mobile { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 登录类型:0默认为普通用户
        /// </summary>
        public int LoginType { get; set; } = 0;

        /// <summary>
        /// 第一次访问时间
        /// </summary>		
        public DateTime? FirstVisitTime { get; set; }

        /// <summary>
        /// 最后访问时间
        /// </summary>		
        public DateTime? LastVisitTime { get; set; }

        /// <summary>
        /// 冻结
        /// </summary>
        public bool IsFreeze { get; set; } = false;
        
        /// <summary>
        /// 备注
        /// </summary>		
        public string Remark { get; set; }

        /// <summary>
        /// 是否为超级管理员
        /// </summary>
        public bool IsAdmin { get; set; } = false;

        /// <summary>
        /// 其他联系信息
        /// </summary>		
        public string OtherContactInformation { get; set; }

        /// <summary>
        /// 头像地址
        /// </summary>		
        public string HeadImage { get; set; }

        /// <summary>
        /// 性别:0男,1女
        /// </summary>
        public short? Sex { get; set; }

        /// <summary>
        /// 角色Ids
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// 用户关联组织架构Ids
        /// </summary>
        public string UserOrganizationIds { get; set; }

    }
}