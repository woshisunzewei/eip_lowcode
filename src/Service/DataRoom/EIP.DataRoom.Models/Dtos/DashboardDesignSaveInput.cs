namespace EIP.DataRoom.Models.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public class DashboardDesignSaveInput
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public int? id { get; set; }

        /// <summary>
        /// 仪表盘名称
        /// </summary>
        public string? name { get; set; }

        /// <summary>
        /// 仪表盘唯一标识符
        /// </summary>
        public string? code { get; set; }

        /// <summary>
        /// 仪表盘页图标
        /// </summary>
        public string? icon { get; set; }

        /// <summary>
        /// 仪表盘首页封面
        /// </summary>
        public string? coverPicture { get; set; }

        /// <summary>
        ///  仪表盘页颜色6+
        /// </summary>
        public string? iconColor { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int orderNum { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? remark { get; set; }

        /// <summary>
        /// 仪表盘整体样式
        /// </summary>
        public string? style { get; set; }

        /// <summary>
        /// 父节点编码
        /// </summary>
        public string? parentCode { get; set; }

        /// <summary>
        /// 图表数据
        /// </summary>
        public List<object> chartList { get; set; } = new List<object>();

        /// <summary>
        /// 页面模板id
        /// </summary>
        public string? pageTemplateId { get; set; }

        /// <summary>
        /// 页面类型
        /// </summary>
        public string? type { get; set; }

        /// <summary>
        /// 表单属性，只有表单类型时才有这个值
        /// </summary>
        public PageConfig pageConfig { get; set; } = new PageConfig();
    }
    /// <summary>
    /// 
    /// </summary>
    public class PageConfig
    {
        /// <summary>
        /// 背景色
        /// </summary>
        public string? bgColor { get; set; }
        /// <summary>
        /// 背景图
        /// </summary>
        public string? bg { get; set; }
        /// <summary>
        /// 组件标题线宽度
        /// </summary>
        public string? titleLineWidth { get; set; }
        /// <summary>
        /// 组件标题线颜色
        /// </summary>
        public string? titleLineColor { get; set; }
        /// <summary>
        /// 组件标题字体大小
        /// </summary>
        public string? titleFontSize { get; set; }
        /// <summary>
        /// 组件标题字体颜色
        /// </summary>
        public string? titleFontColor { get; set; }
        /// <summary>
        /// 定时刷新配置
        /// </summary>
        public List<RefreshConfig> refreshConfig { get; set; } = new List<RefreshConfig>();
    }

    /// <summary>
    /// 定时刷新配置
    /// </summary>
    public class RefreshConfig
    {
        /// <summary>
        /// 组件编码
        /// </summary>

        public string? code { get; set; }

        /// <summary>
        /// 刷新时间，单位秒
        /// </summary>
        public int time { get; set; }

    }

}
