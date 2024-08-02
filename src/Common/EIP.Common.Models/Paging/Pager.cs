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
using System.Collections.Generic;

namespace EIP.Common.Models.Paging
{
    /// <summary>
    /// 说  明:分页信息
    /// 备  注:继承QueryParam,由于在界面上只需传入基础参数,页码,记录总数无须传入,所以此次使用继承来使用继承原始
    /// 编写人:孙泽伟-2015/03/25
    /// </summary>
    public class PagerInfo
    {
        /// <summary>
        /// 总记录数
        /// </summary>
        public long RecordCount { get; set; }

        /// <summary>
        /// 总记录数（兼容）
        /// </summary>
        public long Total { get; set; }
    }

    /// <summary>
    /// 说  明:分页信息
    /// 备  注:
    /// 编写人:孙泽伟-2015/03/25
    /// </summary>
    public class PagedResults<T>
    {
        /// <summary>
        /// 分页信息
        /// </summary>
        public PagerInfo PagerInfo { get; set; }

        /// <summary>
        /// 查询出来数据
        /// </summary>
        public IList<T> Data { get; set; }

    }
    /// <summary>
    /// 
    /// </summary>
    public class PagerFields
    {
        /// <summary>
        /// 表名称
        /// </summary>
        public string Table { get; set; }

        /// <summary>
        /// 字段:根据不同类型数据库处理为不同的格式
        /// </summary>
        public string Fields { get; set; }

        /// <summary>
        /// 别名
        /// </summary>
        public string ReName { get; set; }
    }
}