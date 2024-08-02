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
using EIP.Common.Models.Dtos;

namespace EIP.System.Models.Dtos.User
{
    /// <summary>
    /// 重置密码input
    /// </summary>
    public class SystemUserResetPasswordInput : IdInput
    {
        /// <summary>
        /// 加密后密码
        /// </summary>
        public string EncryptPassword { get; set; }
    }
}