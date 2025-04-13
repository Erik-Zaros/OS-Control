using Microsoft.EntityFrameworkCore;
using OSControl.Domain.Entities;
using OSControl.Domain.Repositories;
using OSControl.Infrastructure.Data;
using OSControl.Infrastructure.Repositories.Base;

namespace OSControl.Infrastructure.Repositories
{
    public class ProdutoRepository : RepositorioBase<Produto>, IProdutoRepository
    {
        public ProdutoRepository(ApplicationDbContext contexto) : base(contexto) { }

        public Task<IEnumerable<Produto>> BuscarAtivosAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Produto> BuscarPorCodigoAsync(string codigo)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExisteCodigoAsync(string codigo)
        {
            return await _contexto.Produto
                .AnyAsync(p => p.Codigo == codigo);
        }
    }
}