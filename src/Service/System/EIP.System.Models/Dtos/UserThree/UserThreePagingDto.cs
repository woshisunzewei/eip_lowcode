using EIP.Base.Models.Entities.System;
using EIP.Common.Models.Paging;
using System;

namespace EIP.System.Models.Dtos.UserThree
{
    /// <summary>
    /// 
    /// </summary>
    public class UserThreePagingInput : QueryParam
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Type { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class UserThreePagingOutput : SystemUserInfoThree
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid? BindUserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string BindHeadImage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string BindUserName { get; set; }
    }
}
