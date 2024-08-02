/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/7/9 22:28:57
* 文件名: SystemSearchFindDto
* 描述: 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Common.Models.Paging;
using System;

namespace EIP.System.Models.Dtos.Search
{
    /// <summary>
    /// 查询参数
    /// </summary>
    public class SystemSearchFindInput : QueryParam
    {
        public Guid? MenuId { get; set; }

        public Guid? UserId { get; set; }
    }

    /// <summary>
    /// 查询输出
    /// </summary>
    public class SystemSearchFindOutput
    {
        /// <summary>
        /// 查询记录Id
        /// </summary>
        public Guid SearchId { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Config { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateTime { get; set; }
    }
}