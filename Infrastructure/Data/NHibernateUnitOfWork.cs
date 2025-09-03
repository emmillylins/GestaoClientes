using NHibernate;

namespace Infrastructure.Data
{
    public class NHibernateUnitOfWork : IUnitOfWork
    {
        private readonly ISession _session;
        private ITransaction? _transacao;

        public NHibernateUnitOfWork(ISession session)
        {
            _session = session;
        }

        public async Task BeginTransactionAsync(CancellationToken ct = default)
        {
            _transacao = _session.BeginTransaction();
            await Task.CompletedTask;
        }

        public async Task CommitAsync(CancellationToken ct = default)
        {
            if (_transacao != null)
            {
                await _transacao.CommitAsync(ct);
                _transacao.Dispose();
                _transacao = null;
            }
        }

        public async Task RollbackAsync(CancellationToken ct = default)
        {
            if (_transacao != null)
            {
                await _transacao.RollbackAsync(ct);
                _transacao.Dispose();
                _transacao = null;
            }
        }

        public void Dispose()
        {
            _transacao?.Dispose();
            _session?.Dispose();
        }
    }
}