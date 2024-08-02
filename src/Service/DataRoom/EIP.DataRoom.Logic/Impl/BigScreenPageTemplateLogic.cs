/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/6/1 16:39:27
* 文件名: BigScreenPageTemplateLogic
* 描述: 页面模板表业务逻辑接口实现
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Base.Models.Entities.DataRoom;
using EIP.Big.Models.Dtos.ScreenPageTemplate;
using EIP.Common.Core.Context;
using EIP.Common.Logic;
using EIP.Common.Models;
using EIP.Common.Models.Dtos;
using EIP.Common.Models.Paging;
using EIP.DataRoom.Repository;

namespace EIP.DataRoom.Logic.Impl
{
    /// <summary>
    /// 页面模板表业务逻辑接口实现
    /// </summary>
    public class BigScreenPageTemplateLogic : DapperAsyncLogic<BigScreenPageTemplate>, IBigScreenPageTemplateLogic
    {
        #region 构造函数

        private readonly IBigScreenPageTemplateRepository _bigScreenPageTemplateRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bigScreenPageTemplateRepository"></param>
        public BigScreenPageTemplateLogic(IBigScreenPageTemplateRepository bigScreenPageTemplateRepository)
        {
            _bigScreenPageTemplateRepository = bigScreenPageTemplateRepository;
        }

        #endregion

        #region 方法
        
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<BigScreenPageTemplateFindOutput>>> Find(BigScreenPageTemplateFindInput input)
        {
            return OperateStatus<PagedResults<BigScreenPageTemplateFindOutput>>.Success(await _bigScreenPageTemplateRepository.Find(input));
        }
         
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity">信息</param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(BigScreenPageTemplate entity)
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
                operateStatus = await DeleteAsync(f => f.id == Int32.Parse(item));
            }
            return operateStatus;
        }

        #endregion
    }
}