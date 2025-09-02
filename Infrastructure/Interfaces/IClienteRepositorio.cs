using Domain.Entidades;
using Domain.ValueObjects;

namespace Infrastructure.Interfaces
{
    public interface IClienteRepositorio
    {
        Task AdicionarAsync(Cliente cliente, CancellationToken ct = default);
        Task<Cliente?> ObterPorIdAsync(Guid id, CancellationToken ct = default);
        Task<bool> ExistePorCnpjAsync(Cnpj cnpj, CancellationToken ct = default);
        Task<IEnumerable<Cliente>> ListarAsync(CancellationToken ct = default);
    }
}
