using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebDemo.Managers;
using WebDemo.Services;
using Microsoft.Extensions.Logging;
using WebDemo.Hubs;

namespace WebDemo
{
    /// <summary>
    /// 启动配置
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 配置信息
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 依赖配置
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOpenApiDocument(c =>
            {
                c.Title = "WebDemo";
                c.Version = "v1";
                c.Description = "WebDemo invoked through grpc or open api.";
                c.DocumentName = "WebDemoOpenApi";
            });

            services.AddScoped<TestsManager>();

            services.AddSignalR();
            services.AddGrpc();
            services.AddControllers().AddNewtonsoftJson();
        }

        /// <summary>
        /// 管道配置
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseOpenApi();
            // NSwag
            app.UseSwaggerUi3(c =>
            {
                c.DocExpansion = "list";
                c.DefaultModelExpandDepth = 3;
                c.ValidateSpecification = true;
            });

            app.UseRouting();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<TestHub>("/test");
                endpoints.MapGrpcService<GreeterService>();
                endpoints.MapGrpcService<MyTestService>();
                endpoints.MapControllers();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync(AppInfoHelper.GetSystemInfo(context.Request, env, Configuration));
                });
            });

            var logger = app.ApplicationServices.GetRequiredService<ILogger<Startup>>();
            AppInfoHelper.WriteAndLogConfiguration(logger, env, Configuration);
        }
    }
}
