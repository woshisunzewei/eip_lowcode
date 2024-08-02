namespace EIP.Agile.Models.Dtos.Event
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemAgileDoByApiInput
    {
        /// <summary>
        /// 
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 参数字符串
        /// </summary>
        public string Param { get; set; }

        /// <summary>
        /// 0:Post,1Get
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ContentType { get; set; } = "application/json";
    }
}
