namespace EIP.Workflow.Engine.Models.Dtos.ProcessInstance
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowEngineSubProcessDataDto
    {
        /// <summary>
        /// 子流程字段
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// 父流程字段
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        public string Type { get; set; }
    }
}