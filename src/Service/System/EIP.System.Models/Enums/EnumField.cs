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
namespace EIP.System.Models.Enums
{
    /// <summary>
    /// 枚举字段对齐方式
    /// </summary>
    public enum EnumFieldAlign : byte
    {
        /// <summary>
        /// Right
        /// </summary>
        Right,
        /// <summary>
        /// Center
        /// </summary>
        Center,
        /// <summary>
        /// Left
        /// </summary>
        Left
    }

    /// <summary>
    /// 字段格式化类型
    /// </summary>
    public enum EnumFieldFormatter : byte
    {
        /// <summary>
        /// Datetime
        /// </summary>
        Datetime,
        /// <summary>
        /// Icon
        /// </summary>
        Icon,
        /// <summary>
        /// Sex
        /// </summary>
        Sex,
        /// <summary>
        /// State
        /// </summary>
        State,
        /// <summary>
        /// YesOrNo
        /// </summary>
        YesOrNo,
        /// <summary>
        /// UpOrdel
        /// </summary>
        UpOrdel,
        /// <summary>
        /// Del
        /// </summary>
        Del,
        /// <summary>
        /// Update
        /// </summary>
        Update,
        /// <summary>
        /// Email
        /// </summary>
        Email
    }

    /// <summary>
    /// 字段排序方式
    /// </summary>
    public enum EnumFieldSortType : byte
    {
        /// <summary>
        /// @Int
        /// </summary>
        @Int,
        /// <summary>
        /// Integer
        /// </summary>
        Integer,
        /// <summary>
        /// @Float
        /// </summary>
        @Float,
        /// <summary>
        /// Number
        /// </summary>
        Number,
        /// <summary>
        /// Currency
        /// </summary>
        Currency,
        /// <summary>
        /// Date
        /// </summary>
        Date,
        /// <summary>
        /// Text
        /// </summary>
        Text
    }
}