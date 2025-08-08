namespace WebAppCubos.Modules
{
    using Application.Services;
    using Infrastructure.DataAccess.Sql;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public static class PostgressExtensions
    {
        public static IServiceCollection AddPostgress(this IServiceCollection services, IConfiguration configuration)
        {
            var serviceProvider = services.BuildServiceProvider();
            var environment = serviceProvider.GetRequiredService<IHostEnvironment>();

            var connectionString = configuration.GetConnectionString("Database");

            serviceProvider.GetService<ILogger<Program>>();

            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();

            services.AddDbContext<DatabaseContext>(
                options =>
                {
                    options
                        .UseNpgsql(connectionString)
                        .EnableSensitiveDataLogging()
                        .UseLazyLoadingProxies()
                        .EnableDetailedErrors()
                        .UseLoggerFactory(loggerFactory);
                });

            services.AddScoped<IUnitOfWork, UnitOfWork>();


            return services;
        }
    }
}