namespace EIP.DataRoom.Models.Dtos
{
    public class BigScreenTypeSaveInput
    {
        public int? id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int orderNum { get; set; }
    }
}
