using Application.DTOs;
using Infrastructure.Interfaces;

namespace Application.Clientes.Atualizar.Ativar
{
    public class AtivaClienteCommandHandler
    {
        private readonly IClienteRepositorio _repo;
        
        public AtivaClienteCommandHandler(IClienteRepositorio repo) => _repo = repo;

        public async Task<ClienteDto?> Handle(AtivaClienteCommand cmd, CancellationToken ct = default)
        {
            var cliente = await _repo.ObterPorIdAsync(cmd.Id, ct);
            
            if (cliente is null)
                return null;

            if (cliente.Ativo)
                throw new InvalidOperationException("Cliente já está ativo.");

            cliente.Ativar();
            await _repo.AtualizarAsync(cliente, ct);

            return new ClienteDto(cliente.Id, cliente.NomeFantasia, cliente.Cnpj, cliente.Ativo);
        }
    }
}