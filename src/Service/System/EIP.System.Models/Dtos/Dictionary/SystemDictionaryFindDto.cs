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

namespace EIP.System.Models.Dtos.Dictionary
{
    /// <summary>
    /// 根据父级查询
    /// </summary>
    public class SystemDictionaryFindInput : QueryParam
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// 是否包含本身
        /// </summary>
        public bool HaveSelf { get; set; } = true;
    }

    /// <summary>
    /// 根据父级id获取字典
    /// </summary>
    public class SystemDictionaryFindOutput 
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid DictionaryId { get; set; }

        public Guid? ParentId { get; set; }

        public int Number { get; set; }

        /// <summary>
        /// 名称
        /// </summary>		
        public string Name { get; set; }

        /// <summary>
        /// 值
        /// </summary>		
        public string Value { get; set; } = string.Empty;

        /// <summary>
        /// 是否允许删除(系统默认配置字段不允许删除)
        /// </summary>		
        public bool CanbeDelete { get; set; }

        /// <summary>
        ///是否冻结
        /// </summary>		
        public bool IsFreeze { get; set; }

        /// <summary>
        /// 排序
        /// </summary>		
		public int OrderNo { get; set; } = 0;

        /// <summary>
        /// 备注
        /// </summary>		
		public string Remark { get; set; } = string.Empty;
        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 创建人员名称
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 修改人员名称
        /// </summary>
        public string UpdateUserName { get; set; }
    }
}