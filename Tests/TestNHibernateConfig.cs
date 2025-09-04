using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Infrastructure.Mappings;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace Tests
{
    public static class TestNHibernateConfig
    {
        public static ISessionFactory CriarSessionFactory()
        {
            var tempDbPath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.db");

            var configuracao = SQLiteConfiguration.Standard
                .ConnectionString($"Data Source={tempDbPath};Version=3;")
                .ShowSql()
                .AdoNetBatchSize(0); // Desabilita batching para testes

            var sessionFactory = Fluently.Configure()
                .Database(configuracao)
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<ClienteMap>())
                .ExposeConfiguration(cfg =>
                {
                    var schemaExport = new SchemaExport(cfg);
                    schemaExport.Create(false, true);
                })
                .BuildSessionFactory();

            return sessionFactory;
        }
    }
}