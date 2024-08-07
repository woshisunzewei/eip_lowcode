/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2018/10/30 22:40:15
* 文件名: 
* 描述: 
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Common.Core.Auth;
using EIP.Common.Core.Util;
using System;

namespace EIP.Common.Log.Handler
{
    /// <summary>
    /// 数据日志记录
    /// </summary>
    public class DataLogHandler : BaseHandler<DataLog>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DataLogHandler(byte operateType,
            string operateTable,
            string operateData = null,
            string operateAfterData = null) : base("DataLog")
        {
            PrincipalUser principalUser = new PrincipalUser
            {
                Name = "匿名用户",
                UserId = Guid.Empty
            };
            //var current = HttpContext.Current;
            //if (current != null)
            //{
            //    principalUser = FormAuthenticationExtension.Current(HttpContext.Current.Request);
            //}
            //if (principalUser == null)
            //{
            //    principalUser = new PrincipalUser()
            //    {
            //        Name = "匿名用户",
            //        UserId = Guid.Empty
            //    };
            //}
            Log = new DataLog()
            {
                OperateType = operateType,
                OperateTable = operateTable,
                OperateData = operateData,
                OperateAfterData = operateAfterData,
                CreateTime = DateTime.Now,
                CreateUserId = principalUser.UserId.ToString(),
                CreateUserCode = principalUser.Code,
                CreateUserName = principalUser.Name,
                DataLogId = CombUtil.NewComb()
            };
        }
    }
}