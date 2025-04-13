using OSControl.Domain.Entities;
using OSControl.Domain.Repositories.Base;
using OSControl.Domain.ValueObjects;

namespace OSControl.Domain.Repositories
{
    public interface IUsuarioRepository : IRepositorioBase<Usuario>
    {
        Task<Usuario?> BuscarPorLoginAsync(string login);
        Task<IEnumerable<Usuario>> BuscarPorTipoAsync(TipoUsuario tipoUsuario);
        Task<bool> ExisteLoginAsync(string login);
    }
}
