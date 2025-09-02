using Application.Clientes.Criar;
using Application.Clientes.Obter;
using Application.Clientes.Listar;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly CriaClienteCommandHandler _manipuladorCriacao;
        private readonly ObtemClientePorIdQueryHandler _manipuladorObterPorId;
        private readonly ListarClientesQueryHandler _manipuladorListagem;

        public ClientesController(
            CriaClienteCommandHandler manipuladorCriacao,
            ObtemClientePorIdQueryHandler manipuladorObterPorId,
            ListarClientesQueryHandler manipuladorListagem)
        {
            _manipuladorCriacao = manipuladorCriacao;
            _manipuladorObterPorId = manipuladorObterPorId;
            _manipuladorListagem = manipuladorListagem;
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriaClienteCommand comando, CancellationToken ct)
        {
            try
            {
                var retorno = await _manipuladorCriacao.Handle(comando, ct);
                return CreatedAtAction(nameof(ObterPorId), new { id = retorno.Id }, retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(Guid id, CancellationToken ct)
        {
            var consulta = new ObtemClientePorIdQuery(id);
            var retorno = await _manipuladorObterPorId.Handle(consulta, ct);
            
            return retorno is null ? NotFound() : Ok(retorno);
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos(CancellationToken ct)
        {
            var consulta = new ListarClientesQuery();
            var retorno = await _manipuladorListagem.Handle(consulta, ct);
            return Ok(retorno);
        }
    }
}