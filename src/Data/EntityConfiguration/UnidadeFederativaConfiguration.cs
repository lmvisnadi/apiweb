using dominio.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Data.EntityConfiguration
{
    public class UnidadeFederativaConfiguration : IEntityTypeConfiguration<UnidadeFederativa>
    {
        public void Configure(EntityTypeBuilder<UnidadeFederativa> builder)
        {
            builder
                .HasKey(p => p.Id);

            builder
                .Property(p => p.Sigla)
                .HasMaxLength(2)
                .IsRequired();

            builder
                .Property(p => p.Nome)
                .HasMaxLength(150)
                .IsRequired();
        }
    }
}
