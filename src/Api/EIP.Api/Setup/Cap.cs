namespace EIP.Api.Setup
{
    /// <summary>
    /// 
    /// </summary>
    public static class Cap
    {
        /// <summary>
        /// Cap
        /// </summary>
        /// <param name="services"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddCapSetUp(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            var redisConnectionString = ConfigurationUtil.GetRedisConnectionString();
            var connectionType = ConfigurationUtil.GetDbConnectionType();
            var connectionString = ConfigurationUtil.GetDbConnectionString();
            switch (connectionType)
            {
                case ResourceDataBaseType.Mysql:
                    services.AddCap(x =>
                    {
                        x.UseMySql(opt =>
                        {
                            opt.ConnectionString = connectionString;
                        });
                        x.UseRedis(redisConnectionString);
                        x.UseDashboard();
                        x.FailedRetryCount = 5;
                    });
                    break;
                default:
                    services.AddCap(x =>
                    {
                        x.UseSqlServer(opt =>
                        {
                            opt.ConnectionString = connectionString;
                        });
                        x.UseRedis(redisConnectionString);
                        x.UseDashboard();
                        x.FailedRetryCount = 5;
                    });
                    break;
            }
        }
    }
}
