using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;


namespace pitchamon.Api.Auth;

public class BearerAuthService : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IConfiguration config;

    public BearerAuthService(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IConfiguration configuration)
        : base(options, logger, encoder)
    {
        config = configuration;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return Task.FromResult(AuthenticateResult.Fail("Missing Authorization header"));
        }
        
        var header = Request.Headers["Authorization"].ToString();
        if (!header.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization header"));
        }
        
        var token = header.Substring("Bearer ".Length).Trim();
        var expectedToken = config["Auth:BearerToken"];
        
        if (token != expectedToken)
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid token"));
        }
        
        var claims = new[] { new Claim(ClaimTypes.Name, "AuthenticatedUser") };
        
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}