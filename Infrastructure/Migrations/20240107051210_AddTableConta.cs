using FluentMigrator;
using System.Data;

namespace Infrastructure.Migrations
{
    [Migration(20240107051210)]
    public class AddTableConta : Migration
    {
        public override void Up()
        {
            Create
                .Table("conta").WithDescription("tabela de conta")
                .WithColumn("id").AsString().PrimaryKey().WithColumnDescription("unique identifier")
                .WithColumn("branch").AsString().Nullable().WithColumnDescription("number identifier from a street")
                .WithColumn("account").AsString().Nullable().WithColumnDescription("street address from a account costumer")
                .WithColumn("created_at").AsDateTime().NotNullable().WithColumnDescription("")
                .WithColumn("updated_at").AsDateTime().Nullable().WithColumnDescription("")
                .WithColumn("pessoa_id").AsString().NotNullable().ForeignKey("conta_fk_pessoa", "pessoa", "id").OnDelete(Rule.Cascade).WithColumnDescription("");
        }

        public override void Down()
        {
            Delete
                .Table("conta");
        }
    }
}