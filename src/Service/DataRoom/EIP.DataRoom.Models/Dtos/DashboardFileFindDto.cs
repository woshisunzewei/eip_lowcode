/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/5/29 15:57:26
* 文件名: DashboardFileFindDto
* 描述: 文件表
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Common.Models.Paging;
using System;

namespace EIP.DataRoom.Models.Dtos
{
    /// <summary>
    /// 文件表查询参数
    /// </summary>
    public class DashboardFileFindInput : QueryParam
    {

    }

    /// <summary>
    /// 文件表查询输出
    /// </summary>
    public class DashboardFileFindOutput
    {
        public string id { get; set; }
        /// <summary>
        /// 模块/类型
        /// </summary>
        public string module { get; set; }

        /// <summary>
        /// 原文件名
        /// </summary>
        public string originalName { get; set; }

        /// <summary>
        /// 新文件名
        /// </summary>
        public string newName { get; set; }

        /// <summary>
        /// 后缀名(如: txt、png、doc、java等)
        /// </summary>
        public string extension { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string path { get; set; }

        /// <summary>
        /// 访问路径
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public int size { get; set; }

        /// <summary>
        /// 下载次数
        /// </summary>
        public int downloadCount { get; set; }

        /// <summary>
        /// 上传用户
        /// </summary>
        public string user_name { get; set; }

        /// <summary>
        /// 桶名称
        /// </summary>
        public string bucket { get; set; }


    }
}