using Core.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebCore
{
  //[Authorize]
  [Route("api/auth")]
  public class AuthorizationController : ControllerBase {

    private readonly Core.Logging.LogManager _logger;
    private readonly IConfiguration _config;

    public AuthorizationController(IConfiguration config, Core.Logging.LogManager logger):base(){
      _logger = logger;
      _config = config;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var users =  new string[] { "User 3", "User 2", "User 1"};
        return Ok(users);
    }

    [HttpPost]
    //[AllowAnonymous]
    [Route("login/{username}/{password}")]
    public IActionResult Authenticate(string username, string password)
    {
      var token = DoAutenticate(username, password);

      if (token == null)
          return BadRequest(new { message = "Username or password is incorrect" });

      return Ok(token);
    }

    private TokenContainer DoAutenticate( string username, string password)
    {
      if (password == "rr") return null;
      // ==============================================================================================
      // User autentication
      // ==============================================================================================
      //var user = new Core.Authentication.User();
      //if (user == null) return null;
      // ==============================================================================================
      // Authentication successful so generate jwt token
      // ==============================================================================================
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
      // ==============================================================================================
      // Return generated jwt token
      // ==============================================================================================
      return new TokenContainer() { Token = tokenValue };
    }

  }
}
