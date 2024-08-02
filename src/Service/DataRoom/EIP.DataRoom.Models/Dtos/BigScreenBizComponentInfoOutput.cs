namespace EIP.DataRoom.Models.Dtos
{
    public class BigScreenBizComponentInfoOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 业务组件中文名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 业务组件编码，唯一标识符
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 分组
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 封面图片文件路径
        /// </summary>
        public string coverPicture { get; set; }

        /// <summary>
        /// vue组件内容
        /// </summary>
        public string vueContent { get; set; }

        /// <summary>
        /// 组件配置内容
        /// </summary>
        public string settingContent { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int orderNum { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 模块编码
        /// </summary>
        public string moduleCode { get; set; }
    }
}
