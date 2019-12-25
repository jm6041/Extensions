using Google.Protobuf.Collections;

namespace AutoMapper
{
    /// <summary>
    /// Grpc <see cref="IMapper"/> 工厂
    /// </summary>
    public class GrpcMapperFactory
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
                cfg.ForAllPropertyMaps((pm) =>
                {
                    if (pm.DestinationType.IsConstructedGenericType)
                    {
                        var destGenericBase = pm.DestinationType.GetGenericTypeDefinition();
                        return destGenericBase == typeof(RepeatedField<>);
                    }
                    return false;
                }, (propertyMap, opts) => opts.UseDestinationValue());
            }).CreateMapper();
            return mapper;
        }
    }
}
