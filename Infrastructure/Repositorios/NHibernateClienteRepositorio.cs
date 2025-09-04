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
            _session = session ?? throw new ArgumentNullException(nameof(session));
        }

        public async Task AdicionarAsync(Cliente cliente, CancellationToken ct = default)
        {
            if (cliente == null) throw new ArgumentNullException(nameof(cliente));

            using var transacao = _session.BeginTransaction();
            try
            {
                await _session.SaveAsync(cliente, ct);
                await _session.FlushAsync(ct);
                await transacao.CommitAsync(ct);
            }
            catch (Exception ex)
            {
                await transacao.RollbackAsync(ct);
                throw new InvalidOperationException($"Erro ao adicionar cliente: {ex.Message}", ex);
            }
        }

        public async Task<Cliente?> ObterPorIdAsync(Guid id, CancellationToken ct = default)
        {
            try
            {
                return await _session.GetAsync<Cliente>(id, ct);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erro ao obter cliente por ID {id}: {ex.Message}", ex);
            }
        }

        public async Task<bool> ExistePorCnpjAsync(Cnpj cnpj, CancellationToken ct = default)
        {
            if (cnpj == null) return false;

            try
            {
                var count = await _session.Query<Cliente>()
                    .Where(c => c.Cnpj.Numero == cnpj.Numero)
                    .CountAsync(ct);
                
                return count > 0;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erro ao verificar existência do CNPJ {cnpj.Numero}: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<Cliente>> ListarAsync(CancellationToken ct = default)
        {
            try
            {
                var clientes = await _session.Query<Cliente>()
                    .ToListAsync(ct);

                return clientes;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erro ao listar clientes: {ex.Message}", ex);
            }
        }

        public async Task AtualizarAsync(Cliente cliente, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(cliente);

            using var transacao = _session.BeginTransaction();
            try
            {
                await _session.UpdateAsync(cliente, ct);
                await _session.FlushAsync(ct);
                await transacao.CommitAsync(ct);
            }
            catch (Exception ex)
            {
                await transacao.RollbackAsync(ct);
                throw new InvalidOperationException($"Erro ao atualizar cliente: {ex.Message}", ex);
            }
        }
    }
}