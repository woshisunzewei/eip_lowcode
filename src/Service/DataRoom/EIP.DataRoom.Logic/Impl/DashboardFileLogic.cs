/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/5/29 15:57:26
* 文件名: DashboardFileLogic
* 描述: 文件表业务逻辑接口实现
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Base.Models.Entities.DataRoom;
using EIP.Common.Core.Context;
using EIP.Common.Logic;
using EIP.Common.Models;
using EIP.Common.Models.Dtos;
using EIP.Common.Models.Paging;
using EIP.DataRoom.Models.Dtos;
using EIP.DataRoom.Repository;

namespace EIP.DataRoom.Logic.Impl
{
    /// <summary>
    /// 文件表业务逻辑接口实现
    /// </summary>
    public class DashboardFileLogic : DapperAsyncLogic<DashboardFile>, IDashboardFileLogic
    {
        #region 构造函数

        private readonly IDashboardFileRepository _dashboardFileRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dashboardFileRepository"></param>
        public DashboardFileLogic(IDashboardFileRepository dashboardFileRepository)
        {
            _dashboardFileRepository = dashboardFileRepository;
        }

        #endregion

        #region 方法
        
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<DashboardFileFindOutput>>> Find(DashboardFileFindInput input)
        {
            return OperateStatus<PagedResults<DashboardFileFindOutput>>.Success(await _dashboardFileRepository.Find(input));
        }
         
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity">信息</param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(DashboardFile entity)
        {
            var edit = await FindAsync(f => f.id == entity.id);
            var currentUser = EipHttpContext.CurrentUser();
            if (edit != null)
            {
                return await UpdateAsync(entity);
            }
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
                operateStatus = await DeleteAsync(f => f.id ==Int32.Parse(item));
            }
            return operateStatus;
        }

        #endregion
    }
}