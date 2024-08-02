namespace EIP.Api.Middleware
{
    public class CustomizationLimitMiddleware : IpRateLimitMiddleware
    {
        private readonly IpRateLimitOptions _options;
        private readonly IIpPolicyStore _ipPolicyStore;

        public CustomizationLimitMiddleware(RequestDelegate next,
            IProcessingStrategy processingStrategy,
            IOptions<IpRateLimitOptions> options,
            IRateLimitCounterStore counterStore,
            IIpPolicyStore policyStore,
            IRateLimitConfiguration config,
            ILogger<IpRateLimitMiddleware> logger) : base(next, processingStrategy, options, counterStore, policyStore, config, logger)
        {
            _options = options.Value;
            _ipPolicyStore = policyStore;
        }

        /// <summary>
        /// 当请求过于频繁时 接口重写
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="rule"></param>
        /// <param name="retryAfter"></param>
        /// <returns></returns>
        public override Task ReturnQuotaExceededResponse(HttpContext httpContext, RateLimitRule rule, string retryAfter)
        {
            var ip = httpContext.Request.Headers["X-Real-IP"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = httpContext.Connection.RemoteIpAddress.ToString();
            }
            var message = string.Format("{{ \"msg\": \"操作频率过快.请稍后重试！.\", \"code\": 500}}", ip, rule.Limit, rule.Period, retryAfter);
            if (!_options.DisableRateLimitHeaders)
            {
                httpContext.Response.Headers["Retry-After"] = retryAfter;
            }
            var request = httpContext.Request;
            var currentUser = EipHttpContext.CurrentUser();
            RateLimitLogHandler rateLimitLogHandler = new RateLimitLogHandler(request, new LogUser
            {
                UserId = currentUser.UserId == Guid.Empty ? null : currentUser.UserId.ToString(),
                Name = currentUser.Name,
                RemoteIpAddress=currentUser.RemoteIpAddress,
                UserAgent = currentUser.UserAgent,
                OsInfo = currentUser.OsInfo,
                Browser = currentUser.Browser
            })
            {
                Log =
                {
                    Message =string.Format("每{0}秒{1}次.{2}秒重试", rule.Period,  rule.Limit, retryAfter),
                    RemoteIp = ip,
                }
            };
            Endpoint endpoint = httpContext.GetEndpoint();
            if (endpoint != null)
            {
                var controllerActionDescriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
                rateLimitLogHandler.Log.ControllerName = controllerActionDescriptor.ControllerName;
                rateLimitLogHandler.Log.ActionName = controllerActionDescriptor.ActionName;
            }
            rateLimitLogHandler.WriteLog();
            //写入数据库记录
            httpContext.Response.StatusCode = _options.QuotaExceededResponse?.StatusCode ?? _options.HttpStatusCode;
            httpContext.Response.ContentType = _options.QuotaExceededResponse?.ContentType ?? "application/json;charset=utf-8";
            return httpContext.Response.WriteAsync(message);
        }
    }
}
