/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/5/29 15:57:27
* 文件名: DashboardPageTemplateLogic
* 描述: 页面模板表业务逻辑接口实现
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
    /// 页面模板表业务逻辑接口实现
    /// </summary>
    public class DashboardPageTemplateLogic : DapperAsyncLogic<DashboardPageTemplate>, IDashboardPageTemplateLogic
    {
        #region 构造函数

        private readonly IDashboardPageTemplateRepository _dashboardPageTemplateRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dashboardPageTemplateRepository"></param>
        public DashboardPageTemplateLogic(IDashboardPageTemplateRepository dashboardPageTemplateRepository)
        {
            _dashboardPageTemplateRepository = dashboardPageTemplateRepository;
        }

        #endregion

        #region 方法
        
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<DashboardPageTemplateFindOutput>>> Find(DashboardPageTemplateFindInput input)
        {
            return OperateStatus<PagedResults<DashboardPageTemplateFindOutput>>.Success(await _dashboardPageTemplateRepository.Find(input));
        }
         
        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<DashboardPageTemplate>> FindById(IdInput<int> input)
        {
            return OperateStatus<DashboardPageTemplate>.Success(await FindAsync(f => f.id == input.Id));
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity">信息</param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(DashboardPageTemplate entity)
        {
            var edit = await FindAsync(f => f.id== entity.id);
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