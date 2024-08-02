/**************************************************************
* Copyright (C) 2022 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2022/01/12 22:40:15
* 文件名: 
* 描述: 
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.System.Models.Dtos.WeChat.User;

namespace EIP.WeChat.Repository
{
    /// <summary>
    /// 微信授权用户
    /// </summary>
    public interface IWeChatUserRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<PagedResults<WeChatUserPagingOutput>> Find(WeChatUserPagingInput input);
    }
}