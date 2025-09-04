using Application.Clientes.Listar.Obter;
using Domain.Entidades;
using Domain.ValueObjects;
using Infrastructure.Interfaces;
using Infrastructure.Repositorios;
using NHibernate;

namespace Tests
{
    public class ObterClientePorIdQueryHandlerTests : IDisposable
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly ISession _session;
        private readonly IClienteRepositorio _repositorio;
        private readonly ObterClientePorIdQueryHandler _manipulador;

        public ObterClientePorIdQueryHandlerTests()
        {
            _sessionFactory = TestNHibernateConfig.CriarSessionFactory();
            _session = _sessionFactory.OpenSession();
            _repositorio = new NHibernateClienteRepositorio(_session);
            _manipulador = new ObterClientePorIdQueryHandler(_repositorio);
        }

        [Fact]
        public async Task Handle_DeveRetornarCliente_QuandoIdExiste()
        {
            var cnpj = new Cnpj("11.222.333/0001-81");
            var cliente = new Cliente("Empresa Teste", cnpj, true);
            await _repositorio.AdicionarAsync(cliente);

            var consulta = new ObterClientePorIdQuery(cliente.Id);

            var retorno = await _manipulador.Handle(consulta);

            Assert.NotNull(retorno);
            Assert.Equal(cliente.Id, retorno.Id);
            Assert.Equal("Empresa Teste", retorno.NomeFantasia);
            Assert.Equal("11222333000181", retorno.Cnpj);
            Assert.True(retorno.Ativo);
        }

        [Fact]
        public async Task Handle_DeveRetornarNull_QuandoIdNaoExiste()
        {
            var idInexistente = Guid.NewGuid();
            var consulta = new ObterClientePorIdQuery(idInexistente);

            var retorno = await _manipulador.Handle(consulta);

            Assert.Null(retorno);
        }

        [Fact]
        public async Task Handle_DeveRetornarClienteCorreto_QuandoExistemMultiplosClientes()
        {
            var cnpj1 = new Cnpj("11.222.333/0001-81");
            var cnpj2 = new Cnpj("52.870.136/0001-56");

            var cliente1 = new Cliente("Primeira Empresa", cnpj1, true);
            var cliente2 = new Cliente("Segunda Empresa", cnpj2, false);

            await _repositorio.AdicionarAsync(cliente1);
            await _repositorio.AdicionarAsync(cliente2);

            var consulta = new ObterClientePorIdQuery(cliente2.Id);

            var retorno = await _manipulador.Handle(consulta);

            Assert.NotNull(retorno);
            Assert.Equal(cliente2.Id, retorno.Id);
            Assert.Equal("Segunda Empresa", retorno.NomeFantasia);
            Assert.Equal("52870136000156", retorno.Cnpj);
            Assert.False(retorno.Ativo);
        }

        [Fact]
        public async Task Handle_DeveRetornarNull_QuandoIdEhGuidEmpty()
        {
            var consulta = new ObterClientePorIdQuery(Guid.Empty);

            var retorno = await _manipulador.Handle(consulta);

            Assert.Null(retorno);
        }

        public void Dispose()
        {
            _session?.Dispose();
            _sessionFactory?.Dispose();
        }
    }
}