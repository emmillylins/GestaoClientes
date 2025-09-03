using Domain.Comum;
using Domain.ValueObjects;

namespace Domain.Entidades
{
    public class Cliente
    {
        public virtual Guid Id { get; protected set; }
        public virtual string NomeFantasia { get; protected set; } = string.Empty;
        public virtual Cnpj Cnpj { get; protected set; }
        public virtual bool Ativo { get; protected set; }

        protected Cliente() { }

        public Cliente(string nomeFantasia, Cnpj cnpj, bool ativo)
        {
            Id = Guid.NewGuid();
            DefinirNome(nomeFantasia);
            Cnpj = cnpj;
            Ativo = ativo;
        }

        public virtual void DefinirNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new DomainException("Nome fantasia não pode ser vazio.");
            NomeFantasia = nome.Trim();
        }

        public virtual void Ativar() => Ativo = true;
        public virtual void Desativar() => Ativo = false;
    }
}
