using AutoMapper;
using Microsoft.Diagnostics.Runtime;
using System;

namespace DumpExplorer.Core
{
    public static class AutoMapper
    {
        private static readonly Lazy<Mapper> _instance = new Lazy<Mapper>(() =>
        {
            var config = new MapperConfiguration(cfg => 
            {
                cfg.CreateMap<ClrObject, Object>();
                cfg.CreateMap<ClrType, Type>();
                cfg.CreateMap<ClrMethod, Method>();
                cfg.CreateMap<ClrInstanceField, Field>();
                cfg.CreateMap<ClrStaticField, Field>();
                cfg.CreateMap<ClrObject, String>()
                    .ForMember(x => x.Value, opt => opt.MapFrom(src => src.AsString(1024 * 1024 * 10) ?? string.Empty));
                cfg.CreateMap<ClrFinalizerRoot, ObjectRoot>();
                cfg.CreateMap<IClrRoot, ObjectRoot>();
                cfg.CreateMap<GCRootPath, ObjectRootPath>();
            });

            return new Mapper(config);
        });

        public static Mapper Instance => _instance.Value;
    }
}
