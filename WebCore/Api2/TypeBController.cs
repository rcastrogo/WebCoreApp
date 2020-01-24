

namespace WebCore.Api2.Controlers
{

  using Core;
  using Core.Api2.Controlers;
  using Microsoft.AspNetCore.Http;
  using System;

  public class TypeBController : BaseController {

    public TypeBController(HttpContext context) : base(context) { }

    public override ActionResult HandleRequest()
    {
      // ===============================================================================
      // Par poder leer Request.Body
      // ===============================================================================
      //this._context.Request.EnableBuffering();
      //this._context.Request.EnableRewind();

      var s2 = _contextWrapper.GetItem("txt4", "rafa");
      var __action = _contextWrapper.GetItem("action", "rafa");
      var __userAgent = _contextWrapper.GetItem("User-Agent");
      var __body = _contextWrapper.GetBody();
      
      //var s2 = _contextWrapper.GetItem("txtNombre", "rafa");
      //var s3 = _contextWrapper.GetItem("accion", "rafa");
      
      return new ActionResult() {
        StatusCode = 200,
        Content = String.Format("TypeBController -> {0}\n{1}\n{2}\n{3}\n{4}",
                                _context.Request.Method,
                                _context.Request.Path.Value,
                                _context.Request.QueryString,
                                __body,
                                _context.Request.ContentType)
      };

    }

  }

}
