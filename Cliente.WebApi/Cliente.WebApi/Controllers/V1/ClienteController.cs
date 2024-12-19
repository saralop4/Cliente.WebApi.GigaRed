using Cliente.WebApi.Aplicacion.Interfaces;
using Cliente.WebApi.Dominio.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cliente.WebApi.Controllers.V1
{
    [Authorize]
    [Route("Api/V{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteServicio _clienteServicio;
        private readonly IIpAddressService _ipAddressService;
        private readonly ILogger<ClienteController> _logger;

        public ClienteController(
            IClienteServicio clienteServicio,
            IIpAddressService ipAddressService,
            ILogger<ClienteController> logger)
        {
            _clienteServicio = clienteServicio;
            _ipAddressService = ipAddressService;
            _logger = logger;
        }

        #region Metodos Asincronos*

        [HttpPost("Guardar")]
        public async Task<IActionResult> Guardar([FromBody] ClienteDto clienteDto)
        {
            try
            {
                // Obtener información de IP
                var ipInfo = _ipAddressService.GetClientIpAddress(HttpContext, clienteDto.IpDeRegistro);

                // Asignar la IP final al DTO
                clienteDto.IpDeRegistro = ipInfo.FinalIp;

                // Log de la IP detectada
                _logger.LogInformation($"IP detectada para cliente - Origen: {ipInfo.Source}, IP: {ipInfo.FinalIp}");

                // Proceder con el guardado
                var response = await _clienteServicio.Guardar(clienteDto);

                if (response.IsSuccess)
                {
                    return Ok(response);
                }

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al guardar cliente: {ex.Message}");
                return StatusCode(500, new
                {
                    IsSuccess = false,
                    Message = "Error interno del servidor al procesar la solicitud"
                });
            }
        }


        [HttpPut("Actualizar/{Id}")]
        public async Task<IActionResult> Actualizar(int Id, [FromBody] ClienteDto ClienteDto)
        {
            var ipDeActualizado = HttpContext.Connection.RemoteIpAddress?.ToString();

            if (ipDeActualizado != null)
            {
                ClienteDto.IpDeActualizado = ipDeActualizado.ToString();
            }

            var Response = await _clienteServicio.Actualizar(Id, ClienteDto);

            if (Response.IsSuccess)
            {
                return Ok(Response);
            }
            return BadRequest(Response);

        }

        [HttpDelete("Eliminar/{Id}")]
        public async Task<IActionResult> Eliminar(int Id)
        {

            var Response = await _clienteServicio.Eliminar(Id);

            if (Response.IsSuccess)
            {
                return Ok(Response);
            }
            return BadRequest(Response);


        }

        [HttpGet("ObtenerPorId/{Id}")]
        public async Task<IActionResult> ObtenerPorId(int Id)
        {
            var Response = await _clienteServicio.ObtenerPorId(Id);

            if (Response.IsSuccess)
            {
                return Ok(Response);
            }
            return BadRequest(Response);


        }

        [HttpGet("ObtenerTodo")]
        public async Task<IActionResult> ObtenerTodo()
        {

            var Response = await _clienteServicio.ObtenerTodo();

            if (Response.IsSuccess)
            {
                return Ok(Response);
            }
            return BadRequest(Response);

        }

        [HttpGet("ObtenerConPaginacion/{NumeroPagina}/{TamañoPagina}")]
        public async Task<IActionResult> ObtenerConPaginacion(int NumeroPagina, int TamañoPagina)
        {

            var Response = await _clienteServicio.ObtenerTodoConPaginación(NumeroPagina, TamañoPagina);

            if (Response.IsSuccess)
            {
                return Ok(Response);
            }
            return BadRequest(Response);

        }

        #endregion
    }
}
