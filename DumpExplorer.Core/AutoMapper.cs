using AutoMapper;
using System;
using System.Linq;

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
                cfg.CreateMap<Microsoft.Diagnostics.Runtime.IClrRoot, GcRootInfo>()
                    .ForMember(x => x.Address, opt => opt.MapFrom(src => src.Address))
                    .ForMember(x => x.Id, opt => opt.MapFrom(src => $"{nameof(Object)}/{src.Object.Address}"));
                cfg.CreateMap<Microsoft.Diagnostics.Runtime.GCRootPath, GcRoot>()
                    .ForMember(x => x.Id, opt => opt.MapFrom(src => $"{nameof(GcRoot)}/{src.Root.Address}"))
                    .ForMember(x => x.Path, opt => opt.MapFrom(src => src.Path.Select(o => $"{nameof(Object)}/{o.Address}").ToList()));
                cfg.CreateMap<Microsoft.Diagnostics.Runtime.ClrThread, Thread>();
                cfg.CreateMap<Microsoft.Diagnostics.Runtime.ClrException, Core.Exception>()
                    .ForMember(x => x.TypeName, opt => opt.MapFrom(src => src.Type.Name))
                    .ForMember(x => x.Thread, opt => opt.AllowNull())
                    .ForMember(x => x.StackTrace, opt => opt.AllowNull())
                    .ForMember(x => x.Message, opt => opt.Ignore());
                cfg.CreateMap<Microsoft.Diagnostics.Runtime.ClrStackFrame, Core.StackFrame>();
            });

            return new Mapper(config);
        });

        public static Mapper Instance => _instance.Value;
    }
}
