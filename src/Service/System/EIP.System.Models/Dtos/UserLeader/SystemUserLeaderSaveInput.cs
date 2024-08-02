using System;
using System.Collections.Generic;

namespace EIP.System.Models.Dtos.UserLeader
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemUserLeaderSaveInput
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Guid> U { get; set; } = new List<Guid>();
    }
}
