/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/5/29 15:57:28
* 文件名: DsDatasetFindDto
* 描述: 数据集表
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
    /// 数据集表查询参数
    /// </summary>
    public class DsDatasetFindInput : QueryParam
    {
        public string? datasetType { get; set; }

        public string? typeId { get; set; }
    }

    /// <summary>
    /// 数据集表查询输出
    /// </summary>
    public class DsDatasetFindOutput
    {
        public int id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 种类ID
        /// </summary>
        public int typeId { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 数据集类型（自定义数据集 custom、模型数据集model、原始数据集original、API数据集api、JSON数据集 json）
        /// </summary>
        public string datasetType { get; set; }

        /// <summary>
        /// 是否可编辑，0 不可编辑 1 可编辑
        /// </summary>
        public int editable { get; set; }

        /// <summary>
        /// 数据源ID
        /// </summary>
        public int? sourceId { get; set; }

        /// <summary>
        /// 是否对执行结构缓存 0 不缓存 1 缓存
        /// </summary>
        public int cache { get; set; }

        /// <summary>
        /// 数据集配置
        /// </summary>
        public object config { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<string> labelIds { get; set; }
    }

    /// <summary>
    /// 数据集表查询输出
    /// </summary>
    public class DsDatasetFindByIdOutput
    {
        public string id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 种类ID
        /// </summary>
        public string typeId { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 数据集类型（自定义数据集 custom、模型数据集model、原始数据集original、API数据集api、JSON数据集 json）
        /// </summary>
        public string datasetType { get; set; }

        /// <summary>
        /// 是否可编辑，0 不可编辑 1 可编辑
        /// </summary>
        public int editable { get; set; }

        /// <summary>
        /// 数据源ID
        /// </summary>
        public int? sourceId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<string> labelList { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<object> fields { get; set; }
    }

    /// <summary>
    /// 数据集表查询输出
    /// </summary>
    public class DsDatasetFindByIdFieldListOutput
    {
        public List<object> fieldList { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string tableName { get; set; }

    }

}