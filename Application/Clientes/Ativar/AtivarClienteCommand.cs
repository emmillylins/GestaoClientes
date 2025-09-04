namespace Application.Clientes.Ativar
{
    public class AtivarClienteCommand
    {
        public Guid Id { get; set; }

        public AtivarClienteCommand()
        {
        }

        public AtivarClienteCommand(Guid id)
        {
            Id = id;
        }
    }
}