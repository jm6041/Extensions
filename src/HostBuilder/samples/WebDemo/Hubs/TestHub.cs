using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace WebDemo.Hubs
{
    /// <summary>
    /// 测试Hub
    /// </summary>
    [AllowAnonymous]
    public class TestHub : Hub
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public async Task Send(string msg)
        {
            string rmsg = string.Format("{0:yyyy-MM-dd HH:mm:ss.fff}: {1}", DateTimeOffset.Now, msg);
            await Clients.All.SendAsync("Received", rmsg);
        }
        /// <summary>
        /// 连接完成
        /// </summary>
        /// <returns></returns>
        public async override Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("Connected", this.Context.ConnectionId);
        }
    }
}
