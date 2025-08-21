namespace Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature;

public sealed class AuthenticateUserResponse
{

    public string Token { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;   

    public string Name { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;
}
