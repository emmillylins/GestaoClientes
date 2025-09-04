using Application.Clientes.Criar;
using Domain.Comum;
using Domain.Entidades;
using Domain.ValueObjects;
using Infrastructure.Interfaces;
using Infrastructure.Repositorios;
using NHibernate;

namespace Tests.Clientes
{
    public class CriarClienteCommandHandlerTests : IDisposable
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly ISession _session;
        private readonly IClienteRepositorio _repositorio;
        private readonly CriaClienteCommandHandler _manipulador;

        public CriarClienteCommandHandlerTests()
        {
            _sessionFactory = TestNHibernateConfig.CriarSessionFactory();
            _session = _sessionFactory.OpenSession();
            _repositorio = new NHibernateClienteRepositorio(_session);
            _manipulador = new CriaClienteCommandHandler(_repositorio);
        }

        [Fact]
        public async Task Handle_DeveCriarClienteComSucesso_QuandoDadosSaoValidos()
        {
            var comando = new CriaClienteCommand("Empresa Teste", "11.222.333/0001-81", true);

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
            var comando1 = new CriaClienteCommand("Primeira Empresa", cnpjExistente, true);
            var comando2 = new CriaClienteCommand("Segunda Empresa", cnpjExistente, true);

            await _manipulador.Handle(comando1);

            var excecao = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _manipulador.Handle(comando2));

            Assert.Equal("CNPJ já cadastrado.", excecao.Message);
        }

        [Fact]
        public async Task Handle_DeveLancarExcecao_QuandoNomeFantasiaEhVazio()
        {
            var comando = new CriaClienteCommand("", "11.222.333/0001-81", true);

            var excecao = await Assert.ThrowsAsync<DomainException>(
                () => _manipulador.Handle(comando));

            Assert.Equal("Nome fantasia não pode ser vazio.", excecao.Message);
        }

        [Fact]
        public async Task Handle_DeveLancarExcecao_QuandoNomeFantasiaEhNull()
        {
            var comando = new CriaClienteCommand(null!, "11.222.333/0001-81", true);

            var excecao = await Assert.ThrowsAsync<DomainException>(
                () => _manipulador.Handle(comando));

            Assert.Equal("Nome fantasia não pode ser vazio.", excecao.Message);
        }

        [Fact]
        public async Task Handle_DeveLancarExcecao_QuandoCnpjEhInvalido()
        {
            var comando = new CriaClienteCommand("Empresa Teste", "11.111.111/0001-11", true);

            var excecao = await Assert.ThrowsAsync<DomainException>(
                () => _manipulador.Handle(comando));

            Assert.Equal("CNPJ inválido.", excecao.Message);
        }

        [Fact]
        public async Task Handle_DeveLancarExcecao_QuandoCnpjEhVazio()
        {
            var comando = new CriaClienteCommand("Empresa Teste", "", true);

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
}
