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

namespace EIP.System.Models.Dtos.Menu
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemMenuFindAgileOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid ConfigId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PublicJson { get; set; } 

        /// <summary>
        /// 
        /// </summary>
        public string ColumnJson { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ConfigType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 菜单
        /// </summary>
        public Guid MenuId { get; set; }
    }

}