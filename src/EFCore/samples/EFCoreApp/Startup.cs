using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using EFCoreData;

namespace EFCore3App
{
    /// <summary>
    /// 启动
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

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString(Configuration.GetSection("db:ConnectionName")?.Value ?? "DefaultConnection");
            string assemblyFullName = this.GetType().Assembly.FullName;
            services.AddDbContext<MyDbContext>(options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly(assemblyFullName)));
            AppInfoHelper.WriteConnectionString(connectionString, Configuration);

            services.AddOpenApiDocument(c =>
            {
                c.Title = "Users";
                c.Version = "v1";
                c.Description = "Users sample open api.";
                c.DocumentName = "UsersOpenApi";
            });

            services.AddControllers();
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            AppInfoHelper.WriteAndLogConfiguration(app.ApplicationServices.GetService<ILogger<Startup>>(), env, Configuration);
        }
    }
}
