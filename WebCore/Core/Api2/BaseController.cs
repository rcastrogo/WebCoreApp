
namespace Core.Api2.Controlers
{

  using Microsoft.AspNetCore.Http;
  using Microsoft.Extensions.Configuration;
  using Core.Logging;
  using Core;

  public class BaseController {

    protected readonly ContextWrapper _contextWrapper;
    protected readonly HttpContext _context;
    protected readonly IConfiguration _config;
    protected readonly LogManager _log;

    public BaseController(HttpContext context){
      _context = context;
      _config = GetService<IConfiguration>();
      _log = GetService<LogManager>();
      _contextWrapper = new ContextWrapper(context);
    }

    public virtual ActionResult HandleRequest() {
      return new ActionResult() { StatusCode = 200, Content = "" };
    }

    private T GetService<T>()
    {
      return (T) _context.RequestServices.GetService(typeof(T));
    }


  }

}
