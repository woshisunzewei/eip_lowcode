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
using EIP.Base.Models.Entities.System;

namespace EIP.System.Models.Dtos.Data
{
    /// <summary>
    /// 数据权限
    /// </summary>
    public class SystemDataOutput : SystemData
    {
        /// <summary>
        /// 模块名称
        /// </summary>
        public string MenuNames { get; set; }
    }
}