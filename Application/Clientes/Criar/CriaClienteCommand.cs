namespace Application.Clientes.Criar
{
    public class CriaClienteCommand
    {
        public string NomeFantasia { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public bool Ativo { get; set; }

        public CriaClienteCommand()
        {
        }

        public CriaClienteCommand(string nomeFantasia, string cnpj, bool ativo)
        {
            NomeFantasia = nomeFantasia;
            Cnpj = cnpj;
            Ativo = ativo;
        }
    }
}
