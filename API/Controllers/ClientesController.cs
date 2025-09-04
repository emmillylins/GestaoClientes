using Application.Clientes.Criar;
using Application.Clientes.Obter;
using Application.Clientes.Listar;
using Application.Clientes.Ativar;
using Application.Clientes.Desativar;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/clientes")]
    public class ClientesController : ControllerBase
    {
        private readonly CriaClienteCommandHandler _manipuladorCriacao;
        private readonly ObtemClientePorIdQueryHandler _manipuladorObterPorId;
        private readonly ListarClientesQueryHandler _manipuladorListagem;
        private readonly AtivarClienteCommandHandler _manipuladorAtivacao;
        private readonly DesativarClienteCommandHandler _manipuladorDesativacao;

        public ClientesController(
            CriaClienteCommandHandler manipuladorCriacao,
            ObtemClientePorIdQueryHandler manipuladorObterPorId,
            ListarClientesQueryHandler manipuladorListagem,
            AtivarClienteCommandHandler manipuladorAtivacao,
            DesativarClienteCommandHandler manipuladorDesativacao)
        {
            _manipuladorCriacao = manipuladorCriacao;
            _manipuladorObterPorId = manipuladorObterPorId;
            _manipuladorListagem = manipuladorListagem;
            _manipuladorAtivacao = manipuladorAtivacao;
            _manipuladorDesativacao = manipuladorDesativacao;
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

        [HttpPatch("{id}/ativar")]
        public async Task<IActionResult> Ativar(Guid id, CancellationToken ct)
        {
            try
            {
                var comando = new AtivarClienteCommand(id);
                var retorno = await _manipuladorAtivacao.Handle(comando, ct);
                
                return retorno is null ? NotFound() : Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}/desativar")]
        public async Task<IActionResult> Desativar(Guid id, CancellationToken ct)
        {
            try
            {
                var comando = new DesativarClienteCommand(id);
                var retorno = await _manipuladorDesativacao.Handle(comando, ct);
                
                return retorno is null ? NotFound() : Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}