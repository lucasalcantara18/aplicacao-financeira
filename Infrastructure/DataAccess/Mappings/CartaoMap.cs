using Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.Mappings
{
    public class CartaoMap : IEntityTypeConfiguration<Cartao>
    {
        public void Configure(EntityTypeBuilder<Cartao> builder)
        {
            builder
                .ToTable("cartao")
                .HasKey(x => x.Id);

            builder
               .Property(x => x.Id)
               .HasColumnName("id")
               .IsRequired();

            builder
               .Property(x => x.Number)
               .HasColumnName("number")
               .IsRequired();

            builder
                .Property(x => x.Type)
                .HasColumnName("type")
                .IsRequired();


            builder
                .Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder
                .Property(x => x.UpdatedAt)
                .HasColumnName("updated_at");


            builder
                .Property(x => x.Cvv)
                .HasColumnName("cvv")
                .IsRequired();

            builder
                .Property(x => x.ContaId)
                .HasColumnName("conta_id")
                .IsRequired();

            builder
                .HasOne(x => x.Conta)
                .WithMany(x => x.Cartoes)
                .HasForeignKey(x => x.ContaId);
        }
    }
}