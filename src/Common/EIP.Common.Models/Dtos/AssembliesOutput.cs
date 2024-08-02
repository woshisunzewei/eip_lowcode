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
namespace EIP.Common.Models.Dtos
{
    /// <summary>
    /// 程序集
    /// </summary>
    public class AssembliesOutput
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Clr运行时
        /// </summary>
        public string ClrVersion { get; set; }

        /// <summary>
        /// 位置路径
        /// </summary>
        public string Location { get; set; }
    }
}