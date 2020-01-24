
namespace Core.Api2.Controlers
{

  using Microsoft.AspNetCore.Http;
  using System;
  using System.Reflection;
  using System.Linq;

  public class ControllerManager {

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
      // ================================================================================================
      // Resolve controller
      // ================================================================================================
      string[] __lines = {"/api/v2/datos-maestros/productores#WebCore.Api2.Controlers.TypeAController",
                          "/api/v2/datos-maestros/otros#WebCore.Api2.Controlers.TypeAController"};

      var __className = __lines.Where(l => {
                                  var __t = l.Split("#");
                                  return new System.Text.RegularExpressions.Regex(__t[0], System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                                                                           .Match(context.Request.Path.Value)
                                                                           .Success;
                                }).FirstOrDefault();

      //var __segment = context.Request.Path.Value.Split("/")[3];

      //var __map = new System.Collections.Generic.Dictionary<string, string>();
      //__map.Add("tipoa", "WebCore.Api2.Controlers.TypeAController");
      //__map.Add("tipob", "WebCore.Api2.Controlers.TypeBController");

      //var __className = "";
      //if (__map.ContainsKey(__segment.ToLower()))
      //{
      //  __className = __map[__segment.ToLower()];
      //}

      var __controllers = Assembly.GetAssembly(typeof(Core.Api2.Controlers.BaseController))
                                  .GetTypes()
                                  .Where(t => typeof(Api2.Controlers.BaseController).IsAssignableFrom(t))
                                  .ToDictionary( t => t.FullName, 
                                                 t => t);
      if(__controllers.ContainsKey(__className)){
        var __controler = Activator.CreateInstance( __controllers[__className], new object[] { context });
        return ((Core.Api2.Controlers.BaseController)__controler).HandleRequest();
      }
      return new ActionResult() { StatusCode = 404, 
                                  Content = string.Format("Controller not found for {0}", 
                                                           context.Request.Path) };
    }

    public ActionResult RouteAuth(HttpContext context)
    {
      return new JwtAuthController(context).HandleRequest();
    }
  }

}
