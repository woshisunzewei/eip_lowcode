﻿/**************************************************************
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
using EIP.Common.Core.Util;
using System;

namespace EIP.Common.Log.Handler
{
    public class SqlLogHandler : BaseHandler<SqlLog>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SqlLogHandler(string operateSql,
            DateTime endDateTime,
            double elapsedTime,
            string parameter
            ) : base("SqlLog")
        {

            Log = new SqlLog
            {
                SqlLogId = CombUtil.NewComb(),
                CreateTime = DateTime.Now,
                OperateSql = operateSql,
                ElapsedTime = elapsedTime,
                EndDateTime = endDateTime,
                Parameter = parameter
            };
        }
    }
}