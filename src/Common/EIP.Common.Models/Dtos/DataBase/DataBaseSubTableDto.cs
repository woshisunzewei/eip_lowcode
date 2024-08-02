/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2018/11/21 14:13:29
* 文件名: DataBaseSubTableDto
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

namespace EIP.Common.Models.Dtos.DataBase
{
    /// <summary>
    /// 
    /// </summary>
    public class DataBaseSubTableDto : QueryParam
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid ConfigId { get; set; }

        /// <summary>
        /// 表
        /// </summary>
        public string Table { get; set; }

        /// <summary>
        /// Id集合
        /// </summary>
        public string Id { get; set; }

    }
}
