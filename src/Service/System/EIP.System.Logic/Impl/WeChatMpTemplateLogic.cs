/**************************************************************
* Copyright (C) 2022 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2022/01/12 22:40:15
* 文件名: 
* 描述: 
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Base.Models.Entities.WeChat;
using EIP.System.Models.Dtos.WeChat.MpTemplate;
using EIP.WeChat.Repository;
namespace EIP.WeChat.Logic.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class WeChatMpTemplateLogic : DapperAsyncLogic<WeChatMpTemplate>, IWeChatMpTemplateLogic
    {
        private readonly IWeChatMpTemplateRepository _weChatMpTemplateRepository;

        /// <summary>
        /// 授权人员
        /// </summary>
        /// <param name="userInfoRepository"></param>
        public WeChatMpTemplateLogic(IWeChatMpTemplateRepository weChatMpTemplateRepository)
        {
            _weChatMpTemplateRepository = weChatMpTemplateRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<OperateStatus<PagedResults<WeChatMpTemplatePagingOutput>>> Find(WeChatMpTemplatePagingInput input)
        {
            return OperateStatus<PagedResults<WeChatMpTemplatePagingOutput>>.Success(await _weChatMpTemplateRepository.Find(input));
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
                operateStatus = await DeleteAsync(f => f.TemplateId == Guid.Parse(item));
            }
            return operateStatus;
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<WeChatMpTemplate>> FindById(IdInput input)
        {
            return OperateStatus<WeChatMpTemplate>.Success(await FindAsync(f => f.TemplateId == input.Id));
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(WeChatMpTemplate input)
        {
            var editData = await FindAsync(f => f.TemplateId == input.TemplateId);
            if (editData != null)
            {
                editData.Id = input.Id;
                return await UpdateAsync(input);
            }
            input.CreateTime = DateTime.Now;
            input.TemplateId = CombUtil.NewComb();
            return await InsertAsync(input);
        }
    }
}