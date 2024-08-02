/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2023/7/1 16:35:32
* 文件名: SystemDatasourceFindDto
* 描述: 数据源管理
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Common.Models.Paging;
using System;

namespace EIP.System.Models.Dtos.Datasource
{
    /// <summary>
    /// 数据源管理查询参数
    /// </summary>
    public class SystemDatasourceFindInput : QueryParam
    {
        
    }

    /// <summary>
    /// 数据源管理查询输出
    /// </summary>
    public class SystemDatasourceFindOutput
    {
                /// <summary>
        /// 
        /// </summary>
        public Guid DataSourceId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 数据源Id
        /// </summary>
        public string DataBaseName { get; set; }

        /// <summary>
        /// 配置信息
        /// </summary>
        public string Config { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int? OrderNo { get; set; }

        /// <summary>
        /// 冻结
        /// </summary>
        public bool? IsFreeze { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 数据使用数量
        /// </summary>
        public int DataSourceNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UpdateUserName { get; set; }


    }
}