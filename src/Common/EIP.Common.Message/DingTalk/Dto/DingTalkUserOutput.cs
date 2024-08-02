namespace EIP.Common.Message.DingTalk.Dto
{
    public class DingTalkUserOutput: DingTalkBaseOutput
    {
        public DingTalkUserResultOutput result { get; set; }
    }

    public class DingTalkUserResultOutput
    {
        public string userid { get; set; }
    }
}
