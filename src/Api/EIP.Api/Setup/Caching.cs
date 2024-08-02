namespace EIP.Api.Setup
{
    /// <summary>
    /// 缓存
    /// </summary>
    public static class Caching
    {
        /// <summary>
        /// 缓存
        /// </summary>
        /// <param name="services"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddCachingSetUp(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            var redisConnectionString = ConfigurationUtil.GetRedisConnectionString();
            services.AddEasyCaching(options =>
            {
                options.UseCSRedis(config =>
                {
                    config.DBConfig = new CSRedisDBOptions
                    {
                        ConnectionStrings = new List<string> { redisConnectionString }
                    };
                }).UseCSRedisLock().WithJson(configure: x =>
                {
                    x.TypeNameHandling = TypeNameHandling.None;
                }, EasyCachingConstValue.DefaultCSRedisName);
            });
            services.ConfigureCastleInterceptor(options => options.CacheProviderName = EasyCachingConstValue.DefaultCSRedisName);
            RedisHelper.Initialization(new CSRedis.CSRedisClient(redisConnectionString));

            //初始化系统配置项（保证启动生成缓存）
            using (var fix = new SqlDatabaseFixture())
            {
                var all = fix.Db.SystemConfig.FindAll();
                IList<GlobalConfig> globalConfigs = new List<GlobalConfig>();
                foreach (var item in all)
                {
                    globalConfigs.Add(new GlobalConfig
                    {
                        Key = item.Key,
                        Value = item.Value
                    });
                }
                GlobalParams.DeleteValue(globalConfigs);
            }
        }
    }
}
