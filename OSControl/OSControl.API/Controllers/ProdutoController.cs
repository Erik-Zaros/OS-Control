using Microsoft.AspNetCore.Mvc;
using OSControl.API.Controllers.Base;
using OSControl.Domain.DTOs;
using OSControl.Domain.Entities;
using OSControl.Domain.Services;

namespace OSControl.API.Controllers
{
    public class ProdutoController : BaseController<Produto>
    {
        private readonly IProdutoService _produtoService;

        public ProdutoController(IProdutoService produtoService) : base(produtoService)
        {
            _produtoService = produtoService;
        }

        [HttpPost("criar")]
        public async Task<IActionResult> CriarProduto([FromBody] ProdutoModel produtoModel)
        {
            try
            {
                var produto = new Produto
                {
                    Codigo = produtoModel.Codigo,
                    Descricao = produtoModel.Descricao,
                    Ativo = produtoModel.Ativo
                };

                var result = await _produtoService.CriarProduto(produto);
                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPut("ativar-desativar/{id}")]
        public async Task<IActionResult> AtivarDesativar(long id)
        {
            try
            {
                var ativo = await _produtoService.AtivarDesativar(id);
                var mensagem = ativo ? "Produto ativado com sucesso" : "Produto desativado com sucesso";
                return Ok(new { success = true, message = mensagem });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("ativos")]
        public async Task<IActionResult> ObterAtivos()
        {
            try
            {
                var result = await _produtoService.ObterAtivos();
                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("codigo/{codigo}")]
        public async Task<IActionResult> ObterPorCodigo(string codigo)
        {
            try
            {
                var result = await _produtoService.ObterPorCodigo(codigo);
                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
