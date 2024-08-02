using EIP.Common.Models.Attributes.MicroOrm;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EIP.Base.Models.Entities.System
{
    /// <summary>
    /// 公告
    /// </summary>
    [Serializable]
	[Table("System_Notice")]
    public class SystemNotice
    {
        /// <summary>
        /// 消息Id
        /// </summary>		
        [Key, Identity, JsonIgnore]
        public long Id { get; set; }

        /// <summary>
        /// 
        /// </summary>		
        public Guid NoticeId{ get; set; }
       
        /// <summary>
        /// 标题
        /// </summary>		
		public string Title{ get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>		
        public DateTime PublicTime { get; set; } = DateTime.Now;
       
        /// <summary>
        /// 是否置顶
        /// </summary>		
		public bool? IsTop{ get; set; }
       
        /// <summary>
        /// 简介
        /// </summary>		
		public string Introduction{ get; set; }
       
        /// <summary>
        /// 消息
        /// </summary>		
		public string Msg{ get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int OrderNo { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>		
		public DateTime CreateTime { get; set; } = DateTime.Now;
       
        /// <summary>
        /// 创建人
        /// </summary>		
		public Guid CreateUserId{ get; set; }
       
        /// <summary>
        /// 创建人名称
        /// </summary>		
		public string CreateUserName{ get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>		
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>		
        public Guid? UpdateUserId { get; set; }

        /// <summary>
        /// 修改人名称
        /// </summary>		
        public string UpdateUserName { get; set; }

        /// <summary>
        /// 状态:0草稿,1发布
        /// </summary>
        public int Status { get; set; } = 1;

        /// <summary>
        /// 
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime EndTime { get; set; }
    } 
}
