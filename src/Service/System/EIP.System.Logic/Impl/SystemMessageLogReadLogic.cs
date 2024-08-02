using EIP.System.Models.Dtos.Message;

namespace EIP.System.Logic.Impl
{
    /// <summary>
    /// 消息阅读记录表,记录那些人员什么时候已经查看
    /// </summary>
    public class SystemMessageLogReadLogic : DapperAsyncLogic<SystemMessageLogRead>, ISystemMessageLogReadLogic
    {
        #region 构造函数

        private readonly ISystemMessageLogReadRepository _systemMessageReadRepository;

        public SystemMessageLogReadLogic(ISystemMessageLogReadRepository systemMessageReadRepository)
        {
            _systemMessageReadRepository = systemMessageReadRepository;
        }

        #endregion

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="input">消息阅读记录表,记录那些人员什么时候已经查看</param>
        /// <returns></returns>
        public async Task<OperateStatus> SaveRead(SystemMessageLogReadSaveInput input)
        {
            IList<SystemMessageLogRead> messageReads = new List<SystemMessageLogRead>();
            //新增
            var readTime = DateTime.Now;
            foreach (var messageLogId in input.MessageLogIds.Split(','))
            {
                messageReads.Add(new SystemMessageLogRead
                {
                    MessageLogIdReadId=Guid.NewGuid(),
                    MessageLogId = Guid.Parse(messageLogId),
                    ReadTime = readTime,
                    ReadUserId = input.UserId,
                    ReadUserName = input.UserName
                });
            }
            return await BulkInsertAsync(messageReads);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> DeleteHaveRead(SystemMessageLogDeleteHaveReadInput input)
        {
            var ids = input.Id.Split(',').Select(s => Guid.Parse(s)).ToList();
            var messageReads = (await FindAllAsync(f => ids.Contains(f.MessageLogId) && f.ReadUserId == input.UserId)).ToList();
            var deleteTime = DateTime.Now;
            foreach (var messageRead in messageReads)
            {
                messageRead.DeleteTime = deleteTime;
            }
            return await BulkUpdateAsync(messageReads);
        }

        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<SystemMessageLogReadFindPagingOutput>>> FindPaging(SystemMessageLogReadFindPagingInput input)
        {
            return OperateStatus<PagedResults<SystemMessageLogReadFindPagingOutput>>.Success(await _systemMessageReadRepository.FindPaging(input));
        }
    }
}
