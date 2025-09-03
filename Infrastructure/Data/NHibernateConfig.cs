using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Infrastructure.Mappings;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace Infrastructure.Data
{
    public static class NHibernateConfig
    {
        public static ISessionFactory CriarSessionFactory(bool emMemoria = false)
        {
            var configuracao = emMemoria 
                ? SQLiteConfiguration.Standard.InMemory()
                : SQLiteConfiguration.Standard.ConnectionString("Data Source=:memory:;Version=3;New=True;");

            return Fluently.Configure()
                .Database(configuracao)
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<ClienteMap>())
                .ExposeConfiguration(cfg => new SchemaExport(cfg).Create(true, true))
                .BuildSessionFactory();
        }
    }
}