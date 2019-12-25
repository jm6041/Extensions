using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace WebCommon.Hubs
{
    [AllowAnonymous]
    public class TestHub : Hub
    {
        public async Task Send(string msg)
        {
            string rmsg = string.Format("{0:yyyy-MM-dd HH:mm:ss.fff}: {1}", DateTimeOffset.Now, msg);
            await Clients.All.SendAsync("Received", rmsg);
        }

        public async override Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("Connected", this.Context.ConnectionId);
        }
    }
}
