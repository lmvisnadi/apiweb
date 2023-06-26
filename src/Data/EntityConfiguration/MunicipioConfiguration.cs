using dominio.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Data.EntityConfiguration
{
    public class MunicipioConfiguration : IEntityTypeConfiguration<Municipio>
    {
        public void Configure(EntityTypeBuilder<Municipio> builder)
        {
            builder
                .HasKey(p => p.Id);


            builder
                .Property(p => p.Nome)
                .HasMaxLength(200)
                .IsRequired();

            builder
                .Property(p => p.UnidadeFederativaId)
                .IsRequired();
        }
    }
}
