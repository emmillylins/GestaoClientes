using Domain.Comum;
using Domain.ValueObjects;

namespace Domain.Entidades
{
    public class Cliente
    {
        public Guid Id { get; private set; }
        public string NomeFantasia { get; private set; }
        public Cnpj Cnpj { get; private set; }
        public bool Ativo { get; private set; }


        private Cliente() { }


        public Cliente(string nomeFantasia, Cnpj cnpj, bool ativo)
        {
            Id = Guid.NewGuid();
            DefinirNome(nomeFantasia);
            Cnpj = cnpj; // VO já valida
            Ativo = ativo;
        }


        public void DefinirNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new DomainException("Nome fantasia não pode ser vazio.");
            NomeFantasia = nome.Trim();
        }


        public void Ativar() => Ativo = true;
        public void Desativar() => Ativo = false;
    }
}
