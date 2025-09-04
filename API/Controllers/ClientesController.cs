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
        private readonly CriaClienteCommandHandler _manipuladorCriacao;
        private readonly ObtemClientePorIdQueryHandler _manipuladorObterPorId;
        private readonly ListaClientesQueryHandler _manipuladorListagem;
        private readonly AtivaClienteCommandHandler _manipuladorAtivacao;
        private readonly DesativaClienteCommandHandler _manipuladorDesativacao;
        private readonly AtualizaClienteCommandHandler _manipuladorAtualizacao;

        public ClientesController(
            CriaClienteCommandHandler manipuladorCriacao,
            ObtemClientePorIdQueryHandler manipuladorObterPorId,
            ListaClientesQueryHandler manipuladorListagem,
            AtivaClienteCommandHandler manipuladorAtivacao,
            DesativaClienteCommandHandler manipuladorDesativacao,
            AtualizaClienteCommandHandler manipuladorAtualizacao)
        {
            _manipuladorCriacao = manipuladorCriacao;
            _manipuladorObterPorId = manipuladorObterPorId;
            _manipuladorListagem = manipuladorListagem;
            _manipuladorAtivacao = manipuladorAtivacao;
            _manipuladorDesativacao = manipuladorDesativacao;
            _manipuladorAtualizacao = manipuladorAtualizacao;
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
            var consulta = new ListaClientesQuery();
            var retorno = await _manipuladorListagem.Handle(consulta, ct);
            return Ok(retorno);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizaClienteCommand comando, CancellationToken ct)
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
                var comando = new AtivaClienteCommand(id);
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
                var comando = new DesativaClienteCommand(id);
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