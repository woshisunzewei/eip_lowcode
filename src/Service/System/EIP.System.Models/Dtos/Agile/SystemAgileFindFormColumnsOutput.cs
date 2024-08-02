namespace EIP.System.Models.Dtos.Agile
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemAgileFindFormColumnsOutput
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 类型:子表subtable
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 可读
        /// </summary>
        public bool IsRead { get; set; } = true;

        /// <summary>
        /// 可写
        /// </summary>
        public bool IsWrite { get; set; } = true;

        /// <summary>
        /// 隐藏
        /// </summary>
        public bool IsHide { get; set; } = false;

        /// <summary>
        /// 可删除
        /// </summary>
        public bool IsDelete { get; set; } = true;

        /// <summary>
        /// 是否显示默认值
        /// </summary>
        public bool IsDefault { get; set; } = true;

        /// <summary>
        /// 验证
        /// </summary>
        public string Validator { get; set; }

        /// <summary>
        /// 子表权限
        /// </summary>
        public string SubtablePermission { get; set; }

        /// <summary>
        /// 子表字段
        /// </summary>
        public string SubtableColumns { get; set; }
    }
}
