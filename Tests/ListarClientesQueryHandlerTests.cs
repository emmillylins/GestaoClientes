using Application.Clientes.Listar;
using Domain.Entidades;
using Domain.ValueObjects;
using Infrastructure.Interfaces;
using Infrastructure.Repositorios;
using NHibernate;

namespace Tests
{
    public class ListarClientesQueryHandlerTests : IDisposable
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly ISession _session;
        private readonly IClienteRepositorio _repositorio;
        private readonly ListaClientesQueryHandler _manipulador;

        public ListarClientesQueryHandlerTests()
        {
            _sessionFactory = TestNHibernateConfig.CriarSessionFactory();
            _session = _sessionFactory.OpenSession();
            _repositorio = new NHibernateClienteRepositorio(_session);
            _manipulador = new ListaClientesQueryHandler(_repositorio);
        }

        [Fact]
        public async Task Handle_DeveRetornarListaVazia_QuandoNaoExistemClientes()
        {
            var consulta = new ListaClientesQuery();

            var retorno = await _manipulador.Handle(consulta);

            Assert.NotNull(retorno);
            Assert.Empty(retorno);
        }

        [Fact]
        public async Task Handle_DeveRetornarTodosOsClientes_QuandoExistemClientes()
        {
            var cnpj1 = new Cnpj("11.222.333/0001-81");
            var cnpj2 = new Cnpj("52.870.136/0001-56");
            var cnpj3 = new Cnpj("43.965.298/0001-87");

            var cliente1 = new Cliente("Primeira Empresa", cnpj1, true);
            var cliente2 = new Cliente("Segunda Empresa", cnpj2, false);
            var cliente3 = new Cliente("Terceira Empresa", cnpj3, true);

            await _repositorio.AdicionarAsync(cliente1);
            await _repositorio.AdicionarAsync(cliente2);
            await _repositorio.AdicionarAsync(cliente3);

            var consulta = new ListaClientesQuery();

            var retorno = await _manipulador.Handle(consulta);

            Assert.NotNull(retorno);
            Assert.Equal(3, retorno.Count());

            var clientesArray = retorno.ToArray();
            Assert.Contains(clientesArray, c => c.NomeFantasia == "Primeira Empresa");
            Assert.Contains(clientesArray, c => c.NomeFantasia == "Segunda Empresa");
            Assert.Contains(clientesArray, c => c.NomeFantasia == "Terceira Empresa");
        }

        [Fact]
        public async Task Handle_DeveRetornarClientesComDadosCorretos()
        {
            var cnpj = new Cnpj("11.222.333/0001-81");
            var cliente = new Cliente("Empresa Teste", cnpj, true);
            await _repositorio.AdicionarAsync(cliente);

            var consulta = new ListaClientesQuery();

            var retorno = await _manipulador.Handle(consulta);

            Assert.NotNull(retorno);
            Assert.Single(retorno);

            var clienteRetornado = retorno.First();
            Assert.Equal(cliente.Id, clienteRetornado.Id);
            Assert.Equal("Empresa Teste", clienteRetornado.NomeFantasia);
            Assert.Equal("11222333000181", clienteRetornado.Cnpj);
            Assert.True(clienteRetornado.Ativo);
        }

        public void Dispose()
        {
            _session?.Dispose();
            _sessionFactory?.Dispose();
        }
    }
}