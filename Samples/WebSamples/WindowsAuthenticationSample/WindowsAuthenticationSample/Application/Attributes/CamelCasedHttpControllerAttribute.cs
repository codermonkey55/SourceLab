using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http.Controllers;

namespace WindowsAuthenticationSample.Application.Attributes
{
    public class CamelCasedHttpControllerAttribute
    {
        public void SetCamelCasedFormatter(HttpControllerSettings settings, HttpControllerDescriptor descriptor)
        {
            var jsonMediaTypeFormatter = settings.Formatters.OfType<JsonMediaTypeFormatter>().SingleOrDefault();

            settings.Formatters.Remove(jsonMediaTypeFormatter);

            jsonMediaTypeFormatter = new JsonMediaTypeFormatter
            {
                SerializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }
            };

            settings.Formatters.Add(jsonMediaTypeFormatter);
        }
    }
}