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
using System.Text;

namespace EIP.Workflow.Repository.Impl
{
    /// <summary>
    /// 工作流实例接口实现
    /// </summary>
    public class WorkflowProcessRepository : IWorkflowProcessRepository
    {
        public Task<IEnumerable<WorkflowProcess>> FindAllProcess()
        {
            StringBuilder sql = new StringBuilder($@"select  pro.TypeId,pro.Icon,pro.Theme,pro.IconColor,pro.Image,pro.Name,pro.Remark, pro.Version, pro.ProcessId from Workflow_Process pro where  pro.IsDelete=0 order by pro.OrderNo");
            return new SqlMapperUtil().SqlWithParams<WorkflowProcess>(sql.ToString());
        }

        /// <summary>
        /// 获取可以发送的流程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<IEnumerable<WorkflowProcess>> FindSendLibrary(IdInput input)
        {
            StringBuilder sql = new StringBuilder($@"select pro.TypeId,pro.Icon,pro.Theme,pro.IconColor,pro.Image,pro.Name,pro.Remark, pro.Version, pro.ProcessId from Workflow_Process pro 
left join Workflow_Permission  per on pro.ProcessId=per.ProcessId
left join System_PermissionUser perUser on per.PrivilegeMasterValue=perUser.PrivilegeMasterValue
where PrivilegeMasterUserId='{input.Id}' and pro.IsDelete=0 and pro.ShowLibrary=1 
group by pro.TypeId,pro.Icon,pro.Theme,pro.IconColor,pro.Image,pro.Name,pro.Remark, pro.Version, pro.ProcessId,pro.OrderNo
order by pro.OrderNo");
            return new SqlMapperUtil().SqlWithParams<WorkflowProcess>(sql.ToString());
        }

        /// <summary>
        /// 根据流程类型获取流程信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResults<WorkflowProcessFindOutput>> Find(WorkflowProcessFindInput input)
        {
            var sql = new StringBuilder(@"SELECT 
                                        process.ProcessId,
                                        process.Name,
                                        process.ShortName,
                                        process.Sn,
                                        process.Version,
                                        process.Remark,
                                        process.IsFreeze,
                                        process.OrderNo,
                                        process.Icon,
                                        process.Image,
                                        process.Theme,
                                        process.CreateTime,
                                        process.CreateUserName,
                                        process.UpdateTime,
                                        process.UpdateUserName,
                                        process.ShowLibrary,
                                        process.Thumbnail,
                                        (select count(*) from System_Menu menu where menu.MenuId=ProcessId) IsAgile,
                                        @rowNumber, @recordCount FROM Workflow_Process process
                                          @where and IsDelete=0  ");/* and process.ProcessId not in (select MenuId from System_Menu )*/
            if (input.Sidx.IsNullOrEmpty())
            {
                input.Sidx = " process.UpdateTime";
            }

            return new SqlMapperUtil().PagingQuerySqlAsync<WorkflowProcessFindOutput>(sql.ToString(), input);
        }
    }
}