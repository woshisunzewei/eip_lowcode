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
using EIP.Common.Core.Util;
using EIP.System.Models.Dtos.Permission;
using EIP.System.Models.Enums;
using System;
using System.Collections.Generic;

namespace EIP.System.Models.Dtos.User
{
    /// <summary>
    /// 用户详细情况Dto
    /// </summary>
    public class SystemUserDetailOutput
    {
        /// <summary>
        /// 用户登录名
        /// </summary>		
        public string Code { get; set; }

        /// <summary>
        /// 用户真实姓名
        /// </summary>		
        public string Name { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>		
        public string Email { get; set; }

        /// <summary>
        /// 电话
        /// </summary>		
        public string Mobile { get; set; }

        /// <summary>
        /// 其他联系信息
        /// </summary>		
        public string OtherContactInformation { get; set; }

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
        public bool IsFreeze { get; set; }

        /// <summary>
        /// 格式化冻结
        /// </summary>
        public string IsFreezeFormatter => IsFreeze ? "冻结" : "未冻结";

        /// <summary>
        /// 备注
        /// </summary>		
        public string Remark { get; set; }

        /// <summary>
        /// 状态
        /// </summary>		
        public short Nature { get; set; }

        /// <summary>
        /// 头像显示
        /// </summary>
        public string HeadImage { get; set; }

        /// <summary>
        /// 组织名称
        /// </summary>
        public string OrganizationName { get;set;}

        /// <summary>
        /// 角色
        /// </summary>
        public IList<SystemPrivilegeDetailListOutput> Role { get; set; }

        /// <summary>
        /// 组
        /// </summary>
        public IList<SystemPrivilegeDetailListOutput> Group { get; set; }

        /// <summary>
        /// 岗位
        /// </summary>
        public IList<SystemPrivilegeDetailListOutput> Post { get; set; }
    }
}