using System;

namespace EIP.System.Models.Dtos.WeChat.User
{
    /// <summary>
    /// 
    /// </summary>
    public class WeChatUserBindInput
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// 主键Id
        /// </summary>
        public Guid WeChatUserId { get; set; }
    }
}
