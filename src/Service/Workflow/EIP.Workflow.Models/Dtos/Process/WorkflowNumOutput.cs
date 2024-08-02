namespace EIP.Workflow.Models.Dtos.Process
{
    /// <summary>
    /// 工作流数量查询
    /// </summary>
    public class WorkflowSearchNumOutput
    {
        /// <summary>
        /// 已办数量
        /// </summary>
        public int HaveDo { get; set; }
        
        /// <summary>
        /// 待办数量
        /// </summary>
        public int NeedDo { get; set; }

        /// <summary>
        /// 已发送数量
        /// </summary>
        public int HaveSend { get; set; }
    }
}
