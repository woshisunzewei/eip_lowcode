using System;
using System.Collections.Generic;

namespace EIP.System.Models.Dtos.DataBase
{
    /// <summary>
    /// 导入
    /// </summary>
    public class SystemDataBaseImportInput
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid ConfigId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 用户名代码
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户组织结构Id
        /// </summary>
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// 用户组织结构名称
        /// </summary>
        public string OrganizationName { get; set; }

        /// <summary>
        /// 导入表
        /// </summary>
        public IEnumerable<IDictionary<string, object>> Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<SystemDataBaseImportColsInput> Clos { get; set; } = new List<SystemDataBaseImportColsInput>();

        /// <summary>
        /// 清空上传0，累加上传1
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 条件字段
        /// </summary>
        public string WhereField { get; set; }
        /// <summary>
        /// 条件值
        /// </summary>
        public string WhereValue { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SystemDataBaseImportColsInput
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 样式
        /// </summary>
        public List<SystemDataBaseReportFindBusinessDataColsStyleInput> Style { get; set; }
    }
}
