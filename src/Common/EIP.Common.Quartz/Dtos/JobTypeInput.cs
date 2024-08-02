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
namespace EIP.Common.Quartz.Dtos
{
    /// <summary>
    /// 作业类型
    /// </summary>
    public class JobTypeInput
    {
        /// <summary>
        /// 方法名
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 程序集名称
        /// </summary>
        public string NamespaceName{ get; set; }
    }
}