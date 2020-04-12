using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using FluentValidation.Results;
using FluentValidation;

namespace WebApp.Interceptors
{
    /// <summary>
    /// 通用拦截器
    /// </summary>
    internal class CommonInterceptor : IInterceptor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CommonInterceptor> _logger;
        public CommonInterceptor(IHttpContextAccessor httpContextAccessor, ILogger<CommonInterceptor> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        /// <summary>
        /// 校验输入参数
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private List<ValidationFailure> ValidateArguments(object[] args)
        {
            List<ValidationFailure> failures = new List<ValidationFailure>();
            foreach (var arg in args)
            {
                var argType = arg.GetType();
                string vn = argType + "Validator";
                var objectHandle = Activator.CreateInstance(argType.Assembly.FullName, vn);
                if (objectHandle != null)
                {
                    var source = objectHandle.Unwrap();
                    if (source != null)
                    {
                        IValidator validator = source as IValidator;
                        ValidationResult vr = validator.Validate(arg);
                        bool success = vr.IsValid;
                        if (!success)
                        {
                            failures.AddRange(vr.Errors);
                        }
                    }
                }
            }
            return failures;
        }

        public void Intercept(IInvocation invocation)
        {
            var methodName = invocation.Method.Name;
            var failures = ValidateArguments(invocation.Arguments);
            if (failures.Any())
            {
                foreach (var f in failures)
                {
                    Console.WriteLine("PropertyName:{0}   Code:{1}   Error:{2}", f.PropertyName, f.ErrorCode, f.ErrorMessage);
                }
                return;
            }
            
            try
            {
                string path = _httpContextAccessor.HttpContext.Request.Path;
                //Console.WriteLine(string.Format("Entered Method:{0}, Arguments: {1}", methodName, string.Join(",", invocation.Arguments)));
                _logger.LogInformation($"Path:{path} Entered Method:{methodName}, Arguments: {string.Join(",", invocation.Arguments)}");
                invocation.Proceed();
                //Console.WriteLine(string.Format("Sucessfully executed method:{0}", methodName));
                _logger.LogInformation("Sucessfully executed method:{0}", methodName);
            }
            catch (Exception e)
            {
                //Console.WriteLine(string.Format("Method:{0}, Exception:{1}", methodName, e.Message));
                _logger.LogWarning("Method:{0}, Exception:{1}", methodName, e.Message);
                throw;
            }
            finally
            {
                //Console.WriteLine(string.Format("Exiting Method:{0}", methodName));
                _logger.LogInformation("Exiting Method:{0}", methodName);
            }
        }
    }
}
