namespace Application.Clientes.Atualizar.Ativar
{
    public class AtivaClienteCommand
    {
        public Guid Id { get; set; }

        public AtivaClienteCommand()
        {
        }

        public AtivaClienteCommand(Guid id)
        {
            Id = id;
        }
    }
}