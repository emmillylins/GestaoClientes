using Domain.Entidades;
using Domain.ValueObjects;
using Infrastructure.Interfaces;
using System.Collections.Concurrent;

namespace Infrastructure.Repositorios
{
    public class EmMemoriaClienteRepositorio : IClienteRepositorio
    {
        private static readonly ConcurrentDictionary<Guid, Cliente> _dados = new();
        private static readonly ConcurrentDictionary<string, Guid> _cnpjs = new();


        public Task AdicionarAsync(Cliente cliente, CancellationToken ct = default)
        {
            _dados[cliente.Id] = cliente;
            _cnpjs[cliente.Cnpj.Numero] = cliente.Id;
            return Task.CompletedTask;
        }


        public Task<Cliente?> ObterPorIdAsync(Guid id, CancellationToken ct = default)
        => Task.FromResult(_dados.TryGetValue(id, out var cliente) ? cliente : null);


        public Task<bool> ExistePorCnpjAsync(Cnpj cnpj, CancellationToken ct = default)
        => Task.FromResult(_cnpjs.ContainsKey(cnpj.Numero));

        public Task<IEnumerable<Cliente>> ListarAsync(CancellationToken ct = default)
        => Task.FromResult(_dados.Values.AsEnumerable());
    }
}
