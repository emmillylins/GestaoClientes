using Domain.Entidades;
using FluentNHibernate.Mapping;

namespace Infrastructure.Mappings
{
    public class ClienteMap : ClassMap<Cliente>
    {
        public ClienteMap()
        {
            Table("Clientes");
            
            Id(x => x.Id)
                .GeneratedBy.Assigned()
                .Column("Id")
                .CustomType<GuidStringUserType>();
                
            Map(x => x.NomeFantasia)
                .Column("NomeFantasia")
                .Length(500)
                .Not.Nullable();
                
            Component(x => x.Cnpj, cnpj =>
            {
                cnpj.Map(c => c.Numero)
                    .Column("Cnpj")
                    .Length(14)
                    .Not.Nullable();
            });
            
            Map(x => x.Ativo)
                .Column("Ativo")
                .CustomType("boolean")
                .Not.Nullable();
        }
    }
}