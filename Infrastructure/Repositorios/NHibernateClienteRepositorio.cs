using Domain.Entidades;
using Domain.ValueObjects;
using Infrastructure.Interfaces;
using NHibernate;
using NHibernate.Linq;

namespace Infrastructure.Repositorios
{
    public class NHibernateClienteRepositorio : IClienteRepositorio
    {
        private readonly ISession _session;

        public NHibernateClienteRepositorio(ISession session)
        {
            _session = session;
        }

        public async Task AdicionarAsync(Cliente cliente, CancellationToken ct = default)
        {
            using var transacao = _session.BeginTransaction();
            try
            {
                await _session.SaveAsync(cliente, ct);
                await transacao.CommitAsync(ct);
            }
            catch
            {
                await transacao.RollbackAsync(ct);
                throw;
            }
        }

        public async Task<Cliente?> ObterPorIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _session.GetAsync<Cliente>(id, ct);
        }

        public async Task<bool> ExistePorCnpjAsync(Cnpj cnpj, CancellationToken ct = default)
        {
            var count = await _session.Query<Cliente>()
                .Where(c => c.Cnpj.Numero == cnpj.Numero)
                .CountAsync(ct);
            
            return count > 0;
        }

        public async Task<IEnumerable<Cliente>> ListarAsync(CancellationToken ct = default)
        {
            return await _session.Query<Cliente>()
                .ToListAsync(ct);
        }
    }
}