namespace OSControl.Domain.Services.Base
{
    public interface IServiceBase<T> where T : class
    {
        Task<T?> ObterPorId(int id);
        Task<IEnumerable<T>> ObterTodos();
        Task<T> Adicionar(T entidade);
        Task<T> Atualizar(T entidade);
        Task<bool> Excluir(int id);
    }
}
