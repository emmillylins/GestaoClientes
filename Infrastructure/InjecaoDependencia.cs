using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Repositorios;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;

namespace Infrastructure
{
    public static class InjecaoDependencia
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection servicos)
        {
            servicos.AddSingleton<ISessionFactory>(provider => 
            {
                var sessionFactory = NHibernateConfig.CriarSessionFactory(emMemoria: true);
                
                using var session = sessionFactory.OpenSession();
                using var transaction = session.BeginTransaction();
                
                try
                {
                    // Testar se tabela existe
                    session.CreateSQLQuery("SELECT COUNT(*) FROM Clientes").UniqueResult();
                }
                catch
                {
                    // Criar tabela se não existir
                    session.CreateSQLQuery(@"
                        CREATE TABLE IF NOT EXISTS Clientes (
                            Id TEXT PRIMARY KEY,
                            NomeFantasia TEXT NOT NULL,
                            Cnpj TEXT NOT NULL UNIQUE,
                            Ativo INTEGER NOT NULL
                        )").ExecuteUpdate();
                }
                
                transaction.Commit();
                return sessionFactory;
            });
            
            servicos.AddScoped<ISession>(provider => 
            {
                var sessionFactory = provider.GetRequiredService<ISessionFactory>();
                return sessionFactory.OpenSession();
            });
            
            servicos.AddScoped<IUnitOfWork, NHibernateUnitOfWork>();
            servicos.AddScoped<IClienteRepositorio, NHibernateClienteRepositorio>();
            
            return servicos;
        }
    }
}
