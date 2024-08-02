/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2020/2/9 19:15:39
* 文件名: SystemUserImportDto
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
    /// 人员导入
    /// </summary>

    public class SystemUserImportInput
    {
        /// <summary>
        /// 
        /// </summary>
        [ExcelColumn(Name = "帐号")]
        public string Code { get; set; }

        /// <summary>
        /// 用户真实姓名
        /// </summary>	
        [ExcelColumn(Name = "用户真实姓名")]
        public string Name { get; set; }

        /// <summary>
        /// 电话
        /// </summary>		
        [ExcelColumn(Name = "电话")]
        public string Mobile { get; set; }

        /// <summary>
        /// 组织机构名称
        /// </summary>
        [ExcelColumn(Name = "组织机构名称")]
        public string ParentIdsName { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [ExcelColumn(Name = "邮箱")]
        public string Email { get; set; }

        /// <summary>
        /// QQ
        /// </summary>		
        [ExcelColumn(Name = "QQ")]
        public string Qq { get; set; }

        /// <summary>
        /// 微信号
        /// </summary>	
        [ExcelColumn(Name = "微信号")]
        public string WeChat { get; set; }

        /// <summary>
        /// 办公电话
        /// </summary>		
        [ExcelColumn(Name = "办公电话")]
        public string OfficeMobile { get; set; }

        /// <summary>
        /// 其他联系信息
        /// </summary>		
        [ExcelColumn(Name = "其他联系信息")]
        public string OtherContactInformation { get; set; }

        /// <summary>
        /// 角色
        /// </summary>		
        [ExcelColumn(Name = "角色")]
        public string Role { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class SystemUserImportUserRole
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid RoleId { get; set; }
    }

}
