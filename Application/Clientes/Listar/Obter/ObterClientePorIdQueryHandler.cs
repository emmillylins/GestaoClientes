using Application.DTOs;
using Infrastructure.Interfaces;

namespace Application.Clientes.Listar.Obter
{
    public class ObterClientePorIdQueryHandler
    {
        private readonly IClienteRepositorio _repo;
        public ObterClientePorIdQueryHandler(IClienteRepositorio repo) => _repo = repo;


        public async Task<ClienteDto?> Handle(ObterClientePorIdQuery query, CancellationToken ct = default)
        {
            var cliente = await _repo.ObterPorIdAsync(query.Id, ct);
            return cliente is null ? null : new ClienteDto(cliente.Id, cliente.NomeFantasia, cliente.Cnpj, cliente.Ativo);
        }
    }
}
