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
using EIP.Common.Models.Dtos;
using System;

namespace EIP.System.Models.Dtos.DataBase
{
    /// <summary>
    /// 表
    /// </summary>
    public class SystemDataBaseTableDoubleWay :IdInput
    {
        /// <summary>
        /// 表名称
        /// </summary>

        public string Table { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string DataType { get; set; }
    }
}