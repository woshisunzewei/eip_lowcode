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

namespace EIP.Common.Core.Extension
{
    /// <summary>
    /// Guid扩展类
    /// </summary>
    public static class GuidExtension
    {
        /// <summary>
        /// 是否为空Guid
        /// </summary>
        /// <param name="guid">guid</param>
        /// <returns>是否为空Guid</returns>
        public static bool IsEmptyGuid(this Guid guid)
        {
            return guid == Guid.Empty;
        }

        /// <summary>
        /// 是否为空或者null的Guid
        /// </summary>
        /// <param name="guid">guid值</param>
        /// <returns>是否为空或者null的Guid</returns>
        public static bool IsNullOrEmptyGuid(this Guid? guid)
        {
            return guid == null || guid == Guid.Empty;
        }
        /// <summary>
        /// 字符串转Guid
        /// </summary>
        /// <param name="value"> 字符串 </param>
        /// <returns></returns>
        public static Guid ToGuid(this string value)
        {
            Guid result = Guid.Empty;
            Guid.TryParse(value, out result);
            return result; 
        }
    }
}