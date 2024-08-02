namespace EIP.System.Models.Dtos.DingTalk
{
    /// <summary>
    /// 
    /// </summary>
    public class DingTalkBaseDto
    {
        /// <summary>
        /// 返回码
        /// </summary>
        public int ErrCode { get; set; }

        /// <summary>
        /// 调用失败时返回的错误信息。
        /// </summary>
        public string? ErrMsg { get; set; }
    }
}
