using EIP.System.Models.Dtos.Notice;

namespace EIP.System.Logic.Impl
{
    /// <summary>
    /// 公告
    /// </summary>
    public class SystemNoticeLogic : DapperAsyncLogic<SystemNotice>, ISystemNoticeLogic
    {
        #region 构造函数

        private readonly ISystemNoticeRepository _systemNoticeRepository;

        public SystemNoticeLogic(ISystemNoticeRepository systemNoticeRepository)
        {
            _systemNoticeRepository = systemNoticeRepository;
        }

        #endregion

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="input">公告</param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(SystemNotice input)
        {
            var notice = await FindAsync(f => f.NoticeId == input.NoticeId);
            var currentUser = EipHttpContext.CurrentUser();
            if (notice == null)
            {
                input.CreateTime = DateTime.Now;
                input.CreateUserId = currentUser.UserId;
                input.CreateUserName = currentUser.Name;
                input.UpdateTime = DateTime.Now;
                input.UpdateUserId = currentUser.UserId;
                input.UpdateUserName = currentUser.Name;
                return await InsertAsync(input);
            }
            input.Id = notice.Id;
            input.CreateTime = notice.CreateTime;
            input.CreateUserId = notice.CreateUserId;
            input.CreateUserName = notice.CreateUserName;

            input.UpdateTime = DateTime.Now;
            input.UpdateUserId = currentUser.UserId;
            input.UpdateUserName = currentUser.Name;
            return await UpdateAsync(input);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<SystemNoticeFindPagingOutput>>> FindPaging(SystemNoticeFindPagingInput input)
        {
            var datas = await _systemNoticeRepository.FindPaging(input);
            return OperateStatus<PagedResults<SystemNoticeFindPagingOutput>>.Success(datas);
        }

        public async Task<OperateStatus<SystemNotice>> FindById(IdInput input)
        {
            return OperateStatus<SystemNotice>.Success(await FindAsync(f => f.NoticeId == input.Id));
        }

        public async Task<OperateStatus> Delete(IdInput<string> input)
        {
            OperateStatus operateStatus = new OperateStatus();
            foreach (var item in input.Id.Split(','))
            {
                var noticeId = Guid.Parse(item);
                operateStatus = await DeleteAsync(f => f.NoticeId == noticeId);
            }
            return operateStatus;
        }
    }
}
