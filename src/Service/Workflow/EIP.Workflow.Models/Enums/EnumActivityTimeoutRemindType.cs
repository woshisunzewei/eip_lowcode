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
namespace EIP.Workflow.Models.Enums
{
    /// <summary>
    /// 审批节点超时提醒方式枚举: 邮件,短信,微信等
    /// </summary>
    public enum EnumActivityTimeoutRemindType
    {
        /// <summary>
        /// 
        /// </summary>
        邮件 = 2,
        /// <summary>
        /// 
        /// </summary>
        短信 = 4,
        /// <summary>
        /// 
        /// </summary>
        微信 = 6
    }
}
