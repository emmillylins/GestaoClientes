using Application.DTOs;
using Infrastructure.Interfaces;

namespace Application.Clientes.Listar
{
    public class ListaClientesQueryHandler
    {
        private readonly IClienteRepositorio _repo;
        public ListaClientesQueryHandler(IClienteRepositorio repo) => _repo = repo;

        public async Task<IEnumerable<ClienteDto>> Handle(ListaClientesQuery query, CancellationToken ct = default)
        {
            var clientes = await _repo.ListarAsync(ct);
            return clientes.Select(cliente => new ClienteDto(cliente.Id, cliente.NomeFantasia, cliente.Cnpj, cliente.Ativo));
        }
    }
}