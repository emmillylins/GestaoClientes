using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Infrastructure.Mappings;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace Infrastructure.Data
{
    public static class NHibernateConfig
    {
        private static ISessionFactory? _sessionFactory;
        private static readonly object _lock = new object();
        private static readonly string _tempDbPath = Path.GetTempFileName() + ".db";

        public static ISessionFactory CriarSessionFactory(bool emMemoria = true)
        {
            if (_sessionFactory != null)
                return _sessionFactory;

            lock (_lock)
            {
                if (_sessionFactory != null)
                    return _sessionFactory;

                IPersistenceConfigurer configuracao;
                
                if (emMemoria)
                {
                    configuracao = SQLiteConfiguration.Standard
                       .ConnectionString($"Data Source={_tempDbPath};Version=3;")
                       .ShowSql();
                }
                else
                {
                    configuracao = SQLiteConfiguration.Standard
                        .ConnectionString("Data Source=clientes.db;Version=3;");
                }

                _sessionFactory = Fluently.Configure()
                    .Database(configuracao)
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<ClienteMap>())
                    .ExposeConfiguration(cfg =>
                    {
                        var schemaExport = new SchemaExport(cfg);
                        schemaExport.Create(false, true);
                    })
                    .BuildSessionFactory();

                return _sessionFactory;
            }
        }
    }
}