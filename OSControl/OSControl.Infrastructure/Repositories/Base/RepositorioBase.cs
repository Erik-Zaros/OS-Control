using Microsoft.EntityFrameworkCore;
using OSControl.Domain.Repositories.Base;
using OSControl.Infrastructure.Data;

namespace OSControl.Infrastructure.Repositories.Base
{
    public class RepositorioBase<T> : IRepositorioBase<T> where T : class
    {
        protected readonly ApplicationDbContext _contexto;
        protected readonly DbSet<T> _entidades;

        public RepositorioBase(ApplicationDbContext contexto)
        {
            _contexto = contexto;
            _entidades = _contexto.Set<T>();
        }

        public virtual async Task<T?> BuscarPorIdAsync(long id)
        {
            return await _entidades.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> BuscarTodosAsync()
        {
            return await _entidades.ToListAsync();
        }

        public virtual async Task AdicionarAsync(T entidade)
        {
            await _entidades.AddAsync(entidade);
            await _contexto.SaveChangesAsync();
        }

        public virtual async Task AtualizarAsync(T entidade)
        {
            _entidades.Update(entidade);
            await _contexto.SaveChangesAsync();
        }

        public virtual async Task ExcluirAsync(long id)
        {
            var entidade = await BuscarPorIdAsync(id);
            if (entidade != null)
            {
                _entidades.Remove(entidade);
                await _contexto.SaveChangesAsync();
            }
        }
    }
}
