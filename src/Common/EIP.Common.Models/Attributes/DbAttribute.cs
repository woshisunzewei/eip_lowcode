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

namespace EIP.Common.Models.Attributes
{
    /// <summary>
    /// DB属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
    public class DbAttribute : BaseAttribute
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="name"></param>
        public DbAttribute(string name)
        {
            Name = name;
        }
        /// <summary>
        /// 数据库名称
        /// </summary>
        public string Name { get; set; }
    }
}