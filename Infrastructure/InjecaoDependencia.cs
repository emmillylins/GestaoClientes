using Infrastructure.Interfaces;
using Infrastructure.Repositorios;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class InjecaoDependencia
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection servicos)
        {
            servicos.AddSingleton<IClienteRepositorio, EmMemoriaClienteRepositorio>();
            return servicos;
        }
    }
}
