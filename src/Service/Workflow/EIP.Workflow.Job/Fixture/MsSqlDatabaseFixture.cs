using System;

namespace EIP.Workflow.Job.Fixture
{
    /// <summary>
    /// 
    /// </summary>
    public class MsSqlDatabaseFixture : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public MsSqlDatabaseFixture()
        {
            Db = new MsSqlDbContext(Config.ConnectionString);
        }
        /// <summary>
        /// 
        /// </summary>
        public MsSqlDbContext Db { get; }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Db.Dispose();
        }
    }
}