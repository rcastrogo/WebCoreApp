
namespace Core.Authentication
{
  using Microsoft.AspNetCore.Mvc;
  
  internal class RequirementsAttribute : TypeFilterAttribute
  {    
    public RequirementsAttribute(string value) : base(typeof(RequirementsFilter))
    {
        Arguments = new object[] { value };
    }
  }

}
