using System;

namespace EIP.System.Models.Dtos.Message
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemMessageLogDeleteHaveReadInput
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; set; }
    }
}