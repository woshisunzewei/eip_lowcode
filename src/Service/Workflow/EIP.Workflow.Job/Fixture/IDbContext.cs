using EIP.Common.Repository.MicroOrm;
using EIP.Workflow.Job.Entities;

namespace EIP.Workflow.Job.Fixture
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDbContext
    {
        /// <summary>
        /// 测试
        /// </summary>
        IDapperRepository<FormDemo> FormDemo { get; }
    }
}