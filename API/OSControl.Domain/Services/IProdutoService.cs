using OSControl.Domain.Entities;
using OSControl.Domain.Services.Base;

namespace OSControl.Domain.Services
{
    public interface IProdutoService : IServiceBase<Produto>
    {
        Task<Produto> CriarProduto(Produto produto);
        Task<bool> AtivarDesativar(long id);
        Task<IEnumerable<Produto>> ObterAtivos();
        Task<Produto> ObterPorCodigo(string codigo);
    }
}
