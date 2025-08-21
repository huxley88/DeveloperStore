using System.Net.Http.Headers;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.WebApi;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Ambev.DeveloperEvaluation.Functional.Infrastructure;

public class CustomWebApplicationFactory : WebApplicationFactory<WebApiMarker>
{
    private static int _testCounter = 0;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        builder.ConfigureServices(services =>
        {
            // Remove DbContext real
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<DefaultContext>));
            if (descriptor != null)
                services.Remove(descriptor);

            var databaseName = $"func-tests-{Interlocked.Increment(ref _testCounter)}";
            services.AddDbContext<DefaultContext>(opt => opt.UseInMemoryDatabase(databaseName));

            // IMPORTANTE: Remove TODAS as configurações de autenticação existentes
            var authenticationServiceDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(AuthenticationSchemeProvider));
            if (authenticationServiceDescriptor != null)
                services.Remove(authenticationServiceDescriptor);

            var authHandlerDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IAuthenticationHandlerProvider));
            if (authHandlerDescriptor != null)
                services.Remove(authHandlerDescriptor);

            var authServiceDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IAuthenticationService));
            if (authServiceDescriptor != null)
                services.Remove(authServiceDescriptor);

            // Remove serviços JWT se existirem
            services.RemoveAll(typeof(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerHandler));

            // Configura APENAS a autenticação de teste como padrão
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Test";
                options.DefaultAuthenticateScheme = "Test";
                options.DefaultChallengeScheme = "Test";
                options.DefaultSignInScheme = "Test";
                options.DefaultSignOutScheme = "Test";
            })
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });

            // Inicializa o banco de dados
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DefaultContext>();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            // Seed de dados de teste
            if (!db.Products.Any())
            {
                db.Products.AddRange(
                    new Domain.Entities.Product
                    {
                        Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        Name = "Amber Ale 500ml",
                        Price = 7m
                    },
                    new Domain.Entities.Product
                    {
                        Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                        Name = "Pale Ale 330ml",
                        Price = 9.5m
                    }
                );
                db.SaveChanges();
            }
        });
    }

    public HttpClient CreateAuthenticatedClient()
    {
        var client = CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test", "token");
        return client;
    }
}