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

  [Route("api/estados-expediente")]
  public class EstadosController : ControllerBase {

    private Core.ContextWrapper _context;
    private readonly Core.Logging.LogManager _logger;
    private readonly IConfiguration _config;

    public EstadosController(IConfiguration config, Core.Logging.LogManager logger):base(){
      _logger = logger;
      _config = config;
    }

    [HttpGet]
    [Requirements("ADMIN;READER")]
    public IActionResult Get() {
      _logger.Write("EstadosController:Get");
      _context = new Core.ContextWrapper(HttpContext);
      return new JsonResult( new string[] { "Pendiente", 
                                            "Cerrado", 
                                            _context.GetItem("33") } );
    }

    [HttpGet("Trace")]
    public IActionResult GetTraceLines() {

      _context = new Core.ContextWrapper(HttpContext);

      Response.ContentType = "text/html";
      StringBuilder _sb = new StringBuilder();
      _sb.AppendLine("<h2>Trace</h2>");
      _sb.AppendLine("<pre>");
      foreach(string line in _logger.GetLines()){
        _sb.AppendLine(line);
      }
      _sb.AppendLine("</pre>");
      return Content(_sb.ToString());

      //return new JsonResult( _logger.GetLines().ToArray<string>());
    }

  }
}
