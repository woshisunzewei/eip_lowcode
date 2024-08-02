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
using EIP.System.Models.Enums;
using System;

namespace EIP.System.Models.Dtos.Organization
{
    /// <summary>
    /// 组织机构输出
    /// </summary>
    public class SystemOrganizationOutput 
    {
        public int Number { get; set; }

        /// <summary>
        /// 主键Id
        /// </summary>
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// 组织机构父id
        /// </summary>		
        public Guid? ParentId { get; set; }
      
        /// <summary>
        /// 名称
        /// </summary>		
        public string Name { get; set; }

        /// <summary>
        /// 简称
        /// </summary>		
        public string ShortName { get; set; } = string.Empty;

        /// <summary>
        /// 主负责人
        /// </summary>		
        public string MainSupervisor { get; set; } = string.Empty;

        /// <summary>
        /// 主负责人联系方式
        /// </summary>		
        public string MainSupervisorContact { get; set; } = string.Empty;

        /// <summary>
        /// 是否冻结
        /// </summary>		
        public bool IsFreeze { get; set; }

        /// <summary>
        /// 排序
        /// </summary>		
        public int OrderNo { get; set; } = 0;

        /// <summary>
        /// 备注
        /// </summary>		
        public string Remark { get; set; } = string.Empty;

        /// <summary>
        /// 性质:0集团,1公司,2分公司,3子公司,4部门
        /// </summary>
        public int? Nature { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public decimal? Latitude { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public decimal? Longitude { get; set; }

    }
}