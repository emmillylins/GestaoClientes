namespace Application.Clientes.Desativar
{
    public class DesativarClienteCommand
    {
        public Guid Id { get; set; }

        public DesativarClienteCommand()
        {
        }

        public DesativarClienteCommand(Guid id)
        {
            Id = id;
        }
    }
}