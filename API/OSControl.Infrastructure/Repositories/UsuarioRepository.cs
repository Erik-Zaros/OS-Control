using Microsoft.EntityFrameworkCore;
using OSControl.Domain.Entities;
using OSControl.Domain.Repositories;
using OSControl.Domain.ValueObjects;
using OSControl.Infrastructure.Data;
using OSControl.Infrastructure.Repositories.Base;

namespace OSControl.Infrastructure.Repositories
{
    public class UsuarioRepository : RepositorioBase<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(ApplicationDbContext contexto) : base(contexto)
        {
        }

        public async Task<Usuario?> BuscarPorLoginAsync(string login)
        {
            return await _contexto.Usuario.FirstOrDefaultAsync(u => u.Login == login);
        }

        public async Task<IEnumerable<Usuario>> BuscarPorTipoAsync(TipoUsuario tipoUsuario)
        {
            return await _contexto.Usuario.Where(u => u.TipoUsuario == tipoUsuario && u.Ativo)
                .ToListAsync();
        }

        public Task<bool> ExisteLoginAsync(string login)
        {
            return _contexto.Usuario.AnyAsync(u => u.Login == login);
        }
    }
}
