using AutoMapper;
using System;

namespace DumpExplorer.Core
{
    public static class AutoMapper
    {
        private static readonly Lazy<Mapper> _instance = new Lazy<Mapper>(() =>
        {
            var config = new MapperConfiguration(cfg => 
            {
                cfg.CreateMap<Microsoft.Diagnostics.Runtime.ClrObject, Object>();
                cfg.CreateMap<Microsoft.Diagnostics.Runtime.ClrType, Type>();
                cfg.CreateMap<Microsoft.Diagnostics.Runtime.ClrMethod, Method>();
                cfg.CreateMap<Microsoft.Diagnostics.Runtime.ClrInstanceField, Field>();
                cfg.CreateMap<Microsoft.Diagnostics.Runtime.ClrStaticField, Field>();
                cfg.CreateMap<Microsoft.Diagnostics.Runtime.ClrObject, String>()
                    .ForMember(x => x.Value, opt => opt.MapFrom(src => src.AsString(1024 * 1024 * 10) ?? string.Empty));
                cfg.CreateMap<Microsoft.Diagnostics.Runtime.ClrFinalizerRoot, FinalizableObjectRoot>();
                cfg.CreateMap<Microsoft.Diagnostics.Runtime.IClrRoot, GcRoot>();
                cfg.CreateMap<Microsoft.Diagnostics.Runtime.GCRootPath, GcRootPath>();
            });

            return new Mapper(config);
        });

        public static Mapper Instance => _instance.Value;
    }
}
