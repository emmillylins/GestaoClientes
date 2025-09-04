using Application.Clientes.Atualizar.Desativar;
using Domain.Entidades;
using Domain.ValueObjects;
using Infrastructure.Interfaces;
using Infrastructure.Repositorios;
using NHibernate;

namespace Tests.Clientes
{
    public class DesativaClienteCommandHandlerTests : IDisposable
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly ISession _session;
        private readonly IClienteRepositorio _repositorio;
        private readonly DesativaClienteCommandHandler _manipulador;

        public DesativaClienteCommandHandlerTests()
        {
            _sessionFactory = TestNHibernateConfig.CriarSessionFactory();
            _session = _sessionFactory.OpenSession();
            _repositorio = new NHibernateClienteRepositorio(_session);
            _manipulador = new DesativaClienteCommandHandler(_repositorio);
        }

        [Fact]
        public async Task Handle_DeveDesativarClienteComSucesso_QuandoClienteEstaAtivo()
        {
            var cnpj = new Cnpj("11.222.333/0001-81");
            var cliente = new Cliente("Empresa Teste", cnpj, true);
            await _repositorio.AdicionarAsync(cliente);

            var comando = new DesativaClienteCommand(cliente.Id);

            var retorno = await _manipulador.Handle(comando);

            Assert.NotNull(retorno);
            Assert.Equal(cliente.Id, retorno.Id);
            Assert.False(retorno.Ativo);

            var clienteSalvo = await _repositorio.ObterPorIdAsync(cliente.Id);
            Assert.NotNull(clienteSalvo);
            Assert.False(clienteSalvo.Ativo);
        }

        [Fact]
        public async Task Handle_DeveRetornarNull_QuandoClienteNaoExiste()
        {
            var idInexistente = Guid.NewGuid();
            var comando = new DesativaClienteCommand(idInexistente);

            var retorno = await _manipulador.Handle(comando);

            Assert.Null(retorno);
        }

        [Fact]
        public async Task Handle_DeveLancarExcecao_QuandoClienteJaEstaInativo()
        {
            var cnpj = new Cnpj("11.222.333/0001-81");
            var cliente = new Cliente("Empresa Teste", cnpj, false);
            await _repositorio.AdicionarAsync(cliente);

            var comando = new DesativaClienteCommand(cliente.Id);

            var excecao = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _manipulador.Handle(comando));

            Assert.Equal("Cliente já está inativo.", excecao.Message);
        }

        [Fact]
        public async Task Handle_DeveManterOutrosDadosInalterados_QuandoDesativa()
        {
            var cnpj = new Cnpj("11.222.333/0001-81");
            var cliente = new Cliente("Empresa Teste", cnpj, true);
            await _repositorio.AdicionarAsync(cliente);

            var comando = new DesativaClienteCommand(cliente.Id);

            var retorno = await _manipulador.Handle(comando);

            Assert.NotNull(retorno);
            Assert.Equal("Empresa Teste", retorno.NomeFantasia);
            Assert.Equal("11222333000181", retorno.Cnpj);
        }

        [Fact]
        public async Task Handle_DeveDesativarApenasClienteEspecifico_QuandoExistemMultiplosClientes()
        {
            var cnpj1 = new Cnpj("11.222.333/0001-81");
            var cnpj2 = new Cnpj("52.870.136/0001-56");

            var cliente1 = new Cliente("Primeira Empresa", cnpj1, true);
            var cliente2 = new Cliente("Segunda Empresa", cnpj2, true);

            await _repositorio.AdicionarAsync(cliente1);
            await _repositorio.AdicionarAsync(cliente2);

            var comando = new DesativaClienteCommand(cliente1.Id);

            var retorno = await _manipulador.Handle(comando);

            Assert.NotNull(retorno);
            Assert.False(retorno.Ativo);

            var cliente1Salvo = await _repositorio.ObterPorIdAsync(cliente1.Id);
            var cliente2Salvo = await _repositorio.ObterPorIdAsync(cliente2.Id);

            Assert.False(cliente1Salvo!.Ativo);
            Assert.True(cliente2Salvo!.Ativo); // Não deve ser afetado
        }

        public void Dispose()
        {
            _session?.Dispose();
            _sessionFactory?.Dispose();
        }
    }
}