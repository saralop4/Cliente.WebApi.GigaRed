using Cliente.WebApi.Dominio.DTOs;
using Microsoft.AspNetCore.Http;


namespace Cliente.WebApi.Aplicacion.Interfaces
{
    public interface IIpAddressService
    {
        IpInfoDto GetClientIpAddress(HttpContext context, string providedIp = null);

    }
}