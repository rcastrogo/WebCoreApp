

namespace Core.Api2.Controlers
{
  using Microsoft.AspNetCore.Http;
  using Microsoft.IdentityModel.Tokens;
  using System;
  using System.IdentityModel.Tokens.Jwt;
  using System.Security.Claims;
  using System.Text;

  public class JwtAuthController : BaseController {

    public JwtAuthController(HttpContext context) : base(context) { }

    public override ActionResult HandleRequest()
    {
      string __body = _contextWrapper.GetBody();
      LoginPayload __paydload = __body.FromJsonTo<LoginPayload>();
      if(__paydload.Password == "xx")
      {
        return new ActionResult() { StatusCode = 401, 
                                    Content = "Username or password is incorrect" };
      }
      // ==========================================================
      // User autentication
      // ==========================================================
      //var user = new Core.Authentication.User();
      //if (user == null) return null;

      // ==========================================================
      // Create token
      // ==========================================================
      return new ActionResult() { StatusCode = 200, 
                                  Content = CreateJwtToken(__paydload.Username) };
    }
    private string CreateJwtToken(string username)
    {
      // ====================================================================================
      // Generate jwt token
      // ====================================================================================
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(_config["JWTAutentication:Secret"]);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new Claim[] {
            new Claim("UserId", username),
            new Claim("Roles", "ADMIN;WRITER;READER")
        }),
        Expires = DateTime.UtcNow.AddHours(12),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), 
                                                    SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);
      var tokenValue = tokenHandler.WriteToken(token);
      // ====================================================================================
      // Return generated jwt token
      // ====================================================================================
      return tokenValue;
    }

    public class LoginPayload
    {
      public string Username { get; set; }
      public string Password { get; set; }
    }
  }

}
