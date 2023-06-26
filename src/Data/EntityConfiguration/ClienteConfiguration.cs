using dominio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Data.EntityConfiguration
{
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder
                .ToTable("Clientes");

            builder
                .HasKey(a => a.Id);

            builder
                .Property(a => a.Nome)
                .IsRequired()
                .HasMaxLength(40);

            builder
               .OwnsMany(p => p.Enderecos, a =>
               {
                   a.ToTable("ClientesEnderecos");
                   a.WithOwner().HasForeignKey("ClienteId");

                   a.Property<Guid>("Id")
                       .ValueGeneratedNever();

                   a.Property(e => e.Cep)
                       .IsRequired()
                       .HasMaxLength(9);

                   a.Property(e => e.Logradouro)
                       .IsRequired()
                       .HasMaxLength(150);

                   a.Property(e => e.Numero)
                       .IsRequired()
                       .HasMaxLength(5);

                   a.Property(e => e.Complemento)
                       .IsRequired(false)
                       .HasMaxLength(150);

                   a.Property(e => e.Bairro)
                       .IsRequired()
                       .HasMaxLength(80);

                   a.Property(e => e.MunicipioId)
                    .IsRequired();

                   a.Property(e => e.UnidadeFederativaId)
                    .IsRequired();

                   a.HasKey("Id");
               });

        }
    }
}
