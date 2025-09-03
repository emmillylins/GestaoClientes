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

                // Verifica se tabela foi criada
                try
                {
                    using var session = _sessionFactory!.OpenSession();
                    using var transaction = session.BeginTransaction();

                    // Testar se consegue contar registros
                    var count = session.CreateSQLQuery("SELECT COUNT(*) FROM Clientes")
                        .UniqueResult<long>();

                    transaction.Commit();
                    Console.WriteLine($"Banco inicializado - {count} registros encontrados");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro na verificação: {ex.Message}");

                    // Tentar criar tabela manualmente
                    try
                    {
                        using var session = _sessionFactory!.OpenSession();
                        using var transaction = session.BeginTransaction();

                        session.CreateSQLQuery(@"
                        CREATE TABLE IF NOT EXISTS Clientes (
                            Id TEXT PRIMARY KEY,
                            NomeFantasia TEXT NOT NULL,
                            Cnpj TEXT NOT NULL UNIQUE,
                            Ativo INTEGER NOT NULL
                        )").ExecuteUpdate();

                        transaction.Commit();
                        Console.WriteLine("Tabela criada manualmente");
                    }
                    catch (Exception ex2)
                    {
                        Console.WriteLine($"Erro ao criar tabela: {ex2.Message}");
                    }
                }

                return _sessionFactory;
            }
        }
    }
}