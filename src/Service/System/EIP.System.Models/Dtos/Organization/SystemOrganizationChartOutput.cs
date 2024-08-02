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
using System.Collections.Generic;

namespace EIP.System.Models.Dtos.Organization
{
    /// <summary>
    /// 组织机构图形
    /// </summary>
    public class SystemOrganizationChartOutput
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 下级
        /// </summary>
        public IList<SystemOrganizationChartOutput> children { get; set; }
    }

}