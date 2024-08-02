using EIP.System.Models.Dtos.Message;

namespace EIP.System.Repository
{
    /// <summary>
    /// 消息阅读记录表,记录那些人员什么时候已经查看
    /// </summary>
    public interface ISystemMessageLogReadRepository
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="input">分页参数</param>
        /// <returns></returns>
        Task<PagedResults<SystemMessageLogReadFindPagingOutput>> FindPaging(SystemMessageLogReadFindPagingInput input);

    }
}
