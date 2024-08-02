/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2023/4/22 23:09:59
* 文件名: SystemSmsconfigRepository
* 描述: 短信配置数据访问接口实现
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.System.Models.Dtos.Smsconfig;

namespace EIP.System.Repository.Impl
{
    /// <summary>
    /// 短信配置数据访问接口实现
    /// </summary>
    public class SystemSmsconfigRepository :ISystemSmsconfigRepository
    {
         /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResults<SystemSmsconfigFindOutput>> Find(SystemSmsconfigFindInput input)
        {
            var sql = new StringBuilder($@"SELECT  
                                                  Id,
                                                  SmsConfigId,
                                                  Name,
                                                  Provide,
                                                  AppId,
                                                  AppKey,
                                                  IsFreeze,
                                                  Remark,
                                                  CreateTime,
                                                  CreateUserId,
                                                  CreateUserName,
                                                  UpdateTime,
                                                  UpdateUserId,
                                                  UpdateUserName,
                                                   @rowNumber, @recordCount FROM system_smsconfig @where ");
            
            if (input.Sidx.IsNullOrEmpty())
            {
                input.Sidx = " Id ";
            }
            return new SqlMapperUtil().PagingQuerySqlAsync<SystemSmsconfigFindOutput>(sql.ToString(), input);
        }
    }
}