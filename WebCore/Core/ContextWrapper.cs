
namespace Core
{

  using Microsoft.AspNetCore.Http;

  public class ContextWrapper{

    private HttpContext _context;

    public ContextWrapper(HttpContext context) {
      _context = context;
    }

    public string GetItem(string name) {
      return ParseString(name);
    }

    public string GetItem(string name, string @default = "") {
      return ParseString(name, @default);
    }
        
    public string GetBody() {    
      return _context.Request.ReadBody();
    }

    private string ParseString(string name, string @default = "")
    {
      string __value = "";
      if (_context is null || _context.Request is null) return @default;
      __value = _context.Request.Query[name].ToString();
      if(__value.Length == 0) __value = _context.Request.Headers[name].ToString();
      if(__value.Length == 0) __value = _context.Request.Form[name].ToString();
      return __value.Length == 0 ? @default : __value;
    }

  }

}
