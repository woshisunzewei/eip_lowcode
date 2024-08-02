/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2018/11/4 14:47:24
* 文件名: SystemUserFindSubordinateOutput
* 描述: 查询下级人员
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Common.Models.Attributes.MicroOrm.Joins;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EIP.System.Models.Dtos.User
{
    /// <summary>
    /// 查询下级人员
    /// </summary>
    [Table("System_UserLeader")]
    public class SystemUserFindSubordinateOutput
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Key]
        public Guid LeaderUserId { get; set; }

        /// <summary>
        /// 人员输出
        /// </summary>
        [LeftJoin("System_UserInfo", "UserId", "UserId")]
        [JsonIgnore]
        public List<SystemUserLeadersOutput> Outputs { get; set; }
    }

    /// <summary>
    /// 查询上级人员
    /// </summary>
    [Table("System_UserLeader")]
    public class SystemUserFindLeaderOutput
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Key]
        public Guid UserId { get; set; }

        /// <summary>
        /// 人员输出
        /// </summary>
        [LeftJoin("System_UserInfo", "LeaderUserId", "UserId")]
        [JsonIgnore]
        public List<SystemUserLeadersOutput> Outputs { get; set; }
    }

    /// <summary>
    /// 人员输出
    /// </summary>
    public class SystemUserLeadersOutput
    {
        /// <summary>
        /// 人员Id
        /// </summary>	
        [Key]
        public Guid UserId { get; set; }

        /// <summary>
        /// 组织机构Id
        /// </summary>
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// 用户真实姓名
        /// </summary>		
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>		
        public string Remark { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>		
        public string Mobile { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>		
        public string Email { get; set; }

        /// <summary>
        /// 微信
        /// </summary>		
        public string WeChat { get; set; }

        /// <summary>
        /// QQ
        /// </summary>		
        public string Qq { get; set; }

        /// <summary>
        /// 组织机构名称
        /// </summary>		
        public string OrganizationName { get; set; }

        /// <summary>
        /// 用户登录名
        /// </summary>		
        public string Code { get; set; }

        /// <summary>
        /// 头像地址
        /// </summary>		
        public string HeadImage { get; set; }

        /// <summary>
        /// 组织机构名称
        /// </summary>
        [NotMapped]
        public string OrganizationNames { get; set; }
    }
}
