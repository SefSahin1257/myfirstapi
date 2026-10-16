using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class TestAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
  public TestAuthenticationHandler(
	IOptionsMonitor<AuthenticationSchemeOptions> options,
 	ILoggerFactory logger,
	UrlEncoder encoder) : base(options, logger, encoder)
	{
	}

  protected override Task<AuthenticateResult> HandleAuthenticateAsync()
  {
	  
	  var role = Request.Headers["X-Test-Role"].ToString();

	  if (String.IsNullOrEmpty(role))
	  {
		  role = "Admin";
	  }


	  var claims = new List<Claim>
	{
 	 new Claim(ClaimTypes.NameIdentifier, "1"),
	 new Claim(ClaimTypes.Name, "TestUser"),
	 new Claim(ClaimTypes.Role, role)
	};

	  if (role == "Admin")
	  {
		  claims.Add(new Claim("Permission", "ManageProducts"));
	  } 
	  
	  var identity = new ClaimsIdentity(claims, "Test");
	  var principal = new ClaimsPrincipal(identity);
	  var ticket = new AuthenticationTicket(principal, "Test");
	  
	  return Task.FromResult(AuthenticateResult.Success(ticket));
  }

}
