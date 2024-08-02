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
using EIP.Common.Core.Util;
using EIP.Common.Models.Dtos;
using System;

namespace EIP.System.Models.Dtos.DataBase
{
    /// <summary>
    /// 表
    /// </summary>
    public class SystemDataBaseTableDto : IdInput
    {
        /// <summary>
        /// 表名称
        /// </summary>

        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 是否来源敏捷开发
        /// </summary>
        public bool IsFromAgile { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid DataBaseId { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public string ConnectionType { get; set; } = ConfigurationUtil.GetDbConnectionType();

        /// <summary>
        /// 链接字符串
        /// </summary>
        public string ConnectionString { get; set; } = ConfigurationUtil.GetDbConnectionString();
    }

    /// <summary>
    /// 
    /// </summary>
    public class SystemDataBaseTableApiDto
    {
        /// <summary>
        /// POST/GET
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 是否分页
        /// </summary>
        public bool Paging { get; set; }
    }
}