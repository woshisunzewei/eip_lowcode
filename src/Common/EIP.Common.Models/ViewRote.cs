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
using System;

namespace EIP.Common.Models
{
    /// <summary>
    /// 路由实体
    /// </summary>
    public class ViewRote
    {
        /// <summary>
        /// 菜单Id
        /// </summary>
        public Guid? MenuId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; set; }
    }
}