
namespace Core.Authentication
{
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.AspNetCore.Mvc.Filters;
  using Microsoft.Extensions.Configuration;
  using Microsoft.IdentityModel.Tokens;
  using System;
  using System.Collections.Generic;
  using System.IdentityModel.Tokens.Jwt;
  using System.Security.Claims;
  using System.Text;
  using System.Linq;

  public class RequirementsFilter : IAuthorizationFilter
  {
    private readonly IConfiguration _config;
    readonly string _value = "";

    public RequirementsFilter(string value, IConfiguration config)
    {
      _value = value;
      _config = config;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
      // =============================================================================
      // Validate Authorization header
      // =============================================================================
      var __tokens = context.HttpContext
                            .Request
                            .Headers["Authorization"]
                            .ToString()
                            .Split(" ");
      if(__tokens[0] != "Bearer")
      {
        context.Result = new UnauthorizedResult();
        return;
      }
      if(__tokens.Length < 2)
      {
        context.Result = new UnauthorizedResult();
        return;
      }
      // =============================================================================
      // Validate token
      // =============================================================================
      try
      {
        var __SEPARATORS = ",;".ToCharArray();
        var __secretKey = _config["JWTAutentication:Secret"];
        var __claims = new JwtValidator().ValidateToken(__tokens[1], __secretKey)
                                         .ToList();
        // =============================================================================
        // User Id
        // =============================================================================
        var __userId = __claims.Where(c => c.Type == "UserId")
                               .FirstOrDefault()
                               .Value;
        // =============================================================================
        // User roles
        // =============================================================================
        var __userRoles = __claims.Where( c => c.Type == "Roles")
                                  .FirstOrDefault()
                                  .Value.Split(__SEPARATORS, 
                                               StringSplitOptions.RemoveEmptyEntries);
        // =============================================================================
        // Valid roles
        // =============================================================================
        var __validRoles = _value.Split(__SEPARATORS, 
                                        StringSplitOptions.RemoveEmptyEntries);

        if (0 == __validRoles.Intersect(__userRoles).ToList().Count)
        {
          context.Result = new UnauthorizedResult();
          return;
        }

        context.HttpContext.Items.Add("userid", __userId);
        context.HttpContext.Items.Add("roles", __userRoles);
      }
      catch (System.Exception)
      {
        context.Result = new UnauthorizedResult();
        return;
      }      
    }
  }


  public class JwtValidator
  {

    public IEnumerable<Claim> ValidateToken(string token, string key)
    {      
      var __validations = new TokenValidationParameters
      {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
          ValidateIssuer = false,
          ValidateAudience = false,
          ValidateLifetime = true,
          ClockSkew = TimeSpan.Zero
      };

      return new JwtSecurityTokenHandler().ValidateToken(token, __validations, out var tokenSecure)
                                          .Claims;
    }

  }
}
