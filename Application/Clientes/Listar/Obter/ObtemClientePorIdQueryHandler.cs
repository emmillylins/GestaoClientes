using Application.DTOs;
using Infrastructure.Interfaces;

namespace Application.Clientes.Listar.Obter
{
    public class ObtemClientePorIdQueryHandler
    {
        private readonly IClienteRepositorio _repo;
        public ObtemClientePorIdQueryHandler(IClienteRepositorio repo) => _repo = repo;


        public async Task<ClienteDto?> Handle(ObtemClientePorIdQuery query, CancellationToken ct = default)
        {
            var cliente = await _repo.ObterPorIdAsync(query.Id, ct);
            return cliente is null ? null : new ClienteDto(cliente.Id, cliente.NomeFantasia, cliente.Cnpj, cliente.Ativo);
        }
    }
}
