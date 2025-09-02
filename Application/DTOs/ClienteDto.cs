namespace Application.DTOs
{
    public class ClienteDto
    {
        public Guid Id { get; set; }
        public string NomeFantasia { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public bool Ativo { get; set; }

        public ClienteDto()
        {
        }

        public ClienteDto(Guid id, string nomeFantasia, string cnpj, bool ativo)
        {
            Id = id;
            NomeFantasia = nomeFantasia;
            Cnpj = cnpj;
            Ativo = ativo;
        }
    }
}
