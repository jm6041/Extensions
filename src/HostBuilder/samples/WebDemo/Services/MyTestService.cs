using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using WebDemo.Managers;

namespace WebDemo.Services
{
    /// <summary>
    /// 测试服务
    /// </summary>
    public class MyTestService : MyTests.MyTestsBase
    {
        private readonly TestsManager _manager;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="manager"></param>
        public MyTestService(TestsManager manager)
        {
            _manager = manager;
        }
        /// <summary>
        /// PutTest
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<TestResult> PutTest(InputDto request, ServerCallContext context)
        {
            var tr = _manager.PutTest(request);
            return Task.FromResult(tr);
        }
        /// <summary>
        /// 获得结果
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<RequestResult> GetRequestResult(Empty request, ServerCallContext context)
        {
            var rr = new RequestResult()
            {
                Host = context.Host,
                AppName = M.AppName,
            };
            var dic = context.RequestHeaders.ToDictionary(key => key.Key, val => val.Value.ToString());
            rr.Headers.Add(dic);
            return Task.FromResult(rr);
        }
    }
}
