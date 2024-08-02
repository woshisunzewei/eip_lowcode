namespace EIP.DataRoom.Models.Dtos
{
    public class BigScreenByCodeOutput
    {
        public int id { get; set; }
        /// <summary>
        /// 页面中文名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 页面编码，页面唯一标识符
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 封面图片文件路径
        /// </summary>
        public string coverPicture { get; set; }

        public string icon { get; set; }

        public string iconColor { get; set; }

        public bool isPreview { get; set; }

        public int isPublish { get; set; } = 0;

        /// <summary>
        /// 页面类型
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 表单属性，只有表单类型时才有这个值
        /// </summary>
        public object pageConfig { get; set; }

        /// <summary>
        /// 父级目录编码
        /// </summary>
        public string parentCode { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int orderNum { get; set; }

        /// <summary>
        /// 备忘
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 所属应用编码
        /// </summary>
        public string appCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<object> chartList { get; set; } = new List<object>();

        public string pageTemplateId { get; set; }

        public string style { get; set; }
    }
}
