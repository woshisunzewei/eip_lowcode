namespace EIP.Workflow.Models.Enums
{
    /// <summary>
    /// 自动同意规则
    /// </summary>
    public enum EnumApproveUserAutoAgree
    {
        /// <summary>
        /// 处理人就是提交人
        /// </summary>
        处理人就是提交人 = 1,
        /// <summary>
        /// 处理人和上一步相同
        /// </summary>
        处理人和上一步相同 = 2,
        /// <summary>
        /// 处理人已经审批过
        /// </summary>
        处理人已经审批过 = 3
    }
}