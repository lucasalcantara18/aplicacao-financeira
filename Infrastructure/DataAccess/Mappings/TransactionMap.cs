using Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.Mappings
{
    public class TransactionMap : IEntityTypeConfiguration<ClientTransaction>
    {
        public void Configure(EntityTypeBuilder<ClientTransaction> builder)
        {
            builder
                .ToTable("transaction")
                .HasKey(x => x.Id);

            builder
               .Property(x => x.Id)
               .HasColumnName("id")
               .IsRequired();

            builder
               .Property(x => x.Value)
               .HasColumnName("value")
               .IsRequired();

            builder
                .Property(x => x.Description)
                .HasColumnName("description")
                .IsRequired();

            builder
                .Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder
                .Property(x => x.UpdatedAt)
                .HasColumnName("updated_at");

            builder
                .Property(x => x.ContaId)
                .HasColumnName("conta_id")
                .IsRequired();


            builder
                .Property(x => x.IsReverted)
                .HasColumnName("reverted")
                .IsRequired();

            builder
                .Property(x => x.Internal)
                .HasColumnName("internal")
                .IsRequired();

            builder
                .HasOne(x => x.Conta)
                .WithMany(x => x.Transactions)
                .HasForeignKey(x => x.ContaId);
        }
    }
}