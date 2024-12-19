using AutoMapper;
using Cliente.WebApi.Aplicacion.Interfaces;
using Cliente.WebApi.Aplicacion.Validadores;
using Cliente.WebApi.Dominio.DTOs;
using Cliente.WebApi.Dominio.Interfaces;
using Cliente.WebApi.Transversal.Interfaces;
using Cliente.WebApi.Transversal.Modelos;
using Microsoft.Extensions.Options;

namespace Cliente.WebApi.Aplicacion.Servicios;

public class ClienteServicio : IClienteServicio
{
    private readonly IClienteRepositorio _clienteRepositorio;
    private readonly ClienteDtoValidador _clienteDtoValidador;
    private readonly ActualizarClientePersonaDtoValidador _actualizarClientePersonaDtoValidador;
    //private readonly AppSettings _appSettings;
    private readonly IMapper _mapper;
    private readonly IAppLogger<ClienteServicio> _logger;
    public ClienteServicio(IMapper mapper, IAppLogger<ClienteServicio> logger, IOptions<AppSettings> appSettings,
                        IClienteRepositorio clienteRepositorio, ClienteDtoValidador clienteDtoValidador, ActualizarClientePersonaDtoValidador actualizarClientePersonaDtoValidador)
    {
        _mapper = mapper;
        _logger = logger;
       // _appSettings = appSettings.Value;
        _clienteRepositorio = clienteRepositorio;
        _clienteDtoValidador = clienteDtoValidador;
        _actualizarClientePersonaDtoValidador = actualizarClientePersonaDtoValidador;
    }


    public async Task<Response<bool>> Actualizar(int id, ClienteDto clienteDto)
    {
        var response = new Response<bool>();

        if (id == 0)
        {
            response.IsSuccess = false;
            response.Message = "Debe proporcionar el id del cliente a actualizar.";
            return response;
        }

        var validation = _actualizarClientePersonaDtoValidador.Validate(clienteDto);

        if (!validation.IsValid)
        {
            response.IsSuccess = false;
            response.Message = "Errores de validación encontrados";
            response.Errors = validation.Errors;
            _logger.LogWarning("Errores de validación en el modelo de cliente");
            return response;
        }

        try
        {
            var clientePersonaExistente = await _clienteRepositorio.ObtenerPorId(id);

            if (clientePersonaExistente == null)
            {
                response.IsSuccess = false;
                response.Message = "El cliente a actualizar no existe";
                _logger.LogWarning("Ocurrio un error, el cliente a actualizar no existe");
                return response;

            }

            // Mapear el modelo DTO a la entidad de cliente
            var cliente = _mapper.Map<Dominio.Persistencia.Entidades.Cliente>(clienteDto);

            response.Data = await _clienteRepositorio.Actualizar(cliente);

            if (response.Data) // no es nulo
            {
                response.IsSuccess = true;
                response.Message = "Actualizacion exitosa!!";
                _logger.LogInformation("Cliente actualizado exitosamente");
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Hubo error al actualizar el cliente";
                _logger.LogWarning("Error al actualizar el cliente");
            }

        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = $"Ocurrio un error: {ex}";
            _logger.LogError($"Ocurrió un error en el servidor: {ex.Message}");
        }
        return response;

    }

    public async Task<Response<bool>> Eliminar(int id)
    {
        var response = new Response<bool>();

        if (id == 0)
        {
            response.IsSuccess = false;
            response.Message = "Debe proporcionar el id del cliente a eliminar.";
            return response;
        }

        try
        {
            var idExistente = await _clienteRepositorio.ObtenerPorId(id);

            if(idExistente == null)
            {
                response.IsSuccess = false;
                response.Message = "El cliente a eliminar no existe!";
                return response;
            }

            response.Data = await _clienteRepositorio.Eliminar(id);

            if (response.Data)
            {
                response.IsSuccess = true;
                response.Message = "Elimiacion exitosa!!";
                _logger.LogInformation("Cliente eliminado exitosamente");
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Hubo error al eliminar el cliente";
                _logger.LogWarning("Error al eliminar el cliente");
            }
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = $"Ocurrió un error: {ex.Message}";
            _logger.LogError($"Ocurrió un error en el servidor: {ex.Message}");
        }

        return response;
    }

    public async  Task<Response<ClienteDto>> ObtenerPorId(int id)
    {
        var response = new Response<ClienteDto>();

        if (id == 0)
        {
            response.IsSuccess = false;
            response.Message = "Debe proporcionar el id del cliente a consultar.";
            return response;
        }

        try
        {

            response.Data = await _clienteRepositorio.ObtenerPorId(id);


            if (response.Data != null)
            {
                response.IsSuccess = true;
                response.Message = "Consulta exitosa!!";
                _logger.LogInformation("Consulta del cliente exitosamente");
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "El cliente no existe";
                _logger.LogWarning("Error al consultar el registro");
            }
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = $"Ocurrió un error: {ex.Message}";
            _logger.LogError($"Ocurrió un error en el servidor: {ex.Message}");
        }
        return response;
    }

    public Task<ResponsePagination<IEnumerable<ClienteDto>>> ObtenerTodoConPaginación(int numeroDePagina, int tamañoDePagina)
    {
        throw new NotImplementedException();
    }

    public async Task<Response<IEnumerable<ClienteDto>>> ObtenerTodo()
    {
        var response = new Response<IEnumerable<ClienteDto>>();

        try
        {
            response.Data = await _clienteRepositorio.ObtenerTodo();

            if (response.Data != null)
            {
                response.IsSuccess = true;
                response.Message = "Consulta exitosa!!";
                _logger.LogInformation("Consulta de registros exitosa");
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Hubo error al obtener los registros";
                _logger.LogWarning("Ocurrio un error al consultar los registros");
            }
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = $"Ocurrió un error: {ex.Message}";
            _logger.LogError($"Ocurrió un error en el servidor: {ex.Message}");

        }

        return response;
    }

    public async Task<Response<bool>> Guardar(ClienteDto clienteDto)
    {
        var response = new Response<bool>();

        try
        {
            var validation = _clienteDtoValidador.Validate(clienteDto);

            if (!validation.IsValid)
            {
                response.IsSuccess = false;
                response.Message = "Errores de validación";
                response.Errors = validation.Errors;
                _logger.LogWarning("Errores de validación en el modelo de cliente");
                return response;
            }


            //// Verificar si ya existe un usuario con el correo proporcionado
            //if (!string.IsNullOrEmpty(clienteDto.NumeroIdentificacion))
            //{
            //    // Console.WriteLine(JsonConvert.SerializeObject(clienteDto.NumeroIdentificacion));

            //    var clienteExistente = await _clienteRepositorio.ObtenerPorNumeroIdentificacion(clienteDto.NumeroIdentificacion);


            //    if (clienteExistente != null)
            //    {
            //        response.IsSuccess = false;
            //        response.Message = "El cliente ya existe";
            //        _logger.LogWarning("El cliente ya existe en la base de datos");
            //        return response;
            //    }
            //}

            // Mapear el modelo DTO a la entidad de cliente
            var cliente = _mapper.Map<Dominio.Persistencia.Entidades.Cliente>(clienteDto);

            // Intentar guardar el usuario
            response.Data = await _clienteRepositorio.Guardar(cliente);



            if (response.Data)
            {
                response.IsSuccess = true;
                response.Message = "Registro exitoso!";
                _logger.LogInformation("Cliente registrado exitosamente");
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Hubo un error al crear el registro";
                _logger.LogWarning("Error al guardar el cliente en la base de datos");
            }
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = $"Ocurrió un error de servidor: {ex.Message}";
            _logger.LogError($"Ocurrió un error en el servidor: {ex.Message}");
        }

        return response;
    }
}
