/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/5/29 15:57:29
* 文件名: DsLabelFindDto
* 描述: 标签
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
    /// 标签查询参数
    /// </summary>
    public class DsLabelFindInput : QueryParam
    {

    }

    /// <summary>
    /// 标签查询输出
    /// </summary>
    public class DsLabelFindOutput
    {
        public int id { get; set; }
        /// <summary>
        /// 标签名称
        /// </summary>
        public string labelName { get; set; }

        /// <summary>
        /// 标签类型
        /// </summary>
        public string labelType { get; set; }

        /// <summary>
        /// 标签描述
        /// </summary>
        public string labelDesc { get; set; }



    }
}