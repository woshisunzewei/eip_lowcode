/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2023/4/22 23:09:59
* 文件名: SystemSmsconfigLogic
* 描述: 短信配置业务逻辑接口实现
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.System.Models.Dtos.Smsconfig;

namespace EIP.System.Logic.Impl
{
    /// <summary>
    /// 短信配置业务逻辑接口实现
    /// </summary>
    public class SystemSmsconfigLogic : DapperAsyncLogic<SystemSmsconfig>, ISystemSmsconfigLogic
    {
        #region 构造函数

        private readonly ISystemSmsconfigRepository _systemSmsconfigRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="systemSmsconfigRepository"></param>
        public SystemSmsconfigLogic(ISystemSmsconfigRepository systemSmsconfigRepository)
        {
            _systemSmsconfigRepository = systemSmsconfigRepository;
        }

        #endregion

        #region 方法
        
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<SystemSmsconfigFindOutput>>> Find(SystemSmsconfigFindInput input)
        {
            return OperateStatus<PagedResults<SystemSmsconfigFindOutput>>.Success(await _systemSmsconfigRepository.Find(input));
        }
         
        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<SystemSmsconfig>> FindById(IdInput input)
        {
            return OperateStatus<SystemSmsconfig>.Success(await FindAsync(f => f.SmsConfigId == input.Id));
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity">信息</param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(SystemSmsconfig entity)
        {
            var edit = await FindAsync(f => f.SmsConfigId == entity.SmsConfigId);
            var currentUser = EipHttpContext.CurrentUser();
            if (edit != null)
            {
                entity.Id = edit.Id;
                entity.UpdateTime = DateTime.Now;
                entity.UpdateUserId = currentUser.UserId;
                entity.UpdateUserName = currentUser.Name;
                return await UpdateAsync(entity);
            }
            entity.SmsConfigId = CombUtil.NewComb();
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
                operateStatus = await DeleteAsync(f => f.SmsConfigId == Guid.Parse(item));
            }
            return operateStatus;
        }

        #endregion
    }
}