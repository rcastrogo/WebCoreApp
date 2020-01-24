
namespace WebCore.Api2.Controlers
{

  using Core;
  using Core.Api2.Controlers;
  using Microsoft.AspNetCore.Http;
  using System;

  public class TypeAController : BaseController {

    public TypeAController(HttpContext context) : base(context) { }

    public override ActionResult HandleRequest()
    {
      return new ActionResult() {
        StatusCode = 200,
        Content = String.Format("TypeAController -> {0}\n{1}\n{2}\n{3}\n{4}",
                                _context.Request.Method,
                                _context.Request.Path.Value,
                                _context.Request.QueryString,
                                _context.Request.ReadBody(),
                                _context.Request.ContentType)
      };
    }

  }

}
