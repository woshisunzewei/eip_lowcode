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

namespace EIP.System.Models.Dtos.Data
{
    /// <summary>
    /// 根据模块获取数据权限
    /// </summary>
    public class SystemDataFindInput : QueryParam
    {
        /// <summary>
        /// 模块Id
        /// </summary>
        public Guid? Id { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SystemDataFindOutput 
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid DataId { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>		
        public string Name { get; set; }

        /// <summary>
        /// 冻结
        /// </summary>
        public bool IsFreeze { get; set; } = false;

        /// <summary>
        /// 排序
        /// </summary>
        public int OrderNo { get; set; } = 0;

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = string.Empty;

        /// <summary>
        /// 模块名称
        /// </summary>
        public string MenuNames { get; set; }

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