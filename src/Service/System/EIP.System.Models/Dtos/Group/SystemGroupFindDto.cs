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

namespace EIP.System.Models.Dtos.Group
{
    /// <summary>
    /// 根据组织机构Id查询组
    /// </summary>
    public class SystemGroupFindInput : QueryParam
    {
        /// <summary>
        /// 组织机构Id
        /// </summary>
        public Guid? Id { get; set; }

    }

    /// <summary>
    /// 根据组织机构Id查询组
    /// </summary>
    public class SystemGroupFindOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid GroupId { get; set; }

        /// <summary>
        /// 组织机构Id
        /// </summary>		
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// 组名称
        /// </summary>		
        public string Name { get; set; }

        /// <summary>
        /// 组类型:0平台、1个人
        /// </summary>		
        public short BelongTo { get; set; }

        /// <summary>
        /// 归属组归属人员的Id
        /// </summary>		
        public Guid? BelongToUserId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>		
        public short? State { get; set; }

        /// <summary>
        /// 冻结
        /// </summary>		
        public bool IsFreeze { get; set; }

        /// <summary>
        /// 排序
        /// </summary>		
        public int OrderNo { get; set; }

        /// <summary>
        /// 备注
        /// </summary>		
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人员名称
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 修改人员名称
        /// </summary>
        public string UpdateUserName { get; set; }

        /// <summary>
        /// 组织机构名称
        /// </summary>
        public string OrganizationName { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public  string ParentIdsName { get; set; }
    }
}