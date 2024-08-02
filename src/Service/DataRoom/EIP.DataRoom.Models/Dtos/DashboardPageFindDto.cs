/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/5/29 15:57:27
* 文件名: DashboardPageFindDto
* 描述: 页面基本信息表
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
    /// 页面基本信息表查询参数
    /// </summary>
    public class DashboardPageFindInput : QueryParam
    {
        public string? type { get; set; }
    }

    /// <summary>
    /// 页面基本信息表查询输出
    /// </summary>
    public class DashboardPageFindOutput
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
        /// 页面图标
        /// </summary>
        public string icon { get; set; }

        /// <summary>
        /// 图标颜色
        /// </summary>
        public string iconColor { get; set; }

        /// <summary>
        /// 页面类型
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 组件布局，记录组件的相对位置和顺序
        /// </summary>
        public string layout { get; set; }

        /// <summary>
        /// 表单属性，只有表单类型时才有这个值
        /// </summary>
        public string config { get; set; }

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