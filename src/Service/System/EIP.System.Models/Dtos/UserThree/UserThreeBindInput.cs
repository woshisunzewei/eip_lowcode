using System;

namespace EIP.System.Models.Dtos.UserThree
{
    /// <summary>
    /// 
    /// </summary>
    public class UserThreeBindInput
    {
        /// <summary>
        /// 系统Id
        /// </summary>
        public Guid? SystemUserId { get; set; }

        /// <summary>
        /// 钉钉第三方用户Id
        /// </summary>
        public Guid ThreeUserId { get; set; }
    }
}
