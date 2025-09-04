namespace Application.Clientes.Atualizar
{
    public class AtualizarClienteCommand
    {
        public Guid Id { get; set; }
        public string NomeFantasia { get; set; } = string.Empty;

        public AtualizarClienteCommand()
        {
        }

        public AtualizarClienteCommand(Guid id, string nomeFantasia)
        {
            Id = id;
            NomeFantasia = nomeFantasia;
        }
    }
}