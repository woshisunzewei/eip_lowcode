namespace EIP.System.Models.Dtos.AgileDataLog
{
    /// <summary>
    /// [{filed:'字段',name:'字段名称',before:'处理前',after:'处理后'}]
    /// </summary>
    public class SystemAgileDataLogContent
    {
        /// <summary>
        /// 字段
        /// </summary>
        public string Filed { get; set; }

        /// <summary>
        /// 字段名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 处理前
        /// </summary>
        public object Before { get; set; }

        /// <summary>
        /// 处理后
        /// </summary>
        public object After { get; set; }

        /// <summary>
        /// 是否隐藏
        /// </summary>
        public bool Hidden { get; set; }
    }
}
