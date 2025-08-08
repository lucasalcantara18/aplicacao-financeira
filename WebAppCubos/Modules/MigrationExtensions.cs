using FluentMigrator.Runner;
using Infrastructure.DataAccess.Sql;
using Microsoft.Data.SqlClient;
using Npgsql;

namespace WebApi.Modules.Database
{
    public static class MigrationExtensions
    {
        public static IServiceCollection AddMigrator(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Database");

            return services.AddMigrator(connectionString);
        }

        public static IServiceCollection AddMigrator(this IServiceCollection services, string connectionString)
        {
            EnsureDatabaseExists(connectionString);

            return services
                .AddFluentMigratorCore()
                .ConfigureRunner(builder =>
                   builder
                       .AddPostgres()
                       .WithVersionTable(MigrationVersionTable.Default)
                       .WithGlobalConnectionString(connectionString)
                       .ScanIn(typeof(DatabaseContext).Assembly).For.Migrations());
        }

        public static IApplicationBuilder UseDatabaseAlwaysUpToDate(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var provider = scope.ServiceProvider;
            var runner = provider.GetRequiredService<IMigrationRunner>();
            var logger = provider.GetRequiredService<ILogger<Program>>();

            logger.LogWarning("Aplicando migrações");

            runner.ListMigrations();

            runner.MigrateUp();

            return app;
        }

        private static void EnsureDatabaseExists(string connectionString)
        {
            var builder = new NpgsqlConnectionStringBuilder(connectionString);
            var schemaName = "public";


            builder.Database = "postgres";

            using var connection = new NpgsqlConnection(builder.ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = $@"
                DO $$
                BEGIN
                    IF NOT EXISTS (
                        SELECT 1
                        FROM information_schema.schemata
                        WHERE schema_name = '{schemaName}'
                    ) THEN
                        EXECUTE 'CREATE SCHEMA ""{schemaName}""';
                    END IF;
                END
                $$;
            ";
            command.ExecuteNonQuery();
        }
    }
}