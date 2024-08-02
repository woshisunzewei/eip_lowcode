using EIP.Common.Models.Attributes.MicroOrm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace EIP.Base.Models.Entities.WeChat
{
    /// <summary>
    /// 微信公众号模版发送
    /// </summary>
    [Serializable]
    [Table("wechat_mp_template_send")]
    public class WeChatMpTemplateSend
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        [Key, Identity, JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// 主键Id
        /// </summary>
        public Guid TemplateSendId { get; set; }

        /// <summary>
        /// 主键Id
        /// </summary>
        public Guid TemplateId { get; set; }

        /// <summary>
        /// 发送数据
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// 已接收人员数量
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 已发送数量
        /// </summary>
        public int HaveSend { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
