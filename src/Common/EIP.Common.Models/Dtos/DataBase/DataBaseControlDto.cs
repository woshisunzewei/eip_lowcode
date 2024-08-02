/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2018/11/16 15:17:34
* 文件名: DataBaseControl
* 描述: 
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using System;
using System.Collections.Generic;

namespace EIP.Common.Models.Dtos.DataBase
{
    /// <summary>
    /// 表单控件输出值
    /// </summary>
    public class DataBaseControlsOutput
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 是否单选
        /// </summary>
        public bool IsSingle { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }

    /// <summary>
    /// 子表
    /// </summary>
    public class DataBaseBatchIdsOutput
    {
        /// <summary>
        /// 关联主表Id
        /// </summary>
        public Guid RelationId { get; set; }

        /// <summary>
        /// 附表主键
        /// </summary>
        public Guid BatchId { get; set; }
    }
    /// <summary>
    /// 子表属性
    /// </summary>
    public class DataBaseBatchControlDetailOutput
    {
        /// <summary>
        /// 明细
        /// </summary>
        public IList<DataBaseControlsOutput> Detail { get; set; }
    }
}
