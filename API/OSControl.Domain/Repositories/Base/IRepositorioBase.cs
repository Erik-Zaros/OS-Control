namespace OSControl.Domain.Repositories.Base
{
    public interface IRepositorioBase<T> where T : class
    {
        Task<T?> BuscarPorIdAsync(long id);
        Task<IEnumerable<T>> BuscarTodosAsync();
        Task AdicionarAsync(T entidade);
        Task AtualizarAsync(T entidade);
        Task ExcluirAsync(long id);
    }
}
