using FCG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Infrastructure.Mappings
{
    public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Nome)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(u => u.Apelido)
                .IsRequired()
                .HasColumnType("varchar(30)");

            builder.HasIndex(u => u.Apelido)
                .IsUnique();

            builder.Property(u => u.Email)
                .IsRequired()
                .HasColumnType($"varchar(200)");

            builder.Property(u => u.Senha)
                .IsRequired()
                .HasColumnType("nvarchar(1000)");

            builder.Property(u => u.Salt)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(u => u.Role)
                .IsRequired()
                .HasColumnType("int");

            builder.ToTable("Usuarios");
        }
    }
}