using System;
using System.Collections.Generic;
using System.Text;

namespace AutoMapper
{
    /// <summary>
    /// 常用配置信息
    /// </summary>
    public class Profiles
    {
        /// <summary>
        /// DateTimeOffset DateTime 与时间戳映射
        /// </summary>
        public static Action<IProfileExpression> DateLongMapProfileAction = x =>
        {
            x.CreateMap<DateTimeOffset, long>().ConstructUsing(y => y.ToUnixTimeMilliseconds()).ReverseMap().ConstructUsing(y => DateTimeOffset.FromUnixTimeMilliseconds(y));
            x.CreateMap<DateTime, long>().ConstructUsing(y => y.ToUnixTimeMilliseconds()).ReverseMap().ConstructUsing(y => DateTimeOffset.FromUnixTimeMilliseconds(y).UtcDateTime);
        };
    }
}
