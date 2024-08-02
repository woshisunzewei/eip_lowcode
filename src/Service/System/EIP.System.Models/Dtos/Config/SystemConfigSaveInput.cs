/**************************************************************
* Copyright (C) www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2019/7/15 7:41:44
* 文件名: SystemConfigSaveInput
* 描述: 
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Base.Models.Entities.System;
using System.Collections.Generic;

namespace EIP.System.Models.Dtos.Config
{
    /// <summary>
    /// 保存配置
    /// </summary>
    public class SystemConfigSaveInput
    {
        /// <summary>
        /// 配置项
        /// </summary>
        public IList<SystemConfig> Configs { get; set; }
    }
}
