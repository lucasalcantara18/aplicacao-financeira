namespace WebAppCubos.Modules
{
    using Application.Services;
    using Application.Services.External.Core;
    using Infrastructure.DataAccess.Sql;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Refit;

    public static class RefitExtensions
    {
        public static IServiceCollection AddRefit(this IServiceCollection services)
        {
           services.AddScoped<SomeHandler>();
           services.AddRefitClient<IExternalApi>(provider => new RefitSettings() { Buffered = true })
                .ConfigureHttpClient(client =>
                {
                    client.BaseAddress = new Uri("https://compliance-api.cubos.io");
                })
                .AddHttpMessageHandler<SomeHandler>();


            return services;
        }

        public class SomeHandler : DelegatingHandler
        {

            public SomeHandler()
            {
            }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                if (request.Content != null)
                {
                    var requestString = await request.Content.ReadAsStringAsync();
                }
                return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}