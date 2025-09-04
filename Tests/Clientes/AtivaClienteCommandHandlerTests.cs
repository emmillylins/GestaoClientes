using Application.Clientes.Atualizar.Ativar;
using Domain.Entidades;
using Domain.ValueObjects;
using Infrastructure.Interfaces;
using Infrastructure.Repositorios;
using NHibernate;

namespace Tests.Clientes
{
    public class AtivaClienteCommandHandlerTests : IDisposable
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly ISession _session;
        private readonly IClienteRepositorio _repositorio;
        private readonly AtivaClienteCommandHandler _manipulador;

        public AtivaClienteCommandHandlerTests()
        {
            _sessionFactory = TestNHibernateConfig.CriarSessionFactory();
            _session = _sessionFactory.OpenSession();
            _repositorio = new NHibernateClienteRepositorio(_session);
            _manipulador = new AtivaClienteCommandHandler(_repositorio);
        }

        [Fact]
        public async Task Handle_DeveAtivarClienteComSucesso_QuandoClienteEstaInativo()
        {
            var cnpj = new Cnpj("11.222.333/0001-81");
            var cliente = new Cliente("Empresa Teste", cnpj, false);
            await _repositorio.AdicionarAsync(cliente);

            var comando = new AtivaClienteCommand(cliente.Id);

            var retorno = await _manipulador.Handle(comando);

            Assert.NotNull(retorno);
            Assert.Equal(cliente.Id, retorno.Id);
            Assert.True(retorno.Ativo);

            var clienteSalvo = await _repositorio.ObterPorIdAsync(cliente.Id);
            Assert.NotNull(clienteSalvo);
            Assert.True(clienteSalvo.Ativo);
        }

        [Fact]
        public async Task Handle_DeveRetornarNull_QuandoClienteNaoExiste()
        {
            var idInexistente = Guid.NewGuid();
            var comando = new AtivaClienteCommand(idInexistente);

            var retorno = await _manipulador.Handle(comando);

            Assert.Null(retorno);
        }

        [Fact]
        public async Task Handle_DeveLancarExcecao_QuandoClienteJaEstaAtivo()
        {
            var cnpj = new Cnpj("11.222.333/0001-81");
            var cliente = new Cliente("Empresa Teste", cnpj, true);
            await _repositorio.AdicionarAsync(cliente);

            var comando = new AtivaClienteCommand(cliente.Id);

            var excecao = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _manipulador.Handle(comando));

            Assert.Equal("Cliente já está ativo.", excecao.Message);
        }

        [Fact]
        public async Task Handle_DeveManterOutrosDadosInalterados_QuandoAtiva()
        {
            var cnpj = new Cnpj("11.222.333/0001-81");
            var cliente = new Cliente("Empresa Teste", cnpj, false);
            await _repositorio.AdicionarAsync(cliente);

            var comando = new AtivaClienteCommand(cliente.Id);

            var retorno = await _manipulador.Handle(comando);

            Assert.NotNull(retorno);
            Assert.Equal("Empresa Teste", retorno.NomeFantasia);
            Assert.Equal("11222333000181", retorno.Cnpj);
        }

        [Fact]
        public async Task Handle_DeveAtivarApenasClienteEspecifico_QuandoExistemMultiplosClientes()
        {
            var cnpj1 = new Cnpj("11.222.333/0001-81");
            var cnpj2 = new Cnpj("43.965.298/0001-87");

            var cliente1 = new Cliente("Primeira Empresa", cnpj1, false);
            var cliente2 = new Cliente("Segunda Empresa", cnpj2, false);

            await _repositorio.AdicionarAsync(cliente1);
            await _repositorio.AdicionarAsync(cliente2);

            var comando = new AtivaClienteCommand(cliente1.Id);

            var retorno = await _manipulador.Handle(comando);

            Assert.NotNull(retorno);
            Assert.True(retorno.Ativo);

            var cliente1Salvo = await _repositorio.ObterPorIdAsync(cliente1.Id);
            var cliente2Salvo = await _repositorio.ObterPorIdAsync(cliente2.Id);

            Assert.True(cliente1Salvo!.Ativo);
            Assert.False(cliente2Salvo!.Ativo);
        }

        public void Dispose()
        {
            _session?.Dispose();
            _sessionFactory?.Dispose();
        }
    }
}