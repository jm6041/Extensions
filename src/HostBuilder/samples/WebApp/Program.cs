
#if DEBUG
WebApplicationHelper.IsDebug = true;
#endif
string contentRoot = WebApplicationHelper.GetContentRoot(args);
// 日志目录
string logsDir = WebApplicationHelper.GetDefaultLogDirectory(contentRoot);
// 状态文件
string statusFileName = WebApplicationHelper.GetFileName(logsDir, "status");
// 系统崩溃错误文件
string errorFileName = WebApplicationHelper.GetFileName(logsDir, "error");

try
{
    var builder = WebApplicationHelper.CreateBuilder(args, statusFileName);
    var app = builder.Build();
    WebApplicationHelper.WriteStartupLog(args, statusFileName);

    app.MapGet("/", async context =>
    {
        await context.Response.WriteAsync(AppInfoHelper.GetSystemInfo(context.Request, app.Environment, app.Configuration));
    });
    app.Run();
}
catch (Exception ex)
{
    WebApplicationHelper.WriteErrorLog(ex, errorFileName);
    throw;
}
finally
{
    WebApplicationHelper.WriteExitLog(statusFileName);
}
