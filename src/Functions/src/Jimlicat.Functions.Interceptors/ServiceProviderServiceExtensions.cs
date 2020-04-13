// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace System
{
    /// <summary>
    /// <see cref="IServiceProvider"/>扩展方法，
    /// 代码参考Microsoft.Extensions.DependencyInjection.Abstractions程序集，ServiceProviderServiceExtensions
    /// </summary>
    internal static class ServiceProviderServiceExtensions
    {
        /// <summary>
        /// 获得 <typeparamref name="T"/> 指定的服务 <see cref="IServiceProvider"/>.
        /// </summary>
        /// <typeparam name="T">获得对象的类型</typeparam>
        /// <param name="provider"><see cref="IServiceProvider"/></param>
        /// <returns>返回一个类型为 <typeparamref name="T"/> 的服务对象，如果没有这样的服务，则返回null</returns>
        public static T GetService<T>(this IServiceProvider provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            return (T)provider.GetService(typeof(T));
        }
    }
}
