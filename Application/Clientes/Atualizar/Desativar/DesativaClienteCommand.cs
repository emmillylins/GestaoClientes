namespace Application.Clientes.Atualizar.Desativar
{
    public class DesativaClienteCommand
    {
        public Guid Id { get; set; }

        public DesativaClienteCommand()
        {
        }

        public DesativaClienteCommand(Guid id)
        {
            Id = id;
        }
    }
}