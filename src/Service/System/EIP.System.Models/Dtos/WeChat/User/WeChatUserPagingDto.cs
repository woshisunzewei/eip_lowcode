using EIP.Common.Models.Paging;
using System;

namespace EIP.System.Models.Dtos.WeChat.User
{
    /// <summary>
    /// 
    /// </summary>
    public class WeChatUserPagingInput : QueryParam
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public class WeChatUserPagingOutput
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public Guid WeChatUserId { get; set; }

        /// <summary>
        /// OpenId
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 公众平台Id
        /// </summary>
        public string UnionId { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 头像路径
        /// </summary>
        public string HeadImgurl { get; set; }

        /// <summary>
        /// 授权时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 最后一次授权时间
        /// </summary>
        public DateTime? LastTime { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// 关注公众号
        /// </summary>
        public int Subscribe { get; set; }

        /// <summary>
        /// 类型:1公众号,2小程序,3企业微信
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 小程序sessionId
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// 小程序SessionKey
        /// </summary>
        public string SessionKey { get; set; }

        /// <summary>
        /// 关联
        /// </summary>
        public string UserName { get; set; }
    }
}
