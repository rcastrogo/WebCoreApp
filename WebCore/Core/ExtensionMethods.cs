
namespace Core
{

  using System.IO;
  using System.Text;
  using System.Runtime.Serialization.Json;
  using Microsoft.AspNetCore.Http;
  using Microsoft.VisualBasic.CompilerServices;

  public static class ExtensionMethods
  {

    public static string ReadBody(this HttpRequest request)
    {
      if(request.Body.CanSeek) request.Body.Seek(0, System.IO.SeekOrigin.Begin);
      using (System.IO.StreamReader streamReader = new System.IO.StreamReader(request.Body))
      {
        string __result = streamReader.ReadToEnd();
        if(request.Body.CanSeek) request.Body.Seek(0, System.IO.SeekOrigin.Begin);
        return __result;
      }   
    }

    public static T FromJsonTo<T>(this string jsonString) where T : class
    {
      using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
      {
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
        return Conversions.ToGenericParameter<T>(serializer.ReadObject(stream));
      }
    }

  }

}
