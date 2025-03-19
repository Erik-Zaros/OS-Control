using OSControl.Application.Helpers;
using OSControl.Application.Services.Base;
using OSControl.Domain.Entities;
using OSControl.Domain.Repositories;
using OSControl.Domain.Repositories.Base;
using OSControl.Domain.Services;

namespace OSControl.Application.Services
{
    public class ProdutoService : ServiceBase<Produto>, IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoService(IRepositorioBase<Produto> repositorioBase, IProdutoRepository produtoRepository) : base(repositorioBase)
        {
            _produtoRepository = produtoRepository;
        }

        public Task<bool> AtivarDesativar(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<Produto> CriarProduto(Produto produto)
        {
            ExceptionHelper.Obrigatorio(produto.Codigo, "Código é obrigatório");
            ExceptionHelper.Obrigatorio(produto.Descricao, "Descrição é obrigatória");

            if (await _produtoRepository.ExisteCodigoAsync(produto.Codigo))
                throw new InvalidOperationException("Já existe um produto com este código");

            produto.Ativo = true;
            produto.CriadoEm = DateTime.UtcNow;

            await Adicionar(produto);
            return produto;
        }

        public Task<IEnumerable<Produto>> ObterAtivos()
        {
            throw new NotImplementedException();
        }

        public Task<Produto> ObterPorCodigo(string codigo)
        {
            throw new NotImplementedException();
        }
    }
}
