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
    /// 流程状态
    /// </summary>
    public enum EnumProcessStatu
    {
        /// <summary>
        /// 处理中
        /// </summary>
        处理中 = 2,
        /// <summary>
        /// 已完成
        /// </summary>
        已完成 = 4,
        /// <summary>
        /// 暂停中
        /// </summary>
        暂停中 = 6
    }

    /// <summary>
    /// 等级
    /// </summary>
    public enum EnumProcessUrgency
    {
        /// <summary>
        /// 正常
        /// </summary>
        正常 = 2,
        /// <summary>
        /// 重要
        /// </summary>
        重要 = 4,
        /// <summary>
        /// 紧急
        /// </summary>
        紧急 = 6
    }
}