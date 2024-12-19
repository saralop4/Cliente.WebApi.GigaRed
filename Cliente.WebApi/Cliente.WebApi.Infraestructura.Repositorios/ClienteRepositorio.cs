using Cliente.WebApi.Dominio.DTOs;
using Cliente.WebApi.Dominio.Interfaces;
using Cliente.WebApi.Dominio.Persistencia;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Cliente.WebApi.Infraestructura.Repositorios
{
    public class ClienteRepositorio : IClienteRepositorio
    {
        private readonly DapperContext _dapperContext;
        public ClienteRepositorio(IConfiguration configuration)
        {
            _dapperContext = new DapperContext(configuration);
        }
        public  async Task<bool> Actualizar(Dominio.Persistencia.Entidades.Cliente modelo)
        {
            using (var conexion = _dapperContext.CreateConnection())
            {
                var query = "ActualizarClienteYPersona";
                var parameters = new DynamicParameters();

                parameters.Add("IdCliente", modelo.IdCliente);
                parameters.Add("IdPersona", modelo.IdPersona);
                parameters.Add("IdIndicativo", modelo.IdPersonaNavigation.IdIndicativo);
                parameters.Add("IdCiudad", modelo.IdPersonaNavigation.IdCiudad);
                parameters.Add("PrimerNombre", modelo.IdPersonaNavigation.PrimerNombre);
                parameters.Add("SegundoNombre", modelo.IdPersonaNavigation.SegundoNombre);
                parameters.Add("PrimerApellido", modelo.IdPersonaNavigation.PrimerApellido);
                parameters.Add("SegundoApellido", modelo.IdPersonaNavigation.SegundoApellido);
                parameters.Add("Telefono", modelo.IdPersonaNavigation.Telefono);
                parameters.Add("UsuarioQueActualiza", modelo.UsuarioQueActualiza);              
                parameters.Add("IpDeActualizado", modelo.IpDeActualizado);

                // Ejecutar el procedimiento almacenado
                var result = await conexion.ExecuteAsync(query, param: parameters, commandType: CommandType.StoredProcedure);

                return result > 0;
            }
        }

        public Task<int> Contar()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Eliminar(int Id)
        {
            using (var conexion = _dapperContext.CreateConnection()) //el metodo Get devuelve la instancia de conexion abierta
            {

                var query = "EliminarClienteYPersona"; 
                var parameters = new DynamicParameters(); 
                parameters.Add("IdCliente", Id);

                var result = await conexion.ExecuteScalarAsync<int>(query, param: parameters, commandType: CommandType.StoredProcedure);

                return result > 0;
            }
        }

        public async Task<bool> Guardar(Dominio.Persistencia.Entidades.Cliente modelo)
        {

            using (var conexion = _dapperContext.CreateConnection())
            {
                var query = "GuardarClienteYPersona";
                var parameters = new DynamicParameters();
                parameters.Add("IdIndicativo", modelo.IdPersonaNavigation.IdIndicativo);
                parameters.Add("IdCiudad", modelo.IdPersonaNavigation.IdCiudad);
                parameters.Add("PrimerNombre", modelo.IdPersonaNavigation.PrimerNombre);
                parameters.Add("SegundoNombre", modelo.IdPersonaNavigation.SegundoNombre);
                parameters.Add("PrimerApellido", modelo.IdPersonaNavigation.PrimerApellido);
                parameters.Add("SegundoApellido", modelo.IdPersonaNavigation.SegundoApellido);
                parameters.Add("Telefono", modelo.IdPersonaNavigation.Telefono);
                parameters.Add("UsuarioQueRegistra", modelo.UsuarioQueRegistra);
                parameters.Add("IpDeRegistro", modelo.IpDeRegistro);

                var result = await conexion.ExecuteAsync(query, param: parameters, commandType: CommandType.StoredProcedure);
                //el metodo execute permite invocar un procedimiento almacenado y enviarle los parametros
                return result > 0;
            }
        }

        public async Task<ClienteDto> ObtenerPorId(int id)
        {
            using (var Conexion = _dapperContext.CreateConnection())
            {
                var Query = "ObtenerClientePorId";
                var Parameters = new DynamicParameters();
                Parameters.Add("IdCliente", id);

                var ClientePersona = await Conexion.QuerySingleOrDefaultAsync<ClienteDto>(Query, param: Parameters, commandType: CommandType.StoredProcedure);

                return ClientePersona;
            }
        }

        public async Task<IEnumerable<ClienteDto>> ObtenerTodo()
        {
            using (var conexion = _dapperContext.CreateConnection())
            {
                var query = "ObtenerTodosLosClientes";

                var result = await conexion.QueryAsync<ClienteDto>(query, commandType: CommandType.StoredProcedure);

                return result;
            }

        }

        public Task<IEnumerable<ClienteDto>> ObtenerTodoConPaginacion(int numeroDePagina, int tamañoPagina)
        {
            throw new NotImplementedException();
        }
    }
}


