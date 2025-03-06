using OSControl.Application.Helpers;
using OSControl.Domain.Repositories.Base;
using OSControl.Domain.Services.Base;

namespace OSControl.Application.Services.Base
{
    public class ServiceBase<T> : IServiceBase<T> where T : class
    {
        protected readonly IRepositorioBase<T> RepositorioBase;

        public ServiceBase(IRepositorioBase<T> repositorioBase)
        {
            RepositorioBase = repositorioBase;
        }

        public virtual async Task<T?> ObterPorId(int id)
        {
            var entidade = await RepositorioBase.BuscarPorIdAsync(id);
            ExceptionHelper.Nulo(entidade, $"Registro não encontrado com o ID: {id}");
            return entidade;
        }

        public virtual async Task<IEnumerable<T>> ObterTodos()
        {
            var lista = await RepositorioBase.BuscarTodosAsync();
            if (!lista.Any())
            {
                return Enumerable.Empty<T>();
            }
            return lista;
        }

        public virtual async Task<T> Adicionar(T entidade)
        {
            ExceptionHelper.Nulo(entidade, "Entidade não pode ser nula");
            await RepositorioBase.AdicionarAsync(entidade);
            return entidade;
        }

        public virtual async Task<T> Atualizar(T entidade)
        {
            ExceptionHelper.Nulo(entidade, "Entidade não pode ser nula");
            await RepositorioBase.AtualizarAsync(entidade);
            return entidade;
        }

        public virtual async Task<bool> Excluir(int id)
        {
            var entidade = await RepositorioBase.BuscarPorIdAsync(id);
            ExceptionHelper.Nulo(entidade, $"Registro não encontrado com o ID: {id}");
            await RepositorioBase.ExcluirAsync(id);
            return true;
        }
    }
}
