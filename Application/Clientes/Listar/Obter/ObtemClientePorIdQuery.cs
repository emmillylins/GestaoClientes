namespace Application.Clientes.Listar.Obter
{
    public class ObtemClientePorIdQuery
    {
        public Guid Id { get; set; }

        public ObtemClientePorIdQuery()
        {
        }

        public ObtemClientePorIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
