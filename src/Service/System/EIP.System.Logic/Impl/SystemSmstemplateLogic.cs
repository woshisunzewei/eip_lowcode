/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2023/4/22 23:09:59
* 文件名: SystemSmstemplateLogic
* 描述: 短信维护业务逻辑接口实现
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.System.Models.Dtos.Smstemplate;

namespace EIP.System.Logic.Impl
{
    /// <summary>
    /// 短信维护业务逻辑接口实现
    /// </summary>
    public class SystemSmstemplateLogic : DapperAsyncLogic<SystemSmstemplate>, ISystemSmstemplateLogic
    {
        #region 构造函数

        private readonly ISystemSmstemplateRepository _systemSmstemplateRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="systemSmstemplateRepository"></param>
        public SystemSmstemplateLogic(ISystemSmstemplateRepository systemSmstemplateRepository)
        {
            _systemSmstemplateRepository = systemSmstemplateRepository;
        }

        #endregion

        #region 方法
        
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<SystemSmstemplateFindOutput>>> Find(SystemSmstemplateFindInput input)
        {
            return OperateStatus<PagedResults<SystemSmstemplateFindOutput>>.Success(await _systemSmstemplateRepository.Find(input));
        }
         
        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<SystemSmstemplate>> FindById(IdInput input)
        {
            return OperateStatus<SystemSmstemplate>.Success(await FindAsync(f => f.SmsTemplateId == input.Id));
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity">信息</param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(SystemSmstemplate entity)
        {
            var edit = await FindAsync(f => f.SmsTemplateId == entity.SmsTemplateId);
            var currentUser = EipHttpContext.CurrentUser();
            if (edit != null)
            {
                entity.Id = edit.Id;
                entity.UpdateTime = DateTime.Now;
                entity.UpdateUserId = currentUser.UserId;
                entity.UpdateUserName = currentUser.Name;
                return await UpdateAsync(entity);
            }
            entity.SmsTemplateId = CombUtil.NewComb();
            entity.CreateTime = DateTime.Now;
            entity.CreateUserId = currentUser.UserId;
            entity.CreateUserName = currentUser.Name;

            entity.UpdateTime = DateTime.Now;
            entity.UpdateUserId = currentUser.UserId;
            entity.UpdateUserName = currentUser.Name;
            return await InsertAsync(entity);
        }

        /// <summary>
        /// 根据Id删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> Delete(IdInput<string> input)
        {
            OperateStatus operateStatus = new OperateStatus();
            foreach (var item in input.Id.Split(','))
            {
                operateStatus = await DeleteAsync(f => f.SmsTemplateId == Guid.Parse(item));
            }
            return operateStatus;
        }

        #endregion
    }
}