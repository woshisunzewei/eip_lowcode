namespace EIP.Api.Setup
{
    /// <summary>
    /// Swagger
    /// </summary>
    public static class Swagger
    {
        /// <summary>
        /// Swagger
        /// </summary>
        /// <param name="services"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddSwaggerSetUp(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("api", new OpenApiInfo
                {
                    Version = "V1.0",
                    Title = "EIP低代码平台",
                    Description = "EIP低代码平台",
                    Contact = new OpenApiContact { Name = "孙泽伟", Email = "1039318332@qq.com", Url = new Uri("https://www.eipflow.com") },
                });
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                c.IncludeXmlComments(Path.Combine(basePath, "EIP.System.Controller.xml"), true);
                c.IncludeXmlComments(Path.Combine(basePath, "EIP.System.Models.xml"), true);
                c.IncludeXmlComments(Path.Combine(basePath, "EIP.Workflow.Controller.xml"), true);
                c.IncludeXmlComments(Path.Combine(basePath, "EIP.Workflow.Models.xml"), true);
                //这里是给Swagger添加验证的部分
                // JWT认证                                                 
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Type = SecuritySchemeType.Http,
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Description = "Authorization:Bearer 请输入带有Bearer的Token<br/><b></b>",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme{Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });
        }
    }
}
