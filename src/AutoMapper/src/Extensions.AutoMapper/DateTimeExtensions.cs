using System;
using System.Collections.Generic;
using System.Text;

namespace AutoMapper
{
    internal static class DateTimeExtensions
    {
        public static long ToUnixTimeMilliseconds(this DateTime dt)
        {
            DateTimeOffset dto = dt;
            return dto.ToUnixTimeMilliseconds();
        }
    }
}
