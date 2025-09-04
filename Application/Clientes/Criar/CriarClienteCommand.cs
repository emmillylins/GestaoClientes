namespace Application.Clientes.Criar
{
    public class CriarClienteCommand
    {
        public string NomeFantasia { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public bool Ativo { get; set; }

        public CriarClienteCommand()
        {
        }

        public CriarClienteCommand(string nomeFantasia, string cnpj, bool ativo)
        {
            NomeFantasia = nomeFantasia;
            Cnpj = cnpj;
            Ativo = ativo;
        }
    }
}
