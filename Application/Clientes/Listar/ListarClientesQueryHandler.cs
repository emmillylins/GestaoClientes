using Application.DTOs;
using Infrastructure.Interfaces;

namespace Application.Clientes.Listar
{
    public class ListarClientesQueryHandler
    {
        private readonly IClienteRepositorio _repo;
        public ListarClientesQueryHandler(IClienteRepositorio repo) => _repo = repo;

        public async Task<IEnumerable<ClienteDto>> Handle(ListarClientesQuery query, CancellationToken ct = default)
        {
            var clientes = await _repo.ListarAsync(ct);
            return clientes.Select(cliente => new ClienteDto(cliente.Id, cliente.NomeFantasia, cliente.Cnpj, cliente.Ativo));
        }
    }
}