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
using Microsoft.AspNetCore.Mvc.Filters;
namespace EIP.Common.Controller.Attribute
{
    public class CreateByAttribute : System.Attribute, IFilterMetadata
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">开发人员编码</param>
        public CreateByAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">开发人员编码</param>
        /// <param name="time">开发时间</param>
        public CreateByAttribute(string name, string time)
        {
            Name = name;
            Time = time;
        }

        /// <summary>
        /// 开发人员编码
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 开发时间
        /// </summary>
        public string Time { get; set; }
    }
}