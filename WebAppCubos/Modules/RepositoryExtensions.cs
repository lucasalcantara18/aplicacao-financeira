using Domain.Exceptions;
using Domain.Interfaces.Base;
using Infrastructure.DataAccess.Sql.Bases;

namespace WebAppCubos.Modules
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection AddDatabaseRepositories(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            var assembly = typeof(BaseRepository<>).Assembly;
            var types = assembly.GetExportedTypes().Where(x => x.IsClass && !x.IsAbstract && x.GetInterface(nameof(IRepositoryBase)) != null).ToList();

            types.ForEach(x =>
            {
                var interfaces = x.GetInterfaces();
                if (!interfaces.Any())
                    throw new ApiException($"O repositório {x.Name}, não tem uma interface correspondente");
                var descriptor = new ServiceDescriptor(interfaces.Last(), x, serviceLifetime);
                services.Add(descriptor);
            });

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddDatabaseRepositories();

            return services;
        }
    }
}