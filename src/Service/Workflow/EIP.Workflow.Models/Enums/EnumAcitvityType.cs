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

using System.ComponentModel;

namespace EIP.Workflow.Models.Enums
{
    /// <summary>
    /// 活动类型
    /// </summary>
    public enum EnumAcitvityType
    {
        /// <summary>
        /// 
        /// </summary>
        [Description("begin")]
        开始 = 2,
        /// <summary>
        /// 
        /// </summary>
        [Description("task")]
        审批 = 4,
        /// <summary>
        /// 
        /// </summary>
        [Description("subprocess")]
        子流程 = 6,
        /// <summary>
        /// 
        /// </summary>
        [Description("fork")]
        条件 = 8,
        /// <summary>
        /// 
        /// </summary>
        [Description("join")]
        合并 = 10,
        /// <summary>
        /// 
        /// </summary>
        [Description("understanding")]
        知会 = 12,

        /// <summary>
        /// 
        /// </summary>
        [Description("invitationread")]
        阅示 = 14,

        /// <summary>
        /// 
        /// </summary>
        [Description("add")]
        加签 = 16,

        /// <summary>
        /// 
        /// </summary>
        [Description("end")]
        结束 = 100
    }
}
