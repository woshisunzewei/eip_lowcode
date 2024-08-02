/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/7/9 22:28:57
* 文件名: SystemSearchLogic
* 描述: 业务逻辑接口实现
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.System.Models.Dtos.Search;

namespace EIP.System.Logic.Impl
{
    /// <summary>
    /// 业务逻辑接口实现
    /// </summary>
    public class SystemSearchLogic : DapperAsyncLogic<SystemSearch>, ISystemSearchLogic
    {
        #region 构造函数

        private readonly ISystemSearchRepository _systemSearchRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="systemSearchRepository"></param>
        public SystemSearchLogic(ISystemSearchRepository systemSearchRepository)
        {
            _systemSearchRepository = systemSearchRepository;
        }

        #endregion

        #region 方法
        
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<SystemSearchFindOutput>>> Find(SystemSearchFindInput input)
        {
            return OperateStatus<PagedResults<SystemSearchFindOutput>>.Success(await _systemSearchRepository.Find(input));
        }
         
        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<SystemSearch>> FindById(IdInput input)
        {
            return OperateStatus<SystemSearch>.Success(await FindAsync(f => f.SearchId == input.Id));
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity">信息</param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(SystemSearch entity)
        {
            var edit = await FindAsync(f => f.SearchId == entity.SearchId);
            var currentUser = EipHttpContext.CurrentUser();
            if (edit != null)
            {
                edit.Name= entity.Name;
                edit.UpdateTime = DateTime.Now;
                edit.UpdateUserId = currentUser.UserId;
                edit.UpdateUserName = currentUser.Name;
                return await UpdateAsync(edit);
            }
            entity.SearchId = CombUtil.NewComb();
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
                operateStatus = await DeleteAsync(f => f.SearchId == Guid.Parse(item));
            }
            return operateStatus;
        }

        #endregion
    }
}