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
                NHibernateConfig.CriarSessionFactory(emMemoria: true));
            
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
