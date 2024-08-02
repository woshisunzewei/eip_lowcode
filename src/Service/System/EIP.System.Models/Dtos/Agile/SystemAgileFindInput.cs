using EIP.Common.Models.Paging;
using System;

namespace EIP.System.Models.Dtos.Agile
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemAgileFindInput : QueryParam
    {
        /// <summary>
        /// 生成类型:1列表配置,2表单配置
        /// </summary>
        public short? ConfigType { get; set; }

        /// <summary>
        /// 表单类型:1设计器,2自定义
        /// </summary>
        public short? FormCategory { get; set; }

        /// <summary>
        /// 配置Id
        /// </summary>
        public Guid? ConfigId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? MenuIdNull { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid? MenuId { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SystemAgileFindOutput
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid ConfigId { get; set; }

        /// <summary>
        /// 表单名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DataFrom { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DataFromName { get; set; }

        /// <summary>
        /// 是否冻结
        /// </summary>
        public bool IsFreeze { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int OrderNo { get; set; }

        /// <summary>
        /// 表单类型:1设计器,2自定义
        /// </summary>
        public int? FormCategory { get; set; }

        /// <summary>
        /// 关联编辑配置Id
        /// </summary>
        public Guid? EditConfigId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 创建人员名称
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 修改人员名称
        /// </summary>
        public string UpdateUserName { get; set; }

        /// <summary>
        /// 缩略图
        /// </summary>
        public string Thumbnail { get; set; }
    }
}