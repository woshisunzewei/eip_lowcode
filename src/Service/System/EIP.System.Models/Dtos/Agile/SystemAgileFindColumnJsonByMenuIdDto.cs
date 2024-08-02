using System;

namespace EIP.System.Models.Dtos.Agile
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemAgileFindColumnJsonByMenuIdInput
    {
        /// <summary>
        /// 菜单Id
        /// </summary>
        public Guid MenuId { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SystemAgileFindColumnJsonByMenuIdOutput
    {
        /// <summary>
        /// 字段Json
        /// </summary>
        public string ColumnJson { get; set; }

        /// <summary>
        /// 数据来源
        /// </summary>
        public string DataFromName { get; set; }

        /// <summary>
        /// 数据来源类型,1表,2视图,3存储过程
        /// </summary>
        public short DataFrom { get; set; }

        /// <summary>
        /// 数据库Id
        /// </summary>
        public Guid DataBaseId { get; set; }

    }
}
