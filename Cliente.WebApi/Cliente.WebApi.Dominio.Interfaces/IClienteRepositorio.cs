using Cliente.WebApi.Dominio.DTOs;
using ClienteEntity = Cliente.WebApi.Dominio.Persistencia.Entidades.Cliente;

namespace Cliente.WebApi.Dominio.Interfaces;

public interface IClienteRepositorio 
{
    #region Metodos Asincronos
    Task<bool> Guardar(ClienteEntity modelo);
    Task<bool> Actualizar(ClienteEntity modelo);
    Task<bool> Eliminar(int id);
    Task<ClienteDto> ObtenerPorId(int id);
    Task<IEnumerable<ClienteDto>> ObtenerTodo();
    Task<IEnumerable<ClienteDto>> ObtenerTodoConPaginacion(int numeroDePagina, int tamañoPagina);
    Task<int> Contar();

    #endregion

}
