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
    /// 紧急程度:正常 重要 紧急
    /// </summary>
    public enum EnumProcessInstanceUrgency
    {
        /// <summary>
        /// 正常
        /// </summary>
        正常 = 0,
        /// <summary>
        /// 重要
        /// </summary>
        重要 = 2,
        /// <summary>
        /// 紧急
        /// </summary>
        紧急 = 4
    }
}