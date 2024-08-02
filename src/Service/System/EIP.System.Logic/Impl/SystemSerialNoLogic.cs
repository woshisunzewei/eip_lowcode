/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2023/8/22 7:58:35
* 文件名: SystemSerialNoLogic
* 描述: 编号规则业务逻辑接口实现
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
namespace EIP.System.Logic.Impl
{
    /// <summary>
    /// 编号规则业务逻辑接口实现
    /// </summary>
    public class SystemSerialNoLogic : DapperAsyncLogic<SystemSerialNo>, ISystemSerialNoLogic
    {
        #region 构造函数

        private readonly ISystemSerialNoRepository _systemSerialnoRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="systemSerialnoRepository"></param>
        public SystemSerialNoLogic(ISystemSerialNoRepository systemSerialnoRepository)
        {
            _systemSerialnoRepository = systemSerialnoRepository;
        }

        #endregion

        /// <summary>
        /// 保存后，清除用户对应的单号信息
        /// </summary>
        /// <param name="prefix">单据前缀</param>
        /// <param name="UserId">用户Id</param>
        public void Clear(Guid configId, string model)
        {
            using (var fix = new SqlDatabaseFixture())
            {
                var user = EipHttpContext.CurrentUser();
                fix.Db.SystemSerialNoUser.Update(u => u.UserId == user.UserId && u.ConfigId == configId && u.Model == model, new { Value = "" });
            }
        }

        /// <summary>
        /// 获取用户ID、单据前缀
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="prefix"></param>
        private async Task<OperateStatus<string>> GetUserNo(SqlDatabaseFixture fix, SystemSerialCreateInput input)
        {
            OperateStatus<string> operateStatus = new OperateStatus<string>();
            var user = EipHttpContext.CurrentUser();
            var serialNo = await fix.Db.SystemSerialNo.FindAsync(f => f.ConfigId == input.ConfigId && f.Model == input.Model);
            var serialNoUser = await fix.Db.SystemSerialNoUser.FindAsync(f => f.UserId == user.UserId && f.ConfigId == input.ConfigId && f.Model == input.Model);

            //申请的单号
            if (serialNoUser != null)
            {
                operateStatus.Msg = Chs.Successful;
                operateStatus.Code = ResultCode.Success;
                operateStatus.Data = serialNoUser.Value;
                return operateStatus;
            }

            if (serialNo != null)
            {
                //判断是否重复
                var rule = serialNo.Rule.ReplaceEditor();
                var h1Elements = serialNo.Rule.GetNodes();
                if (h1Elements != null)
                {
                    foreach (var element in h1Elements)
                    {
                        //解码
                        var id = HttpUtility.UrlDecode(element.Id);
                        var data = id.JsonStringToObject<SystemSerialNoDataDto>();
                        switch (data.Type)
                        {
                            case "datetime"://时间
                                rule = rule.Replace(element.OuterHtml, DateTime.Now.ToString(data.Format));
                                break;
                            case "number"://数量
                                //不重复
                                if (data.Repeat.IsNotNullOrEmpty())
                                {
                                    //判断是否已经到了需要重新生成
                                    string repeatTime = "";
                                    switch (data.Repeat)
                                    {
                                        case "day":
                                            repeatTime = DateTime.Now.ToString("yyyyMMdd");
                                            break;
                                        case "month":
                                            repeatTime = DateTime.Now.ToString("yyyyMM");
                                            break;
                                        case "year":
                                            repeatTime = DateTime.Now.ToString("yyyy");
                                            break;
                                        default:
                                            break;
                                    }
                                    if (serialNo.RepeatTime.IsNullOrEmpty() || serialNo.RepeatTime != repeatTime)
                                    {
                                        serialNo.Value = string.Empty;
                                        serialNo.RepeatTime = repeatTime;
                                    }
                                }
                                //是否已生成了编号
                                if (serialNo.Value.IsNullOrEmpty())
                                {
                                    //生成编号
                                    serialNo.Value = data.BeginNumber.ToString().PadLeft(data.Length, '0');
                                    await fix.Db.SystemSerialNo.UpdateAsync(serialNo);
                                }
                                else
                                {
                                    //原有编号加1
                                    var numStr = (Convert.ToInt32(serialNo.Value) + 1).ToString().PadLeft(data.Length, '0');
                                    //是否能够继续编号
                                    if (numStr.Length > data.Length)
                                    {
                                        if (!data.Incremental)
                                        {
                                            operateStatus.Code = ResultCode.Error;
                                            operateStatus.Msg = "自动编号超过上限！";
                                            return operateStatus;
                                        }
                                    }
                                    serialNo.Value = numStr;
                                    await fix.Db.SystemSerialNo.UpdateAsync(serialNo);
                                }

                                rule = rule.Replace(element.OuterHtml, serialNo.Value);
                                break;
                            case "column"://字段
                                var value = "";
                                var column = input.Columns.FirstOrDefault(f => f.Name == data.Column);
                                if (column != null)
                                {
                                    value = column.Value;
                                }
                                rule = rule.Replace(element.OuterHtml, value);
                                break;
                            default:
                                break;
                        }
                    }
                }

                if (serialNoUser == null)
                {
                    SystemSerialNoUser systemSerialNoUser = new SystemSerialNoUser();
                    systemSerialNoUser.SerialNoUserId = CombUtil.NewComb();
                    systemSerialNoUser.ConfigId = input.ConfigId;
                    systemSerialNoUser.Model = input.Model;
                    systemSerialNoUser.UserId = user.UserId;
                    systemSerialNoUser.Value = rule;
                    systemSerialNoUser.CreateTime = DateTime.Now;
                    systemSerialNoUser.UpdateTime = DateTime.Now;
                    systemSerialNoUser.CreateUserId = user.UserId;
                    systemSerialNoUser.CreateUserName = user.Name;
                    systemSerialNoUser.UpdateUserId = user.UserId;
                    systemSerialNoUser.UpdateUserName = user.Name;
                    await fix.Db.SystemSerialNoUser.InsertAsync(systemSerialNoUser);
                }
                else
                {
                    serialNoUser.Value = rule;
                    await fix.Db.SystemSerialNoUser.UpdateAsync(serialNoUser);
                }
                operateStatus.Msg = Chs.Successful;
                operateStatus.Code = ResultCode.Success;
                operateStatus.Data = rule;
                return operateStatus;
            }
            return operateStatus;
        }

        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<string>> Create(SystemSerialCreateInput input)
        {
            OperateStatus<string> operateStatus = new OperateStatus<string>();
            var user = EipHttpContext.CurrentUser();
            using (var fix = new SqlDatabaseFixture())
            {
                var serialNo = await fix.Db.SystemSerialNo.FindAsync(f => f.ConfigId == input.ConfigId && f.Model == input.Model);
                if (serialNo == null)
                {
                    try
                    {
                        serialNo = new SystemSerialNo();
                        serialNo.SerialNoId = CombUtil.NewComb();
                        serialNo.Rule = input.Rule;
                        serialNo.Model = input.Model;
                        serialNo.ConfigId = input.ConfigId;
                        serialNo.CreateTime = DateTime.Now;
                        serialNo.UpdateTime = DateTime.Now;
                        serialNo.CreateUserId = user.UserId;
                        serialNo.CreateUserName = user.Name;
                        serialNo.UpdateUserId = user.UserId;
                        serialNo.UpdateUserName = user.Name;
                        await fix.Db.SystemSerialNo.InsertAsync(serialNo);
                    }
                    catch (Exception ex)
                    {
                        operateStatus.Msg = ex.Message;
                        return operateStatus;
                    }
                }
                else
                {
                    serialNo.Rule = input.Rule;
                    serialNo.UpdateTime = DateTime.Now;
                    await fix.Db.SystemSerialNo.UpdateAsync(serialNo);
                }

                if (input.UserOccupation)
                {
                    operateStatus = await GetUserNo(fix, input);
                    return operateStatus;
                }
                //判断是否重复
                var rule = serialNo.Rule.ReplaceEditor();
                var h1Elements = serialNo.Rule.GetNodes();
                if (h1Elements != null)
                {
                    foreach (var element in h1Elements)
                    {
                        //解码
                        var id = HttpUtility.UrlDecode(element.Id);
                        var data = id.JsonStringToObject<SystemSerialNoDataDto>();
                        switch (data.Type)
                        {
                            case "datetime"://时间
                                rule = rule.Replace(element.OuterHtml, DateTime.Now.ToString(data.Format));
                                break;
                            case "number"://数量
                                //不重复
                                if (data.Repeat.IsNotNullOrEmpty())
                                {
                                    //判断是否已经到了需要重新生成
                                    string repeatTime = "";
                                    switch (data.Repeat)
                                    {
                                        case "day":
                                            repeatTime = DateTime.Now.ToString("yyyyMMdd");
                                            break;
                                        case "month":
                                            repeatTime = DateTime.Now.ToString("yyyyMM");
                                            break;
                                        case "year":
                                            repeatTime = DateTime.Now.ToString("yyyy");
                                            break;
                                        default:
                                            break;
                                    }
                                    if (serialNo.RepeatTime.IsNullOrEmpty() || serialNo.RepeatTime != repeatTime)
                                    {
                                        serialNo.Value = string.Empty;
                                        serialNo.RepeatTime = repeatTime;
                                    }
                                }
                                //是否已生成了编号
                                if (serialNo.Value.IsNullOrEmpty())
                                {
                                    //生成编号
                                    serialNo.Value = data.BeginNumber.ToString().PadLeft(data.Length, '0');
                                    await fix.Db.SystemSerialNo.UpdateAsync(serialNo);
                                }
                                else
                                {
                                    //原有编号加1
                                    var numStr = (Convert.ToInt32(serialNo.Value) + 1).ToString().PadLeft(data.Length, '0');
                                    //是否能够继续编号
                                    if (numStr.Length > data.Length)
                                    {
                                        if (!data.Incremental)
                                        {
                                            operateStatus.Code = ResultCode.Error;
                                            operateStatus.Msg = "自动编号超过上限！";
                                            return operateStatus;
                                        }
                                    }
                                    serialNo.Value = numStr;
                                    await fix.Db.SystemSerialNo.UpdateAsync(serialNo);
                                }

                                rule = rule.Replace(element.OuterHtml, serialNo.Value);
                                break;
                            case "column"://字段
                                var value = "";
                                var column = input.Columns.FirstOrDefault(f => f.Name == data.Column);
                                if (column != null)
                                {
                                    value = column.Value;
                                }
                                rule = rule.Replace(element.OuterHtml, value);
                                break;
                            default:
                                break;
                        }
                    }
                }
                operateStatus.Code = ResultCode.Success;
                operateStatus.Msg = Chs.QuerySuccessful;
                operateStatus.Data = rule;
            }
            return operateStatus;
        }
    }
}