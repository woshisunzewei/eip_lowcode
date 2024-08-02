namespace EIP.DataRoom.Models.Dtos
{
    public class DsDatasetSaveInput
    {
        public int? id { get; set; }

        public object config { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }


        /// <summary>
        /// 种类ID
        /// </summary>
        public string typeId { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 数据集类型（自定义数据集 custom、模型数据集model、原始数据集original、API数据集api、JSON数据集 json）
        /// </summary>
        public string datasetType { get; set; }

        /// <summary>
        /// 模块编码
        /// </summary>
        public string? moduleCode { get; set; }

        /// <summary>
        /// 是否可编辑，0 不可编辑 1 可编辑
        /// </summary>
        public int editable { get; set; }

        /// <summary>
        /// 数据源ID
        /// </summary>
        public int? sourceId { get; set; }

        /// <summary>
        /// 是否对执行结构缓存 0 不缓存 1 缓存
        /// </summary>
        public int cache { get; set; }

        public List<int> labelIds { get; set; } = new List<int>();
    }
}
