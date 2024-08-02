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
using MiniExcelLibs.Attributes;
using System;

namespace EIP.System.Models.Dtos.User
{
    /// <summary>
    /// 用户Dto
    /// </summary>
    public class SystemUserOutput 
    {
        /// <summary>
        /// 人员Id
        /// </summary>	
        [ExcelColumn(Ignore = true)]
        public Guid UserId { get; set; }

        /// <summary>
        /// 组织机构Id
        /// </summary>
        [ExcelColumn(Ignore = true)]
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// 用户登录名
        /// </summary>		
        [ExcelColumn(Name = "登录名")]
        public string Code { get; set; }

        /// <summary>
        /// 用户真实姓名
        /// </summary>	
        [ExcelColumn(Name = "名称")]
        public string Name { get; set; }

        /// <summary>
        /// 电话
        /// </summary>		
        [ExcelColumn(Name = "手机号码")]
        public string Mobile { get; set; }

        /// <summary>
        /// 组织机构名称
        /// </summary>
        [ExcelColumn(Name = "组织机构名称")]
        public string ParentIdsName { get; set; }

        /// <summary>
        /// 组织机构Ids
        /// </summary>
        [ExcelColumn(Ignore = true)]
        public string ParentIds { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [ExcelColumn(Name = "邮箱")]
        public string Email { get; set; }

        /// <summary>
        /// 其他联系信息
        /// </summary>		
        [ExcelColumn(Name = "其他联系信息")]
        public string OtherContactInformation { get; set; }

        /// <summary>
        /// 登录类型:0默认为普通用户
        /// </summary>
        [ExcelColumn(Ignore = true)]
        public int LoginType { get; set; }

        /// <summary>
        /// 第一次访问时间
        /// </summary>		
        [ExcelColumn(Ignore = true)]
        public DateTime? FirstVisitTime { get; set; }

        /// <summary>
        /// 最后访问时间
        /// </summary>		
        [ExcelColumn(Name = "最后访问时间")]
        public DateTime? LastVisitTime { get; set; }

        /// <summary>
        /// 冻结
        /// </summary>
        [ExcelColumn(Name = "冻结")]
        public bool IsFreeze { get; set; }

        /// <summary>
        /// 备注
        /// </summary>		
        [ExcelColumn(Name = "备注")]
        public string Remark { get; set; }

        /// <summary>
        /// 状态
        /// </summary>	
        [ExcelColumn(Ignore = true)]
        public short Nature { get; set; }

        /// <summary>
        /// 是否为超级管理员
        /// </summary>
        [ExcelColumn(Ignore = true)]
        public bool IsAdmin { get; set; }
      
        /// <summary>
        /// 头像显示
        /// </summary>
        [ExcelColumn(Ignore = true)]
        public string HeadImage { get; set; }

        /// <summary>
        /// 是否存在
        /// </summary>
        [ExcelColumn(Ignore = true)]
        public bool Exist { get; set; }

        /// <summary>
        /// 组织机构名称
        /// </summary>		
        [ExcelColumn(Name = "归属组织")]
        public string OrganizationName { get; set; }

        /// <summary>
        /// 关联组织
        /// </summary>
        [ExcelColumn(Name = "关联组织")]
        public string UserOrganizationNames { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>		
        [ExcelColumn(Ignore = true)]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人员名称
        /// </summary>
        [ExcelColumn(Ignore = true)]
        public string CreateUserName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>		
        [ExcelColumn(Ignore = true)]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 修改人员名称
        /// </summary>
        [ExcelColumn(Ignore = true)]
        public string UpdateUserName { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        [ExcelColumn(Name = "角色")]
        public string Role { get; set; }

    }
}