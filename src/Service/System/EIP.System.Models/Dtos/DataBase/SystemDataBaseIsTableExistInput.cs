/**************************************************************
* Copyright (C) www.eipflow.com 孙泽伟版权所有(盗版必究)
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
using EIP.Common.Models.Dtos;
using System;

namespace EIP.System.Models.Dtos.DataBase
{
    /// <summary>
    /// 表单是否存在
    /// </summary>
    public class SystemDataBaseIsTableExistInput : CheckSameValueInput
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid? MenuId { get; set; }

        /// <summary>
        /// 库Id
        /// </summary>
        public Guid ConfigId { get; set; }

        /// <summary>
        /// 表名称
        /// </summary>
        public string DataFromName { get; set; }

        /// <summary>
        /// 数据库Id
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
}