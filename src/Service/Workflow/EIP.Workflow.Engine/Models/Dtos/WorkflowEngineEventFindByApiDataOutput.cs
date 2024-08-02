namespace EIP.Workflow.Engine.Models.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowEngineEventDoByApiInput
    {
        /// <summary>
        /// Api路径
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 头
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// 请求方式:post，get
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 传递数据
        /// </summary>
        public WorkflowEngineEventDoByApiDataOutput Data { get; set; }
    }

    /// <summary>
    /// 工作流根据接口获取数据
    /// </summary>
    public class WorkflowEngineEventDoByApiDataOutput
    {
        /// <summary>
        /// 控件
        /// </summary>
        public IList<WorkflowEngineEventDoByApiControlsInput> Controls { get; set; }

        /// <summary>
        /// 当前任务信息
        /// </summary>
        public WorkflowEngineEventDoByApiTaskOutput Task { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowEngineEventDoByApiControlsInput
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 是否为单选
        /// </summary>
        public string IsSingle { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public string Default { get; set; }

    }
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowEngineEventDoByApiTaskOutput
    {
        /// <summary>
        /// 流程Id
        /// </summary>
        public Guid? ProcessId { get; set; }

        /// <summary>
        /// 实例Id
        /// </summary>
        public Guid? ProcessInstanceId { get; set; }

        /// <summary>
        /// 活动Id
        /// </summary>
        public Guid? ActivityId { get; set; }

        /// <summary>
        /// 活动类型
        /// </summary>
        public string Type { get; set; }
       
        /// <summary>
        /// 任务Id
        /// </summary>
        public Guid? TaskId { get; set; }

        /// <summary>
        /// 当前表单Id
        /// </summary>
        public Guid? FormId { get; set; }

        /// <summary>
        /// 自定义数据
        /// </summary>
        public string CustomData { get; set; }
    }
}