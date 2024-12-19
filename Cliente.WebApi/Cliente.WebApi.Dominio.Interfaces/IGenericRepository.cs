namespace Cliente.WebApi.Dominio.Interfaces;

public interface IGenericRepository<T> where T : class //agregamos una restriccion para que T siempre sea de tipo class
{
    #region Metodos Asincronos
    Task<bool> Guardar(T modelo);
    Task<bool> Actualizar(T modelo);
    Task<bool> Eliminar(int id);
    Task<T> ObtenerPorId(int id);
    Task<IEnumerable<T>> ObtenerTodo();
    Task<IEnumerable<T>> ObtenerTodoConPaginacion(int numeroDePagina, int tamañoPagina);
    Task<int> Contar();

    #endregion
}
