using EIP.Common.Models.Paging;
using System;

namespace EIP.System.Models.Dtos.Notice
{
    /// <summary>
    /// 查询公告
    /// </summary>
    public class SystemNoticeFindPagingInput : QueryParam
    {
       
    }

    /// <summary>
    /// 查询公告
    /// </summary>
    public class SystemNoticeFindPagingOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid NoticeId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>		
        public string Title { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>		
        public DateTime PublicTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 是否置顶
        /// </summary>		
        public bool? IsTop { get; set; }

        /// <summary>
        /// 简介
        /// </summary>		
        public string Introduction { get; set; }

        /// <summary>
        /// 消息
        /// </summary>		
        public string Msg { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int OrderNo { get; set; }

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

        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人员名称
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>		
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 修改人员名称
        /// </summary>
        public string UpdateUserName { get; set; }
    }
}