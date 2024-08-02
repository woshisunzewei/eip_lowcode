namespace EIP.DataRoom.Models.Dtos
{
    public class DsLabelSaveInput
    {
        public int? id { get; set; }
        /// <summary>
        /// 标签名称
        /// </summary>
        public string labelName { get; set; }

        /// <summary>
        /// 标签类型
        /// </summary>
        public string labelType { get; set; }

        /// <summary>
        /// 标签描述
        /// </summary>
        public string labelDesc { get; set; }

    }
}
