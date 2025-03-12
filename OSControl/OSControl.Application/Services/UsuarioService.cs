using OSControl.Application.Helpers;
using OSControl.Application.Services.Base;
using OSControl.Domain.DTOs;
using OSControl.Domain.Entities;
using OSControl.Domain.Repositories;
using OSControl.Domain.Repositories.Base;
using OSControl.Domain.Services;
using OSControl.Domain.ValueObjects;

namespace OSControl.Application.Services
{
    public class UsuarioService : ServiceBase<Usuario>, IUsuarioService
    {
        private readonly IUsuarioRepository UsuarioRepository;

        public UsuarioService(IRepositorioBase<Usuario> repositorioBase, IUsuarioRepository usuarioRepository) : base(repositorioBase)
        {
            UsuarioRepository = usuarioRepository;
        }        

        public async Task<IEnumerable<Usuario>> ObterPorTipo(TipoUsuario tipo)
        {
            return await UsuarioRepository.BuscarPorTipoAsync(tipo);
        }

        public async Task<Usuario> AtivarDesativar(long id, bool ativar)
        {
            var usuario = await ObterPorId((int)id);
            ExceptionHelper.Nulo(usuario, $"Usuário não encontrado com o ID: {id}");

            usuario.Ativo = ativar;

            await Atualizar(usuario);
            return usuario;
        }

        public async Task<Usuario> ObterUsuarioLogado(long id)
        {
            var usuario = await ObterPorId((int)id);
            ExceptionHelper.Nulo(usuario, "Usuário não encontrado.");
            ExceptionHelper.Inativo(usuario.Ativo, "Usuário está inativo.");

            return usuario;
        }

        public Task<LoginResponse> Autenticar(string login, string senha)
        {
            throw new NotImplementedException();
        }

        public Task<Usuario> CriarUsuario(Usuario usuario, string senha)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AlterarSenha(long userId, string senhaAtual, string novaSenha)
        {
            throw new NotImplementedException();
        }
    }
}
