using OSControl.Domain.Entities;
using OSControl.Domain.Repositories.Base;

namespace OSControl.Domain.Repositories
{
    public interface IProdutoRepository : IRepositorioBase<Produto>
    {
        Task<bool> ExisteCodigoAsync(string codigo);
        Task<Produto> BuscarPorCodigoAsync(string codigo);
        Task<IEnumerable<Produto>> BuscarAtivosAsync();
        Task<IEnumerable<Produto>> BuscarTodosAsync();
        Task<Produto> BuscarPorIdAsync(long id);
    }
}
