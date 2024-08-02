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
    /// 当前流程状态
    /// </summary>
    public enum EnumProcessInstanceStatus
    {
        /// <summary>
        /// 正常
        /// </summary>
        处理中 = 0,

        /// <summary>
        /// 终止
        /// </summary>
        终止 = 2,

        /// <summary>
        /// 暂停
        /// </summary>
        暂停 = 4,

        /// <summary>
        /// 即将到期
        /// </summary>
        即将到期 = 6,

        /// <summary>
        /// 拒绝
        /// </summary>
        拒绝 = 8,

        /// <summary>
        /// 撤销
        /// </summary>
        撤销 = 10,

        /// <summary>
        /// 删除
        /// </summary>
        删除 = 12,

        /// <summary>
        /// 退回
        /// </summary>
        退回 = 14,

        /// <summary>
        /// 取消
        /// </summary>
        取消 = 16,

        /// <summary>
        /// 完成
        /// </summary>
        完成 = 100
    }
}