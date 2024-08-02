namespace EIP.System.Repository.Impl
{
    /// <summary>
    ///  敏捷开发
    /// </summary>
    public class SystemAgileRepository : ISystemAgileRepository
    {
        /// <summary>
        /// 获取操作日志
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        public Task<PagedResults<SystemAgileFindOutput>> Find(SystemAgileFindInput paging)
        {
            var sql = new StringBuilder($@"SELECT ConfigId
                                                 ,Name
                                                 ,DataFrom
                                                 ,DataFromName
                                                 ,IsFreeze
                                                 ,Remark
                                                 ,OrderNo
                                                 ,FormCategory
                                                 ,EditConfigId
                                                 ,CreateTime
                                                 ,CreateUserName
                                                 ,UpdateTime
                                                 ,UpdateUserName
                                                 ,Thumbnail
                                                 ,@rowNumber 
                                                 ,@recordCount FROM System_Agile @where and IsDelete=0 ");
            if (paging.Sidx.IsNullOrEmpty())
            {
                paging.Sidx = " OrderNo ";
            }

            if (paging.ConfigType.HasValue)
            {
                sql.Append($" and ConfigType={paging.ConfigType} ");
            }
            if (paging.FormCategory.HasValue)
            {
                sql.Append($" and FormCategory={paging.FormCategory} ");
            }
            if (paging.MenuIdNull.HasValue && (bool)paging.MenuIdNull)
            {
                sql.Append($" and menuId is null  ");
            }
            if (paging.MenuId.HasValue)
            {
                sql.Append($" and menuId='{paging.MenuId}'  ");
            }
            if (paging.ConfigId.HasValue)
            {
                sql.Append($" and ConfigId='{paging.ConfigId}' ");
            }
            return new SqlMapperUtil().PagingQuerySqlAsync<SystemAgileFindOutput>(sql.ToString(), paging);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<IEnumerable<SystemAgileFindBaseOutput>> FindBase(SystemAgileFindBaseInput input)
        {
            StringBuilder sql = new StringBuilder($@"select ConfigId,Name from System_Agile config where IsFreeze=0 and IsDelete=0 ");
            if (input.ConfigType.HasValue)
            {
                sql.Append($" and ConfigType={input.ConfigType} ");
            }
            if (input.ConfigId.HasValue)
            {
                sql.Append($" and ConfigId='{input.ConfigId}' ");
            }
            return new SqlMapperUtil().SqlWithParams<SystemAgileFindBaseOutput>(sql.ToString());
        }
    }
}