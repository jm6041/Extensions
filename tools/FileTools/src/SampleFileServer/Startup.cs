using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SampleFileServer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SampleFileServer
{
    /// <summary>
    ///启动配置
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
        /// 依赖注入配置
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            var dirs = GetFileDirs(Configuration);
            FileDirsInfo fileDirsInfo = new FileDirsInfo() { Data = dirs };
            services.AddSingleton(fileDirsInfo);

            services.AddControllersWithViews();
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
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            var dirs = GetFileDirs(Configuration);
            foreach (var item in dirs)
            {
                var dir = item.Dir;
                Directory.CreateDirectory(dir);
                app.UseFileServer(new FileServerOptions
                {
                    FileProvider = new PhysicalFileProvider(dir),
                    RequestPath = item.Path,
                    EnableDirectoryBrowsing = true
                });
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            StartupHelper.WriteAndLogConfiguration(app.ApplicationServices.GetService<ILogger<Startup>>(), env, Configuration);
        }

        private static List<FileDirModel> GetFileDirs(IConfiguration config)
        {
            List<FileDirModel> fds = new List<FileDirModel>();
            var dirs = config.GetSection("Dirs").GetChildren();
            HashSet<string> dvs = new HashSet<string>();
            foreach (var kv in dirs)
            {
                var v = kv.Value?.Trim();
                if (string.IsNullOrEmpty(v))
                {
                    v = "/sources";
                }
                if (dvs.Add(v))
                {
                    var k = kv.Key?.Trim();
                    if (!string.IsNullOrEmpty(k) && k != "/")
                    {
                        if (!k.StartsWith("/"))
                        {
                            k = "/" + k;
                        }
                        fds.Add(new FileDirModel() { Path = k, Dir = v });
                    }
                }
            }
            if (!fds.Any())
            {
                fds.Add(FileDirModel.Default);
            }
            return fds;
        }
    }
}
