using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ambev.DeveloperEvaluation.Functional.Infrastructure;

public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Para testes, sempre consideramos o usuário autenticado se o header estiver presente
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            Logger.LogWarning("No Authorization header found");
            return Task.FromResult(AuthenticateResult.Fail("Missing Authorization header"));
        }

        var authHeader = Request.Headers["Authorization"].ToString();
        if (!authHeader.StartsWith("Test", StringComparison.OrdinalIgnoreCase))
        {
            Logger.LogWarning($"Invalid Authorization scheme: {authHeader}");
            return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization scheme"));
        }

        // Cria claims de teste
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "test-user"),
            new Claim(ClaimTypes.NameIdentifier, "test-user-id"),
            new Claim("sub", "test-user-id"),
            new Claim("email", "test@example.com")
        };

        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "Test");

        Logger.LogInformation("Test authentication successful");
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}