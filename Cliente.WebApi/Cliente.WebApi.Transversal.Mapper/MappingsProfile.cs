using AutoMapper;
using Cliente.WebApi.Dominio.DTOs;

namespace Cliente.WebApi.Transversal.Mapper
{
    public class MappingsProfile : Profile
    {
        public MappingsProfile()
        {
            CreateMap<Dominio.Persistencia.Entidades.Cliente, ClienteDto>()
                .ForMember(dest => dest.IdCliente, opt => opt.MapFrom(src => src.IdCliente))
                .ForMember(dest => dest.IdPersona, opt => opt.MapFrom(src => src.IdPersonaNavigation.IdPersona))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado))
                .ForMember(dest => dest.IdIndicativo, opt => opt.MapFrom(src => src.IdPersonaNavigation.IdIndicativo))
                .ForMember(dest => dest.IdCiudad, opt => opt.MapFrom(src => src.IdPersonaNavigation.IdCiudad))
                .ForMember(dest => dest.PrimerNombre, opt => opt.MapFrom(src => src.IdPersonaNavigation.PrimerNombre))
                .ForMember(dest => dest.SegundoNombre, opt => opt.MapFrom(src => src.IdPersonaNavigation.SegundoNombre))
                .ForMember(dest => dest.PrimerApellido, opt => opt.MapFrom(src => src.IdPersonaNavigation.PrimerApellido))
                .ForMember(dest => dest.SegundoApellido, opt => opt.MapFrom(src => src.IdPersonaNavigation.SegundoApellido))
                .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.IdPersonaNavigation.Telefono))
                .ForMember(dest => dest.UsuarioQueRegistra, opt => opt.MapFrom(src => src.UsuarioQueRegistra))
                .ForMember(dest => dest.IpDeRegistro, opt => opt.MapFrom(src => src.IpDeRegistro))
                .ForMember(dest => dest.UsuarioQueActualiza, opt => opt.MapFrom(src => src.UsuarioQueActualiza))
                .ForMember(dest => dest.IpDeActualizado, opt => opt.MapFrom(src => src.IpDeActualizado))
                .ReverseMap(); // Para permitir también el mapeo inverso de ClienteDto a Cliente si lo necesitas
        }
    }
}