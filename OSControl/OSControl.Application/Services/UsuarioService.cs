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
        private readonly ITokenService TokenService;
        private readonly BCryptHash BCrypt;

        public UsuarioService(IRepositorioBase<Usuario> repositorioBase, IUsuarioRepository usuarioRepository, ITokenService tokenService, BCryptHash hash) : base(repositorioBase)
        {
            UsuarioRepository = usuarioRepository;
            TokenService = tokenService;
            BCrypt = hash;
        }

        public async Task<LoginResponse> Autenticar(string login, string senha)
        {
            var usuario = await UsuarioRepository.BuscarPorLoginAsync(login);
            ExceptionHelper.Nulo(usuario, "Usuário não encontrado.");
            ExceptionHelper.Inativo(usuario.Ativo, "Usuário está inativo.");

            var senhaValida = BCrypt.Verify(senha, usuario.Senha);

            if (!senhaValida)
            {
                ExceptionHelper.NaoAutorizado("Senha incorreta.");
            }

            usuario.UltimoAcesso = DateTime.UtcNow;
            await UsuarioRepository.AtualizarAsync(usuario);

            return new LoginResponse
            {
                Usuario = new AuthResponseModel
                {
                    Nome = usuario.Nome,
                    Login = usuario.Login,
                    Email = usuario.Email,
                    TipoUsuario = usuario.TipoUsuario.ToString()
                },
                Token = TokenService.GerarToken(usuario)
            };
        }

        public async Task<Usuario> CriarUsuario(Usuario usuario, string senha)
        {
            ExceptionHelper.Obrigatorio(senha, "Senha é obrigatória.");
            ExceptionHelper.Existe(await UsuarioRepository.ExisteLoginAsync(usuario.Login), "Login já existe.");
            ExceptionHelper.Nulo(usuario.TipoUsuario, "Tipo de usuário é obrigatório");

            usuario.Senha = BCrypt.Hash(senha);
            usuario.CriadoEm = DateTime.UtcNow;
            usuario.Ativo = true;

            await Adicionar(usuario);
            return usuario;
        }

        public async Task<bool> AlterarSenha(long userId, string senhaAtual, string novaSenha)
        {
            var usuario = await UsuarioRepository.BuscarPorIdAsync(userId);

            ExceptionHelper.Nulo(usuario, "Usuário não encontrado.");
            ExceptionHelper.Inativo(usuario.Ativo, "Usuário está inativo.");

            if (!BCrypt.Verify(senhaAtual, usuario.Senha))
            {
                ExceptionHelper.NaoAutorizado("Senha atual incorreta.");
            }

            usuario.Senha = BCrypt.Hash(novaSenha);

            await UsuarioRepository.AtualizarAsync(usuario);
            return true;
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
    }
}
