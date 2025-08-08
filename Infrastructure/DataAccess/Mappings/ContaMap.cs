using Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.Mappings
{
    public class ContaMap : IEntityTypeConfiguration<Conta>
    {
        public void Configure(EntityTypeBuilder<Conta> builder)
        {
            builder
                .ToTable("conta")
                .HasKey(x => x.Id);

            builder
               .Property(x => x.Id)
               .HasColumnName("id")
               .IsRequired();

            builder
               .Property(x => x.Branch)
               .HasColumnName("branch")
               .IsRequired();


            builder
               .Property(x => x.Account)
               .HasColumnName("account")
               .IsRequired();


            builder
                .Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder
                .Property(x => x.UpdatedAt)
                .HasColumnName("updated_at");

            builder
               .Property(x => x.PessoaId)
               .HasColumnName("pessoa_id")
               .IsRequired();

            builder
                .HasOne(x => x.Pessoa)
                .WithMany(x => x.Contas)
                .HasForeignKey(x => x.PessoaId);

            builder
                .HasMany(x => x.Cartoes)
                .WithOne(x => x.Conta)
                .HasForeignKey(x => x.ContaId);

            builder
                .HasMany(x => x.Transactions)
                .WithOne(x => x.Conta)
                .HasForeignKey(x => x.ContaId);
        }
    }
}