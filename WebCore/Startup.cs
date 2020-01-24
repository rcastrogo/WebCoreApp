using Core.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Threading.Tasks;

namespace WebCore
{

  public class Startup {

    public readonly IConfiguration Config;

    public Startup(IConfiguration config) {
      Config = config;
    }

    public void ConfigureServices(IServiceCollection services) {

      services.AddCors();
      services.AddLoggers(Config);
      //services.AddMvc()
      //        .WithRazorPagesAtContentRoot();

      //// =================================================================================================
      //// configure jwt authentication
      //// =================================================================================================
      //var key = Encoding.ASCII.GetBytes(Config["JWTAutentication:Secret"]);
      //services.AddAuthentication(x =>
      //{
      //    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      //    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      //})
      //.AddJwtBearer(x =>
      //{
      //    x.RequireHttpsMetadata = true;
      //    x.SaveToken = true;
      //    x.TokenValidationParameters = new TokenValidationParameters
      //    {
      //        ValidateIssuerSigningKey = true,
      //        IssuerSigningKey = new SymmetricSecurityKey(key),
      //        ValidateIssuer = false,
      //        ValidateAudience = false
      //    };
      //});

    }

    public void Configure(IApplicationBuilder app, 
                          IHostingEnvironment env, 
                          Core.Logging.LogManager log, 
                          IConfiguration config) {

      log.Write("=============================================");
      log.Write("Configure");
      log.Write("=============================================");

      if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
      }
 
      app.Use(async (context, next) => {
        log.Write(string.Format("{0} {1}{2}", 
                                context.Request.Method,
                                WebUtility.UrlDecode(context.Request.Path),
                                WebUtility.UrlDecode(context.Request.QueryString.Value)));
        try{
          await next.Invoke();
        } catch (Exception ex) {
          log.Write(ex.Message);
          throw;
        }
      }); 

      // ===================================================
      // global cors policy
      // ===================================================
      app.UseCors(x => x.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
      app.UseStaticFiles();
      //app.UseAuthentication();
      //app.UseMvc();
      app.Run(_handleRequest);

    }

    private async Task _handleRequest(HttpContext context){

      Func<HttpContext, Core.Api2.Controlers.ActionResult> __Proc = null;

      if (context.Request.Path.StartsWithSegments("/auth/login"))  __Proc = new Core.Api2.Controlers.ControllerManager().RouteAuth;
      if (context.Request.Path.StartsWithSegments("/api/v2")) __Proc = new Core.Api2.Controlers.ControllerManager().RouteRequest;
      if (context.Request.Path.StartsWithSegments("/json"))  __Proc = new JsonController().HandleRequest;
      
      try{

        if(__Proc is null)
        {
          context.Response.StatusCode = 404;
          await context.Response.WriteAsync("Resource not found");
          return;
        }
       
        Core.Api2.Controlers.ActionResult __result = __Proc.Invoke(context);
        context.Response.ContentType = "application/json; charset=utf-8";
        context.Response.StatusCode = __result.StatusCode;
        await context.Response.WriteAsync(__result.Content);
      }
      catch (Exception ex){
        await context.Response.WriteAsync(ex.Message);
      }

    }

  }

  public class JsonController {
  
    public Core.Api2.Controlers.ActionResult HandleRequest(HttpContext context) {

      var header = context.Request.Headers["Authorization"];
      var ser = context.RequestServices.GetService(typeof(Core.Logging.LogManager));
      return new Core.Api2.Controlers.ActionResult() { 
        StatusCode = 200, 
        Content = String.Format("JSON: {0}", context.Request.Path.Value)
      };

    }  
    
  }

}           
                                                               
