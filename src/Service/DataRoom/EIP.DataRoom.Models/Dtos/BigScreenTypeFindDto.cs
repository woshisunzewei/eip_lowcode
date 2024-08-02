/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/6/1 16:54:21
* 文件名: BigScreenTypeFindDto
* 描述: 大屏、资源库、组件库分类
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Common.Models.Paging;
using System;

namespace EIP.Big.Models.Dtos.ScreenType
{
    /// <summary>
    /// 大屏、资源库、组件库分类查询参数
    /// </summary>
    public class BigScreenTypeFindInput : QueryParam
    {

    }

    /// <summary>
    /// 大屏、资源库、组件库分类查询输出
    /// </summary>
    public class BigScreenTypeFindOutput
    {
        public int id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int orderNum { get; set; }

    }
}