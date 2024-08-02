namespace EIP.DataRoom.Models.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public class DashboardChartInput
    {
        /// <summary>
        /// 页面编码
        /// </summary>
        public string? pageCode { get; set; }
        /// <summary>
        /// 图表编码
        /// </summary>
        public string? chartCode { get; set; }
        /// <summary>
        /// 内部图表编码
        /// </summary>
        public string? innerChartCode { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string? type { get; set; }
        /// <summary>
        /// 当前页
        /// </summary>
        public int current { get; set; }
        /// <summary>
        /// 每页记录数
        /// </summary>
        public int size { get; set; }
        /// <summary>
        /// 树父节点id
        /// </summary>
        public string? treeParentId { get; set; }
        /// <summary>
        /// 图表配置，仅在根据配置临时获取数据时使用
        /// </summary>
        public DashboardChartOptionInput? chart { get; set; }
        /// <summary>
        /// 使用数据模型已有的关联关系进行联动查询
        /// </summary>
        public bool linkByRelation { get; set; } = false;
        /// <summary>
        /// 联动模型编码
        /// </summary>
        public string? relationModelCode { get; set; }
        /// <summary>
        /// 联动关联值
        /// </summary>
        public string? relationValue { get; set; }
        /// <summary>
        /// 过滤条件
        /// </summary>
        public List<DashboardChartFilter> filterList { get; set; } = new List<DashboardChartFilter>();

    }
    /// <summary>
    /// 
    /// </summary>
    public class DashboardChartFilter
    {
        /// <summary>
        /// 表格列
        /// </summary>
        public string? column { get; set; }
        /// <summary>
        /// 条件
        /// </summary>
        public string? @operator { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public List<string?> value { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DashboardChartOptionInput
    {
        /// <summary>
        /// 版本
        /// </summary>
        public string? version { get; set; }

        /// <summary>
        /// 图表唯一标识
        /// </summary>
        public string? code { get; set; }

        /// <summary>
        /// 数据源
        /// </summary>
        public DashboardChartOptionDataSourceInput dataSource { get; set; } = new DashboardChartOptionDataSourceInput();

        /// <summary>
        /// 显示图表标题
        /// </summary>
        /// <param name=""></param>
        public bool showTitle { get; set; } = true;

        /// <summary>
        /// 图表标题
        /// </summary>
        public string? title { get; set; }

        /// <summary>
        /// 组件类型
        /// </summary>
        /// <param name=""></param>
        public string? type { get; set; }

        /// <summary>
        /// 宽度
        /// </summary>
        public int w { get; set; } = 1;

        /// <summary>
        /// 高度
        /// </summary>
        public int h { get; set; } = 1;

        /// <summary>
        /// X坐标点
        /// </summary>
        public int x { get; set; } = 0;

        /// <summary>
        /// Y坐标点
        /// </summary>
        public int y { get; set; } = 1;

        /// <summary>
        /// Z图层
        /// </summary>
        public int z { get; set; }

        /// <summary>
        /// 分组标识
        /// </summary>
        public string? group { get; set; }

        /// <summary>
        /// 锁定
        /// </summary>
        public bool locked { get; set; } = false;

        /// <summary>
        /// 联动
        /// </summary>
        public Linkage linkage { get; set; } = new Linkage();

        /// <summary>
        /// 联动入参配置
        /// </summary>
        public List<InParam> inParams { get; set; } = new List<InParam>();
    }
    public class InParam
    {
        public string? name { get; set; }

        public string? code { get; set; }

    }
    public class Linkage
    {
        /// <summary>
        /// 联动执行的逻辑
        /// </summary>
        public Action action { get; set; } = new Action();

        /// <summary>
        /// 组件的唯一标识，用于知道和谁做联动
        /// </summary>
        public List<Component> components = new List<Component>();

    }

    public class Action
    {
        /// <summary>
        /// 联动类型
        /// </summary>
        public string? type;
        /// <summary>
        /// 联动执行的逻辑
        /// </summary>
        public string? script;
    }

    public class Component
    {
        /// <summary>
        /// 组件的唯一标识，用于知道和谁做联动
        /// </summary>
        public string? componentKey { get; set; }
        /// <summary>
        /// 使用数据模型已有的关联关系进行联动查询
        /// </summary>
        public bool linkByRelation { get; set; }
        /// <summary>
        /// 映射关系
        /// </summary>
        public List<Mapping> maps { get; set; } = new List<Mapping>();
    }

    public class Mapping
    {
        /// <summary>
        /// 源字段
        /// </summary>
        public string? sourceField { get; set; }
        /// <summary>
        /// 目标字段
        /// </summary>
        public string? targetField { get; set; }
        /// <summary>
        /// 查询规则
        /// </summary>
        public string? queryRule { get; set; }
    }

    public class DashboardChartOptionDataSourceInput
    {
        public string? businessKey { get; set; }
    }

    public class DashboardChartJson
    {

        public dynamic columnData { get; set; }

        public object data { get; set; }
    }

    public class DashboardChartOutput : DashboardChartJson
    {
        public bool executionByFrontend { get; set; }

        public string? sql { get; set; }
        public bool success { get; set; } = true;

        public int totalCount { get; set; } = 0;

        public int totalPage { get; set; } = 0;
    }
}
