
namespace Core.Api2.Controlers
{

  using Microsoft.AspNetCore.Http;
  using System;
  using System.Collections.Generic;
  using System.Reflection;
  using System.Linq;
  using System.Text.RegularExpressions;

  public class ControllerManager {

    static readonly Dictionary<string,Type> _controllers = null;
    static readonly string[] _lines = null;
    static ControllerManager()
    {
      _controllers = Assembly.GetAssembly(typeof(Core.Api2.Controlers.BaseController))
                             .GetTypes()
                             .Where(t => typeof(Api2.Controlers.BaseController).IsAssignableFrom(t))
                             .ToDictionary( t => t.FullName, t => t);
      _lines = new string[] {"WebCore.Api2.Controlers.TypeAController /api/v2/datos-maestros/productores",
                             "WebCore.Api2.Controlers.TypeBController /api/v2/datos-maestros/otros"};
    }

    public ActionResult RouteRequest(HttpContext context) {

      // =============================================================================
      // Validate Authorization header
      // =============================================================================
      var __tokens = context.Request
                            .Headers["Authorization"]
                            .ToString()
                            .Split(" ");
      if (__tokens[0] != "Bearer")
      {
        return new ActionResult() {
          StatusCode = 401,
          Content = "Bearer is required" };
      }
      if (__tokens.Length < 2)
      {
        return new ActionResult() {
          StatusCode = 401,
          Content = "Token is not present" };
      }
      // ==============================================================================
      // Resolve controller
      // ==============================================================================
      var __controller = ResolveControler(context);
      if(__controller is null)
      {
        return new ActionResult() {
          StatusCode = 404,
          Content = string.Format("Controller not found for {0}", context.Request.Path)
        };
      }
      return ((Core.Api2.Controlers.BaseController)__controller).HandleRequest();
    }

    public ActionResult RouteAuth(HttpContext context)
    {
      return new JwtAuthController(context).HandleRequest();
    }

    private BaseController ResolveControler(HttpContext context)
    {
      string __path = context.Request.Path.Value.ToLower();     
      string __className = (_lines.Select( l => l.Split(" ", StringSplitOptions.RemoveEmptyEntries))
                                  .Where(tokens => __path.StartsWith(tokens[1]))
                                  .FirstOrDefault() ?? new string[] { "",""}
                           )[0];
      if(__className != "" && _controllers.ContainsKey(__className)){
        return (BaseController)Activator.CreateInstance(_controllers[__className], new object[] { context });        
      }
      return null;
    }
  }

}
