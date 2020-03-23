using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebCommon.Hubs;

namespace WebCommon.Works
{
    public class TestWorker : BackgroundService
    {
        private readonly IHubContext<TestHub> _hubContext;
        private readonly ILogger<TestWorker> _logger;
        private int _executionCount = 0;

        public TestWorker(IHubContext<TestHub> hubContext, ILogger<TestWorker> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
        }

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

    public class TestHubMsg
    {
        public int Count { get; set; }
        public string Time { get; set; } = DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss zzz");
    }
}
