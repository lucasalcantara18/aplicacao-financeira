using FluentMigrator;
using System.Data;

namespace Infrastructure.Migrations
{
    [Migration(20240107051214)]
    public class AddTableTransaction : Migration
    {
        public override void Up()
        {
            Create
                .Table("transaction").WithDescription("tabela de transações")
                .WithColumn("id").AsString().PrimaryKey().WithColumnDescription("unique identifier")
                .WithColumn("value").AsDecimal().NotNullable().WithColumnDescription("value")
                .WithColumn("description").AsString().NotNullable().WithColumnDescription("transaction description")
                .WithColumn("created_at").AsDateTime().NotNullable().WithColumnDescription("")
                .WithColumn("updated_at").AsDateTime().Nullable().WithColumnDescription("")
                .WithColumn("reverted").AsBoolean().NotNullable().WithColumnDescription("")
                .WithColumn("internal").AsString().NotNullable().WithColumnDescription("")
                .WithColumn("conta_id").AsString().NotNullable().ForeignKey("transaction_fk_conta", "conta", "id").OnDelete(Rule.Cascade).WithColumnDescription("");
        }

        public override void Down()
        {
            Delete
                .Table("transaction");
        }
    }
}