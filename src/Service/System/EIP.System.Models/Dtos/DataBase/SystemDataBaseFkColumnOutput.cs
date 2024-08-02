/**************************************************************
* Copyright (C) www.eipflow.com 孙泽伟版权所有(盗版必究)
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

namespace EIP.System.Models.Dtos.DataBase
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemDataBaseFkColumnOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public string ThisTable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ThisColumn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OtherTable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OtherColumn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ConstraintName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Owner { get; set; }
    }
}