using EIP.Common.Models.Attributes.MicroOrm;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EIP.Base.Models.Entities.WeChat
{
    /// <summary>
    /// 微信公众号模版
    /// </summary>
    [Serializable]
    [Table("wechat_mp_template")]
    public class WeChatMpTemplate
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        [Key, Identity, JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// 主键Id
        /// </summary>
        public Guid TemplateId { get; set; }

        /// <summary>
        /// 模版代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 模版名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 模版参数设计
        /// </summary>
        public string Design { get; set; }

        /// <summary>
        /// 一级行业
        /// </summary>
        public string IndustryOne { get; set; }

        /// <summary>
        /// 二级行业
        /// </summary>
        public string IndustryTwo { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
