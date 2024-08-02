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
    /// 连线类型
    /// </summary>
    public enum EnumLineType
    {
        /// <summary>
        /// 节点连线
        /// </summary>
        节点连线 = 2,
        /// <summary>
        /// 条件连线
        /// </summary>
        条件连线 = 4,
        /// <summary>
        /// 退回连线
        /// </summary>
        退回连线 = 6
    }

    /// <summary>
    /// 退回连线-策略-退回策略:
    /// </summary>
    public enum EnumLineReturnPolicy
    {
        /// <summary>
        /// 根据处理策略退回
        /// </summary>
        根据处理策略退回 = 2,
        /// <summary>
        /// 一人退回全部退回
        /// </summary>
        一人退回全部退回 = 4,
        /// <summary>
        /// 所人有退回才退回
        /// </summary>
        所人有退回才退回 = 6,
        /// <summary>
        /// 不能退回
        /// </summary>
        不能退回 = 8
    }
}