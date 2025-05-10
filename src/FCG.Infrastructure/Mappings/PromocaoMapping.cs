using FCG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Infrastructure.Mappings
{
    public class PromocaoMapping : IEntityTypeConfiguration<Promocao>
    {
        public void Configure(EntityTypeBuilder<Promocao> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Cupom)
                .IsRequired()
                .HasColumnType("varchar(20)");

            builder.HasIndex(p => p.Cupom)
                .IsUnique();

            builder.Property(p => p.Descricao)
                .IsRequired()
                .HasColumnType("varchar(1000)");

            builder.Property(p => p.TipoDesconto)
                .IsRequired()
                .HasColumnType("int");

            builder.Property(p => p.ValorDesconto)
                .IsRequired()
                .HasColumnType("numeric(8,2)");

            builder.ToTable("Promocoes");
        }
    }
}