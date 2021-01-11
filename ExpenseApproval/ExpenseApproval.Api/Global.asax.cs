using System.Web;
using System.Web.Http;
using ExpenseApproval.Logging;
using Newtonsoft.Json.Serialization;

namespace ExpenseApproval.Api
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.Services.Replace(typeof(System.Web.Http.Tracing.ITraceWriter), new NLogger());
            HttpConfiguration config = GlobalConfiguration.Configuration;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;
            config.Filters.Add(new ExpenseExceptionFilterAttribute());
        }
    }
}
