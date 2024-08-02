/**************************************************************
* Copyright (C) www.eipflow.com 孙泽伟版权所有(盗版必究)
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
using System.Collections.Generic;

namespace EIP.System.Models.Dtos.DataBase
{
    /// <summary>
    /// 表单控件
    /// </summary>
    public class SystemDataBaseSaveBusinessDataColumns
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 单选
        /// </summary>
        public bool IsSingle { get; set; }

        /// <summary>
        /// 是否删除:App端如扫码新增可能不需要删除
        /// </summary>
        public bool IsDelete { get; set; } = true;

        /// <summary>
        /// 子表
        /// </summary>
        public IList<DataBaseFormSubtableControl> Subtable { get; set; }=new List<DataBaseFormSubtableControl>();

        #region 规则编号
        /// <summary>
        /// 默认值
        /// </summary>
        public string Rule { get; set; }

        /// <summary>
        /// 加载时显示
        /// </summary>
        public bool LoadDisplay { get; set; } = false;

        /// <summary>
        /// 用户占用
        /// </summary>
        public bool UserOccupation { get; set; } = false;
        #endregion

        #region 默认值
        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue { get; set; }
        #endregion
    }

    /// <summary>
    /// 子表
    /// </summary>
    public class DataBaseFormSubtableControl
    {

    }

}
