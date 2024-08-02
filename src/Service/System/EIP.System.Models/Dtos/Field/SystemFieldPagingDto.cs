/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2018/10/30 22:40:15
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

namespace EIP.System.Models.Dtos.Field
{
    /// <summary>
    /// 系统字段分页参数
    /// </summary>
    public class SystemFieldInput : QueryParam
    {
        /// <summary>
        /// 模块
        /// </summary>
        public Guid? MenuId { get; set; }

        /// <summary>
        /// 是否显示隐藏
        /// </summary>
        public bool IsShowHidden { get; set; }
    }

    /// <summary>
    /// 字段Dto
    /// </summary>
    public class SystemFieldOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid FieldId { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// 字段名称
        /// </summary>		
		public string Name { get; set; }

        /// <summary>
        /// 查询sql字段
        /// </summary>		
        public string SqlField { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>		
		public string Label { get; set; }

        /// <summary>
        /// 排序名称
        /// </summary>		
        public string Index { get; set; }

        /// <summary>
        /// 显示列宽
        /// </summary>		
        public int? Width { get; set; }

        /// <summary>
        /// 对齐方式
        /// </summary>		
        public string Align { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>		
        public bool Hidden { get; set; }

        /// <summary>
        /// 列宽是否重新计算
        /// </summary>		
        public bool Fixed { get; set; }

        /// <summary>
        /// 自定义转换
        /// </summary>		
        public string Formatter { get; set; }

        /// <summary>
        /// 排序类型
        /// </summary>		
        public string Sorttype { get; set; }

        /// <summary>
        /// 排序
        /// </summary>		
        public int OrderNo { get; set; } = 0;

        /// <summary>
        /// 备注
        /// </summary>		
        public string Remark { get; set; }

        /// <summary>
        /// 导出/打印
        /// </summary>		
        public bool CanbeDerive { get; set; }

        /// <summary>
        /// 是否冻结
        /// </summary>		
        public bool IsFreeze { get; set; }

        /// <summary>
        /// 只读
        /// </summary>		
        public bool ReadOnly { get; set; }
    }
}