namespace EIP.Api.Setup
{
    /// <summary>
    /// 压缩
    /// </summary>
    public static class Compression
    {
        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="services"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddCompressionSetUp(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });
            services.AddResponseCompression(options =>
            {
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes;//使用默认配置
                options.Providers.Add<BrotliCompressionProvider>();//优先使用Br方式压缩响应
                options.Providers.Add<GzipCompressionProvider>();//使用Gzip方式压缩响应
            });
        }
    }
}
