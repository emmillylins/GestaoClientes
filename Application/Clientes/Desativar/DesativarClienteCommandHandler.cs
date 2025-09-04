using Application.DTOs;
using Infrastructure.Interfaces;

namespace Application.Clientes.Desativar
{
    public class DesativarClienteCommandHandler
    {
        private readonly IClienteRepositorio _repo;
        
        public DesativarClienteCommandHandler(IClienteRepositorio repo) => _repo = repo;

        public async Task<ClienteDto?> Handle(DesativarClienteCommand cmd, CancellationToken ct = default)
        {
            var cliente = await _repo.ObterPorIdAsync(cmd.Id, ct);
            
            if (cliente is null)
                return null;

            if (!cliente.Ativo)
                throw new InvalidOperationException("Cliente já está inativo.");

            cliente.Desativar();
            await _repo.AtualizarAsync(cliente, ct);

            return new ClienteDto(cliente.Id, cliente.NomeFantasia, cliente.Cnpj, cliente.Ativo);
        }
    }
}