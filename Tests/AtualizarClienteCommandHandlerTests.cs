using Application.Clientes.Atualizar;
using Domain.Comum;
using Domain.Entidades;
using Domain.ValueObjects;
using Infrastructure.Interfaces;
using Infrastructure.Repositorios;
using NHibernate;

namespace Tests
{
    public class AtualizarClienteCommandHandlerTests : IDisposable
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly ISession _session;
        private readonly IClienteRepositorio _repositorio;
        private readonly AtualizarClienteCommandHandler _manipulador;

        public AtualizarClienteCommandHandlerTests()
        {
            _sessionFactory = TestNHibernateConfig.CriarSessionFactory();
            _session = _sessionFactory.OpenSession();
            _repositorio = new NHibernateClienteRepositorio(_session);
            _manipulador = new AtualizarClienteCommandHandler(_repositorio);
        }

        [Fact]
        public async Task Handle_DeveAtualizarClienteComSucesso_QuandoDadosSaoValidos()
        {
            var cnpj = new Cnpj("11.222.333/0001-81");
            var cliente = new Cliente("Nome Original", cnpj, true);
            await _repositorio.AdicionarAsync(cliente);

            var comando = new AtualizarClienteCommand(cliente.Id, "Nome Atualizado");

            var retorno = await _manipulador.Handle(comando);

            Assert.NotNull(retorno);
            Assert.Equal(cliente.Id, retorno.Id);
            Assert.Equal("Nome Atualizado", retorno.NomeFantasia);
            Assert.Equal("11222333000181", retorno.Cnpj);
            Assert.True(retorno.Ativo);

            var clienteSalvo = await _repositorio.ObterPorIdAsync(cliente.Id);
            Assert.NotNull(clienteSalvo);
            Assert.Equal("Nome Atualizado", clienteSalvo.NomeFantasia);
        }

        [Fact]
        public async Task Handle_DeveRetornarNull_QuandoClienteNaoExiste()
        {
            var idInexistente = Guid.NewGuid();
            var comando = new AtualizarClienteCommand(idInexistente, "Nome Teste");

            var retorno = await _manipulador.Handle(comando);

            Assert.Null(retorno);
        }

        [Fact]
        public async Task Handle_DeveLancarExcecao_QuandoNomeFantasiaEhVazio()
        {
            var cnpj = new Cnpj("11.222.333/0001-81");
            var cliente = new Cliente("Nome Original", cnpj, true);
            await _repositorio.AdicionarAsync(cliente);

            var comando = new AtualizarClienteCommand(cliente.Id, "");

            var excecao = await Assert.ThrowsAsync<DomainException>(
                () => _manipulador.Handle(comando));

            Assert.Equal("Nome fantasia não pode ser vazio.", excecao.Message);
        }

        [Fact]
        public async Task Handle_DeveLancarExcecao_QuandoNomeFantasiaEhNull()
        {
            var cnpj = new Cnpj("11.222.333/0001-81");
            var cliente = new Cliente("Nome Original", cnpj, true);
            await _repositorio.AdicionarAsync(cliente);

            var comando = new AtualizarClienteCommand(cliente.Id, null!);

            var excecao = await Assert.ThrowsAsync<DomainException>(
                () => _manipulador.Handle(comando));

            Assert.Equal("Nome fantasia não pode ser vazio.", excecao.Message);
        }

        [Fact]
        public async Task Handle_DeveManterOutrosDadosInalterados_QuandoAtualizaNome()
        {
            var cnpj = new Cnpj("11.222.333/0001-81");
            var cliente = new Cliente("Nome Original", cnpj, false);
            await _repositorio.AdicionarAsync(cliente);

            var comando = new AtualizarClienteCommand(cliente.Id, "Nome Atualizado");

            var retorno = await _manipulador.Handle(comando);

            Assert.NotNull(retorno);
            Assert.Equal("11222333000181", retorno.Cnpj); // CNPJ deve permanecer o mesmo
            Assert.False(retorno.Ativo); // Status deve permanecer o mesmo
        }

        [Fact]
        public async Task Handle_DeveAtualizarApenasPropiedadeNomeFantasia()
        {
            var cnpj = new Cnpj("11.222.333/0001-81");
            var cliente = new Cliente("Nome Original", cnpj, true);
            var idOriginal = cliente.Id;
            await _repositorio.AdicionarAsync(cliente);

            var comando = new AtualizarClienteCommand(cliente.Id, "   Nome Com Espacos   ");

            var retorno = await _manipulador.Handle(comando);

            Assert.NotNull(retorno);
            Assert.Equal(idOriginal, retorno.Id);
            Assert.Equal("Nome Com Espacos", retorno.NomeFantasia); // Deve fazer trim
        }

        public void Dispose()
        {
            _session?.Dispose();
            _sessionFactory?.Dispose();
        }
    }
}