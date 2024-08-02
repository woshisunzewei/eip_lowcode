using EIP.Common.Models.Dtos;

namespace EIP.System.Models.Dtos.User
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemUserCheckUserCodeInput : IdInput
    {
        /// <summary>
        /// 代码
        /// </summary>
        public string Code { get; set; }
    }
}