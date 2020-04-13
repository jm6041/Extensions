using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Jimlicat.Functions.Interceptors
{
    /// <summary>
    /// <see cref="FunctionAttribute"/>拦截器
    /// </summary>
    public sealed class FunctionInterceptorAttribute : AbstractInterceptorAttribute
    {
        /// <summary>
        /// 校验输入参数
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private List<ValidationFailure> ValidateParameters(IServiceProvider serviceProvider, object[] parameters)
        {
            List<ValidationFailure> failures = new List<ValidationFailure>();
            foreach (var para in parameters)
            {
                var v = typeof(IValidator<>);
                var vt = v.MakeGenericType(para.GetType());
                IValidator validator = (IValidator)serviceProvider.GetService(vt);
                if (validator != null)
                {
                    ValidationResult vr = validator.Validate(para);
                    bool success = vr.IsValid;
                    if (!success)
                    {
                        failures.AddRange(vr.Errors);
                    }
                }
            }
            return failures;
        }

        /// <summary>
        /// 拦截器调用方法
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            var serviceProvider = context.ServiceProvider;
            var logger = serviceProvider.GetService<ILogger<FunctionInterceptorAttribute>>();
            var funAttr = context.ServiceMethod.GetCustomAttribute<FunctionAttribute>(false);
            string funName = funAttr.Name;
            var functionStore = serviceProvider.GetService<IFunctionStore>();
            var funInfo = functionStore.GetFunction(funName);
            if(!funInfo.Enabled)
            {
                throw new InvalidOperationException("Not enabled");
            }
            var methodName = context.ImplementationMethod.Name;
            var failures = ValidateParameters(serviceProvider, context.Parameters);
            if (failures.Any())
            {
                foreach (var f in failures)
                {
                    Console.WriteLine("PropertyName:{0}   Code:{1}   Error:{2}", f.PropertyName, f.ErrorCode, f.ErrorMessage);
                }
                //return;
                throw new ArgumentException("验证错误");
            }
            // 是否启用运行日志
            bool loggingEnable = funInfo.LoggingEnable;
            try
            {
                var httpContextAccessor = context.ServiceProvider.GetService<IHttpContextAccessor>();
                string path = httpContextAccessor.HttpContext.Request.Path;
                //Console.WriteLine(string.Format("Entered Method:{0}, Arguments: {1}", methodName, string.Join(",", invocation.Arguments)));
                if (loggingEnable)
                {
                    logger.LogInformation($"Path:{path} Entered Method:{methodName}, Arguments: {string.Join(",", context.Parameters)}");
                }
                await next(context);
                //Console.WriteLine(string.Format("Sucessfully executed method:{0}", methodName));
                if (loggingEnable)
                {
                    logger.LogInformation("Sucessfully executed method:{0}", methodName);
                }
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
                if (loggingEnable)
                {
                    logger.LogInformation("Exiting Method:{0}", methodName);
                }
            }
        }
    }
}
