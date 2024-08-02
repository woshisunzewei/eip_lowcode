/**************************************************************
* Copyright (C) www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2018/11/16 17:50:33
* 文件名: SystemDataBaseDeleteBusinessDataInput
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
    /// 删除业务数据
    /// </summary>
    public class SystemDataBaseDeleteBusinessDataInput : IdInput<string>
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid ConfigId { get; set; }
    }
}
