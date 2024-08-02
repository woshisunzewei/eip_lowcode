namespace EIP.Workflow.Models.Enums
{
    /// <summary>
    /// 
    /// </summary>
    public enum EnumApproveUserStrategy
    {
        /// <summary>
        /// 列表中的第一处理人
        /// </summary>
        列表中的第一处理人 = 0,
        /// <summary>
        /// 发送给列表中的所有处理人
        /// </summary>
        发送给列表中的所有处理人 = 0,
        /// <summary>
        /// 弹出选择审批人
        /// </summary>
        弹出选择审批人 = 2
    }
}