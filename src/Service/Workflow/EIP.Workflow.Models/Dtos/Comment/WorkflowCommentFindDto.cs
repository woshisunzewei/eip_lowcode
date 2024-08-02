/**************************************************************
* Copyright (C) 2022 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2022/01/12 22:40:15
* 文件名: 
* 描述: 
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Common.Models.Paging;
using System;

namespace EIP.Workflow.Models.Dtos.Comment
{
    /// <summary>
    /// 查询参数
    /// </summary>
    public class WorkflowCommentFindInput : QueryParam
    {
        /// <summary>
        /// 若为0则查询当前用户
        /// </summary>
        public int? Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid? UserId { get; set; }
    }

    /// <summary>
    /// 意见输出
    /// </summary>
    public class WorkflowCommentFindOutput
    {
        /// <summary>
        /// 
        /// </summary>		
        public Guid CommentId { get; set; }

        /// <summary>
        /// 意见
        /// </summary>		
        public string Content { get; set; }

        /// <summary>
        /// 类型:0管理员添加、1员工添加
        /// </summary>		
        public int Type { get; set; }

        /// <summary>
        /// 排序
        /// </summary>		
        public int OrderNo { get; set; } = 0;

        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 创建人员名字
        /// </summary>		
        public string CreateUserName { get; set; }
    }
}