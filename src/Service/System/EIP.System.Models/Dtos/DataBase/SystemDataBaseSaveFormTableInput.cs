/**************************************************************
* Copyright (C) www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2018/11/9 16:40:27
* 文件名: SystemDataBaseSaveFormTableInput
* 描述: 
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Common.Core.Util;
using System;

namespace EIP.System.Models.Dtos.DataBase
{
    /// <summary>
    /// 创建表单
    /// </summary>
    public class SystemDataBaseSaveFormTableInput
    {
        /// <summary>
        /// 数据库连接Id
        /// </summary>
        public Guid ConfigId { get; set; }

        /// <summary>
        /// 修改前名称
        /// </summary>
        public string Param { get; set; }

        /// <summary>
        ///  表备注名称
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 当前代码
        /// </summary>
        public string DataFromName { get; set; }

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
