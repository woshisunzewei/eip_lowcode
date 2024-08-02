/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/5/29 15:57:28
* 文件名: DsCategoryTreeFindDto
* 描述: 数据集种类树
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Common.Models.Paging;

namespace EIP.DataRoom.Models.Dtos
{
    /// <summary>
    /// 数据集种类树查询参数
    /// </summary>
    public class DsCategoryTreeFindInput
    {
        public string? type { get; set; }
    }

    /// <summary>
    /// 数据集种类树查询输出
    /// </summary>
    public class DsCategoryTreeFindOutput
    {
        /// <summary>
        /// id序列
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
        public int? parentId { get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
        public string parentName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }

        public List<DsCategoryTreeFindOutput> children { get; set; }
    }
}