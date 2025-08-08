using FluentMigrator;
using System.Data;

namespace Infrastructure.Migrations
{
    [Migration(20240107051212)]
    public class AddTableMonthlyIncome : Migration
    {
        public override void Up()
        {
            Create
                .Table("cartao").WithDescription("cartoes do cliente")
                .WithColumn("id").AsString().PrimaryKey().WithColumnDescription("unique identifier")
                .WithColumn("type").AsString().NotNullable().WithColumnDescription("monthly income from a costumer")
                .WithColumn("number").AsString().NotNullable().WithColumnDescription("monthly income from a costumer")
                .WithColumn("cvv").AsString().NotNullable().WithColumnDescription("monthly income from a costumer")
                .WithColumn("created_at").AsDateTime().NotNullable().WithColumnDescription("")
                .WithColumn("updated_at").AsDateTime().Nullable().WithColumnDescription("")
                .WithColumn("conta_id").AsString().NotNullable().ForeignKey("cartao_fk_conta", "conta", "id").OnDelete(Rule.Cascade).WithColumnDescription("");
        }

        public override void Down()
        {
            Delete
                .Table("cartao");
        }
    }
}