/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2023/8/22 7:58:35
* 文件名: SystemSerialNoFindDto
* 描述: 编号规则
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Common.Models.Paging;
using System;

namespace EIP.System.Models.Dtos.Serialno
{
    /// <summary>
    /// 编号规则查询参数
    /// </summary>
    public class SystemSerialNoFindInput : QueryParam
    {
        
    }

    /// <summary>
    /// 编号规则查询输出
    /// </summary>
    public class SystemSerialNoFindOutput
    {
                /// <summary>
        /// 
        /// </summary>
        public Guid SerialNoId { get; set; }

        /// <summary>
        /// 代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 表达式({d}代表日期，{n}代表数字)
        /// </summary>
        public string Rule { get; set; }

        /// <summary>
        /// 最新值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 日期格式
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// 数字长度
        /// </summary>
        public int? Len { get; set; }

        /// <summary>
        /// 新的一年重新分配1号开始
        /// </summary>
        public string IsAuto { get; set; }


    }
}