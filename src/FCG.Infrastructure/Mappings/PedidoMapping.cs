using FCG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Infrastructure.Mappings
{
    public class PedidoMapping : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Valor)
                .IsRequired()
                .HasColumnType("numeric(8,2)");

            builder.HasOne(p => p.Usuario)
                .WithMany(u => u.Pedidos)
                .HasPrincipalKey(u => u.Id);

            builder.HasOne(p => p.Usuario)
                .WithMany(u => u.Pedidos)
                .HasForeignKey(p => p.UsuarioId);

            builder.ToTable("Pedidos");
        }
    }
}