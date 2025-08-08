using FluentMigrator;

namespace Infrastructure.Migrations
{
    [Migration(20240107051208)]
    public class AddTablePessoa : Migration
    {
        public override void Up()
        {
            Create
                .Table("pessoa").WithDescription("Stores pessoa data")
                .WithColumn("id").AsString().PrimaryKey().WithColumnDescription("unique identifier")
                .WithColumn("documento").AsString().NotNullable().WithColumnDescription("documento")
                .WithColumn("nome").AsString().NotNullable().WithColumnDescription("name from a costumer")
                .WithColumn("password").AsString().Nullable().WithColumnDescription("password")
                .WithColumn("created_at").AsDateTime().NotNullable().WithColumnDescription("")
                .WithColumn("updated_at").AsDateTime().Nullable().WithColumnDescription("");
        }

        public override void Down()
        {
            Delete
                .Table("pessoa");
        }
    }
}