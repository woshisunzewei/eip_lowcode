using EIP.System.Models.Dtos.Message;

namespace EIP.System.Logic.Impl
{
    /// <summary>
    /// 消息记录表
    /// </summary>
    public class SystemMessageLogLogic : DapperAsyncLogic<SystemMessageLog>, ISystemMessageLogLogic
    {
        #region 构造函数

        private readonly ISystemMessageLogRepository _systemMessageLogRepository;

        public SystemMessageLogLogic(ISystemMessageLogRepository systemMessageRepository)
        {
            _systemMessageLogRepository = systemMessageRepository;
        }

        #endregion

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="input">消息记录表</param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(SystemMessageLog input)
        {
            //新增
            if (input.MessageLogId.IsEmptyGuid())
            {
                input.MessageLogId = CombUtil.NewComb();
                return await InsertAsync(input);
            }
            return await UpdateAsync(input);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<SystemMessageLogFindPagingOutput>>> FindPaging(SystemMessageLogFindPagingInput input)
        {
            return OperateStatus<PagedResults<SystemMessageLogFindPagingOutput>>.Success(await _systemMessageLogRepository.FindPaging(input));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<SystemMessageLogFindMonitorPagingOutput>>> FindMonitorPaging(SystemMessageLogFindMonitorPagingInput input)
        {
            return OperateStatus<PagedResults<SystemMessageLogFindMonitorPagingOutput>>.Success(await _systemMessageLogRepository.FindMonitorPaging(input));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<SystemMessageLogFindPagingOutput>>> FindMyAllPaging(SystemMessageLogFindPagingInput input)
        {
            return OperateStatus<PagedResults<SystemMessageLogFindPagingOutput>>.Success(await _systemMessageLogRepository.FindMyAllPaging(input));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<SystemMessageLog>> FindById(IdInput input)
        {
            return OperateStatus<SystemMessageLog>.Success(await FindAsync(f => f.MessageLogId == input.Id));
        }
    }
}
