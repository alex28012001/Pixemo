using AutoMapper;
using System.Collections.Generic;

namespace BL.Helper
{
    public static class GenericMapperHelper<TSource, TDestination>
    {
        public static TDestination Convert(TSource source)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<TSource, TDestination>()).CreateMapper();
            return mapper.Map<TSource, TDestination>(source);
        }
    }
}
