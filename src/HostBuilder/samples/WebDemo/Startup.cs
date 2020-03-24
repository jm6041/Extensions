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
using WebCommon.Hubs;
using WebCommon.Works;
using Microsoft.Extensions.Logging;

namespace WebDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 配置信息
        /// </summary>
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
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
            services.AddHostedService<TestWorker>();

            services.AddSignalR();
            services.AddGrpc();
            services.AddControllers().AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
                endpoints.MapGrpcService<MyTestService>();
                endpoints.MapControllers();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync(S.GetSystemInfo(context.Request, env, Configuration));
                });
            });

            var logger = app.ApplicationServices.GetService<ILogger<Startup>>();
            S.WriteAndLogConfiguration(logger, env, Configuration);
        }
    }
}
