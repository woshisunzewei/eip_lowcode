namespace EIP.DataRoom.Models.Dtos
{
    public class BigScreenPageSaveInput
    {
        public int? id { get; set; }
        /// <summary>
        /// 页面中文名称
        /// </summary>
        public string? name { get; set; }

        /// <summary>
        /// 页面编码，页面唯一标识符
        /// </summary>
        public string? code { get; set; }

        /// <summary>
        /// 封面图片文件路径
        /// </summary>
        public string? coverPicture { get; set; }

        /// <summary>
        /// 页面类型
        /// </summary>
        public string? type { get; set; }

        /// <summary>
        /// 父级目录编码
        /// </summary>
        public string? parentCode { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int orderNum { get; set; }

        /// <summary>
        /// 备忘
        /// </summary>
        public string? remark { get; set; }

        /// <summary>
        /// 所属应用编码
        /// </summary>
        public string? appCode { get; set; }

        /// <summary>
        /// 图表数据
        /// </summary>
        public List<object> chartList { get; set; } = new List<object>();

        /// <summary>
        /// 表单属性，只有表单类型时才有这个值
        /// </summary>
        public BigScreenPagePageConfig pageConfig { get; set; } = new BigScreenPagePageConfig();
    }
    /// <summary>
    /// 
    /// </summary>
    public class BigScreenPagePageConfig
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
        public object? cacheDataSets { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? customTheme { get; set; }
        /// <summary>
        /// 组件标题字体大小
        /// </summary>
        public string? fitMode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int h { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? lightBg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? lightBgColor { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int opacity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int w { get; set; }

        /// <summary>
        /// 定时刷新配置
        /// </summary>
        public List<BigScreenPageRefreshConfig> refreshConfig { get; set; } = new List<BigScreenPageRefreshConfig>();
    }

    /// <summary>
    /// 定时刷新配置
    /// </summary>
    public class BigScreenPageRefreshConfig
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
