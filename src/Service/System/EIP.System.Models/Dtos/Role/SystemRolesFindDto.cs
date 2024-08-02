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
using EIP.Common.Models.Paging;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EIP.System.Models.Dtos.Role
{
    /// <summary>
    /// 根据组织机构Id获取角色
    /// </summary>
    public class SystemRolesFindInput : QueryParam
    {
        /// <summary>
        /// 组织机构id
        /// </summary>
        public Guid? Id { get; set; }
    }

    /// <summary>
    /// 根据组织机构Id获取角色
    /// </summary>
    public class SystemRolesFindOutput
    {
        /// <summary>
        /// 角色id
        /// </summary>		
        public Guid RoleId { get; set; }

        /// <summary>
        /// 组织机构id
        /// </summary>		
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>		
        public string Name { get; set; }

        /// <summary>
        /// 状态,临时角色等
        /// </summary>		
        public short? State { get; set; }

        /// <summary>
        /// 是否允许删除
        /// </summary>		
        public bool CanbeDelete { get; set; }

        /// <summary>
        /// 排序
        /// </summary>		
        public int OrderNo { get; set; } = 0;

        /// <summary>
        /// 备注
        /// </summary>		
        public string Remark { get; set; }

        /// <summary>
        /// 冻结
        /// </summary>		
        public bool IsFreeze { get; set; }

        /// <summary>
        /// 组织机构名称
        /// </summary>
        [NotMapped]
        public string ParentIdsName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人员名称
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>		
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 修改人员名称
        /// </summary>
        public string UpdateUserName { get; set; }
    }
}