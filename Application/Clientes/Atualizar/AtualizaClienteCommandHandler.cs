using Application.DTOs;
using Infrastructure.Interfaces;

namespace Application.Clientes.Atualizar
{
    public class AtualizaClienteCommandHandler
    {
        private readonly IClienteRepositorio _repo;
        
        public AtualizaClienteCommandHandler(IClienteRepositorio repo) => _repo = repo;

        public async Task<ClienteDto?> Handle(AtualizaClienteCommand cmd, CancellationToken ct = default)
        {
            var cliente = await _repo.ObterPorIdAsync(cmd.Id, ct);
            
            if (cliente is null)
                return null;

            cliente.DefinirNome(cmd.NomeFantasia);
            await _repo.AtualizarAsync(cliente, ct);

            return new ClienteDto(cliente.Id, cliente.NomeFantasia, cliente.Cnpj, cliente.Ativo);
        }
    }
}