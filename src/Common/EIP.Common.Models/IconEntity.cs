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
namespace EIP.Common.Models
{
    /// <summary>
    /// 图标实体
    /// </summary>
    public class IconEntity
    {
        /// <summary>
        /// 图标大小
        /// </summary>
        public long Length { get; set; }
        /// <summary>
        /// 图标地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 图标名字
        /// </summary>
        public string Name { get; set; }
    }
}