using Application.DTOs;
using Domain.Entidades;
using Domain.ValueObjects;
using Infrastructure.Interfaces;

namespace Application.Clientes.Criar
{
    public class CriarClienteCommandHandler
    {
        private readonly IClienteRepositorio _repo;
        public CriarClienteCommandHandler(IClienteRepositorio repo) => _repo = repo;


        public async Task<ClienteDto> Handle(CriarClienteCommand cmd, CancellationToken ct = default)
        {
            var cnpj = new Cnpj(cmd.Cnpj);


            if (await _repo.ExistePorCnpjAsync(cnpj, ct))
                throw new InvalidOperationException("CNPJ já cadastrado.");


            var cliente = new Cliente(cmd.NomeFantasia, cnpj, cmd.Ativo);
            await _repo.AdicionarAsync(cliente, ct);


            return new ClienteDto(cliente.Id, cliente.NomeFantasia, cliente.Cnpj, cliente.Ativo);
        }
    }
}
