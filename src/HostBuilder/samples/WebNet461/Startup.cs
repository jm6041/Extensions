using System;
using System.IO;
using Grpc.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebCommon.Hubs;
using WebCommon.Works;

namespace WebNet461
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOpenApiDocument(c =>
            {
                c.Title = "WebNet461";
                c.Version = "v1";
                c.Description = "WebNet461 open api.";
                c.DocumentName = "WebNet461OpenApi";
            });

            GrpcOptions grpcOpt = new GrpcOptions();
            Configuration.GetSection("Grpc").Bind(grpcOpt);

            // 服务端证书文件
            var serverCertificateFile = Path.Combine(AppContext.BaseDirectory, grpcOpt.ServerCertificateFile);
            // 证书文件
            var certificateFile = Path.Combine(AppContext.BaseDirectory, grpcOpt.ClientCertificateFile);
            // 证书文件私钥
            var certificateKeyFile = Path.Combine(AppContext.BaseDirectory, grpcOpt.ClientCertificateKeyFile);

            // 服务端证书
            var serverCertificate = File.ReadAllText(serverCertificateFile);
            // 证书
            var clientcert = File.ReadAllText(certificateFile);
            // 私钥
            var clientkey = File.ReadAllText(certificateKeyFile);

            var ssl = new SslCredentials(serverCertificate, new KeyCertificatePair(clientcert, clientkey));
            var channel = new Channel(grpcOpt.Host, grpcOpt.Port, ssl);
            services.AddSingleton((sp) => new Tests.MyTests.MyTestsClient(channel));

            services.AddHostedService<TestWorker>();

            services.AddSignalR();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseOpenApi();
            // NSwag
            app.UseSwaggerUi3(c =>
            {
                c.DocExpansion = "list";
                c.DefaultModelExpandDepth = 3;
                c.ValidateSpecification = true;
            });

            app.UseSignalR(routes =>
            {
                routes.MapHub<TestHub>("/test");
            });

            app.UseMvc();

            app.Map("/systeminfo", appbuilder =>
            {
                appbuilder.Run(async (context) =>
                {
                    await context.Response.WriteAsync(S.GetSystemInfo(context.Request, env, Configuration));
                });
            });

            S.WriteConfiguration(env, Configuration);
        }
    }
}
