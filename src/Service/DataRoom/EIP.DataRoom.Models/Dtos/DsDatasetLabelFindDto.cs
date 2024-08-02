/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/5/29 15:57:28
* 文件名: DsDatasetLabelFindDto
* 描述: 数据集与标签关联表
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
    /// 数据集与标签关联表查询参数
    /// </summary>
    public class DsDatasetLabelFindInput : QueryParam
    {
        
    }

    /// <summary>
    /// 数据集与标签关联表查询输出
    /// </summary>
    public class DsDatasetLabelFindOutput
    {
                /// <summary>
        /// 数据集ID
        /// </summary>
        public int? dataset_id { get; set; }

        /// <summary>
        /// 标签ID
        /// </summary>
        public int? label_id { get; set; }


    }
}