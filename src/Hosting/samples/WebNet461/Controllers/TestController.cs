using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tests;

namespace WebNet461.Controllers
{
    /// <summary>
    /// 测试控制器
    /// </summary>
    [Route(M.ApiScope + "/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly MyTests.MyTestsClient _client;
        public TestController(MyTests.MyTestsClient client)
        {
            _client = client;
        }

        /// <summary>
        /// 测试输入
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<TestResult> PutTest([FromBody]InputDto request)
        {
            return await _client.PutTestAsync(request);
        }

        /// <summary>
        /// 获得请求信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<RequestResult> GetRequestResult()
        {
            return await _client.GetRequestResultAsync(new Google.Protobuf.WellKnownTypes.Empty());
        }
    }
}
