/**************************************************************
* Copyright (C) www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2018/11/15 15:34:01
* 文件名: SystemDataBaseFindByIdBusinessDataInput
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
    /// 根据生成Id和主键Id获取数据
    /// </summary>
    public class SystemDataBaseFindBusinessDataByIdInput : IdInput
    {
        /// <summary>
        /// 代码生成Id
        /// </summary>
        public Guid ConfigId { get; set; }
    }
}
