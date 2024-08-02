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
    /// 字段验证
    /// </summary>
    public enum EnumControlValidator
    {
        /// <summary>
        /// 
        /// </summary>
        不能为空,
        /// <summary>
        /// 
        /// </summary>
        数字,
        /// <summary>
        /// 
        /// </summary>
        数字或空,
        /// <summary>
        /// 
        /// </summary>
        小数,
        /// <summary>
        /// 
        /// </summary>
        小数或空,
        /// <summary>
        /// 
        /// </summary>
        必须电话格式,
        /// <summary>
        /// 
        /// </summary>
        必须电话格式或空,
        /// <summary>
        /// 
        /// </summary>
        手机格式,
        /// <summary>
        /// 
        /// </summary>
        手机格式或空,
        /// <summary>
        /// 
        /// </summary>
        电子邮件格式,
        /// <summary>
        /// 
        /// </summary>
        电子邮件格式或空,
        /// <summary>
        /// 
        /// </summary>
        身份证格式,
        /// <summary>
        /// 
        /// </summary>
        身份证格式或空,
        /// <summary>
        /// 
        /// </summary>
        自定义
    }
}