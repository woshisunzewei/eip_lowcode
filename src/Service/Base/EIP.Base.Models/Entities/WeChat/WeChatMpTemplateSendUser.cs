using EIP.Common.Models.Attributes.MicroOrm;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EIP.Base.Models.Entities.WeChat
{
    /// <summary>
    /// 微信公众号模版发送接收人员
    /// </summary>
    [Serializable]
    [Table("wechat_mp_template_send_user")]
    public class WeChatMpTemplateSendUser
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        [Key, Identity, JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// 主键Id
        /// </summary>
        public Guid TemplateSendUserId { get; set; }

        /// <summary>
        /// 主键Id
        /// </summary>
        public Guid TemplateSendId { get; set; }

        /// <summary>
        /// 接收OpenId
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        public string SendTime { get; set; }
    }
}
