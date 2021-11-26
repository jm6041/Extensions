using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebDemo.Hubs;

namespace WebDemo.Works
{
    /// <summary>
    /// 测试Worker
    /// </summary>
    public class TestWorker : BackgroundService
    {
        private readonly IHubContext<TestHub> _hubContext;
        private readonly ILogger<TestWorker> _logger;
        private int _executionCount = 0;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="hubContext"></param>
        /// <param name="logger"></param>
        public TestWorker(IHubContext<TestHub> hubContext, ILogger<TestWorker> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
        }
        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("TestWorker running at: {time}", DateTimeOffset.Now);
                await DoWork();
                await Task.Delay(1000, stoppingToken);
            }
        }

        private async Task DoWork()
        {
            _executionCount++;
            TestHubMsg msg = new TestHubMsg() { Count = _executionCount };
            await _hubContext.Clients.All.SendAsync("DataReceived", msg);
        }
    }
    /// <summary>
    /// 测试消息
    /// </summary>
    public class TestHubMsg
    {
        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public string Time { get; set; } = DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss zzz");
    }
}
