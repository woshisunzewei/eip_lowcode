/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/6/1 16:54:21
* 文件名: BigScreenPageFindDto
* 描述: 页面基本信息表
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Common.Models.Paging;
using System;

namespace EIP.Big.Models.Dtos.ScreenPage
{
    /// <summary>
    /// 页面基本信息表查询参数
    /// </summary>
    public class BigScreenPageFindInput : QueryParam
    {

    }

    /// <summary>
    /// 页面基本信息表查询输出
    /// </summary>
    public class BigScreenPageFindOutput
    {
        public int id { get; set; }
        /// <summary>
        /// 页面中文名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 页面编码，页面唯一标识符
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 封面图片文件路径
        /// </summary>
        public string coverPicture { get; set; }

        /// <summary>
        /// 页面类型
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 页面配置
        /// </summary>
        public String config { get; set; }

        /// <summary>
        /// 父级目录编码
        /// </summary>
        public string parentCode { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int orderNum { get; set; }

        /// <summary>
        /// 备忘
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 所属应用编码
        /// </summary>
        public string appCode { get; set; }
    }
}