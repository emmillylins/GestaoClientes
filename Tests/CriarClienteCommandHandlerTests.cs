using Application.Clientes.Criar;
using Application.Clientes.Listar.Obter;
using Domain.Comum;
using Domain.Entidades;
using Domain.ValueObjects;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Repositorios;
using NHibernate;

namespace Tests
{
    public class CriarClienteCommandHandlerTests : IDisposable
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly ISession _session;
        private readonly IClienteRepositorio _repositorio;
        private readonly CriarClienteCommandHandler _manipulador;

        public CriarClienteCommandHandlerTests()
        {
            _sessionFactory = NHibernateConfig.CriarSessionFactory(emMemoria: true);
            _session = _sessionFactory.OpenSession();
            _repositorio = new NHibernateClienteRepositorio(_session);
            _manipulador = new CriarClienteCommandHandler(_repositorio);
        }

        [Fact]
        public async Task Handle_DeveCriarClienteComSucesso_QuandoDadosSaoValidos()
        {
            var comando = new CriarClienteCommand("Empresa Teste", "11.222.333/0001-81", true);

            var retorno = await _manipulador.Handle(comando);

            Assert.NotNull(retorno);
            Assert.Equal("Empresa Teste", retorno.NomeFantasia);
            Assert.Equal("11222333000181", retorno.Cnpj);
            Assert.True(retorno.Ativo);
            Assert.NotEqual(Guid.Empty, retorno.Id);

            var clienteSalvo = await _repositorio.ObterPorIdAsync(retorno.Id);
            Assert.NotNull(clienteSalvo);
        }

        [Fact]
        public async Task Handle_DeveLancarExcecao_QuandoCnpjJaExiste()
        {
            var cnpjExistente = "11.222.333/0001-81";
            var comando1 = new CriarClienteCommand("Primeira Empresa", cnpjExistente, true);
            var comando2 = new CriarClienteCommand("Segunda Empresa", cnpjExistente, true);

            await _manipulador.Handle(comando1);

            var excecao = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _manipulador.Handle(comando2));

            Assert.Equal("CNPJ já cadastrado.", excecao.Message);
        }

        [Fact]
        public async Task Handle_DeveLancarExcecao_QuandoNomeFantasiaEhVazio()
        {
            var comando = new CriarClienteCommand("", "11.222.333/0001-81", true);

            var excecao = await Assert.ThrowsAsync<DomainException>(
                () => _manipulador.Handle(comando));

            Assert.Equal("Nome fantasia não pode ser vazio.", excecao.Message);
        }

        [Fact]
        public async Task Handle_DeveLancarExcecao_QuandoNomeFantasiaEhNull()
        {
            var comando = new CriarClienteCommand(null!, "11.222.333/0001-81", true);

            var excecao = await Assert.ThrowsAsync<DomainException>(
                () => _manipulador.Handle(comando));

            Assert.Equal("Nome fantasia não pode ser vazio.", excecao.Message);
        }

        [Fact]
        public async Task Handle_DeveLancarExcecao_QuandoCnpjEhInvalido()
        {
            var comando = new CriarClienteCommand("Empresa Teste", "11.111.111/0001-11", true);

            var excecao = await Assert.ThrowsAsync<DomainException>(
                () => _manipulador.Handle(comando));

            Assert.Equal("CNPJ inválido.", excecao.Message);
        }

        [Fact]
        public async Task Handle_DeveLancarExcecao_QuandoCnpjEhVazio()
        {
            var comando = new CriarClienteCommand("Empresa Teste", "", true);

            var excecao = await Assert.ThrowsAsync<DomainException>(
                () => _manipulador.Handle(comando));

            Assert.Equal("CNPJ inválido.", excecao.Message);
        }

        public void Dispose()
        {
            _session?.Dispose();
            _sessionFactory?.Dispose();
        }
    }

    public class ObtemClientePorIdQueryHandlerTests : IDisposable
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly ISession _session;
        private readonly IClienteRepositorio _repositorio;
        private readonly ObterClientePorIdQueryHandler _manipulador;

        public ObtemClientePorIdQueryHandlerTests()
        {
            _sessionFactory = NHibernateConfig.CriarSessionFactory(emMemoria: true);
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
            var cnpj2 = new Cnpj("22.333.444/0001-25");

            var cliente1 = new Cliente("Primeira Empresa", cnpj1, true);
            var cliente2 = new Cliente("Segunda Empresa", cnpj2, false);

            await _repositorio.AdicionarAsync(cliente1);
            await _repositorio.AdicionarAsync(cliente2);

            var consulta = new ObterClientePorIdQuery(cliente2.Id);

            var retorno = await _manipulador.Handle(consulta);

            Assert.NotNull(retorno);
            Assert.Equal(cliente2.Id, retorno.Id);
            Assert.Equal("Segunda Empresa", retorno.NomeFantasia);
            Assert.Equal("22333444000125", retorno.Cnpj);
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
