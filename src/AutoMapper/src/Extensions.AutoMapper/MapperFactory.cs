using Google.Protobuf.Collections;

namespace AutoMapper
{
    /// <summary>
    /// <see cref="IMapper"/> 工厂
    /// </summary>
    public class MapperFactory
    {
        /// <summary>
        /// 创建<see cref="IMapper"/>，建议把创建好的对象作为静态变量
        /// </summary>
        /// <typeparam name="TProfile"><see cref="Profile"/></typeparam>
        /// <returns><see cref="IMapper"/></returns>
        public static IMapper Create<TProfile>()
            where TProfile : Profile, new()
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<TProfile>();
            }).CreateMapper();
            return mapper;
        }
    }
}
