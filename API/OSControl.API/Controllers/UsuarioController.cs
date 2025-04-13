using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OSControl.API.Controllers.Base;
using OSControl.Domain.DTOs;
using OSControl.Domain.Entities;
using OSControl.Domain.Services;
using OSControl.Domain.Services.Base;

namespace OSControl.API.Controllers
{
    public class UsuarioController : BaseController<Usuario>
    {
        private readonly IUsuarioService UsuarioService;

        public UsuarioController(IServiceBase<Usuario> service, IUsuarioService usuarioService) : base(service)
        {
            UsuarioService = usuarioService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                var response = await UsuarioService.Autenticar(model.Login, model.Senha);

                return Ok(new { success = true, data = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("criar-usuario")]
        public async Task<IActionResult> CriarUsuario([FromBody] UsuarioModel usuarioModel)
        {
            try
            {
                var usuario = new Usuario
                {
                    Nome = usuarioModel.Nome,
                    Login = usuarioModel.Login,
                    TipoUsuario = usuarioModel.TipoUsuario,
                    Email = usuarioModel.Email,
                    Telefone = usuarioModel.Telefone,
                    Especialidade = usuarioModel.Especialidade,
                    Ativo = usuarioModel.Ativo
                };

                var novoUsuario = await UsuarioService.CriarUsuario(usuario, usuarioModel.Senha);

                return Ok(new { success = true, data = novoUsuario });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("alterar-senha")]
        public async Task<IActionResult> AlterarSenha([FromBody] AlterarSenhaModel model)
        {
            try
            {
                await UsuarioService.AlterarSenha(model.UserId, model.SenhaAtual, model.NovaSenha);
                return Ok(new { success = true, message = "Senha alterada com sucesso" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("ativar-desativar/{id}")]
        public async Task<IActionResult> AtivarDesativar(int id, [FromBody] bool ativar)
        {
            try
            {
                var usuario = await UsuarioService.AtivarDesativar(id, ativar);

                return Ok(new { success = true, data = usuario });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
