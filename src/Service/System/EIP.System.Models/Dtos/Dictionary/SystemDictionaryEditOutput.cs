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
using System;

namespace EIP.System.Models.Dtos.Dictionary
{
    /// <summary>
    /// 字典输出
    /// </summary>
    [Serializable]
    public class SystemDictionaryEditOutput
    {
        /// <summary>
        /// 主键
        /// </summary>		
        public Guid DictionaryId { get; set; }

        /// <summary>
        /// 父级id
        /// </summary>		
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 字典代码
        /// </summary>		
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>		
        public string Name { get; set; }

        /// <summary>
        /// 值
        /// </summary>		
        public string Value { get; set; }

        /// <summary>
        /// 是否允许删除(系统默认配置字段不允许删除)
        /// </summary>		
        public bool CanbeDelete { get; set; }

        /// <summary>
        /// 排序
        /// </summary>		
        public int OrderNo { get; set; }=0;

        /// <summary>
        /// 备注
        /// </summary>		
        public string Remark { get; set; }

        /// <summary>
        /// 上级所有字符串方便查询
        /// </summary>		
        public string ParentIds { get; set; }

        /// <summary>
        /// 父级名称
        /// </summary>
        public string ParentName { get; set; }

        /// <summary>
        /// 父级代码
        /// </summary>
        public string ParentCode { get; set; }

        /// <summary>
        /// 是否冻结
        /// </summary>
        public bool IsFreeze { get; set; }
    }
}