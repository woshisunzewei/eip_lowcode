using EIP.System.Models.Dtos.Notice;

namespace EIP.System.Repository
{
    /// <summary>
    /// 公告
    /// </summary>
    public interface ISystemNoticeRepository
    {
        /// <summary>
        /// 获取公告信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResults<SystemNoticeFindPagingOutput>> FindPaging(SystemNoticeFindPagingInput input);
    }
}
