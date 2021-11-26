using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace WebDemo.Managers
{
    /// <summary>
    /// 测试
    /// </summary>
    public class TestsManager
    {
        /// <summary>
        /// PutTest
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public TestResult PutTest(InputDto request)
        {
            DateTimeOffset now = DateTimeOffset.Now;
            Guid gid = Guid.NewGuid();
            var gbs = gid.ToByteArray();
            TestResult tr = new TestResult()
            {
                Date = now.ToString(),
                Timestamp = now.ToUnixTimeMilliseconds(),
                Gid = gid.ToString("N"),
                Base64 = Base64UrlEncoder.Encode(gbs),
                Myid = NewId(),
                Input = request,
            };
            return tr;
        }

        private static string NewId()
        {
            byte[] gbs = Guid.NewGuid().ToByteArray();
            return string.Format("CT{0:yyyyMMdd}{1}", DateTimeOffset.UtcNow, Base64UrlEncoder.Encode(gbs));
        }
    }
}
