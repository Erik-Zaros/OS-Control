using OSControl.Domain.DTOs;
using OSControl.Domain.Entities;
using OSControl.Domain.Services.Base;
using OSControl.Domain.ValueObjects;

namespace OSControl.Domain.Services
{
    public interface IUsuarioService : IServiceBase<Usuario>
    {
        Task<LoginResponse> Autenticar(string login, string senha);
        Task<Usuario> CriarUsuario(Usuario usuario, string senha);
        Task<bool> AlterarSenha(long userId, string senhaAtual, string novaSenha);
        Task<IEnumerable<Usuario>> ObterPorTipo(TipoUsuario tipo);
        Task<Usuario> AtivarDesativar(long id, bool ativar);
        Task<Usuario> ObterUsuarioLogado(long id);
    }
}
