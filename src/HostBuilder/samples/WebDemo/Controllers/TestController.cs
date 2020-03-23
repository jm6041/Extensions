using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebDemo.Managers;
using Tests;

namespace WebDemo.Controllers
{
    /// <summary>
    /// 测试控制器
    /// </summary>
    [Route(M.ApiScope + "/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly TestsManager _manager;
        public TestController(TestsManager manager)
        {
            _manager = manager;
        }

        /// <summary>
        /// 测试输入
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<TestResult> PutTest([FromBody]InputDto request)
        {
            var tr = _manager.PutTest(request);
            return Task.FromResult(tr);
        }

        /// <summary>
        /// 获得请求信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public RequestResult GetRequestResult()
        {
            var rr = new RequestResult()
            {
                AppName = M.AppName,
                Host = Request.Host.Value,
            };
            var dic = Request.Headers.ToDictionary(key => key.Key, val => val.Value.ToString());
            rr.Headers.Add(dic);
            return rr;
        }
    }
}
