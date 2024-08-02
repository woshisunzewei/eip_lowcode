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
    /// 活动状态
    /// </summary>
    public enum EnumActivityStatus
    {
        /// <summary>
        /// 
        /// </summary>
        等待 = 2,
        /// <summary>
        /// 
        /// </summary>
        正在处理 = 4,
        /// <summary>
        /// 
        /// </summary>
        完成 = 6,
        /// <summary>
        /// 
        /// </summary>
        暂停 = 8,
        /// <summary>
        /// 
        /// </summary>
        撤销 = 10,
        /// <summary>
        /// 
        /// </summary>
        打回 = 12,
        /// <summary>
        /// 
        /// </summary>
        取消 = 14
    }

}
