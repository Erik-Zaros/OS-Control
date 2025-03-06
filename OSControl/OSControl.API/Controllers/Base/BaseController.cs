using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OSControl.Domain.Services.Base;

namespace OSControl.API.Controllers.Base
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController<T> : ControllerBase where T : class
    {
        private readonly IServiceBase<T> _service;

        public BaseController(IServiceBase<T> service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet]
        public virtual async Task<IActionResult> Get()
        {
            try
            {
                var entities = await _service.ObterTodos();
                return Ok(new { success = true, data = entities });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public virtual async Task<IActionResult> Get(int id)
        {
            try
            {
                var entity = await _service.ObterPorId(id);
                return Ok(new { success = true, data = entity });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost]
        public virtual async Task<IActionResult> Post([FromBody] T entity)
        {
            try
            {
                var result = await _service.Adicionar(entity);
                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut]
        public virtual async Task<IActionResult> Put([FromBody] T entity)
        {
            try
            {
                var result = await _service.Atualizar(entity);
                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.Excluir(id);
                return Ok(new { success = true, message = "Registro excluído com sucesso" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
