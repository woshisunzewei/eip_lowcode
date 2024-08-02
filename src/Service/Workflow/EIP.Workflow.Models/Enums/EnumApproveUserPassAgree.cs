namespace EIP.Workflow.Models.Enums
{
    /// <summary>
    /// 通过策略
    /// </summary>
    public enum EnumApproveUserPassAgree
    {
        /// <summary>
        /// 一人同意即可
        /// </summary>
        一人同意即可 = 1,
        /// <summary>
        /// 所有人必须同意
        /// </summary>
        所有人必须同意 = 2,
        /// <summary>
        /// 通过百分比
        /// </summary>
        通过百分比 = 3
    }
}