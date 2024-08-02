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
using EIP.System.Models.Dtos.WeChat.User;
using EIP.WeChat.Repository;
namespace EIP.WeChat.Logic.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class WeChatUserLogic : DapperAsyncLogic<SystemUserInfoThree>, IWeChatUserLogic
    {
        private readonly IWeChatUserRepository _weChatUserRepository;

        /// <summary>
        /// 授权人员
        /// </summary>
        /// <param name="userInfoRepository"></param>
        public WeChatUserLogic(IWeChatUserRepository weChatUserRepository)
        {
            _weChatUserRepository = weChatUserRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<OperateStatus<PagedResults<WeChatUserPagingOutput>>> Find(WeChatUserPagingInput input)
        {
            return OperateStatus<PagedResults<WeChatUserPagingOutput>>.Success(await _weChatUserRepository.Find(input));
        }
        public async Task<OperateStatus> Delete(IdInput<string> input)
        {
            OperateStatus operateStatus = new OperateStatus();
            foreach (var item in input.Id.Split(','))
            {
                var noticeId = Guid.Parse(item);
                operateStatus = await DeleteAsync(f => f.ThreeUserId == noticeId);
            }
            return operateStatus;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> BindUser(WeChatUserBindInput input)
        {
            return await UpdateAsync(u =>  u.ThreeUserId == input.WeChatUserId, new { UserId =input.UserId});
        }

        /// <summary>
        /// 解绑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> UnBindUser(IdInput<string> input)
        {
            OperateStatus operateStatus = new OperateStatus();
            foreach (var id in input.Id.Split(','))
            {
                var weChatUserId = Guid.Parse(id);
                var data = await FindAsync(f => f.ThreeUserId == weChatUserId);
                if (data == null)
                {
                    return OperateStatus.Error("未找到授权信息");
                }
                else
                {
                    data.SystemUserId =Guid.NewGuid();
                    operateStatus = await UpdateAsync(data);
                }
            }
            return operateStatus;
        }

        /// <summary>
        /// 解绑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> WeChartInit()
        {
            OperateStatus operateStatus = new OperateStatus();
            List<string> openIds = new List<string>();
            //获取最大的OpenId
            var data = await FindAllAsync(f => f.Type == 1);
            operateStatus = await GetAllOpenIds(openIds, "");
            string appId = Config.SenparcWeixinSetting.WeixinAppId;
            foreach (var item in openIds)
            {
                var have = data.FirstOrDefault(f => f.PlatformUserId == item);
                if(have == null)
                {
                    var user = await UserApi.InfoAsync(appId, item);
                    SystemUserInfoThree weChatUser = new SystemUserInfoThree()
                    {
                        ThreeUserId = Guid.NewGuid(),
                        PlatformUserId = user.openid,
                        UnionId = user.unionid,
                        Name = user.nickname,
                        Avatar = user.headimgurl,
                        CreateTime = DateTime.Now,
                        Type = 1
                    };
                    operateStatus = await InsertAsync(weChatUser);
                }
            }

            return operateStatus;
        }

        /// <summary>
        /// 获取所有OpenIds
        /// </summary>
        /// <returns></returns>
        private async Task<OperateStatus> GetAllOpenIds(List<string> openIds, string nextOpenId)
        {
            OperateStatus operateStatus = new OperateStatus();
            string appId = Config.SenparcWeixinSetting.WeixinAppId;
            var openIdResult = await UserApi.GetAsync(appId, nextOpenId);
            if (openIdResult.errcode == ReturnCode.请求成功)
            {
                if (openIdResult.data != null)
                {
                    //写入
                    foreach (var item in openIdResult.data.openid)
                    {
                        openIds.Add(item);
                    }
                    if (openIds.Count != openIdResult.count)
                    {
                        await GetAllOpenIds(openIds, openIds[openIds.Count - 1]);
                    }
                }
               
                operateStatus.Code = ResultCode.Success;
                operateStatus.Msg = "查询成功";
            }
            else
            {
                operateStatus.Msg = openIdResult.errmsg;
            }
            return operateStatus;
        }
    }
}