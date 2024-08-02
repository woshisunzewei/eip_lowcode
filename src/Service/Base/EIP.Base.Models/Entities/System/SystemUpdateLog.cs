using EIP.Common.Models.Attributes.MicroOrm;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EIP.Base.Models.Entities.System
{
    [Serializable]
    [Table("System_UpdateLog")]
    public class SystemUpdateLog
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        [Key, Identity, JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// 主键Id
        /// </summary>
        public Guid UpdateLogId { get; set; }

        /// <summary>
        /// 更新日志内容
        /// </summary>		
		public string Msg { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }

    }
}
