using AspectCore.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FluentValidation.Results;
using FluentValidation;

namespace WebApp2.Interceptors
{
    /// <summary>
    /// 通用拦截器
    /// </summary>
    internal class CommonInterceptorAttribute : AbstractInterceptorAttribute
    {
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

        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            var logger = context.ServiceProvider.GetService<ILogger<CommonInterceptorAttribute>>();
            var methodName = context.ImplementationMethod.Name;
            var failures = ValidateArguments(context.Parameters);
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
                var httpContextAccessor = context.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
                string path = httpContextAccessor.HttpContext.Request.Path;
                //Console.WriteLine(string.Format("Entered Method:{0}, Arguments: {1}", methodName, string.Join(",", invocation.Arguments)));
                logger.LogInformation($"Path:{path} Entered Method:{methodName}, Arguments: {string.Join(",", context.Parameters)}");
                await next(context);
                //Console.WriteLine(string.Format("Sucessfully executed method:{0}", methodName));
                logger.LogInformation("Sucessfully executed method:{0}", methodName);
            }
            catch (Exception e)
            {
                //Console.WriteLine(string.Format("Method:{0}, Exception:{1}", methodName, e.Message));
                logger.LogWarning("Method:{0}, Exception:{1}", methodName, e.Message);
                throw;
            }
            finally
            {
                //Console.WriteLine(string.Format("Exiting Method:{0}", methodName));
                logger.LogInformation("Exiting Method:{0}", methodName);
            }
        }
    }
}
