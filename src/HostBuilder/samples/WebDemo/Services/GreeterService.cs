using Grpc.Core;
using WebDemo;

namespace WebDemo.Services
{
    /// <summary>
    /// Greeter
    /// </summary>
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Hello
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }
    }
}
