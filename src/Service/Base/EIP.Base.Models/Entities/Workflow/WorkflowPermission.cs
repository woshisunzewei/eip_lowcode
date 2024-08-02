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

namespace EIP.Base.Models.Entities.Workflow
{
    /// <summary>
    /// 流程权限
    /// </summary>
    [Serializable]
    [Table("Workflow_Permission")]
    public class WorkflowPermission
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        [Key, Identity, JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// 流程Id
        /// </summary>
        public Guid ProcessId { get; set; }

        /// <summary>
        /// 类型:组织机构,人员,角色,组,岗位等对应Id
        /// </summary>
        public Guid PrivilegeMasterValue { get; set; }

        /// <summary>
        /// 类型:组织机构,人员,角色,组,岗位等
        /// </summary>
        public byte PrivilegeMaster { get; set; }
    }
}
