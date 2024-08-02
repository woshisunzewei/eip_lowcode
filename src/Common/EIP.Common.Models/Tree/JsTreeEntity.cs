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

namespace EIP.Common.Models.Tree
{
    /// <summary>
    /// 
    /// </summary>
    public class JsTreeEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Object id { get; set; }

        /// <summary>
        /// 父级
        /// </summary>
        public Object parent { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string text { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string icon { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public JsTreeStateEntity state { get; set; } = new JsTreeStateEntity();

        /// <summary>
        /// 地址
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 扩展
        /// </summary>
        public string extention { get; set; }

        /// <summary>
        /// 设置
        /// </summary>
        public bool children { get; set; }

        /// <summary>
        /// 所有父级
        /// </summary>
        public string parents { get; set; } = string.Empty;
    }
    /// <summary>
    /// 
    /// </summary>
    public class JsTreeStateEntity
    {
        /// <summary>
        /// 是否打开
        /// </summary>
        public bool opened { get; set; } = true;

        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool disabled { get; set; } = false;
    }
}