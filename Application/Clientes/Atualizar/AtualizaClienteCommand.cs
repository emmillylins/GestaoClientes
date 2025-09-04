namespace Application.Clientes.Atualizar
{
    public class AtualizaClienteCommand
    {
        public Guid Id { get; set; }
        public string NomeFantasia { get; set; } = string.Empty;

        public AtualizaClienteCommand()
        {
        }

        public AtualizaClienteCommand(Guid id, string nomeFantasia)
        {
            Id = id;
            NomeFantasia = nomeFantasia;
        }
    }
}