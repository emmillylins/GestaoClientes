namespace Application.Clientes.Listar.Obter
{
    public class ObterClientePorIdQuery
    {
        public Guid Id { get; set; }

        public ObterClientePorIdQuery()
        {
        }

        public ObterClientePorIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
