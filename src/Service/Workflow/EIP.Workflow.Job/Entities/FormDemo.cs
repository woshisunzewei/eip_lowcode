using EIP.Common.Models.Attributes.MicroOrm;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EIP.Workflow.Job.Entities
{
    /// <summary>
    /// 测试表
    /// </summary>

    [Serializable]
    [Table("FormDemo")]
    public class FormDemo
    {
        /// <summary>
        /// 
        /// </summary>		
        [Key]
        [Identity]
        public long DemoId { get; set; }

        
    }
}
