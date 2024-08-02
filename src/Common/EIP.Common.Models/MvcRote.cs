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
    public class MvcRote
    {
        /// <summary>
        /// 控制器
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// 方法
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// 完整地址
        /// </summary>
        public string Url
        {
            get;set;
        }

        /// <summary>
        /// 开发者代码
        /// </summary>
        public string ByDeveloperCode { get; set; }

        /// <summary>
        /// 开发者时间
        /// </summary>
        public string ByDeveloperTime { get; set; }

        /// <summary>
        /// 备注描述
        /// </summary>
        public string Description { get; set; }

    }
}