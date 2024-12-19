using Cliente.WebApi.Dominio.DTOs;
using Cliente.WebApi.Transversal.Modelos;

namespace Cliente.WebApi.Aplicacion.Interfaces
{
    public interface IClienteServicio
    {
        #region Metodos Asincronos

        Task<Response<bool>> Guardar(ClienteDto clienteDto);
        Task<Response<bool>> Actualizar(int id,ClienteDto clienteDto);
        Task<Response<bool>> Eliminar(int id);
        Task<Response<ClienteDto>> ObtenerPorId(int id);
        Task<Response<IEnumerable<ClienteDto>>> ObtenerTodo();
        Task<ResponsePagination<IEnumerable<ClienteDto>>> ObtenerTodoConPaginación(int numeroDePagina, int tamañoDePagina);

        #endregion

    }
}
