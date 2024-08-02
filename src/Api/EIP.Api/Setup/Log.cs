namespace EIP.Api.Setup
{
    /// <summary>
    /// 日志
    /// </summary>
    public static class Log
    {
        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="services"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddNLogSetUp(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            var connectionType = ConfigurationUtil.GetDbConnectionType();
            string? configName;
            switch (connectionType)
            {
                case ResourceDataBaseType.Mysql:
                    configName = "nlog.mysql.config";
                    break;
                default:
                    configName = "nlog.config";
                    break;
            }
            LogManager.Setup().LoadConfigurationFromFile(configName).GetCurrentClassLogger();
            LogManager.Configuration.Variables["connectionString"] = ConfigurationUtil.GetDbConnectionString();
        }
    }
}
