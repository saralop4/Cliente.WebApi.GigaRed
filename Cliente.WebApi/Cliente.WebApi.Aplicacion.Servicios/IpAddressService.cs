using Cliente.WebApi.Aplicacion.Interfaces;
using Cliente.WebApi.Dominio.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Runtime;

namespace Cliente.WebApi.Aplicacion.Servicios
{
    public class IpAddressService : IIpAddressService
    {
        private readonly ILogger<IpAddressService> _logger;

        public IpAddressService(ILogger<IpAddressService> logger)
        {
            _logger = logger;
        }

        public IpInfoDto GetClientIpAddress(HttpContext context, string providedIp = null)
        {
            var ipInfo = new IpInfoDto
            {
                ProvidedIp = providedIp
            };

            try
            {
                ipInfo.DetectedIp = GetServerDetectedIp(context);

                if (!string.IsNullOrEmpty(providedIp))
                {
                    ipInfo.FinalIp = providedIp;
                    ipInfo.Source = "Frontend";
                }
                else if (!string.IsNullOrEmpty(ipInfo.DetectedIp))
                {
                    ipInfo.FinalIp = ipInfo.DetectedIp;
                    ipInfo.Source = "Backend";
                }
                else
                {
                    ipInfo.FinalIp = "0.0.0.0";
                    ipInfo.Source = "Default";
                }

                _logger.LogInformation($"IP detectada - Origen: {ipInfo.Source}, IP: {ipInfo.FinalIp}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener IP: {ex.Message}");
                ipInfo.FinalIp = "0.0.0.0";
                ipInfo.Source = "Error";
            }

            return ipInfo;
        }


        private string GetServerDetectedIp(HttpContext context)
        {
            string ip = null;

            // Lista de headers comunes donde se puede encontrar la IP
            var headers = new[]
            {
                "CF-Connecting-IP",      // Cloudflare
                "X-Forwarded-For",       // Proxy general
                "X-Real-IP",             // Nginx
                "True-Client-IP",        // Akamai
                "HTTP_X_FORWARDED_FOR",
                "HTTP_CLIENT_IP",
                "HTTP_X_REAL_IP",
                "HTTP_X_FORWARDED",
                "HTTP_X_CLUSTER_CLIENT_IP",
                "HTTP_FORWARDED_FOR",
                "HTTP_FORWARDED",
                "REMOTE_ADDR"
            };

            // Buscar en los headers
            foreach (var header in headers)
            {
                ip = context.Request.Headers[header].FirstOrDefault();
                if (!string.IsNullOrEmpty(ip))
                {
                    // Si hay múltiples IPs, tomar la primera
                    if (ip.Contains(","))
                    {
                        ip = ip.Split(',')[0].Trim();
                    }
                    break;
                }
            }

            // Si no se encontró en los headers, usar la conexión remota
            if (string.IsNullOrEmpty(ip) && context?.Connection?.RemoteIpAddress != null)
            {
                var remoteIp = context.Connection.RemoteIpAddress;

                if (remoteIp.IsIPv4MappedToIPv6)
                {
                    ip = remoteIp.MapToIPv4().ToString();
                }
                else if (remoteIp.ToString() == "::1")
                {
                    ip = "127.0.0.1";
                }
                else
                {
                    ip = remoteIp.ToString();
                }
            }

            return ip;
        }
    }
}
