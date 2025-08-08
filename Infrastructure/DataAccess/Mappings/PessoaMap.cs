using Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.Mappings
{
    public class PessoaMap : IEntityTypeConfiguration<Pessoa>
    {
        public void Configure(EntityTypeBuilder<Pessoa> builder)
        {
            builder
                .ToTable("pessoa")
                .HasKey(x => x.Id);

            builder
               .Property(x => x.Id)
               .HasColumnName("id")
               .IsRequired();

            builder
               .Property(x => x.Name)
               .HasColumnName("nome")
               .IsRequired();

            builder
               .Property(x => x.Password)
               .HasColumnName("password");


            builder
                .Property(x => x.Documento)
                .HasColumnName("documento")
                .IsRequired();

            builder
                .Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder
                .Property(x => x.UpdatedAt)
                .HasColumnName("updated_at");

            builder
                .HasMany(x => x.Contas)
                .WithOne(x => x.Pessoa)
                .HasForeignKey(x => x.PessoaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}