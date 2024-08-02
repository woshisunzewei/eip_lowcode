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
using EIP.Common.Models.Attributes.MicroOrm;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EIP.Base.Models.Entities.System
{
    /// <summary>
    /// System_Permission表实体类
    /// </summary>
    [Serializable]
    [Table("System_Permission")]
    public class SystemPermission
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        [Key, Identity, JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// 权限归属(0模块、1按钮等)
        /// </summary>		
        public short PrivilegeAccess { get; set; }

        /// <summary>
        /// 对应类型主键值(角色id,人员id,组id,岗位id,)
        /// </summary>		
        public Guid PrivilegeMasterValue { get; set; }

        /// <summary>
        /// 对应权限归属主键(模块id,按钮id等)
        /// </summary>		
        public Guid PrivilegeAccessValue { get; set; }

        /// <summary>
        /// 给予权限类型
        /// </summary>
        public short PrivilegeMaster { get; set; }

        /// <summary>
        /// 对应模块Id
        /// </summary>
        public Guid? PrivilegeMenuId { get; set; }
    }
}
