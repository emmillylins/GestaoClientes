using Application.Clientes.Criar;
using Application.Clientes.Listar;
using Application.Clientes.Atualizar;
using Application.Clientes.Atualizar.Ativar;
using Application.Clientes.Atualizar.Desativar;
using Microsoft.AspNetCore.Mvc;
using Application.Clientes.Listar.Obter;

namespace API.Controllers
{
    [ApiController]
    [Route("api/clientes")]
    public class ClientesController : ControllerBase
    {
        private readonly CriarClienteCommandHandler _manipuladorCriacao;
        private readonly ObterClientePorIdQueryHandler _manipuladorObterPorId;
        private readonly ListarClientesQueryHandler _manipuladorListagem;
        private readonly AtivarClienteCommandHandler _manipuladorAtivacao;
        private readonly DesativarClienteCommandHandler _manipuladorDesativacao;
        private readonly AtualizarClienteCommandHandler _manipuladorAtualizacao;

        public ClientesController(
            CriarClienteCommandHandler manipuladorCriacao,
            ObterClientePorIdQueryHandler manipuladorObterPorId,
            ListarClientesQueryHandler manipuladorListagem,
            AtivarClienteCommandHandler manipuladorAtivacao,
            DesativarClienteCommandHandler manipuladorDesativacao,
            AtualizarClienteCommandHandler manipuladorAtualizacao)
        {
            _manipuladorCriacao = manipuladorCriacao;
            _manipuladorObterPorId = manipuladorObterPorId;
            _manipuladorListagem = manipuladorListagem;
            _manipuladorAtivacao = manipuladorAtivacao;
            _manipuladorDesativacao = manipuladorDesativacao;
            _manipuladorAtualizacao = manipuladorAtualizacao;
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarClienteCommand comando, CancellationToken ct)
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
            var consulta = new ObterClientePorIdQuery(id);
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarClienteCommand comando, CancellationToken ct)
        {
            try
            {
                comando.Id = id;
                var retorno = await _manipuladorAtualizacao.Handle(comando, ct);
                
                return retorno is null ? NotFound() : Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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