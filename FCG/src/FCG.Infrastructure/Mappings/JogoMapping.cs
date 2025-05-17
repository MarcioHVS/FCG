using FCG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Infrastructure.Mappings
{
    public class JogoMapping : IEntityTypeConfiguration<Jogo>
    {
        public void Configure(EntityTypeBuilder<Jogo> builder)
        {
            builder.HasKey(j => j.Id);

            builder.Property(j => j.Titulo)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.HasIndex(j => j.Titulo)
                .IsUnique();

            builder.Property(j => j.Descricao)
                .IsRequired()
                .HasColumnType("varchar(1000)");

            builder.Property(j => j.Genero)
                .IsRequired()
                .HasColumnType("int");

            builder.Property(j => j.Valor)
                .IsRequired()
                .HasColumnType("numeric(8,2)");

            builder.ToTable("Jogos");
        }
    }
}